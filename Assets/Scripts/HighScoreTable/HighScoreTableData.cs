using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScoreTableData { 
    public string player_name; //Jatekos neve
    public int score_points; //A jatekos altal elert pontszam a palyan 
    public string score_time; //A jatekos mennyi idot toltott a palyan

    public void kiir(){
        Debug.Log("kiir name: " + player_name + " points: " + score_points + " time: " + score_time);
    }
}