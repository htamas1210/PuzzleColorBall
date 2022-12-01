using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class DatabaseData : MonoBehaviour
{
    public TMP_InputField input;
    private WriteFile wf;

    private void Start() {
        wf = FindObjectOfType<WriteFile>();
    }

    public void GetPlayerData() => StartCoroutine(IGetPlayerData());
    public void PostNewPlayerData() => StartCoroutine(IPostNewPlayerData());
    public void PostNewScoreData() => StartCoroutine(IPostNewScoreData());
    public void PostNewPalyaData() => StartCoroutine(IPostNewPalyaData());

    private IEnumerator IGetPlayerData() {
        input.text = "Loading...";

        string uri = "http://localhost:3000/player";

        using (UnityWebRequest request = UnityWebRequest.Get(uri)) {
            yield return request.SendWebRequest();

            if(request.isNetworkError || request.isHttpError) { //lecserelni majd
                input.text = request.error;
            } else {
                input.text = request.downloadHandler.text;
            }
        }
    }

    private IEnumerator IPostNewPlayerData() {
        input.text = "loading...";

        string uri = "http://localhost:3000/newplayer";

        /*List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=player_name"));
        formData.Add(new MultipartFormFileSection("bevitel1", "thewarrior1210"));*/

        wf.writeUserName("newtesztUsername");

        WWWForm form = new WWWForm();
        //form.AddField("bevitel1","");

        using(UnityWebRequest request = UnityWebRequest.Post(uri, "")) {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) { //lecserelni majd
                input.text = request.error;
            } else {
                input.text = request.downloadHandler.text;
            }
        }
    }


    private IEnumerator IPostNewScoreData() {
        input.text = "loading...";

        string uri = "http://localhost:3000/newscore";

        /*List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=player_name"));
        formData.Add(new MultipartFormFileSection("bevitel1", "thewarrior1210"));*/

        wf.WritePlayerid(2);
        wf.WritePalyaid(1);
        wf.WriteScore(10);
        wf.WriteTime("00:01:24");

        WWWForm form = new WWWForm();
        //form.AddField("bevitel1", "");

        using (UnityWebRequest request = UnityWebRequest.Post(uri, "")) {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) { //lecserelni majd
                input.text = request.error;
            } else {
                input.text = request.downloadHandler.text;
            }
        }
    }

    private IEnumerator IPostNewPalyaData() {
        input.text = "loading...";

        string uri = "http://localhost:3000/newpalya";

        /*List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=player_name"));
        formData.Add(new MultipartFormFileSection("bevitel1", "thewarrior1210"));*/

        wf.WritePalyaNev("Easy2");

        WWWForm form = new WWWForm();
        //form.AddField("bevitel1", "");

        using (UnityWebRequest request = UnityWebRequest.Post(uri, "")) {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) { //lecserelni majd
                input.text = request.error;
            } else {
                input.text = request.downloadHandler.text;
            }
        }
    }
}
