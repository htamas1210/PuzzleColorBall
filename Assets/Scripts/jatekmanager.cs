using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Cinemachine;

public class jatekmanager : MonoBehaviour
{
    //gamestate-s cuccok
    public static jatekmanager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    //gameobjectek,gombok,scriptek
    public GameObject playGomb;
    public GameObject garazs;
    public GameObject homeGomb;
    public GameObject settingsGomb;
    public GameObject shopGomb;
    public GameObject volumeSlide;
    public GameObject musicSlide;
    public GameObject sfxSlide;

    public GameObject goLeftButton;
    public GameObject jumpButton;
    public GameObject goRightButton;

    public GameObject boby;

    public TMP_Text scoreText;
    public TMP_Text timerText;


    private DatabaseData db;
    private UsernameHandler usernameHandler;
    private Score score;
    private Timer timer;
    private CoinCounter cc;

    [SerializeField] private GameObject controlTypeSwitcher;

    [SerializeField] private CinemachineVirtualCamera homeCamera;


    public AudioMixer audioMixer;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        
        Instance = this;
        db = FindObjectOfType<DatabaseData>();
        usernameHandler = FindObjectOfType<UsernameHandler>();
        score = FindObjectOfType<Score>();
        timer = FindObjectOfType<Timer>();
        cc = FindObjectOfType<CoinCounter>();
    }

    private void Start()
    {
        UpdateGameState(GameState.Home);
    }

    public void SetMainVolume(float mainVolume)
    {
        audioMixer.SetFloat("Master", mainVolume);
    }
    public void SetMusicVolume(float musicVolume)
    {
        audioMixer.SetFloat("Music", musicVolume);
    }
    public void SetSfxVolume(float sfxVolume)
    {
        audioMixer.SetFloat("Sfx", sfxVolume);
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

        controlTypeSwitcher.SetActive(false);
    }

    private async void HandleHome()
    { 
        //deactivate buttons
        StartCoroutine(TimerHome());
        playGomb.SetActive(true);
        GetComponent<GroundController>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        goLeftButton.SetActive(false);
        jumpButton.SetActive(false);
        goRightButton.SetActive(false);
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        garazs.SetActive(true);
        volumeSlide.SetActive(false);
        musicSlide.SetActive(false);
        sfxSlide.SetActive(false);
        controlTypeSwitcher.SetActive(false);
    }

    IEnumerator TimerHome()
    {
        yield return new WaitForSecondsRealtime(2);
        
        
    }

    public void ChangeToSettings()
    {
        UpdateGameState(GameState.Settings);
        playGomb.SetActive(false);
        volumeSlide.SetActive(true);
        musicSlide.SetActive(true);
        sfxSlide.SetActive(true);
        controlTypeSwitcher.SetActive(true);
    }

    private async void HandleSettings()
    {
        playGomb.SetActive(false);
        goLeftButton.SetActive(false);
        jumpButton.SetActive(false);
        goRightButton.SetActive(false);
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        playGomb.SetActive(false);
        volumeSlide.SetActive(false);
        musicSlide.SetActive(false);
        sfxSlide.SetActive(false);
        controlTypeSwitcher.SetActive(false);
    }

    public void ChangeToShop()
    {
        UpdateGameState(GameState.Shop); 
    }

    private async void HandleShop()
    {
        playGomb.SetActive(false);
        goLeftButton.SetActive(false);
        jumpButton.SetActive(false);
        goRightButton.SetActive(false);
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        volumeSlide.SetActive(false);
        musicSlide.SetActive(false);
        sfxSlide.SetActive(false);
        controlTypeSwitcher.SetActive(false);
    }

    public void ChangeToGame()
    {
        homeCamera.m_Lens.NearClipPlane = 0.3f;

        homeGomb.SetActive(false);
        settingsGomb.SetActive(false);
        shopGomb.SetActive(false);
        playGomb.SetActive(false);
        UpdateGameState(GameState.Game);
        volumeSlide.SetActive(false);
        musicSlide.SetActive(false);
        sfxSlide.SetActive(false);
        controlTypeSwitcher.SetActive(false);
    }

    IEnumerator TimerGame()
    {
        yield return new WaitForSecondsRealtime(1);
        garazs.SetActive(false);


    }

    private async void HandleGame()
    {
        StartCoroutine(TimerGame());

        boby.transform.position = new Vector3(0, boby.transform.position.y, boby.transform.position.z);

        GetComponent<GroundController> ().enabled = true;
        GetComponent<PlayerController>().enabled = true;

        
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

        //coin feltoltes
        db.PostUpdateCoinData(cc.coin, usernameHandler.userid);

        SceneUIManager.LoadScene(1); //HighScore scene


        /*
        homeGomb.SetActive(true);
        GetComponent<GroundController>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        garazs.SetActive(true);*/
    }
}