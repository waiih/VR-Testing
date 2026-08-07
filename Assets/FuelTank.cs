using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour
{
    public bool filled = false;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fuel"))
        {
            filled = true;
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
    }
}
