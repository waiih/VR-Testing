using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Reloader : MonoBehaviour
{
    [SerializeField] public bool isDupe = false;
    [SerializeField] public bool isLoaded = true;
    private bool destroying = false;
    private bool canEmpty = true;
    
    public void Empty(SimpleShoot gun)
    {
        if (destroying || !isLoaded || !canEmpty) return;

        Rigidbody rb = GetComponent<Rigidbody>();
        
        XRGrabInteractable interactor = GetComponent<XRGrabInteractable>();
        IXRSelectInteractor hand = interactor.firstInteractorSelecting;
            
        if (gun)
        {
            gun.magEmpty = true;
        }
        
        if (interactor != null)
        {
            interactor.interactionManager.SelectExit(hand, interactor);
        }

        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;


        StartCoroutine(EmptyOut(1.5f));
    }

    public void OnDeselect()
    {
        if (!isDupe) return;
        Destroy(gameObject, 0.5f);
    }

    public void Reload(SimpleShoot forGun, Vector3 location, Transform mag_loc)
    {   
        if (isLoaded) return;
        canEmpty = false;

        XRGrabInteractable interactor = GetComponent<XRGrabInteractable>();
        if (interactor != null && interactor.isSelected)
        {
            IXRSelectInteractor hand = interactor.firstInteractorSelecting;
            interactor.interactionManager.SelectExit(hand, interactor);
        }

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
        transform.rotation = forGun.transform.rotation;

        
        forGun.magEmpty = false;
        forGun.Reload();
        transform.SetParent(mag_loc);
        transform.position = location;

        StartCoroutine(SetCanEmpty());
    }

    IEnumerator EmptyOut(float delayInSeconds)
    {
        destroying = true;
        yield return new WaitForSeconds(delayInSeconds); 
        Destroy(gameObject);
    }

    IEnumerator SetCanEmpty()
    {
        yield return new WaitForSeconds(1f);
        canEmpty = true;
    }
}
