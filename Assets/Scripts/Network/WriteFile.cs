using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteFile : MonoBehaviour
{
    //player
    public void writeUserName(string username) {
        StreamWriter writer = new StreamWriter(@"../backend/username.txt", false);
        writer.Write(username);
        writer.Close();
    }
    //player vege

    //Score
    public void WritePlayerid(int playerid) {
        StreamWriter writer = new StreamWriter(@"../backend/playerid.txt", false);
        writer.Write(playerid);
        writer.Close();
    }

    public void WritePalyaid(int palyaid) {
        StreamWriter writer = new StreamWriter(@"../backend/palyaid.txt", false);
        writer.Write(palyaid);
        writer.Close();
    }

    public void WriteScore(int score) {
        StreamWriter writer = new StreamWriter(@"../backend/score.txt", false);
        writer.Write(score);
        writer.Close();
    }

    public void WriteTime(string time) {
        StreamWriter writer = new StreamWriter(@"../backend/time.txt", false);
        writer.Write(time);
        writer.Close();
    }
    //Score vege

    //Palya
    public void WritePalyaNev(string palyanev) {
        StreamWriter writer = new StreamWriter(@"../backend/ppalyanev.txt", false);
        writer.Write(palyanev);
        writer.Close();
    }
    //palya vege
}
