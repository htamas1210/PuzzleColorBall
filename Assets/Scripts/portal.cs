using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    private GroundController gc;
    private int random;
    
    void Awake()
    {
        gc = FindObjectOfType<GroundController>();

        random = UnityEngine.Random.Range(0, gc.materials.Length);

        gameObject.GetComponent<MeshRenderer>().material = gc.materials[random];
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player")){
            gc.ChangeMaterial(gc.materials[random]);
            gc.ModuleColorChange();
        }
    }

}
