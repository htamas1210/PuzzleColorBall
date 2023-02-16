using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player { 
    public int player_id; //jatkos id-ja
    public string player_name; //jatekos neve
    public Date joindate; //elso csatlakozasi datuma
    public string player_join_date; 

    public void ConvertDate() { //mindig meg kell hivni
        joindate = new Date(player_join_date);
    }
}