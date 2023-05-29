using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    private GroundController gc;
    // Start is called before the first frame update
    void Awake()
    {
        gc = FindObjectOfType<GroundController>();

        

        gameObject.GetComponent<MeshRenderer>().material = gc.materials[gc.materialIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player")){
            gc.changeMaterialIndex();
        }
    }

}
