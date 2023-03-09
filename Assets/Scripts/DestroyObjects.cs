using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log("destroy wall trigger");
        Destroy(other.gameObject);
    }
}
