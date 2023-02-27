using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    private CoinCounter counter;

    private void Awake() {
        counter = FindObjectOfType<CoinCounter>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            counter.AddCoin(1);
            Destroy(gameObject);
        }
    }
}
