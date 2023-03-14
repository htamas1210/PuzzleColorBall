using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Text;

public class CoinCounter : MonoBehaviour
{
    public ulong coin; //szedje le db-bol a playerhez a coint
    public TMP_Text coinCounterUI;
    private string path;
    private StreamWriter writer;
    private StreamReader reader;

    private void Awake() {  
        path = Application.persistentDataPath + "/coins.txt";

        if(!File.Exists(path)){ //ha nincs meg ilyen fajl hozza letre
            writer = new StreamWriter(path, false, Encoding.Default);
            writer.Write(0);
            writer.Close();
        }

        reader = new StreamReader(path);
        coin = ulong.Parse(reader.ReadLine());
        reader.Close();
        Debug.Log("Coins at start: " + coin);

        coinCounterUI.text = "Coins: " + coin.ToString();
    }

    public void AddCoin(ulong number){
        coin += number;
        coinCounterUI.text = "Coins: " + coin.ToString();
       
        writer = new StreamWriter(path, false, Encoding.Default);
        writer.Write(coin);
        writer.Close();
    }

    public void RemoveCoin(ulong number){
        coin -= number;
        //coinCounterUI.text = "Coins: " + coin.ToString();
        
        writer = new StreamWriter(path, false, Encoding.Default);
        writer.Write(coin);
        writer.Close();
    }
}
