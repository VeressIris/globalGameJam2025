using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 targetPos;
    Vector3 posDerv;
    
    void Start()
    {
        
    }
    void Update()
    {
        targetPos = player.position;
        targetPos.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref posDerv, .5f);
    }
}
