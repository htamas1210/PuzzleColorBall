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
    [SerializeField] GameObject portalModul;

    [SerializeField] private int groundCounter = 0;
    [SerializeField] private int portalSpawnNumber = 15; //ennyi modulonkent spawnoljon portalt

    //private CollectibleSpawner cs;

    private Material newMaterial;

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

        newMaterial = materials[0]; //correct color
        Debug.Log(newMaterial.color);
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
        {   //ground objecteket mozgatja
            Move(ground[i]);
        }

        for (int i = 0; i < sideObjectsSpawned.Length; i++)
        {
            //sideObjectsSpawned[i].transform.position = sideObjectsSpawned[i].transform.position + new Vector3(0,0, -groundMoveSpeed * Time.deltaTime);
            Move(sideObjectsSpawned[i]);
        }

        if (sideObjectsSpawned[sideObjectsSpawned.Length - 1].transform.position.z < 145)
        {
            CreateNewSideObjects(false);
            CreateNewSideObjects(true);
        }

        //uj ground letrehozas 
        if (ground[ground.Length - 1].transform.position.z <= 120)
        {
            if (groundCounter == portalSpawnNumber)
            {
                CreateNewGround(true);
                groundCounter = 0; //ne menjen a vegtelensegig a counter
            }
            else
            {
                CreateNewGround();
            }

            ground = GameObject.FindGameObjectsWithTag("Ground");

            ModuleColorChange();
        }

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
                side.Add(item); //bal oldal
            }
            else if (item.transform.position.x > 0 && !isLeftSide)
            {
                side.Add(item); //jobb oldal
            }
        }

        int random = UnityEngine.Random.Range(0, sideObjects.Length); //random sorsolasa a modulhoz
        //random = 0; //csak debug

        GameObject inst = sideObjects[random]; //random modul object eltarolas

        //remake to get width
        Vector3 offset = new Vector3(0, 0, 0);

        if (side.Count > 0)
        {
            if (side[side.Count - 1].gameObject.name.Contains("haz1")) //haz1Clone
                offset = new Vector3(0, 0, 15f); //TODO adjust
            else if (side[side.Count - 1].gameObject.name.Contains("haz2"))
                offset = new Vector3(0, 0, 20f); //TODO adjust
        }
        //

        Vector3 pos = new Vector3(9f, -5f, -10f);
        Quaternion rotation = inst.transform.rotation;

        if (side.Count > 0)
            //pos = sideObjectsSpawned[sideObjectsSpawned.Length - 1].transform.position + offset;
            pos = side[side.Count - 1].transform.position + offset;
        else
            pos = pos + offset;


        if (isLeftSide && pos.x > 0) //x negativ hogy a bal oldalra keruljon
            pos.x = -pos.x;


        Instantiate(inst, pos, rotation);

        sideObjectsSpawned = GameObject.FindGameObjectsWithTag("SideObject");
        OrderArrayByZ(sideObjectsSpawned);
    }

    public void ModuleColorChange()
    {
        ground = GameObject.FindGameObjectsWithTag("Ground");
        
        for (int i = 0; i < ground.Length; i++)
        {
            Transform[] lanes = new Transform[3];
            lanes[0] = ground[i].transform.Find("Lane1");
            lanes[1] = ground[i].transform.Find("Lane2");
            lanes[2] = ground[i].transform.Find("Lane3");

            foreach (var item in lanes)
            {
                item.GetComponent<MeshRenderer>().material = newMaterial;
            }
        }
    }

    public void changeMaterialIndex()
    {
        int materialteszt;
        bool teszteljtovabb = true;

        while (teszteljtovabb)
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

    public void ChangeMaterial(Material mat)
    {
        newMaterial = mat;
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

    private void CreateNewGround(bool portalModulSpawn = false)
    {
        int random = UnityEngine.Random.Range(0, loadFrom.Length);

        GameObject inst;

        if (!portalModulSpawn)
            inst = loadFrom[random];
        else
            inst = portalModul;


        //egy modullal elobb tolt be, annak az iranyanak megfeleloen, +80 a ket modul hossza
        Instantiate(inst, new Vector3(ground[ground.Length - 1].transform.position.x, ground[ground.Length - 1].transform.position.y, ground[ground.Length - 1].transform.position.z + 40), ground[ground.Length - 1].transform.rotation);
        groundCounter++;
    }
}