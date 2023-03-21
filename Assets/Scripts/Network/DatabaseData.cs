using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class DatabaseData : MonoBehaviour
{
    //public TMP_InputField input; //szoveg megjelenitese
    public PlayerList players; //jatekos adatok
    public HighScoreTableDataContainer htdc; //itt van a tomb
    public HighScoreTable hst; //high score table ui

    private StreamWriter writer;
    public string jsondata; //json szoveg

    private CoinCounter coinc;

    public ulong coins = 0;

    private void Awake() {
        hst = FindObjectOfType<HighScoreTable>(); //High Score Table referencia
        htdc = new HighScoreTableDataContainer(); //High Score Table Container objektum
        
        coinc = FindObjectOfType<CoinCounter>();
        //writer = new StreamWriter(Application.persistentDataPath + "/coins.txt", false, Encoding.Default);
    }

    private void Start() {
        GetHighScoreData();
        //StartCoroutine(GetCoinData(1));
    }

    public void jsonParser(string jsondata) { //beerkezo json adat eltarolasa
        players = new PlayerList(); //jatekos lista osztaly || lehet okozza a hibat!!
        players = JsonUtility.FromJson<PlayerList>("{\"player\":" + jsondata + "}");       
    }

    public void jsonParserHighScore(string jsondata) { //beerkezo json adat eltarolasa
        htdc = JsonUtility.FromJson<HighScoreTableDataContainer>("{\"htd\":" + jsondata + "}");       
    }

    public void ParserCoin(string jsondata){
        string data = "";
        for (int i = 0; i < jsondata.Length; i++)
        {
            if(jsondata[i] == '0' || jsondata[i] == '1' || jsondata[i] == '2' || jsondata[i] == '3' || jsondata[i] == '4' || jsondata[i] == '5' || jsondata[i] == '6' || jsondata[i] == '7' || jsondata[i] == '8' || jsondata[i] == '9'){
                data += jsondata[i]; //kiszedi a szamokat a stringbol
            }
        }
        Debug.Log(data);
        coins = ulong.Parse(data);
    }

    //fuggvenyek amik meghivjak a rutint
    public void GetPlayerData() => StartCoroutine(IGetPlayerData());
    public void GetHighScoreData() => StartCoroutine(IGetHighScoreData());
    public void PostNewPlayerData() => StartCoroutine(IPostNewPlayerData());
    public void PostNewScoreData(int playerid, ulong score, string time) => StartCoroutine(IPostNewScoreData(playerid, score, time));
    public void PostNewPalyaData() => StartCoroutine(IPostNewPalyaData());

    public ulong GetCoins(int userid){
        StartCoroutine(GetCoinData(userid));

        return coins;
    }

    private IEnumerator GetCoinData(int userid){
        string uri = "http://localhost:3000/coinget";

        var uwr = new UnityWebRequest(uri, "GET");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":"+userid+"}");
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend); 
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            jsondata = uwr.downloadHandler.text; //json szoveg eltarolasa
            Debug.Log(jsondata);
            ParserCoin(jsondata);
        }
    }


    private IEnumerator IGetPlayerData() {
        //input.text = "Loading..."; //ideiglenes szoveg amig nem jelenik meg az adat szoveg

        string uri = "http://localhost:3000/player"; //backend vegpont linkje

        using (UnityWebRequest request = UnityWebRequest.Get(uri)) { //uj webrequest objektum letrehozasa, aminek megadjuk hogy ez egy get-es lekerdezes
            yield return request.SendWebRequest(); //amig be nem fejezodik az fv ide fog visszaterni

            if(request.isNetworkError || request.isHttpError) { //ha valami hiba tortent kiirjuk a kepernyore
                //input.text = request.error;
            } else {
                jsondata = request.downloadHandler.text; //json szoveg eltarolasa
                jsonParser(jsondata); //json adat atalakitasa 
                foreach(var p in players.player) {
                    p.ConvertDate(); //datum eltarolasa es atalakitasa datum tipussa
                    Debug.Log("p_id: " + p.player_id + " username: " + p.player_name + " join date: " + p.joindate.printDate() + "\n");
                }

                //input.text = "";
                foreach(var p in players.player) {
                    p.ConvertDate();
                    //adatok kiirasa kepernyore
                    //input.text += "p_id: " + p.player_id + " username: " + p.player_name + " join date: " + p.joindate.printDate() + "\n";
                }
            }
        }
    }



    private IEnumerator IGetCurretPlayer(int userid){
        string uri = "http://localhost:3000/currentplayer";

        var uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":"+userid+"}"); //palya id megadasa
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend); //felkuldi a palya id-t
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            jsondata = uwr.downloadHandler.text; //json szoveg eltarolasa
            Debug.Log("current player json: " + jsondata);
            jsonParser(jsondata); //json adat atalakitasa 
            foreach(var p in players.player) {
                p.ConvertDate(); //datum eltarolasa es atalakitasa datum tipussa
                Debug.Log("p_id: " + p.player_id + " username: " + p.player_name + " join date: " + p.joindate.printDate() + "\n");
            }
        }
    }

    private IEnumerator IGetHighScoreData(){
        string uri = "http://localhost:3000/toplist";

        var uwr = new UnityWebRequest(uri, "GET");
        //byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":"+palya_id+"}"); //palya id megadasa
        //uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend); //felkuldi a palya id-t
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            //input.text = uwr.downloadHandler.text;
            Debug.Log(uwr.downloadHandler.text);
            jsonParserHighScore(uwr.downloadHandler.text);
            if(SceneManager.GetActiveScene().name.Equals("HighScore"))
                hst.CreateTable(htdc.htd); //high score tabla letrehozasa
        }
    }


    private IEnumerator IGetHighScoreDataNew(){
        string uri = "http://localhost:3000/toplist";

        var uwr = new UnityWebRequest(uri, "GET");
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            //input.text = uwr.downloadHandler.text;
            Debug.Log(uwr.downloadHandler.text);
            jsonParserHighScore(uwr.downloadHandler.text);
            hst.CreateTable(htdc.htd); //high score tabla letrehozasa
        }
    }



    private IEnumerator IGetPlayerCoins(string username){
        string uri = "http://localhost:3000/coinget";

        var uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":"+username+"}"); //palya id megadasa
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend); //felkuldi a palya id-t
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            Debug.Log(uwr.downloadHandler.text);
            coinc.coin = ulong.Parse(uwr.downloadHandler.text);
        }
    }



    private IEnumerator IPostNewPlayerData() {
        //input.text = "loading...";

        string uri = "http://localhost:3000/newplayer";

        var uwr = new UnityWebRequest(uri, "POST"); //post beallitasa
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{ \"bevitel1\":\"postusername\"}"); //felviteli json
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            //input.text = uwr.downloadHandler.text; //vissza erzkezes arrol hogy sikeres a felvitel vagy nem
            Debug.Log(uwr.downloadHandler.text);           
        }
    }

    private IEnumerator IPostNewScoreData(int playerid, ulong score, string time) {
        //input.text = "loading...";

        string uri = "http://localhost:3000/newscore";

        //felhasznalonevet ki kell irni fajlba
        //ha username keresve lenne es ha nincs akkor kap egy uj id-t ami ideiglenesen tarolodik (ures string a visszateres?)
        //ha van akkor akkor lekeri az id-t es ideiglenesen tarolja


        var uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = 
        new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":"+playerid+",\"bevitel2\":"+score+",\"bevitel3\":\""+time+"\"}");
        //playerid, points, time
        
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            //input.text = uwr.downloadHandler.text;
            Debug.Log(uwr.downloadHandler.text);
        }
    }

    private IEnumerator IPostNewPalyaData() {
        //input.text = "loading...";

        string uri = "http://localhost:3000/newpalya";

        var uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{ \"bevitel1\":\"Easy3\"}");
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            //input.text = uwr.downloadHandler.text;
            Debug.Log(uwr.downloadHandler.text);
        }
    }
}