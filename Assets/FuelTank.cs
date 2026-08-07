using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour
{
    public AudioClip splashSfx;
    public bool filled = false;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fuel"))
        {
            filled = true;
            GetComponent<AudioSource>().PlayOneShot(splashSfx);
        }

        if (collision.gameObject.CompareTag("Car") && filled)
        {
            Fuel();
        }
    }

    public void Fuel()
    {
        filled = false;
        GameManager.Instance.FillCar();
        GetComponent<AudioSource>().PlayOneShot(splashSfx);
    }
}
