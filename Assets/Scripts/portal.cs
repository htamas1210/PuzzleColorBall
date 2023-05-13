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
    }

    private void OnTriggerEnter(Collider other)
    {
        gc.changeMaterialIndex();
    }

}
