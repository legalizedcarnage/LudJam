using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public Rigidbody2D player;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //follow player   
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
