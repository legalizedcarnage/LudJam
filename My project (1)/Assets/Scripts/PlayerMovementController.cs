using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementController : MonoBehaviour
{
    //unityobjects
    public CameraShake cameraShake;
    public ParticleSystem jumpParticles;
    public ParticleSystem dashParticles;
    [SerializeField] private LayerMask terrainLayerMask;
    [SerializeField] private LayerMask grapplelessLayerMask;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    private Vector2 grapplePoint;
    private Vector2 currentGrapplePosition;
    private LineRenderer lr;
    private float maxDistance = 14f;
    private SpringJoint2D joint;
    //speed
    public float speed = 10f;
    private float movement = 0f;
    private float multiplier = 1f;
    //jump
    private int jumpBuffer = 6;
    private int jumpTimer = 0;
    private float lastY = 0;
    public  float jumpSpeed = 50f;
    private bool jump = false;
    private bool jumped = false;
    public int jumps =0;
    public int maxJumps =0;
    //dash
    private bool shift = false;
    private int dashDuration = 10;
    private int maxDashDuration = 10;
    private int dashCooldown = 60;
    private int maxDashCooldown = 60;
    private float dashMultiplier = 3f;
    //health
    public int health = 20;
    public int maxHealth = 20;

    //upgrades
    public bool dashPower = false;
    public bool grapplePower = false;
    private float timestart = 0;


    void Start()
    {
        joint = GetComponent<SpringJoint2D>();
        joint.enabled = false;
        boxCollider2D = GetComponent<BoxCollider2D> ();
        rigidBody = GetComponent<Rigidbody2D> ();
        rigidBody.freezeRotation = true;
        spriteRenderer=  GetComponent<SpriteRenderer> ();
        lr = GetComponent<LineRenderer>();
        timestart = Time.time;
    }


    void Update () {
        if(Input.GetButtonDown("Fire2")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(grapplePower) {
            if (Input.GetMouseButtonDown(0)) {
                StartGrapple();
            }
            else if (Input.GetMouseButtonUp(0)) {
                if(IsGrappling()) {
                    StopGrapple();
                }
            }
        }
        if(Input.GetButtonDown("Jump")) {
           
            jump = true;
        }  
        if(Input.GetButtonDown("Fire3")) {
            shift = true;
        }

    }
    void FixedUpdate()
    {
        //dash
        if(dashPower && shift && dashCooldown == maxDashCooldown && !IsGrounded()) {
            StopGrapple();
            dashParticles.Play();
            
            multiplier = dashMultiplier;
            dashDuration--;
            dashCooldown--;
        }
        if(dashDuration < maxDashDuration) {
            dashDuration--;
            rigidBody.gravityScale = 0.0f;
            rigidBody.velocity = new Vector2 (rigidBody.velocity.x,0);
            
            if(dashDuration <= 0|| IsGrounded()) {
                dashDuration = maxDashDuration;
                multiplier = 1f;
                rigidBody.gravityScale = 10f;
                
            }
            
        }
        if (dashCooldown < maxDashCooldown ) {
            dashCooldown--;
            if(dashCooldown <= 0 || IsGrounded()) {
                dashCooldown = maxDashCooldown;
            }
                
        }

        //standard movement
        movement = Input.GetAxisRaw ("Horizontal");
        if(!IsGrappling()) {
            if (movement > 0f) {
            rigidBody.velocity = new Vector2 ( multiplier * speed, rigidBody.velocity.y);
            }
            else if (movement < 0f) {
            rigidBody.velocity = new Vector2 ( -1 * multiplier * speed, rigidBody.velocity.y);
            } 
            else if(!IsGrappling()){
            rigidBody.velocity = new Vector2 (0,rigidBody.velocity.y);
            }
        }else {
            if (movement > 0f) {
            rigidBody.AddForce ( new Vector2 ( 2*multiplier * speed, rigidBody.velocity.y));
            }
            else if (movement < 0f) {
            rigidBody.AddForce ( new Vector2 ( -2 * multiplier * speed, rigidBody.velocity.y));
            } 
        }

        //jumping
        if(IsGrounded()) {
            jumpTimer = 0;
            jumps = maxJumps;
            jumped = false;
            
            //fall damage
            float diff = (lastY - boxCollider2D.bounds.center.y);
            
            if (diff > 10 ) {
                health -= Mathf.RoundToInt(diff) - 10;
                if(health ==0) {
                    death();
                }else {
                    float ratio = (health*1.0f)/maxHealth;
                    spriteRenderer.color = new Color(ratio,ratio,ratio);
                }
            }
            lastY = boxCollider2D.bounds.center.y;
        }else {
            jumpTimer++;
            if(jumpTimer > jumpBuffer && !jumped) {
                jumps--;
                jumped = true;
            }
            //save peak of jump
            if (lastY <  boxCollider2D.bounds.center.y) {
                lastY  =  boxCollider2D.bounds.center.y;
            }
        }
        if(jump && jumps > 0)
        {
            //jump
            StopGrapple();
            jumped = true;
            rigidBody.velocity =new Vector2(rigidBody.velocity.x,jumpSpeed);
            jumpEffect();
            jumps--;
            //cancels dash
            dashDuration = maxDashDuration;
            multiplier = 1f;
            rigidBody.gravityScale = 10f;
           
        }
        jump = false;
        shift = false;
    }
    void LateUpdate() {
        DrawRope();
    }
   
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 6) {
            
        }
        //gameObject.SetActive(false);
    }
    bool IsGrounded() {
        float extraHeight = .11f;

        RaycastHit2D raycastHit =Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size+ new Vector3(.01f,0f,0f), 0f, Vector2.down, extraHeight,terrainLayerMask | grapplelessLayerMask);
        
        return raycastHit.collider != null;
    }
    void jumpEffect() {
        jumpParticles.Play();
    }
    public void heal() {
        
        health += 50;
        float ratio = (health*1.0f)/maxHealth;
        spriteRenderer.color = new Color(ratio,ratio,ratio);
        if (health > maxHealth) {
            health = maxHealth;
        }
    }
    void death() {

    }
    void StartGrapple() {

        RaycastHit2D hit = Physics2D.Raycast(boxCollider2D.bounds.center, Camera.main.ScreenToWorldPoint(Input.mousePosition)-boxCollider2D.bounds.center, maxDistance, terrainLayerMask);
        if (hit) {
            lastY = boxCollider2D.bounds.center.y;
            grapplePoint = hit.point;
            joint.enabled = true;
            //joint = gameObject.AddComponent<SpringJoint2D>();
            joint.autoConfigureConnectedAnchor = false;
            joint.autoConfigureDistance = false;
            joint.distance = Vector2.Distance(boxCollider2D.bounds.center,hit.point)-0f;
            joint.connectedAnchor = grapplePoint;
            joint.enableCollision = true;
            joint.dampingRatio = 0f;

            joint.frequency = 1000f;

            lr.positionCount = 2;
            currentGrapplePosition = boxCollider2D.bounds.center;
        }
    }
    void StopGrapple() {
        lr.positionCount = 0;
        joint.enabled = false;
        //Destroy(joint);
    }
    void DrawRope() {
        //If not grappling, don't draw rope
        if (!IsGrappling()) return;

        currentGrapplePosition = Vector2.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, boxCollider2D.bounds.center);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling() {
        return joint.enabled;
    }

    public Vector2 GetGrapplePoint() {
        return grapplePoint;
    }
    public float GetTime() {
        return (Time.time - timestart);
    }

}
