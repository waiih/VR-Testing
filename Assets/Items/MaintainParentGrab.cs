using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class MaintainParentGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Transform originalParent;
    private Vector3 localPositionOffset;
    private Quaternion localRotationOffset;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        originalParent = transform.parent;

        if (originalParent != null)
        {
            localPositionOffset = transform.localPosition;
            localRotationOffset = transform.localRotation;
        }
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (originalParent != null)
        {
            transform.SetParent(originalParent);
        }
    }
}
