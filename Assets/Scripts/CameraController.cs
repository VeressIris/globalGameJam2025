using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform player;
    Vector3 targetPos;
    Vector3 posDerv;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
    }
    void Update()
    {
        targetPos = player.position;
        targetPos.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref posDerv, .5f);
    }
}
