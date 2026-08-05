using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "KeyItem") return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.keyItemsCount++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "KeyItem") return;

        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.keyItemsCount--;
        }
    }
}
