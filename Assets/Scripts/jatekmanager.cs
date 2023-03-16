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
    public static jatekmanager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

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

    private async void HandleHome()
    {

    }

    private async void HandleSettings()
    {

    }

    private async void HandleShop()
    {

    }

    private async void HandleGame()
    {

    }

    private async void HandleMeghaltal()
    {

    }


    //application target frame rate
}

   