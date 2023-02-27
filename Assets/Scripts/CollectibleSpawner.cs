using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject coin;
    private GameObject[] spawnedCollectibles;
    private GroundController gc;

    public void SpawnCoin(){
        int x = UnityEngine.Random.Range(-3,3);
    
        if(x >= 1.5f){
            x = 3;
        }else if(x <= -1.5f){
            x = -3;
        }else{
            x = 0;
        }
        
        Instantiate(coin, new Vector3(x,1.5f,-7), coin.transform.rotation);
    }

     private IEnumerator RepeatSpawn(){
        yield return new WaitForSeconds(50000000.0f); // Initial Delay
        while(true)
        {
            SpawnCoin();
            yield return new WaitForSeconds(500000.0f);
        }
        yield return null;
    }

    private void Update() {
        //StartCoroutine(SpawnCoin());

        spawnedCollectibles = GameObject.FindGameObjectsWithTag("Collectible");

        foreach (var item in spawnedCollectibles)
        {
            item.transform.position += new Vector3(0,0, -gc.groundMoveSpeed * Time.deltaTime);
             
            if(item.transform.position.z <= -80){
               Destroy(item);
            }
        }
    }

    private void Start() {
        //InvokeRepeating("SpawnCoin", 20000f, 300000f);
        StartCoroutine(RepeatSpawn());
    }

    private void Awake() {
        gc = FindObjectOfType<GroundController>();
    }
}
