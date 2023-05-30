using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
   public ulong score;
   public TMP_Text scoreUI;

    private void Awake() {
        score = 0;
        scoreUI.text = score.ToString();
    }

    public void addScore(ulong number){
        score += number;
        scoreUI.text = score.ToString();
    }
}