using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    private GameObject[] ground;
    public GameObject[] loadFrom;
    private float groundMoveSpeed = 10f;


    private void Awake() {
        LoadPrefabs("Prefabs/Modulok");
        Debug.Log(loadFrom.Length);
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
        OrderGroundArrayByZ(); //rendezzuk z szerint hogy sorba legyenek

        for (int i = 0; i < ground.Length; i++){ //ground objecteket mozgatja
            ground[i].transform.position = ground[i].transform.position + new Vector3(0,0, -groundMoveSpeed * Time.deltaTime);
        }     

        //uj ground letrehozas 
        if(ground[ground.Length-1].transform.position.z <= 80){
            CreateNewGround();
        }       
        
        //ellenorzi hogy torolheto e az object
        foreach (var item in ground){
            if(CheckGroundToDestroy(item)){
                Destroy(item);
            }
        }
    }

    private void LoadPrefabs(string path){ //toltese be a palya objecteket a resources mappabol pl: "Prefabs/Modulok" 
        loadFrom = Resources.LoadAll<GameObject>(path);
    }

    private bool CheckGroundToDestroy(GameObject toCheck){
        //z = -80 -nal lehet torolni
        if(toCheck.transform.position.z <= -80){
            Debug.Log("elerte " + toCheck.name);
            return true; //torolheto
        }

        return false; //nem torolheto
    }

    private void OrderGroundArrayByZ(){
        GameObject csere;
        for (int i = 0; i < ground.Length; i++){
            for(int j = 0; j < i; j++){
                if(ground[j].transform.position.z > ground[j+1].transform.position.z){
                    csere = ground[j];
                    ground[j] = ground[j+1];
                    ground[j+1] = csere;
                }
            }
        }
    }

    private void CreateNewGround(){
        int random = UnityEngine.Random.Range(0, loadFrom.Length);
        //ketto modullal elobb tolt be, annak az iranyanak megfeleloen, +80 a ket modull hossza
        Instantiate(loadFrom[random], new Vector3(0,0, ground[ground.Length-2].transform.position.z + 80), ground[ground.Length-2].transform.rotation);
    }


}
