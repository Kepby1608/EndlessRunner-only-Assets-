using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    float rotationSpeed = 100;
    public AudioClip coinSound;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed += Random.Range(0, rotationSpeed / 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.gameObject.SetActive(false);
            CoinsCount.Instance.countCoin++;
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.parent.gameObject.SetActive(false);
        }
    }
    */
}
