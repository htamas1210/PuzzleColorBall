using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Date
{
    private int year; //ev
    private int month; //honap
    private int day; //nap

    public Date(string date) {
        date = date.Substring(0, date.Length - 14); //idozona levagasa
        string[] sp = date.Split('-'); //szoveg feladarabolasa
        year    = int.Parse(sp[0]);
        month   = int.Parse(sp[1]);
        day     = int.Parse(sp[2]);
    }

    public string printDate() { //datum kiiratasa
        return getYear() + "." + getMonth() + "." + getDay();
    }

    //get fv-k
    public int getYear() {
        return year;
    }
    public int getMonth() {
        return month;
    }
    public int getDay() {
        return day;
    }
}
