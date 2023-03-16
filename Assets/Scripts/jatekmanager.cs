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

    private void Awake()
    {
        Instance = this;
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
    }

    private async void HandleHome()
    {
        StartCoroutine(TimerHome());
        GetComponent<GroundController>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
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
    }

    public void ChangeToShop()
    {
        UpdateGameState(GameState.Shop);
    }

    private async void HandleShop()
    {
        playButton.SetActive(false);
    }

    public void ChangeToGame()
    {
        UpdateGameState(GameState.Game);
        homeGomb.SetActive(false);
        settingsGomb.SetActive(false);
        shopGomb.SetActive(false);
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
    }

    public void ChangeToMeghaltal()
    {
        UpdateGameState(GameState.Meghaltal);
    }

    private async void HandleMeghaltal()
    {
        playButton.SetActive(false);
        homeGomb.SetActive(true);
        GetComponent<GroundController>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        garazs.SetActive(true);
    }


    //application target frame rate
}

   