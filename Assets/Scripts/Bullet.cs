using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float lifeTime;

    private Rigidbody rigidbody;
    private float startTime;

    private void Awake()
    {
        startTime = Time.time;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = transform.forward * speed;
        if(Time.time - startTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AircraftHealth health;
        if(other.transform.root.TryGetComponent<AircraftHealth>(out health))
        {
            health.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
