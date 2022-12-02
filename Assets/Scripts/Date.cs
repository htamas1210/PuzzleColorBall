using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Date
{
    private int year;
    private int month;
    private int day;

    public Date(string date) {
        date = date.Substring(0, date.Length - 14);
        string[] sp = date.Split('-');
        year    = int.Parse(sp[0]);
        month   = int.Parse(sp[1]);
        day     = int.Parse(sp[2]);
    }

    public string printDate() {
        return getYear() + "." + getMonth() + "." + getDay();
    }

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
