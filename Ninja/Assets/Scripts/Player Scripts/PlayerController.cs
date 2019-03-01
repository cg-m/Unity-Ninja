using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance;
    private float moveForce = 100f;
    private float jumpForce = 1300f;
    private float maxVelocity = 4f;

    private bool grounded;
    private Rigidbody2D myBody;
    private Animator anim;

    public AudioClip audioJump, audioDied, audioSlash, audioStabbed;
    public AudioSource audioSource;

    public float timeSlash, timeJump, timeSlash2, timeSlash3, ts2;
    public bool slash, jump, beStabbed, died, slash2, slash1=true;
    public float playerTransX, playerTransY, ScaleX, scaleX;
    private bool moveLeft, moveRight, moveSlash, moveSlash2, moveSlash3, moveJump, nJump;
    Vector3 scale, temp;
    //private bool moveLeft, moveRight;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        MakeInstance();

    } 
    // Use this for initialization
    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start () {
        scaleX = transform.localScale.x;
        audioSource.enabled = true;
        audioSource.loop = false ;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //PlayerMoveKeyboard();
        PlayerMoveJoyStick();
        AudioControl();
        BeStabbed();
        Slash();
        Jump();
        Died();
        temp = transform.position;
        playerTransX = temp.x;
        playerTransY = temp.y;
        scale = transform.localScale;
        ScaleX = scale.x;
        //Debug.Log("" + jump);
    }
    void AudioControl()
    {
        if (GameMangerController.instance.GetMute() == 0)
        {
            audioSource.enabled = true;
        }
        else
        {
            audioSource.enabled = false;
        }
    }
    public void SetMoveLeft(bool moveLeft)
    {
        this.moveLeft = moveLeft;
    }
    public void SetMoveRight(bool moveRight)
    {
        this.moveRight = moveRight;
    }
    public void SetSlash(int i)
    {
        if (i == 1)
        {
            moveSlash = true;
        }else if (i == 2)
        {
            moveSlash2 = true;
        }
        else if(i == 3)
        {
            moveSlash3 = true;
        }
    }
    public void SetJump(bool moveJump)
    {
        jump = moveJump;
        nJump = moveJump;
    }
    public void StopMoving()
    {
        this.moveLeft = false;
        this.moveRight = false;
        //jump = false;
    }

    void BeStabbed()
    {
        if(beStabbed)
        {
            moveForce = 0f;
            jumpForce = 0f;
           // Debug.Log("Bi Dam");
            anim.SetBool("Stabbed", true);
            
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioStabbed);
            }
        }
        else
        {
            anim.SetBool("Stabbed", false);
            moveForce = 100f;
            jumpForce = 1300f;
            
            // Debug.Log("AAAAAAAAA");
        }
    }
    //Use JoyStick Play Game
    void PlayerMoveJoyStick()
    {
        float forceX = 0f;
        float vel = Mathf.Abs(myBody.velocity.x);

        if (moveRight && !died && !beStabbed)
        {
            if (vel < maxVelocity)
            {
                if (grounded)
                {
                    forceX = moveForce*1.3f;
                }
                else
                {
                    forceX = moveForce * 2f;
                }
            }
            scale.x = scaleX;
            transform.localScale = scale;
            if (grounded && !jump)
            {
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
            }
        }
        else if (moveLeft && !died && !beStabbed)
        {
            if (vel < maxVelocity)
            {
                if (grounded)
                {
                    forceX = -moveForce*1.3f;
                }
                else
                {
                    forceX = -moveForce * 2f;
                }
            }
            scale.x = -scaleX;
            transform.localScale = scale;
            if (grounded && !jump)
            {
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
            }
        }
        else
        {
            forceX = 0;
            anim.SetBool("Run", false);
        }
        myBody.AddForce(new Vector2(forceX, 0));
        
    }
    //Use Keyboard Play Game
    void PlayerMoveKeyboard()
    {
        float forceX = 0f;
        float forceY = 0f;

        float vel = Mathf.Abs(myBody.velocity.x);
        float h = Input.GetAxisRaw("Horizontal");

        if (h > 0 && !beStabbed)
        {
            if (vel < maxVelocity)
            {
                if (grounded)
                {
                    forceX = moveForce*1.3f;
                }
                else
                {
                    forceX = moveForce * 2f;
                }
            }
            
            scale.x = scaleX;
            transform.localScale = scale;
            if (grounded && !jump)
            {
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
            }


        }
        else if (h < 0 && !beStabbed)
        {
            if (vel < maxVelocity)
            {
                if (grounded)
                {
                    forceX = -moveForce*1.3f;           
                }
                else
                {
                    forceX = -moveForce * 2f;
                }
            }
            scale.x = -scaleX;
            transform.localScale = scale;
            if (grounded && !jump)
            {
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
            }
        }
        else if (h == 0)
        {
            anim.SetBool("Run", false);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jump = true;
            nJump = true;
        }

        myBody.AddForce(new Vector2(forceX, forceY));

        if (Input.GetKey(KeyCode.C))
        {
            moveSlash = true;

        }
        

    }
    
    private void Slash()
    {
        if (moveSlash)
        {

            timeSlash += UnityEngine.Time.deltaTime * 10;
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioSlash);
            }
            anim.SetBool("Slash", true);           
            slash = true;
            if (timeSlash > 3f)
            {
                anim.SetBool("Slash", false);
                moveSlash = false;
                slash = false;
                timeSlash = 0;
            }
        }
        if (moveSlash2)
        {

            timeSlash2 += UnityEngine.Time.deltaTime * 10;
            anim.SetBool("Slash2", true);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioSlash);
            }
            slash = true;
            if (timeSlash2 > 5f)
            {
                anim.SetBool("Slash2", false);
                moveSlash2 = false;
                slash = false;
                timeSlash2 = 0;
            }
        }
        if (moveSlash3)
        {

            timeSlash3 += UnityEngine.Time.deltaTime * 10;
            anim.SetBool("Slash3", true);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioSlash);
            }
            slash = true;
            if (timeSlash3 > 3f)
            {
                anim.SetBool("Slash3", false);
                moveSlash3 = false;
                slash = false;
                timeSlash3 = 0;
            }
        }

    }
    private void Jump()
    {
        if (jump && !died && !beStabbed)
        {
            myBody.drag = 0;
            if (grounded && nJump)
            {
                nJump = false;
                grounded = false;
                myBody.AddForce(new Vector2(0, jumpForce));
                
            }
            anim.SetBool("Jump", true);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioJump);
            }
            
            timeJump += UnityEngine.Time.deltaTime * 10;
            if (timeJump > 7f)
            {
                myBody.drag = 1f;
                anim.SetBool("Jump", false);
                timeJump = 0f;
                jump = false;
            }                 
        }
        
    }
    private void Died()
    {
        if (died)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioDied);
                //audioSource.enabled = false;
            }
            anim.SetBool("Died", true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Patch")
        {
            grounded = true;
        }
        
    }
}
