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
        yield return new WaitForSeconds(interval);
        Instantiate(zombiePrefab, transform.position, transform.rotation);
        StartCoroutine(Spawn());
    }
}
