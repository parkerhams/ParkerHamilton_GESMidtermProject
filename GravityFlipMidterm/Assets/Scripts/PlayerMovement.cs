using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    float movementSpeed = 2;
    [SerializeField]
    float jumpStrength = 5;
    [SerializeField]
    Transform groundDetectCenterPoint;
    [SerializeField]
    float groundDetectRadius = 0.2f;

    [SerializeField]
    LayerMask whatCountsAsGround;

    private float horizontalInput;

    private bool isOnGround;
	private bool isOnCeiling;

    private Vector2 gravity;

    private bool shouldJump;
    private bool facingRight = true;
    public bool doubleJump = false;
    Animator anim;

    private AudioSource audioSource;


    public bool gravitySwitch;

    private Vector2 jumpForce;
    Rigidbody2D myRigidbody;

    
    //Use this for initialization
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gravity = Physics.gravity;
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
        //Teleports game object to new location
        myRigidbody = GetComponent<Rigidbody2D>();
        jumpForce = new Vector2(0, jumpStrength);

    }

    private void Update()
    {
        GetMovementInput();
        GetJumpInput();
        UpdateIsOnGround();
        GravityReverse();
        if ((isOnGround) && Input.GetButtonDown("Fire1"))
        {
            anim.SetBool("Ground", true);
            myRigidbody.AddForce(new Vector2( 0, jumpStrength));
            audioSource.Play();
        }
        if (Input.GetButtonDown("Jump")) //Detect if player presses E to flip gravity
        {
            gravitySwitch = !gravitySwitch;
            if (gravitySwitch)
            {
                Physics.gravity = new Vector2(0, 20); //Invert
            }
            else if (!gravitySwitch)
            {
                Physics.gravity = new Vector2(0, -20); //Default unity
            }
            audioSource.Play();
        }

    }

    private void GravityReverse()
	{
        
        if (Input.GetButtonDown("Jump"))
        {
			myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, myRigidbody.velocity.y * 0.6f);
            myRigidbody.gravityScale *= -1;
			jumpForce = jumpForce * -1;

            myRigidbody.AddForce(jumpForce, ForceMode2D.Impulse);

            //facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
			theScale.y *= -1;
			transform.localScale = theScale;
            audioSource.Play();
      
        }
        //else if (Input.GetButtonDown("Fire1"))
        //{
        //    //Physics2D.gravity = new Vector2(-20f, );
        //    myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x * 0.6f, myRigidbody.velocity.y);
        //}
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }

    private void GetJumpInput()
    {
        if (Input.GetButtonDown("Fire2") && isOnGround || isOnCeiling)
        {
            shouldJump = true;
        }
    }

    private void GetMovementInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
		anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
    }

    private void FixedUpdate()
    {
        Physics.gravity = gravity;
        isOnGround = Physics2D.OverlapCircle(groundDetectCenterPoint.position, groundDetectRadius, whatCountsAsGround);
        anim.SetBool("Ground", isOnGround);
		//anim.SetBool ("Ceiling", isOnCeiling);
        Debug.Log("Are we on the ground? " + isOnGround);
        anim.SetFloat("vSpeed", myRigidbody.velocity.y);
        Move();
        Jump();
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }
        //if(Input.GetButtonDown("Jump"))
        //{
        //    gravity.x = -25;
        //    gravity.y = 0;
        //}
    }

    private void UpdateIsOnGround()
    {
        Collider2D[] groundObjects = Physics2D.OverlapCircleAll(groundDetectCenterPoint.position, groundDetectRadius, whatCountsAsGround);
        isOnGround = groundObjects.Length > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOnGround = true;
    }

    private void Jump()
    {
        if (shouldJump)
        {
            anim.SetBool("Ground", true);
            anim.SetFloat("vSpeed", myRigidbody.velocity.y);
            myRigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
            //myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            isOnGround = false;
			isOnCeiling = false;
            shouldJump = false;
            audioSource.Play();
           
        }
        //else if (myRigidbody.velocity.y > 0 && Input.GetButton("Jump"))
        //{
        //    myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        //}
    }

    private void Move()
    {
        myRigidbody.velocity = new Vector2(horizontalInput * movementSpeed, myRigidbody.velocity.y);
    }


}
