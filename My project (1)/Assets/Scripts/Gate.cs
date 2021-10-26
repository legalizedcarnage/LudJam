using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gate : MonoBehaviour
{
    public TMP_Text clock;



    void OnTriggerEnter2D(Collider2D col)
    {
        float timer = col.GetComponent<PlayerMovementController>().GetTime();
        float minutes = Mathf.Floor(timer / 60);
        float seconds = Mathf.RoundToInt(timer%60);
        string data = "Completed: " + minutes + ":";
        if(seconds < 10) {
            data += "0" + seconds;
        }
        else {
            data += seconds;
            }
        clock.gameObject.SetActive(true);
        clock.text = data;
        //Debug.Log(minutes + "." + seconds);
    }

}
