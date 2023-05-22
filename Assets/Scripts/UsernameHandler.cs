using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using TMPro;

public class UsernameHandler : MonoBehaviour
{
    public string username; //playerlistbe benne van az id-val
    public int userid;
    private string path;

    public TMP_InputField input;
    public GameObject inputBackground;
    public Canvas usernameInputCanvas;

    private StreamWriter writer;
    private StreamReader reader;
    private DatabaseData db;
    private PlayerList playerList;
    private CoinCounter cc;

    private void Awake() {
        input.gameObject.SetActive(false);
        inputBackground.SetActive(false); 

        path = Application.persistentDataPath + "/username.txt";

        cc = FindObjectOfType<CoinCounter>();
        db = FindObjectOfType<DatabaseData>();

        db.GetPlayerData(); 
    }

    private void Start() {
        usernameCheck();
    }
    


    public void CallReadUsername(string username) => StartCoroutine(ReadUsername(username));

    private IEnumerator ReadUsername(string username){ //kiirja az inputbol kapott usernevet fajlba
        this.username = username;
        Debug.Log("username" + this.username);

        writer = new StreamWriter(path, false, Encoding.Default);
        writer.Write(username);
        writer.Close();

        input.gameObject.SetActive(false);
        inputBackground.SetActive(false);  
        usernameInputCanvas.gameObject.SetActive(false);

        db.PostNewPlayerData(username); //nincs meg ilyen username az adatbazisba ezert vigye fel uj kent 

        Debug.Log("<color=red>uwr before:</color>" + db.GetUnityWebRequest().downloadHandler.text);
        yield return new WaitUntil(() => db.GetUnityWebRequest().downloadHandler.text.Equals("Uj username!"));
        Debug.Log("<color=red>uwr after:</color>" + db.GetUnityWebRequest().downloadHandler.text); 


        //eltarol playerek frissitese
        db.players.player = null;
        db.GetPlayerData();
        yield return new WaitUntil(() => db.players.player != null); //varjon amig lekeri az adatokat

        getId(true);
    }   

    private void getId(bool newuser){
        foreach(var item in db.players.player){
            if(item.player_name.Equals(username)){
                userid = item.player_id;
                break;
            }
        }  

        if(newuser){
            db.PostNewCoinData(0, userid); //ne uressel terjen vissza
        }

        db.GetCoinDataCall(userid);       
    }

    private void usernameCheck(){
        string data = "";
        
        try
        {
            reader = new StreamReader(path);
        }
        catch (System.IO.FileNotFoundException)
        {
            //ha nem letezik a fajl aktivalja az inputot
            usernameInputCanvas.gameObject.SetActive(true);
            inputBackground.SetActive(true);
            input.gameObject.SetActive(true);
        }

        if(File.Exists(path)){
            while(!reader.EndOfStream){
                data += reader.ReadLine();
            }

            if(data.Equals("")){ //nincs username meg
                usernameInputCanvas.gameObject.SetActive(true);
                input.gameObject.SetActive(true);
                inputBackground.SetActive(true);              
            }else{
                //van username
                username = data;
                Debug.Log("username: " + username);         

                getId(false);
              
                //StartCoroutine(waitForCoins());
                
                

                input.gameObject.SetActive(false);
                inputBackground.SetActive(false);
                usernameInputCanvas.gameObject.SetActive(false);                              
            }
        }
    }

    private IEnumerator waitForCoins(){
        yield return new WaitUntil(() => db.coins != 0);

        cc.SetCoin(db.GetCoins(userid));
        Debug.Log("uh coin"+cc.coin);
    }
}