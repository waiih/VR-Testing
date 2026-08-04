using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;
    [Tooltip("Ammo capacity")] [SerializeField] private int ammoCapacity = 7;
    [Tooltip("Current ammo")] private int ammo = 7;
    [Tooltip("Firerate in ms/shot")] [SerializeField] private float firerate = 100f;
    [SerializeField] public float damage = 34f;
    [SerializeField] public float headshotMultiplier = 1.5f;
    private bool cooldown = false;

    public bool magEmpty = false;
    private bool isChambered = true;

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }

    public void Fire()
    {
        if (cooldown) return;
        
        if (ammo <= 0 || !isChambered)
        {
            isChambered = false;
            OnEmptyShoot();
            return;
        }

        if (magEmpty && ammo > 0)
        {
            ammo = 1;
            gunAnimator.Play("Fire");
            isChambered = false;
        } else {
            gunAnimator.Play("Fire");
        }

        cooldown = true;
        StartCoroutine(FireCooldown());
    }

    //This function creates the bullet behavior
    public void Shoot()
    {


        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        GameObject bullet;
        // Create a bullet and add force on it in direction of the barrel
        bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        bullet.GetComponent<Bullet>().shooter = this;
        bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

        ammo--;
        Destroy(bullet, 1.0f);
    }

    void OnEmptyShoot()
    {
        // TODO
    }

    public void Reload()
    {
        ammo += ammoCapacity;
        Debug.Log("Reloaded: " + ammo);
    }

    public void AnimateChamber()
    {
        if (isChambered) return;
        gunAnimator.Play("Chamber");
    }

    public void Chamber()
    {
        isChambered = true;
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

    IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(firerate / 1000f);
        cooldown = false;
    }
}
