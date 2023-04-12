using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Targyak : MonoBehaviour
{
    int fsz_elerheto = 1;  //ennyi darab fenséges szárny tárgya van a játékosnak

    public GameObject bobby; //a karakter akit mozgatunk

    public Transform targetFent; //a fenti pont ahova felrepül

    public Transform targetLent; //a lenti pont ahol alapból fut

    public float speed; //a sebesség amivel felrepül


    private void FensegesSzarnyak()
    {

        if (fsz_elerheto > 0) //ha van legalább 1 ilyen tárgya
        {
            Vector3 a = transform.position;

            Vector3 b = targetFent.position;

            Vector3 c = targetLent.position;

            bobby.transform.position = Vector3.MoveTowards(a, b, speed); //itt felrepül

            TimerSzarnyak(); //eddig repülsz

            bobby.transform.position = Vector3.MoveTowards(a, c, speed); //itt pedig visszaérkezik a földre



        }

    }

    IEnumerator TimerSzarnyak()
    {
        yield return new WaitForSecondsRealtime(5); //vár 5 másodpercet
    }
}
