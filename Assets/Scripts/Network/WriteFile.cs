using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteFile : MonoBehaviour
{
    

    public void writeUserName(string username) {
        StreamWriter writer = new StreamWriter(@"adat.txt", false);
        writer.Write(username);
        writer.Close();
    }
    
    
}
