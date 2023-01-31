using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Camera camera;
    private Vector3 offset;

    void Start()
    {
        offset = camera.transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        camera.transform.position = player.transform.position + offset;
    }
}
