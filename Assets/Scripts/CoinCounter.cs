using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public ulong coin = 0; //szedje le db-bol a playerhez a coint
    public TMP_Text coinCounterUI;

    private DatabaseData dd;

    //get player coin at startup based on username
    private void Awake() {
        dd = FindObjectOfType<DatabaseData>();

        //dd.GetCoinData("Playmaker1210");
    }

    public void AddCoin(ulong number){
        coin += number;
        coinCounterUI.text = "Coins: " + coin.ToString();
    }

    public void RemoveCoin(ulong number){
        coin -= number;
        //coinCounterUI.text = "Coins: " + coin.ToString();
    }
}
