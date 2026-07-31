using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineHolder : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Mag" || transform.childCount > 0) return;
        Reloader reloader = other.GetComponent<Reloader>();
        if (reloader.isLoaded) return;
        Vector3 mag_location = transform.position;
        SimpleShoot gunScript = transform.parent.GetComponent<SimpleShoot>();
        


        reloader.Reload(gunScript, mag_location, transform);
        reloader.isLoaded = true;
    }
}
