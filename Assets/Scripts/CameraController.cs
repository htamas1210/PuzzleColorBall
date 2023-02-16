using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; //jatekos objektum
    public Camera camera; //fo kamera
    private Vector3 offset; //kamera pozicio
    public float xPostion = 0; //x tengely pozicio

    private void Start(){ //kezdeskor a kamera ugorjon a jatekos moge
        offset.x = xPostion;
        offset.y = camera.transform.position.y - player.transform.position.y;
        offset.z = camera.transform.position.z - player.transform.position.z;

    }

    private void LateUpdate(){   //kamera pozicio frissitese
        if(player.transform.position.x != 3 || player.transform.position.x != -3)
            camera.transform.position = player.transform.position + offset;
    }
}