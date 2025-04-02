using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShoot : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float projectileSpeed = 20f;

    Rocket rocket;

    void Start()
    {
        rocket = GetComponent<Rocket>();
    }

    
    void Update()
    {
        if (!rocket.GetIsFly()) return;
        if (Input.GetKeyDown(KeyCode.W))
        {
            Shoot();
        }
    }


    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = firePoint.up * projectileSpeed;
    }    
}
