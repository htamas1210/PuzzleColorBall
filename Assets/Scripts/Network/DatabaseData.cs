using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;


public class DatabaseData : MonoBehaviour
{
    public TMP_InputField input;
    public PlayerList players;
    public HighScoreTableDataContainer htdc;
    public HighScoreTable hst;
    public string jsondata;

    private void Awake() {
        hst = FindObjectOfType<HighScoreTable>();
        htdc = new HighScoreTableDataContainer();
        players = new PlayerList();
    }

    public HighScoreTableDataContainer htdcReturn(){
        return htdc;
    }

    public void jsonParser(string jsondata) {
        players = JsonUtility.FromJson<PlayerList>("{\"player\":" + jsondata + "}");       
    }

    public void jsonParserHighScore(string jsondata) {
        htdc = JsonUtility.FromJson<HighScoreTableDataContainer>("{\"htd\":" + jsondata + "}");       
    }

    public void GetPlayerData() => StartCoroutine(IGetPlayerData());
    public void GetHighScoreData(int palya_id) => StartCoroutine(IGetHighScoreData(palya_id));
    public void PostNewPlayerData() => StartCoroutine(IPostNewPlayerData());
    public void PostNewScoreData() => StartCoroutine(IPostNewScoreData());
    public void PostNewPalyaData() => StartCoroutine(IPostNewPalyaData());

    private IEnumerator IGetPlayerData() {
        input.text = "Loading...";

        string uri = "http://localhost:3000/player";

        using (UnityWebRequest request = UnityWebRequest.Get(uri)) {
            yield return request.SendWebRequest();

            if(request.isNetworkError || request.isHttpError) { 
                input.text = request.error;
            } else {
                jsondata = request.downloadHandler.text;
                jsonParser(jsondata);
                foreach(var p in players.player) {
                    p.ConvertDate();
                    Debug.Log("p_id: " + p.player_id + " username: " + p.player_name + " join date: " + p.joindate.printDate() + "\n");
                }

                input.text = "";
                foreach(var p in players.player) {
                    p.ConvertDate();
                    input.text += "p_id: " + p.player_id + " username: " + p.player_name + " join date: " + p.joindate.printDate() + "\n";
                }
            }
        }
    }

    private void Start() {
        GetHighScoreData(2);
    }

    private IEnumerator IGetHighScoreData(int palya_id){
        string uri = "http://localhost:3000/toplist";

        var uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":"+palya_id+"}");
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log(uwr.error);
        } else {
            input.text = uwr.downloadHandler.text;
            Debug.Log(uwr.downloadHandler.text);
            jsonParserHighScore(uwr.downloadHandler.text);
            hst.CreateTable(htdc.htd);
        }
    }

    private IEnumerator IPostNewPlayerData() {
        input.text = "loading...";

        string uri = "http://localhost:3000/newplayer";

        var uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{ \"bevitel1\":\"postusername\"}");
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

    private IEnumerator IPostNewScoreData() {
        input.text = "loading...";

        string uri = "http://localhost:3000/newscore";

        var uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{\"bevitel1\":2,\"bevitel2\":1,\"bevitel3\":400,\"bevitel4\":\"00:05:06\"}");
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
