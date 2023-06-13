using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : Singleton<CameraTracking>
{
    public Transform Player;
    Vector3 target;
    public float trackingSpeed = 1.5f;
    public float posYOverPlayer = 2.5f;
    Vector3 currentPosition;

    private void Start()
    {
        Die();
    }

    private void Update()
    {
        currentPosition = Vector3.Lerp(transform.position, target, trackingSpeed * Time.deltaTime);
        transform.position = currentPosition;

        target = new Vector3(Player.transform.position.x, Player.transform.position.y + posYOverPlayer, transform.position.z);
    }

    public void Die()
    {
        transform.position = new Vector3(0, Player.transform.position.y + posYOverPlayer, -8);
    }
}
