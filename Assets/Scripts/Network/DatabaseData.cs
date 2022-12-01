using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class DatabaseData : MonoBehaviour
{
    public TMP_InputField input;


    public void GetData() => StartCoroutine(GetData_Coroutine());
    public void PostData() => StartCoroutine(PostData_Coroutine());

    IEnumerator GetData_Coroutine() {
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

    IEnumerator PostData_Coroutine() {
        input.text = "loading...";

        string uri = "http://localhost:3000/newplayer";

        /*List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=player_name"));
        formData.Add(new MultipartFormFileSection("bevitel1", "thewarrior1210"));*/

        WWWForm form = new WWWForm();
        form.AddField("bevitel1","thewarrior1210");

        using(UnityWebRequest request = UnityWebRequest.Post(uri, form)) {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) { //lecserelni majd
                input.text = request.error;
            } else {
                input.text = request.downloadHandler.text;
            }
        }
    }
}
