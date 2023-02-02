using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Camera camera;
    private Vector3 offset;
    public float xPostion = 0;

    void Start(){
        offset.x = xPostion;
        offset.y = camera.transform.position.y - player.transform.position.y;
        offset.z = camera.transform.position.z - player.transform.position.z;

    }

    void LateUpdate()
    {
        if(player.transform.position.x != 3 || player.transform.position.x != -3)
            camera.transform.position = player.transform.position + offset;
    }
}