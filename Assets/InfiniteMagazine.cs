using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class InfiniteMagazine : MonoBehaviour
{
    [SerializeField] private GameObject magazinePrefab;
    public void GrabMagazine()
    {
        XRSimpleInteractable interactor = GetComponent<XRSimpleInteractable>();
        if (interactor != null)
        {
            IXRSelectInteractor hand = interactor.firstInteractorSelecting;
            GameObject newMag = Instantiate(magazinePrefab);
            XRGrabInteractable newMagInteractable = newMag.GetComponent<XRGrabInteractable>();
            
            if (newMagInteractable != null) {
                interactor.interactionManager.SelectExit(hand, interactor);
                interactor.interactionManager.SelectEnter(hand, newMagInteractable);
            }
        }
    }
}
