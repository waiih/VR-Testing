using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float interval = 30f;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        Instantiate(zombiePrefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(interval);
        StartCoroutine(Spawn());
    }
}
