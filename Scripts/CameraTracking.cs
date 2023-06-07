using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform Player;
    Vector3 target;
    public float trackingSpeed = 1.5f;

    private void Update()
    {
        Vector3 currentPosition = Vector3.Lerp(transform.position, target, trackingSpeed * Time.deltaTime);
        transform.position = currentPosition;

        target = new Vector3(Player.transform.position.x, Player.transform.position.y + 7, transform.position.z);
    }
}
