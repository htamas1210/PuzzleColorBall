using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreTable : MonoBehaviour
{
    public Transform entryContainer; //A helyezeseket tartalmazo helyhez referencia
    public Transform entryTemplate; //Egy helyezeshez egy template (tartalmazza a helyezest, nevet, pontszamot, es idot), ebbol letrehozott masolatok lesznek az entryContainerbe

    private DatabaseData dbData; //Database osztalyhoz referencia
    private HighScoreTableDataContainer htdc; //A tombot tarolo osztalyhoz referencia

    private void Start() {
        dbData.GetHighScoreData();
    }

    public void CreateTable(HighScoreTableData[] htd){
        foreach(HighScoreTableData item in htd){
            item.kiir(); //konzolra Debug miatt kiirja az adatokat
        }

        entryTemplate.gameObject.SetActive(false); //Objektum kikapcsolasa hogy ne legyen lathato

        float templateHight = 35f; //Egy sor magassaga

        for (int i = 0; i < htd.Length; i++){ //minden adaton vegigmegyunk a tombben
            Transform entryTransform = Instantiate(entryTemplate, entryContainer); //uj sor letrehozasa az entryContainerben
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            RectTransform entryContainerRect = entryContainer.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHight * i + (entryContainerRect.rect.height / 2) - 20); //A sor poziciojanak beallitasa x,y koordinataval
            entryTransform.gameObject.SetActive(true); //Objektum megjelenitese


            //a helyezes szoveg letrehozasa 1.: 1st, 2.: 2nd, 3.: 3rd, 4.: 4th...
            int rank = i+1;
            string rankString;

            switch(rank){
                default: rankString = rank + "th"; break;
                case 1: rankString = rank + "st"; break;
                case 2: rankString = rank + "nd"; break;
                case 3: rankString = rank + "rd"; break;
            }

            //a tombben levo adatok beirasa
            entryTransform.Find("posText").GetComponent<TMP_Text>().text = rankString;
            entryTransform.Find("nameText").GetComponent<TMP_Text>().text = htd[i].player_name;
            entryTransform.Find("scoreText").GetComponent<TMP_Text>().text = htd[i].score_points.ToString();
            entryTransform.Find("timeText").GetComponent<TMP_Text>().text = htd[i].score_time;

        }
    }



    //////////

        /*private void Awake() {
        dbData = FindObjectOfType<DatabaseData>();
        dbData.GetHighScoreData(1);    
        htdc = dbData.htdcReturn();
    }

    private void Start() {
        foreach(HighScoreTableData htd in htdc.htd){
            htd.kiir();
        }

        entryTemplate.gameObject.SetActive(false);

        float templateHight = 35f;

        for (int i = 0; i < 10; i++){ //for size of player list or highscore data lekerdezes meret
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            RectTransform entryContainerRect = entryContainer.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHight * i + (entryContainerRect.rect.height / 2) - 20);
            entryTransform.gameObject.SetActive(true);

            int rank = i+1;
            string rankString;

            switch(rank){
                default: rankString = rank + "th"; break;
                case 1: rankString = rank + "st"; break;
                case 2: rankString = rank + "nd"; break;
                case 3: rankString = rank + "rd"; break;
            }

            entryTransform.Find("posText").GetComponent<TMP_Text>().text = rankString;
            entryTransform.Find("nameText").GetComponent<TMP_Text>().text = "";
            //entryTransform.Find("scoreText").GetComponent<TMP_Text>().text = "";
            //entryTransform.Find("timeText").GetComponent<TMP_Text>().text = "";

        }
    }*/
}
