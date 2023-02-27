using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public ulong coin = 0;

    public void AddCoin(ulong number){
        coin += number;
    }

    public void RemoveCoin(ulong number){
        coin -= number;
    }
}
