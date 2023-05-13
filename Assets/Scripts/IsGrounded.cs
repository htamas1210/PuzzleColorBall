using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    public bool isGrounded = true;

    private void OnCollisionEnter(Collision other) {
        //Debug.Log("pc <color=blue>"+other.gameObject.name+"</color>");
        isGrounded = true;
    }
}
