using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuplayer : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D> ();
        rigidBody.freezeRotation = true; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float movement =  Mathf.PerlinNoise(Time.time * .5f, 0.0f);
        if (movement > .7f) {
            rigidBody.AddForce ( new Vector2 ( 2 * 20, rigidBody.velocity.y));
        }
        else if (movement < .3f) {
            rigidBody.AddForce ( new Vector2 ( -2  * 20, rigidBody.velocity.y));
        } 
    }
}
