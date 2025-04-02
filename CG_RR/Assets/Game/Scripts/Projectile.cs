using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitVFX)
        {
            if (collision.gameObject.tag == "Destructible")
            {
                HitEffect();
                collision.gameObject.GetComponent<Destructible>().DestroyObstacle();
                Destroy(collision.gameObject);
            }
            else
            {
                HitEffect();
            }
            
        }
        Destroy(gameObject);
    }

    void HitEffect()
    {
        GameObject hitEffect = Instantiate(hitVFX, transform.position, Quaternion.identity);
        Destroy(hitEffect, 0.5f);
    }

}
