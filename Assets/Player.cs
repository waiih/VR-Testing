using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void OollisionEnter(Collision collision)
    {
        if (collision.body.tag == "Zombie")
        {
            Debug.Log("Zombie damaged player!");
            GameManager.Instance.Damage(collision.body.GetComponent<Zombie>().damage);
        }        
    }
}
