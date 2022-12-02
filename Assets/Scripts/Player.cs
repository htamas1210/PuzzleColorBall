using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player { 
    public int player_id;
    public string player_name;
    public Date joindate;
    public string player_join_date;

    public void ConvertDate() { //mindig meg kell hivni
        joindate = new Date(player_join_date);
    }
}


