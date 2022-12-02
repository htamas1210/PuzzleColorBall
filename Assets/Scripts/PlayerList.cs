using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerList
{
    public Player[] players;
    
    public void kiir() {
        foreach(Player p in players) {
            Debug.Log("p_id: " + p.playerid + " username" + p.username);
        }
    }
}
