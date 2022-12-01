using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum Colors {
    None = -1,
    Grey,
    Red,
    Blue,
    Green,
    Yellow,
    Rainbow
};

public class ColorManager : MonoBehaviour
{
    public Material[] colorMaterials = new Material[Enum.GetNames(typeof(Colors)).Length];

    public void WashColor(GameObject washed) {
        SetColor(Colors.Grey, washed);
    }

    public void SetColor(Colors color, GameObject toPaint, Collision collision = null) {
        if(color == Colors.None) {
            if (collision.transform.CompareTag("ColorChanger")) {
                toPaint.GetComponent<Renderer>().material.color = collision.transform.GetComponent<Renderer>().material.color;
            }
        } else {
            toPaint.GetComponent<Renderer>().material.color = colorMaterials[(int)color].color;
        }
        
    }

    public void CheckGroundColor(GameObject collided) {
        if (transform.GetComponent<Renderer>().material.color != collided.transform.GetComponent<Renderer>().material.color) {
            Debug.Log("The colors are not the same! GroundColor: " + collided.transform.GetComponent<Renderer>().material.color + " PlayerColor: "
                + transform.GetComponent<Renderer>().material.color);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Water")) {
            WashColor(collision.gameObject);
        } else if (collision.collider.CompareTag("Ground")) {
            CheckGroundColor(collision.gameObject);
        } else if (collision.collider.CompareTag("ColorChanger")) {
            SetColor(Colors.None, transform.gameObject, collision);
        }
    }
}
