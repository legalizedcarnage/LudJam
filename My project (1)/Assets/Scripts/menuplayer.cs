using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuplayer : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector2 currentGrapplePosition;
    public Vector2 grapplePoint; 
    private LineRenderer lr;
    private BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D> ();
        rigidBody = GetComponent<Rigidbody2D> ();
        rigidBody.freezeRotation = true; 
        lr = GetComponent<LineRenderer>();
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
    void Update() {
       DrawRope();
    }
     void DrawRope() {
        
        currentGrapplePosition = Vector2.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, boxCollider2D.bounds.center);
        lr.SetPosition(1, currentGrapplePosition);
    }
}
