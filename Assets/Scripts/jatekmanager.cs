using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class jatekmanager : MonoBehaviour
{
    //gamestate-s cuccok
    public static jatekmanager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    //gameobjectek,gombok,scriptek
    public GameObject playButton;
    public GameObject garazs;
    public GameObject homeGomb;
    public GameObject settingsGomb;
    public GameObject shopGomb;

    public GameObject goLeftButton;
    public GameObject jumpButton;
    public GameObject goRightButton;

    public TMP_Text scoreText;
    public TMP_Text timerText;


    private DatabaseData db;
    private UsernameHandler usernameHandler;
    private Score score;
    private Timer timer;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        db = FindObjectOfType<DatabaseData>();
        usernameHandler = FindObjectOfType<UsernameHandler>();
        score = FindObjectOfType<Score>();
        timer = FindObjectOfType<Timer>();
    }

    private void Start()
    {
        UpdateGameState(GameState.Home);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Home:
                HandleHome();
                break;
            case GameState.Settings:
                HandleSettings();
                break;
            case GameState.Shop:
                HandleShop();
                break;
            case GameState.Game:
                HandleGame();
                break;
            case GameState.Meghaltal:
                HandleMeghaltal();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public enum GameState
    {
        Home,
        Settings,
        Shop,
        Game,
        Meghaltal
    }

    public void ChangeToHome()
    {
        UpdateGameState(GameState.Home);
        homeGomb.SetActive(true);
        settingsGomb.SetActive(true);
        shopGomb.SetActive(true);
        goLeftButton.SetActive(false);
        jumpButton.SetActive(false);
        goRightButton.SetActive(false);
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    private async void HandleHome()
    { 
        //deactivate buttons
        StartCoroutine(TimerHome());
        GetComponent<GroundController>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        goLeftButton.SetActive(false);
        jumpButton.SetActive(false);
        goRightButton.SetActive(false);
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        garazs.SetActive(true);
    }

    IEnumerator TimerHome()
    {
        yield return new WaitForSecondsRealtime(2);
        playButton.SetActive(true);
    }

    public void ChangeToSettings()
    {
        UpdateGameState(GameState.Settings);
    }

    private async void HandleSettings()
    {
        playButton.SetActive(false);
        goLeftButton.SetActive(false);
        jumpButton.SetActive(false);
        goRightButton.SetActive(false);
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    public void ChangeToShop()
    {
        UpdateGameState(GameState.Shop);
    }

    private async void HandleShop()
    {
        playButton.SetActive(false);
        goLeftButton.SetActive(false);
        jumpButton.SetActive(false);
        goRightButton.SetActive(false);
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    public void ChangeToGame()
    {
        homeGomb.SetActive(false);
        settingsGomb.SetActive(false);
        shopGomb.SetActive(false);
        UpdateGameState(GameState.Game);
    }

    IEnumerator TimerGame()
    {
        yield return new WaitForSecondsRealtime(1);
        garazs.SetActive(false);
    }

    private async void HandleGame()
    {
        playButton.SetActive(false);
        StartCoroutine(TimerGame());
        GetComponent <GroundController> ().enabled = true;
        GetComponent <PlayerController>().enabled = true;
        goLeftButton.SetActive(true);
        jumpButton.SetActive(true);
        goRightButton.SetActive(true);
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        timer.playTime.Start();
    }

    public void ChangeToMeghaltal()
    {
        UpdateGameState(GameState.Meghaltal);
    }

    private async void HandleMeghaltal()
    {
        //ora leallitasa
        timer.playTime.Stop();

        //Valtson at high score tabla scenebe utana ha megnyomott egy gombot ugy menjen vissza a menube
        //toltse fel az adatokat a run-rol
        db.PostNewScoreData(usernameHandler.userid, score.score, timer.convertTimeToString());

        SceneUIManager.LoadScene(1); //HighScore scene

        //

        /*playButton.SetActive(false);
        homeGomb.SetActive(true);
        GetComponent<GroundController>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        garazs.SetActive(true);*/
    }
}