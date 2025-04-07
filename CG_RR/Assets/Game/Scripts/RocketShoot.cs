using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShoot : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float projectileSpeed = 20f;

    [SerializeField] float fireRate = 10f;
    float nextFireTime;

    Rocket rocket;

    void Start()
    {
        rocket = GetComponent<Rocket>();
    }

    
    void Update()
    {
        if (!rocket.GetIsFly()) return;
        if (Input.GetKey(KeyCode.W) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }


    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = firePoint.up * projectileSpeed;
    }    

    public IEnumerator ShootBoost(float newFireRate, float duration)
    {
        float currentFireFate = fireRate;
        fireRate *= newFireRate;
        yield return new WaitForSeconds(duration);
        fireRate = currentFireFate;
    }
}
