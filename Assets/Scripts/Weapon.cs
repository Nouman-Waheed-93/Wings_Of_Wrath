using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float firingInterval;
    [SerializeField]
    private GameObject bulletPrefab;

    private bool isShooting;

    private float lastBulletFiredTime;
    [SerializeField]
    private float targettingAngle;
    [SerializeField]
    private float targettingDistance;
    [SerializeField]
    private Transform target;


    public void StartShooting()
    {
        isShooting = true;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, target.position) < targettingDistance &&
                Vector3.Angle(transform.forward, target.position - transform.position) < targettingAngle)
        {
            StartShooting();
        }
        else
        {
            StopShooting();
        }

        if (isShooting)
        {
            if(Time.time - lastBulletFiredTime > firingInterval)
            {
                lastBulletFiredTime = Time.time;
                Instantiate(bulletPrefab, transform.position, transform.rotation);
            }
        }
    }

    public void StopShooting()
    {
        isShooting = false;
    }
}
