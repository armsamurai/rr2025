using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[SelectionBase]
public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    AudioSource au;
    GameObject[] rocketParts;

    bool isFly = true;

    [Header("Мощность тяги")]
    [SerializeField] float thrustForce = 20f;
    [Header("Скорость разворота")]
    [SerializeField] float rotateForce = 5f;
    [Space]

    [Header("Explosion params")]
    [SerializeField] float explosionForce = 100f;
    [SerializeField] float explosionRadius = 5f;
    [Space]

    [Header("Sounds")]
    [SerializeField] AudioClip winSFX;
    [SerializeField] AudioClip destroySFX;
    [Space]

    [Header("Effects")]
    [SerializeField] ParticleSystem leftEngineVFX;
    [SerializeField] ParticleSystem rightEngineVFX;
    [SerializeField] ParticleSystem winVFX;
    [SerializeField] ParticleSystem destroyVFX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        au = GetComponent<AudioSource>();

        rocketParts = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            rocketParts[i] = transform.GetChild(i).gameObject;
        }
    }

    void FixedUpdate()
    {
        if (isFly)
        {
            AddThrust();
            RotateRocket();
        }
    }

    void AddThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(0, thrustForce, 0);
            if (!au.isPlaying)
            {
                au.Play();
                leftEngineVFX.Play();
                rightEngineVFX.Play();
            }
        }
        else
        {
            au.Stop();
            leftEngineVFX.Stop();
            rightEngineVFX.Stop();
        }
    }

    void RotateRocket()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.angularVelocity = Vector3.zero;
            transform.Rotate(0, 0, rotateForce);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.angularVelocity = Vector3.zero;
            transform.Rotate(0, 0, -rotateForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFly) return;

        if (collision.transform.CompareTag("Start"))
        {
            return;
        }
        else if (collision.transform.CompareTag("Finish"))
        {
            StartCoroutine(LoadNextLevel());
        }
        else
        {
            StartCoroutine(Explode());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ShootBooster")
        {
            float power = other.GetComponent<Booster>().power;
            float duration = other.GetComponent<Booster>().duration;
            StartCoroutine(GetComponent<RocketShoot>().ShootBoost(power, duration));
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "ThrustBooster")
        {
            float power = other.GetComponent<Booster>().power;
            float duration = other.GetComponent<Booster>().duration;
            StartCoroutine(ThrustBooster(power, duration));
            Destroy(other.gameObject);
        }
    }

    IEnumerator Explode()
    {
        isFly = false;
        au.Stop();
        au.PlayOneShot(destroySFX);
        leftEngineVFX.Stop();
        rightEngineVFX.Stop();
        destroyVFX.Play();

        foreach (GameObject part in rocketParts)
        {
            part.transform.SetParent(null);

            Rigidbody _prb = part.AddComponent<Rigidbody>();
            if (_prb != null)
            {
                _prb.gameObject.layer = 0;
                _prb.isKinematic = false;
                _prb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        yield return new WaitForSeconds(3f);

        RestartLevel();
    }

    void RestartLevel()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadNextLevel()
    {
        isFly = false;
        au.Stop();
        au.PlayOneShot(winSFX);
        leftEngineVFX.Stop();
        rightEngineVFX.Stop();
        winVFX.Play();
        yield return new WaitForSeconds(2f);
        
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex + 1);
    }

    public bool GetIsFly()
    {
        return isFly;
    }

    IEnumerator ThrustBooster(float newThrustForce, float duration)
    {
        float currentThrustForce = thrustForce;
        thrustForce *= newThrustForce;
        yield return new WaitForSeconds(duration);
        thrustForce = currentThrustForce;
    }
}
