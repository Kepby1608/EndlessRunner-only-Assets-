using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    float rotationSpeed = 100;
    public CoinsCount CC;
    public AudioClip coinSound;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed += Random.Range(0, rotationSpeed / 4.0f);
        CC  = GameObject.Find("Canvas").GetComponent<CoinsCount>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.parent.gameObject.SetActive(false);
            CC.countCoin++;
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
