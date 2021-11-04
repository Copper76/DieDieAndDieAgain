using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public EdgeCollider2D ec;
    public GameObject corpse;
    public GameObject environment;
    public float speed;
    public float maxJumpHeight;
    public Vector3 respawnPoint;
    public int lives;

    private float horizontal;
    private float vertical;

    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        speed = 10.0f;
        maxJumpHeight = 10.0f;
        horizontal = 0.0f;
        vertical = 0.0f;
        isGrounded = false;
    }

    //generate a corpse where he died
    void dummy()
    {
        GameObject deadPlayer = Instantiate(corpse, this.transform.position, Quaternion.identity);
        deadPlayer.transform.parent = environment.transform;
    }

    //moves the player to a designated respawn point
    void die()
    {
        // if (lives > 0)
        // {
            this.transform.Translate(respawnPoint-this.transform.position+new Vector3(0,gameObject.GetComponent<SpriteRenderer>().bounds.size.y));
            this.rb.velocity = new Vector2(0f,0f);
            // lives -= 1;
        // }
        // else
        // {
        //     //game lost code
        // }
    }

    //put a dummy then die, applicable to anything other than falling
    void respawn()
    {
        dummy();
        die();
    }

    // Update is called once per frame
    void Update()
    {
        /* just some debug logging
        if (rb.velocity.sqrMagnitude <= 0.05 && Keyboard.current.IsPressed())
        {
            Debug.LogError(string.Format("Frame {0} : Stuck? vel {1}, drag = {2}, isGrounded = {3}, position = {4}", Time.frameCount, rb.velocity.ToString(), rb.drag, ec.IsTouchingLayers(LayerMask.GetMask("Ground")), gameObject.transform.position.ToString()));
        }
        */
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        // work out the player location/if they're grounded (not used atm)
        Bounds colliderBounds = bc.bounds;
        float colliderRadius = bc.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);

        //check player is grounded (not used atm)
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheckPos, bc.size, 0.0f, LayerMask.GetMask("Ground"));//3 is set to ground
        //check if player main collider is in the list of overlapping colliders

        //uncomment next line for previous result
        //isGrounded = false; 
        if (bc.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }

        if (bc.IsTouchingLayers(LayerMask.GetMask("Death")))
        {
            die();
        }

        //Debug.Log(string.Format("Grounded: {0} on frame {1}", isGrounded, Time.frameCount));

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            Debug.Log("Jumping");
            rb.velocity = new Vector2(rb.velocity.x, maxJumpHeight);
            isGrounded = false;
        }

        if (Keyboard.current.ctrlKey.wasPressedThisFrame)
        {
            respawn();
        }

        // Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        // Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
    }

    void LateUpdate()
    {
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log(string.Format("Jump action called at {0}", Time.frameCount));
    }
}
