using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pause : MonoBehaviour
{
    public GameObject menu;
    private bool locker = false;
    private bool paused = false;
    public PlayerMovementController playerMovementController;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel") && !locker) {
            locker = true;
            if(!paused) {
                Time.timeScale = 0;
                paused = true;
                menu.SetActive(true);
            }else {
                paused =false;
                Time.timeScale = 1;
                menu.SetActive(false);
            }

        }  
        if(Input.GetButtonUp("Cancel") && locker) {
            locker = false;
            
            
        } 
    }

    public void quit ()  {
        Application.Quit();
    }
}
