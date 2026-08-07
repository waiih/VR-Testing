using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FuelStatus : MonoBehaviour
{
    private void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        if (GameManager.Instance != null && text != null)
        {
            GameManager.Instance.RegisterFuelUI(text);
        }
    }
}
