using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;


public class DatabaseData : MonoBehaviour
{
    public TMP_InputField input;
    private WriteFile wf;
    public PlayerList players;
    public string jsondata;

    private void Start() {
        wf = FindObjectOfType<WriteFile>();
        players = new PlayerList();
    }

    public void jsonParser(string jsondata) {
        players = JsonUtility.FromJson<PlayerList>("{\"player\":" + jsondata + "}");       
    }

    public void GetPlayerData() => StartCoroutine(IGetPlayerData());
    public void PostNewPlayerData() => StartCoroutine(IPostNewPlayerData());
    //public void PostNewPlayerDataTest() => StartCoroutine(IPostNewPlayerDataTest());
    public void PostNewScoreData() => StartCoroutine(IPostNewScoreData());
    //public void PostNewScoreDataTest() => StartCoroutine(IPostNewScoreDataTest());
    public void PostNewPalyaData() => StartCoroutine(IPostNewPalyaData());
    //public void PostNewPalyaDataTest() => StartCoroutine(IPostNewPalyaDataTest());

    private IEnumerator IGetPlayerData() {
        input.text = "Loading...";

        string uri = "http://localhost:3000/player";

        using (UnityWebRequest request = UnityWebRequest.Get(uri)) {
            yield return request.SendWebRequest();

            if(request.isNetworkError || request.isHttpError) { //lecserelni majd
                input.text = request.error;
            } else {
                jsondata = request.downloadHandler.text;
                jsonParser(jsondata);

                input.text = "";
                foreach(var p in players.player) {
                    p.ConvertDate();
                    input.text += "p_id: " + p.player_id + " username: " + p.player_name + " join date: " + p.joindate.printDate() + "\n";
                }
            }
        }
    }

    //Player test post
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
    /////////////////////////////////////////////


    /*private IEnumerator IPostNewPlayerData() {
        input.text = "loading...";

        string uri = "http://localhost:3000/newplayer";

        wf.writeUserName("newtesztUsername");

        //WWWForm form = new WWWForm();
        //form.AddField("bevitel1","");

        using(UnityWebRequest request = UnityWebRequest.Post(uri, "")) {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) { //lecserelni majd
                input.text = request.error;
            } else {
                input.text = request.downloadHandler.text;
            }
        }
    }*/


    /*private IEnumerator IPostNewScoreData() {
        input.text = "loading...";

        string uri = "http://localhost:3000/newscore";

        wf.WritePlayerid(2);
        wf.WritePalyaid(1);
        wf.WriteScore(10);
        wf.WriteTime("00:01:24");

        //WWWForm form = new WWWForm();
        //form.AddField("bevitel1", "");

        using (UnityWebRequest request = UnityWebRequest.Post(uri, "")) {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) { //lecserelni majd
                input.text = request.error;
            } else {
                input.text = request.downloadHandler.text;
            }
        }
    }*/

    //Score test post
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
    /////////////////////////////////////////////


    /*private IEnumerator IPostNewPalyaData() {
        input.text = "loading...";

        string uri = "http://localhost:3000/newpalya";

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=player_name"));
        formData.Add(new MultipartFormFileSection("bevitel1", "thewarrior1210"));

        wf.WritePalyaNev("Easy2");

        //WWWForm form = new WWWForm();
        //form.AddField("bevitel1", "");

        using (UnityWebRequest request = UnityWebRequest.Post(uri, "")) {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) { //lecserelni majd
                input.text = request.error;
            } else {
                input.text = request.downloadHandler.text;
            }
        }
    }*/

    //Player test post
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
    /////////////////////////////////////////////

}
