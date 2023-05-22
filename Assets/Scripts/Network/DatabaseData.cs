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
    private CoinCounter coinc;

    public string jsondata; //json szoveg
    public ulong coins = 0;

    private UnityWebRequest uwr;
    
    private bool forceLocalUrl = true;
    private const int PORT = 24002;

    #if UNITY_EDITOR || UNITY_EDITOR_64
        private string url = "localhost:"  + PORT.ToString();
    #else
        private string url = "nodejs.dszcbaross.edu.hu:" + PORT.ToString();
    #endif

    public UnityWebRequest GetUnityWebRequest(){ return uwr; }

    private void Awake() {
        hst = FindObjectOfType<HighScoreTable>(); //High Score Table referencia
        htdc = new HighScoreTableDataContainer(); //High Score Table Container objektum   
        coinc = FindObjectOfType<CoinCounter>();

        //writer = new StreamWriter(Application.persistentDataPath + "/coins.txt", false, Encoding.Default);

        /*if((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.OSXEditor) && forceLocalUrl){
            //ha az editorba van
            url = "http://localhost:"  + PORT.ToString();
        }else if(!forceLocalUrl){
            url = "nodejs.dszcbaross.edu.hu:" + PORT.ToString();
        }else{
            url = "nodejs.dszcbaross.edu.hu:" + PORT.ToString();
        }*/

        //url = "nodejs.dszcbaross.edu.hu:" + PORT.ToString();
        Debug.Log("<color=pink>url: </color>" + url);
    }

    private void Start() {
        GetHighScoreData(); //highscore scenehez
    }

    public void jsonParser(string jsondata) { //beerkezo json adat eltarolasa
        players = new PlayerList(); //jatekos lista osztaly || lehet okozza a hibat!!
        players = JsonUtility.FromJson<PlayerList>("{\"player\":" + jsondata + "}");       
    }

    public void jsonParserHighScore(string jsondata) { //beerkezo json adat eltarolasa
        htdc = JsonUtility.FromJson<HighScoreTableDataContainer>("{\"htd\":" + jsondata + "}");       
    }

    public void ParserCoin(string jsondata){
        //Debug.Log("<color=orange>JsonDataCoin:" + jsondata + "</color>");
        string data = "";
        for (int i = 0; i < jsondata.Length; i++)
        {
            if(jsondata[i] == '0' || jsondata[i] == '1' || jsondata[i] == '2' || jsondata[i] == '3' || jsondata[i] == '4' || jsondata[i] == '5' || jsondata[i] == '6' || jsondata[i] == '7' || jsondata[i] == '8' || jsondata[i] == '9'){
                data += jsondata[i]; //kiszedi a szamokat a stringbol
            }
        }
        Debug.Log("data coin: " + data);
        coins = ulong.Parse(data);
        Debug.Log("<color=orange>db coins: " + coins + "</color>");
        coinc.SetCoin(coins);
    }

    //fuggvenyek amik meghivjak a rutint
    public void GetPlayerData() => StartCoroutine(IGetPlayerData());
    public void GetHighScoreData() => StartCoroutine(IGetHighScoreData());
    public void PostNewPlayerData(string postusername) => StartCoroutine(IPostNewPlayerData(postusername));
    public void PostNewScoreData(int playerid, ulong score, string time) => StartCoroutine(IPostNewScoreData(playerid, score, time));
    public void PostNewPalyaData() => StartCoroutine(IPostNewPalyaData());

    public void GetCoinDataCall(int userid) => StartCoroutine(GetCoinData(userid));
    public void PostUpdateCoinData(ulong coins, int userid) => StartCoroutine(IPostUpdateCoinData(coins, userid));
    public void PostNewCoinData(ulong coins, int userid) => StartCoroutine(IPostNewCoinData(coins, userid));

    public ulong GetCoins(int userid){
        StartCoroutine(GetCoinData(userid));

        Debug.Log("<color=orange> Return coin: " + coins + "</color>");

        return coins;
    }

    private IEnumerator GetCoinData(int userid){
        string uri = url + "/coinget";

        uwr = new UnityWebRequest(uri, "GET");
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

        string uri = url + "/player"; //backend vegpont linkje

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
                /*foreach(var p in players.player) {
                    p.ConvertDate();
                    //adatok kiirasa kepernyore
                    //input.text += "p_id: " + p.player_id + " username: " + p.player_name + " join date: " + p.joindate.printDate() + "\n";
                }*/
            }
        }
    }



    private IEnumerator IGetCurretPlayer(int userid){
        string uri = url + "/currentplayer";

        uwr = new UnityWebRequest(uri, "POST");
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
        string uri = url + "/toplist";

        uwr = new UnityWebRequest(uri, "GET");
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
        string uri = url + "/toplist";

        uwr = new UnityWebRequest(uri, "GET");
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
        string uri = url + "/coinget";

        uwr = new UnityWebRequest(uri, "POST");
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



    private IEnumerator IPostNewPlayerData(string postusername) {
        //input.text = "loading...";

        string uri = url + "/newplayer";

        uwr = new UnityWebRequest(uri, "POST"); //post beallitasa
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":\""+postusername+"\"}"); //felviteli json
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            //input.text = uwr.downloadHandler.text; //vissza erzkezes arrol hogy sikeres a felvitel vagy nem
            Debug.Log(uwr.downloadHandler.text);     

            //hozzon letre uj coin adatot hogy a coinba ne ures adattal terjen vissza    
        }
    }

        private IEnumerator IPostNewCoinData(ulong coins, int userid) {
        string uri = url + "/newcoin";

        uwr = new UnityWebRequest(uri, "POST"); //post beallitasa
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":"+userid+",\"bevitel2\":"+coins+"}"); //felviteli json
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
              Debug.Log(uwr.downloadHandler.text);       
        }
    }

    private IEnumerator IPostNewScoreData(int playerid, ulong score, string time) {
        //input.text = "loading...";

        string uri = url + "/newscore";

        //felhasznalonevet ki kell irni fajlba
        //ha username keresve lenne es ha nincs akkor kap egy uj id-t ami ideiglenesen tarolodik (ures string a visszateres?)
        //ha van akkor akkor lekeri az id-t es ideiglenesen tarolja


        uwr = new UnityWebRequest(uri, "POST");
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

        string uri = url + "/newpalya";

        uwr = new UnityWebRequest(uri, "POST");
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

        private IEnumerator IPostUpdateCoinData(ulong coins, int userid) {
        string uri = url + "/coinUpdate";

        uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":"+coins+",\"bevitel2\":"+userid+"}");
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