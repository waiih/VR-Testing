using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrench : MonoBehaviour
{
    public AudioClip wrenchSfx;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            GameManager.Instance.SetCarFixed(true);
            GetComponent<AudioSource>().PlayOneShot(wrenchSfx);
        }
    }
}
