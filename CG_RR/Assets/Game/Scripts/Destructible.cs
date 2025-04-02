using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    GameObject[] parts;

    [Header("Explosion params")]
    [SerializeField] float explosionForce = 100f;
    [SerializeField] float explosionRadius = 5f;



    void Start()
    {
        parts = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            parts[i] = transform.GetChild(i).gameObject;
        }
    }


    public void DestroyObstacle()
    {
        foreach (GameObject part in parts)
        {
            part.transform.SetParent(null);

            Rigidbody _prb = part.AddComponent<Rigidbody>();
            if (_prb != null)
            {
                _prb.isKinematic = false;
                _prb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }
}
