using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int powerUp;
    private bool used = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        
        if(used) {
            used = false;
            PlayerMovementController player =col.GetComponent<PlayerMovementController>();
            switch(powerUp) {
                case 0:
                    player.maxJumps++;
                    player.jumps++;
                    break;
                case 1:
                    player.dashPower = true;
                    break;
                case 2:
                    player.grapplePower = true;
                    break;
                case 3:
                    player.heal();
                    break;

                
            }
        

            gameObject.SetActive(false);
        }
    }
}
