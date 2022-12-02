using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerList
{
    public Player[] player;
    
    public void kiir() {
        foreach(Player p in player) {
            Debug.Log("p_id: " + p.player_id + " username" + p.player_join_date);
        }
    }
}
