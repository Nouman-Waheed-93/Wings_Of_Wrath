using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftHealth : MonoBehaviour
{
    private int totalHealth = 10;
    private int currHealth;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private float respawnDistance;

    private void Start()
    {
        currHealth = totalHealth;
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        if(currHealth <= 0)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        currHealth = totalHealth;
        Vector3 newPosition = player.position + Random.onUnitSphere * respawnDistance;
        if (newPosition.y < 20)
            newPosition.y = 20;
        transform.position = newPosition;
    }
}
