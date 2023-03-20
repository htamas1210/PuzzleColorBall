using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        //Debug.Log("destroy wall trigger");
        Destroy(other.gameObject);
    }

    private void OnCollisionEnter(Collision other) {
        //Debug.Log("collision enter");
        Destroy(other.gameObject);
    }
}
