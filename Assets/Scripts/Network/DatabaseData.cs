using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class DatabaseData : MonoBehaviour
{
    public TMP_InputField input; //szoveg megjelenitese
    public PlayerList players; //jatekos adatok
    public HighScoreTableDataContainer htdc; //itt van a tomb
    public HighScoreTable hst; //high score table ui
    public string jsondata; //json szoveg

    private CoinCounter coinc;

    private void Awake() {
        hst = FindObjectOfType<HighScoreTable>(); //High Score Table referencia
        htdc = new HighScoreTableDataContainer(); //High Score Table Container objektum
        players = new PlayerList(); //jatekos lista osztaly
        coinc = FindObjectOfType<CoinCounter>();
    }

    private void Start() {
        GetHighScoreData(2);
    }

    public void jsonParser(string jsondata) { //beerkezo json adat eltarolasa
        players = JsonUtility.FromJson<PlayerList>("{\"player\":" + jsondata + "}");       
    }

    public void jsonParserHighScore(string jsondata) { //beerkezo json adat eltarolasa
        htdc = JsonUtility.FromJson<HighScoreTableDataContainer>("{\"htd\":" + jsondata + "}");       
    }

    //fuggvenyek amik meghivjak a rutint
    public void GetPlayerData() => StartCoroutine(IGetPlayerData());
    public void GetHighScoreData(int palya_id) => StartCoroutine(IGetHighScoreData(palya_id));
    public void PostNewPlayerData() => StartCoroutine(IPostNewPlayerData());
    public void PostNewScoreData() => StartCoroutine(IPostNewScoreData());
    public void PostNewPalyaData() => StartCoroutine(IPostNewPalyaData());

    public void GetCoinData(string username) => StartCoroutine(IGetPlayerCoins(username));

    private IEnumerator IGetPlayerData() {
        input.text = "Loading..."; //ideiglenes szoveg amig nem jelenik meg az adat szoveg

        string uri = "http://localhost:3000/player"; //backend vegpont linkje

        using (UnityWebRequest request = UnityWebRequest.Get(uri)) { //uj webrequest objektum letrehozasa, aminek megadjuk hogy ez egy get-es lekerdezes
            yield return request.SendWebRequest(); //amig be nem fejezodik az fv ide fog visszaterni

            if(request.isNetworkError || request.isHttpError) { //ha valami hiba tortent kiirjuk a kepernyore
                input.text = request.error;
            } else {
                jsondata = request.downloadHandler.text; //json szoveg eltarolasa
                jsonParser(jsondata); //json adat atalakitasa 
                foreach(var p in players.player) {
                    p.ConvertDate(); //datum eltarolasa es atalakitasa datum tipussa
                    Debug.Log("p_id: " + p.player_id + " username: " + p.player_name + " join date: " + p.joindate.printDate() + "\n");
                }

                input.text = "";
                foreach(var p in players.player) {
                    p.ConvertDate();
                    //adatok kiirasa kepernyore
                    input.text += "p_id: " + p.player_id + " username: " + p.player_name + " join date: " + p.joindate.printDate() + "\n";
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

    private IEnumerator IGetHighScoreData(int palya_id){
        string uri = "http://localhost:3000/toplist";

        var uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":"+palya_id+"}"); //palya id megadasa
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend); //felkuldi a palya id-t
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            input.text = uwr.downloadHandler.text;
            Debug.Log(uwr.downloadHandler.text);
            jsonParserHighScore(uwr.downloadHandler.text);
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
            input.text = uwr.downloadHandler.text;
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
        input.text = "loading...";

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
            input.text = uwr.downloadHandler.text; //vissza erzkezes arrol hogy sikeres a felvitel vagy nem
            Debug.Log(uwr.downloadHandler.text);           
        }
    }

    private IEnumerator IPostNewScoreData() {
        input.text = "loading...";

        string uri = "http://localhost:3000/newscore";

        var uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = 
        new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":2,\"bevitel2\":1,\"bevitel3\":400,\"bevitel4\":\"00:05:06\"}");
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            input.text = uwr.downloadHandler.text;
            Debug.Log(uwr.downloadHandler.text);
        }
    }

    private IEnumerator IPostNewPalyaData() {
        input.text = "loading...";

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
            input.text = uwr.downloadHandler.text;
            Debug.Log(uwr.downloadHandler.text);
        }
    }
}