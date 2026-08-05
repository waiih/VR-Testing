using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "KeyItem") return;

        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "KeyItem") return;

    }
}
