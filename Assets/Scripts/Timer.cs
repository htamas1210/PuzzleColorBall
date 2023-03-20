using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using TMPro;


public class Timer : MonoBehaviour
{
    public Stopwatch playTime;

    public TMP_Text time;

    private void Awake() {
        playTime = new Stopwatch();
    }
    private void Update() {
        time.text = playTime.Elapsed.Seconds.ToString();
    }

    public string convertTimeToString(){
        UnityEngine.Debug.Log(playTime.Elapsed.ToString(@"hh\:mm\:ss"));
        return playTime.Elapsed.ToString(@"hh\:mm\:ss");
    }

}
