using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScoreTableData { 
    public string player_name;
    public int score_points;
    public string score_time;

    public void kiir(){
        Debug.Log("kiir name: " + player_name + " points: " + score_points + " time: " + score_time);
    }
}


