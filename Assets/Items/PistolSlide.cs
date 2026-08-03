using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PistolSlide : MonoBehaviour
{
    [Header("Components")]
    public XRGrabInteractable grabInteractable;
    public Transform slideTransform;
    
    [Header("Settings")]
    public float pullThreshold = 0.04f; // Distance it must be pulled (meters)
    public UnityEvent onChamberLoaded;
    private Vector3 startLocalPos;
    private bool isPulledBack = false;

    void Start()
    {
        startLocalPos = slideTransform.localPosition;
        grabInteractable.selectExited.AddListener(OnSlideReleased);
    }

    void Update()
    {
        if (grabInteractable.isSelected)
        {
            float pullDistance = Mathf.Abs(startLocalPos.z - slideTransform.localPosition.z);

            if (pullDistance >= pullThreshold && !isPulledBack)
            {
                isPulledBack = true;
                // TODO: Click audio
            }
        }
    }

    private void OnSlideReleased(SelectExitEventArgs args)
    {
        if (isPulledBack)
        {
            onChamberLoaded.Invoke();
            isPulledBack = false;
        }
    }
}