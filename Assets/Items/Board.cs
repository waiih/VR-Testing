using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Board : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public bool isConstructed = false;

    public void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }
    public void Update()
    {
        grabInteractable.enabled = !isConstructed;
    }
    public void Construct(Transform pos)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.position = pos.position;
        transform.rotation = pos.rotation;
        isConstructed = true;
    }


    public void OnGrab()
    {
        if (isConstructed)
        {
            
        }
    }
}
