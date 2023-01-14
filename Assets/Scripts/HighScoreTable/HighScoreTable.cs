using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;

    private DatabaseData dbData;
    private HighScoreTableDataContainer htdc;

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

    public void CreateTable(HighScoreTableData[] htda){
        foreach(HighScoreTableData item in htda){
            item.kiir();
        }

        entryTemplate.gameObject.SetActive(false);

        float templateHight = 35f;

        for (int i = 0; i < htda.Length; i++){ //for size of player list or highscore data lekerdezes meret
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
            entryTransform.Find("nameText").GetComponent<TMP_Text>().text = htda[i].player_name;
            entryTransform.Find("scoreText").GetComponent<TMP_Text>().text = htda[i].score_points.ToString();
            entryTransform.Find("timeText").GetComponent<TMP_Text>().text = htda[i].score_time;

        }
    }
}
