using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        if (GameManager.Instance != null && text != null)
        {
            GameManager.Instance.RegisterHealthText(text);
        }
    }
}   