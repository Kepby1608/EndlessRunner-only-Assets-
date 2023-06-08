using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject explosionPlayer;
    private GameObject cloneExplosion;

    private void OnTriggerEnter(Collider other)
    {
        var rb = GetComponent<Rigidbody>();

        if (other.tag == "Bolt")
        {
            cloneExplosion = Instantiate(explosion, rb.position, rb.rotation) as GameObject;

            Destroy(other.gameObject);
            Destroy(gameObject);
            Destroy(cloneExplosion, 1f);
        }

        if (other.tag == "Player")
        {
            cloneExplosion = Instantiate(explosionPlayer, rb.position, rb.rotation) as GameObject;

            Destroy(other.gameObject);
            Destroy(gameObject);
            Destroy(cloneExplosion, 1f);
        }
    }
}
