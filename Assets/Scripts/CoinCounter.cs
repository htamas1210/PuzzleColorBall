using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public ulong coin = 0; //szedje le db-bol a playerhez a coint

    private DatabaseData dd;

    //get player coin at startup based on username
    private void Awake() {
        dd = FindObjectOfType<DatabaseData>();

        //dd.GetCoinData("Playmaker1210");
    }

    public void AddCoin(ulong number){
        coin += number;
    }

    public void RemoveCoin(ulong number){
        coin -= number;
    }
}
