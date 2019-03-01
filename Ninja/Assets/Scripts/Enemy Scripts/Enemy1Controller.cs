using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour {
    public static Enemy1Controller instance;
    //public GameObject grandChid;
	// Use this for initialization
    private float speed = 0.5f;
    private Rigidbody2D myBody;
    private Animator anim;

    public GameObject fire;

    public AudioSource audioSource;
    public AudioClip audioHit, audioDied, audioStab;

    private bool trigger, rotateRight, rotateLeft, enemyDown, aud = true;
    float timer, TransX, TransY, timeFire;
    Vector3 temp, scale;
    int slash = 0, ratio;
    public bool stab;
    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        MakeInstance();
        DoorController.instance.enemyCount++;
    }
    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
	
	// Update is called once per frame
	void Update () {
        Move();
        ChangeDirection();
        AudioControl();
        Stab();
        BeStabbed();
        Died();
        temp = transform.position;
        TransX = temp.x;
        TransY = temp.y;
        if (PlayerController.instance.died|| GamePlayController.instance.pauseGame)
        {
            audioSource.enabled = false;
        }
        else
        {
            audioSource.enabled = true;
        }
        
        Debug.Log("Enemy Stab: x" + GameMangerController.instance.GetLevel());

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
    private void Move()
    {
        myBody.velocity = new Vector2(transform.localScale.x, 0) * speed;
        if(Mathf.Abs(TransX - PlayerController.instance.playerTransX) > 0.5f && !stab )
        {
            speed = 0.5f;
        }
        else if(Mathf.Abs(TransY - PlayerController.instance.playerTransY) < 0.5f)
        {
            speed = 0f;
        }
    }
    private void ChangeDirection()
    {
        
        if (Mathf.Abs(TransX - PlayerController.instance.playerTransX) < 1f && stab
            && Mathf.Abs(TransY - PlayerController.instance.playerTransY) < 0.34f)
        {
            if (TransX - PlayerController.instance.playerTransX <= 0)
            {   //xoay sang phai
                rotateRight = true;
                rotateLeft = false;        
            }
            else
            {
                //xoay sang trai
                rotateLeft = true;
                rotateRight = false;              
            }
        }
        else
        {
            rotateRight = false;
            rotateLeft = false;
        }
        if (true)
        {        
            scale = transform.localScale;
            if(trigger && scale.x == 2.2f || rotateLeft == true)
            {
                trigger = false;
                scale.x = -2.2f;
                rotateLeft = false;
            }
            else if(trigger && scale.x == -2.2f || rotateRight == true)
            {
                trigger = false;
                scale.x = 2.2f;
                rotateRight = false;
            }
            transform.localScale = scale;
        }
        
    }
    void Stab()
    {
        if (stab)
        {              
            timer += UnityEngine.Time.deltaTime * 10;
            if(timer > 2f && timer < 5f && !PlayerController.instance.slash && slash < 50f * ratio * 1.2f
                && Mathf.Abs(TransY - PlayerController.instance.playerTransY) < 0.5f)
            {
                anim.SetBool("Stab", true);
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(audioStab);
                }
                PlayerController.instance.beStabbed = true;
                GameObject.Find("GamePlayController").GetComponent<SliderController>().hp -= 0.5f;
            }
            else if (timer >= 10f)
            {
                anim.SetBool("Stab", false);
                PlayerController.instance.beStabbed = false;                
                timer = 0f;              
            }
            else
            {
                anim.SetBool("Stab", false);
                PlayerController.instance.beStabbed = false;
            }
            if (Mathf.Abs(TransX - PlayerController.instance.playerTransX) > 1.6f)
            {                
               stab = false;
               anim.SetBool("Stab", false);
               PlayerController.instance.beStabbed = false;
            }

        }
        //anim.SetBool("Stab", false);
        //PlayerController.instance.beStabbed = false;
    }
    bool checkFaceToFace()
    {
        if(Mathf.Abs(TransX - PlayerController.instance.playerTransX) < 1.6f && Mathf.Abs(scale.x + PlayerController.instance.ScaleX) < 2f
            && Mathf.Abs(TransY - PlayerController.instance.playerTransY) < 0.5f)
        {
            return true;
        }
        return false;
    }
    bool checkStandNear()
    {
        if(Mathf.Abs(TransX - PlayerController.instance.playerTransX) < 0.2f && Mathf.Abs(TransY - PlayerController.instance.playerTransY) < 0.5f)
        {
            return true;
        }
        return false;
    }
    void BeStabbed()
    {
        if (checkFaceToFace()||checkStandNear())
        {
            
            if (PlayerController.instance.slash == true )
            {
                anim.SetBool("Stabbed", true);
                speed = 0f;
                
                slash++;
                timeFire += UnityEngine.Time.deltaTime*10;
                if (timeFire > 3f)
                {
                   
                    Instantiate(fire, transform.position, Quaternion.identity);
                    timeFire = 0f;
                }
                //Debug.Log(slash.ToString());
            }
            else
            {
                anim.SetBool("Stabbed", false);
            }

        }
        else
        {
            anim.SetBool("Stabbed", false);
        }          
    }
    
    void Died()
    {
        ratio = GameMangerController.instance.GetLevel();
        if (ratio == 0)
        {
            ratio = 1;
        }

        if (slash >= 50 * ratio * 1.2f)
        {
            speed = 0f;
            
            if (!audioSource.isPlaying && aud)
            {
                audioSource.PlayOneShot(audioDied);
                aud = false;
            }
            anim.SetBool("Stabbed", false);
            anim.SetBool("Died", true);
            Destroy(gameObject, 1.6f);
            //slash = 0;
            if (!enemyDown)
            {
                EnemyCount();
            }
        }

    }
    void EnemyCount()
    {
        enemyDown = true;
        DoorController.instance.DecrementEnemy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            speed = 0f;
            stab = true;

        }
        if (collision.gameObject.tag == "Redirect")
        {
            //Destroy(gameObject);
            trigger = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        //anim.SetBool("Stab", false);
    }
}
