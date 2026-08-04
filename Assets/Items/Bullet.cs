using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public SimpleShoot shooter;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Zombie" && shooter != null)
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.health -= shooter.damage;
            }
        } else if (collision.collider.tag == "ZombieHead" && shooter != null)
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.health -= shooter.damage * shooter.headshotMultiplier;
                Debug.Log("Headshot!");
            }
        }
    }
}
