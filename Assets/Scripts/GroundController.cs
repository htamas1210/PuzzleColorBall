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
    private Vector3 lastSideObjectPos = new Vector3(0, 0, 0);

    //private CollectibleSpawner cs;

    private void Awake()
    {
        //cs = FindObjectOfType<CollectibleSpawner>();

        //Loading modules
        loadFrom = LoadPrefabs("Prefabs/Modulok");
        sideObjects = LoadPrefabs("Prefabs/WorldObjects/World1/");
        Debug.Log("loadFrom length: " + loadFrom.Length);
        Debug.Log("sideObjects Length: " + sideObjects.Length);

        //getting all of the ground objects by the tag
        ground = GameObject.FindGameObjectsWithTag("Ground");
        if (ground.Length == 0)
        {
            Debug.Log("Nem talalt ground objectet");
        }
        else
        {
            Debug.Log("ground length: " + ground.Length);
        }
    }

    private void Move(GameObject move)
    {
        move.transform.position = move.transform.position + new Vector3(0, 0, -groundMoveSpeed * Time.deltaTime);
    }

    private void Update()
    {
        ground = GameObject.FindGameObjectsWithTag("Ground"); //torles miatt ujra le kell kerni  a ground objecteket
        sideObjectsSpawned = GameObject.FindGameObjectsWithTag("SideObject");

        OrderArrayByZ(ground); //rendezzuk z szerint a talajt
        OrderArrayByZ(sideObjectsSpawned);

        for (int i = 0; i < ground.Length; i++)
        { //ground objecteket mozgatja
            //ground[i].transform.position = ground[i].transform.position + new Vector3(0,0, -groundMoveSpeed * Time.deltaTime);
            Move(ground[i]);
        }

        for (int i = 0; i < sideObjectsSpawned.Length; i++)
        {
            //sideObjectsSpawned[i].transform.position = sideObjectsSpawned[i].transform.position + new Vector3(0,0, -groundMoveSpeed * Time.deltaTime);
            Move(sideObjectsSpawned[i]);
        }

        //uj ground letrehozas 
        if (ground[ground.Length - 1].transform.position.z <= 120)
        {
            CreateNewGround();

            for (int k = 0; k < 3; k++)
            {
                CreateNewSideObjects(false);
                CreateNewSideObjects(true);
            }

            ground = GameObject.FindGameObjectsWithTag("Ground");

            for (int i = 0; i < ground.Length; i++)
            {
                /*foreach (GameObject child in ground[i].transform){
                    if (child.name == "Lane1" || child.name == "Lane2" || child.name == "Lane3"){
                        Debug.Log(child.name + " " + transform.gameObject.name);
                    }
                }*/
                Transform[] lanes = new Transform[3];
                lanes[0] = ground[i].transform.Find("Lane1");
                lanes[1] = ground[i].transform.Find("Lane2");
                lanes[2] = ground[i].transform.Find("Lane3");

                foreach (var item in lanes)
                {
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

    private GameObject[] LoadPrefabs(string path)
    { //toltese be a palya objecteket a resources mappabol pl: "Prefabs/Modulok" 
        GameObject[] arr = Resources.LoadAll<GameObject>(path);

        return arr;
    }

    private void CreateNewSideObjects(bool isLeftSide)
    {
        List<GameObject> side = new List<GameObject>(); //eltarolja egy oldal sideObjectjeit, hogy a jo oldalhoz nezze a kovetkezo elemet;

        foreach (var item in sideObjectsSpawned)
        {
            if (item.transform.position.x < 0 && isLeftSide)
            {
                side.Add(item); //ball oldal
            }
            else
            {
                side.Add(item); //jobb oldal
            }
        }

        int random = UnityEngine.Random.Range(0, sideObjects.Length);
        random = 0; //csak debug

        GameObject inst = sideObjects[random];

        //remake to get width
        Vector3 offset = new Vector3(0, 0, 10f);

        if (sideObjectsSpawned.Length > 0)
        {
            if (sideObjectsSpawned[sideObjectsSpawned.Length - 1].gameObject.name.Contains("haz1")) //haz1Clone
                offset = new Vector3(0, 0, 10f); //TODO adjust
            else if (sideObjectsSpawned[sideObjectsSpawned.Length - 1].gameObject.name.Contains("haz2"))
                offset = new Vector3(0, 0, 20f); //TODO adjust
        }
        //

        Vector3 pos = new Vector3(9f, 0, 0);

        if (sideObjectsSpawned.Length > 0)
            pos = sideObjectsSpawned[sideObjectsSpawned.Length - 1].transform.position + offset;
        else
            pos = pos + offset;

        if (isLeftSide) pos.x = -pos.x;

        Instantiate(inst, pos, inst.transform.rotation);

        sideObjectsSpawned = GameObject.FindGameObjectsWithTag("SideObject");
        OrderArrayByZ(sideObjectsSpawned);
    }

    public void changeMaterialIndex()
    {
        int materialteszt;
        bool teszteljtovabb = true;

        while (teszteljtovabb == true)
        {
            materialteszt = UnityEngine.Random.Range(0, materials.Length);
            Debug.Log(materialteszt);
            if (materialteszt == materialIndex)
            {

            }
            else
            {
                materialIndex = materialteszt;
                teszteljtovabb = false;
            }
        }

        teszteljtovabb = true;
    }

    private bool CheckGroundToDestroy(GameObject toCheck)
    {
        //z = -80 -nal lehet torolni
        if (toCheck.transform.position.z <= -80)
        {
            Debug.Log("elerte " + toCheck.name);
            return true; //torolheto
        }

        return false; //nem torolheto
    }

    private void OrderArrayByZ(GameObject[] array)
    {
        GameObject csere;
        for (int i = 0; i < array.Length; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (array[j].transform.position.z > array[j + 1].transform.position.z)
                {
                    csere = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = csere;
                }
            }
        }
    }

    private void CreateNewGround()
    {
        int random = UnityEngine.Random.Range(0, loadFrom.Length);
        //egy modullal elobb tolt be, annak az iranyanak megfeleloen, +80 a ket modul hossza
        Instantiate(loadFrom[random], new Vector3(0, 0, ground[ground.Length - 1].transform.position.z + 40), ground[ground.Length - 1].transform.rotation);
    }
}