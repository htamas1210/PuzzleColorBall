using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    private GameObject[] ground;
    public GameObject[] loadFrom;
    public GameObject[] sideObjects;
    public GameObject[] sideObjectsSpawned;
    public Material[] materials;
    public int materialIndex = 0;
    public float groundMoveSpeed = 10f;
    private Vector3 lastSideObjectPos = new Vector3(0,0,0);

    //private CollectibleSpawner cs;

    private void Awake() {
        //cs = FindObjectOfType<CollectibleSpawner>();

        //Loading modules
        loadFrom = LoadPrefabs("Prefabs/Modulok");
        sideObjects = LoadPrefabs("Prefabs/WorldObjects/World1/");
        Debug.Log("loadFrom length: " + loadFrom.Length);
        Debug.Log("sideObjects Length: " + sideObjects.Length);

        //getting all of the ground objects by the tag
        ground = GameObject.FindGameObjectsWithTag("Ground");
        if(ground.Length == 0){
            Debug.Log("Nem talalt ground objectet");
        }else{
            Debug.Log("ground length: " + ground.Length);
        }
    }

    private void Update() {
        ground = GameObject.FindGameObjectsWithTag("Ground"); //torles miatt ujra le kell kerni  a ground objecteket
        sideObjectsSpawned = GameObject.FindGameObjectsWithTag("SideObject");

        OrderArrayByZ(ground); //rendezzuk z szerint a talajt
        OrderArrayByZ(sideObjectsSpawned);  

        if(sideObjectsSpawned.Length > 0){
            lastSideObjectPos = sideObjectsSpawned[sideObjectsSpawned.Length-1].transform.position;
        }

        for (int i = 0; i < ground.Length; i++){ //ground objecteket mozgatja
            ground[i].transform.position = ground[i].transform.position + new Vector3(0,0, -groundMoveSpeed * Time.deltaTime);
        }  

        for(int i = 0; i < sideObjectsSpawned.Length; i++){
            sideObjectsSpawned[i].transform.position = sideObjectsSpawned[i].transform.position + new Vector3(0,0, -groundMoveSpeed * Time.deltaTime);
        }   

        //uj ground letrehozas 
        if(ground[ground.Length-1].transform.position.z <= 120){
            CreateNewGround();
            CreateNewSideObjects(false);
            
            ground = GameObject.FindGameObjectsWithTag("Ground");

            for(int i = 0; i < ground.Length; i++){
                /*foreach (GameObject child in ground[i].transform){
                    if (child.name == "Lane1" || child.name == "Lane2" || child.name == "Lane3"){
                        Debug.Log(child.name + " " + transform.gameObject.name);
                    }
                }*/
                Transform[] lanes = new Transform[3];
                lanes[0] = ground[i].transform.Find("Lane1");
                lanes[1] = ground[i].transform.Find("Lane2");
                lanes[2] = ground[i].transform.Find("Lane3");

                foreach(var item in lanes){
                    item.GetComponent<MeshRenderer>().material = materials[materialIndex];
                }

            }         
        }       
        
        //ellenorzi hogy torolheto e az object || mar nem szukseges mert van egy trigger box
        /*foreach (var item in ground){
            if(CheckGroundToDestroy(item)){
                Destroy(item);
            }
        }*/

        //cs.SpawnCoin();
    }

    private GameObject[] LoadPrefabs(string path){ //toltese be a palya objecteket a resources mappabol pl: "Prefabs/Modulok" 
        GameObject[] arr = Resources.LoadAll<GameObject>(path);

        return arr;
    }

    private void CreateNewSideObjects(bool isLeftSide){
        int random = UnityEngine.Random.Range(0, sideObjects.Length);

        GameObject inst = sideObjects[random];
        Vector3 pos = new Vector3(0,0,0);

        if(inst.name == "haz1" && !isLeftSide){
            pos = new Vector3(4,0,0); //check pos in editor TODO!!
        }else if(inst.name == "haz2"){
            pos = new Vector3(9,0,0);
        }

        if(isLeftSide) pos.x = -pos.x;

        Instantiate(inst, lastSideObjectPos + pos, ground[0].transform.rotation);
    }

    public void changeMaterialIndex(){
        
    }

    private bool CheckGroundToDestroy(GameObject toCheck){
        //z = -80 -nal lehet torolni
        if(toCheck.transform.position.z <= -80){
            Debug.Log("elerte " + toCheck.name);
            return true; //torolheto
        }

        return false; //nem torolheto
    }

    private void OrderArrayByZ(GameObject[] array){
        GameObject csere;
        for (int i = 0; i < array.Length; i++){
            for(int j = 0; j < i; j++){
                if(array[j].transform.position.z > array[j+1].transform.position.z){
                    csere = array[j];
                    array[j] = array[j+1];
                    array[j+1] = csere;
                }
            }
        }
    }

    private void CreateNewGround(){
        int random = UnityEngine.Random.Range(0, loadFrom.Length);
        //egy modullal elobb tolt be, annak az iranyanak megfeleloen, +80 a ket modul hossza
        Instantiate(loadFrom[random], new Vector3(0,0, ground[ground.Length-1].transform.position.z + 40), ground[ground.Length-1].transform.rotation);
    }
}