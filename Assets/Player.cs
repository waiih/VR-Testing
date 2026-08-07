using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            GameManager.Instance.Damage(other.GetComponent<Zombie>().damage);
        }

        if (other.gameObject.CompareTag("Helicopter"))
        {
            GameManager.Instance.PlayerInExfilZone = true;
        }
    }
}
