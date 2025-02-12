using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[SelectionBase]
public class Rocket : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float thrustForce = 20f;
    [SerializeField] float rotateForce = 5f;

    public GameObject[] rocketParts;

    bool isFly = true;

    [SerializeField] float explosionForce = 100f;
    [SerializeField] float explosionRadius = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

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
            print("Вызов функции финиша!");
        }
        else
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        isFly = false;

        foreach (GameObject part in rocketParts)
        {
            part.transform.SetParent(null);

            Rigidbody _prb = part.AddComponent<Rigidbody>();
            if (_prb != null)
            {
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
}
