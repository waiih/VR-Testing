using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class EndingUI : MonoBehaviour
{
    public static EndingUI Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private float fadeDuration = 2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowEnding(string message)
    {
        endingText.text = message;
        StartCoroutine(FadeToBlackRoutine());
    }

    private IEnumerator FadeToBlackRoutine()
    {
        canvasGroup.blocksRaycasts = true; 
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}