using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SliderGrabInteractable : XRGrabInteractable
{
    protected override void Awake()
    {
        base.Awake();
        movementType = MovementType.VelocityTracking;
    }

    protected override void Detach()
    {
        if (retainTransformParent && transform.parent != null)
        {
            return; 
        }
         
        base.Detach();
    }
}
