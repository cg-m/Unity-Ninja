using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAxeController : MonoBehaviour {
    private float speed = 0.3f;
    private Rigidbody2D myBody;
    private Animator anim;
    
    public GameObject fire;

    public AudioSource audioSource;
    public AudioClip audioHit, audioDied, audioStab;

    bool move = false, stab = false, rotateRight, rotateLeft, trigger, enemyDown, aud = true;
    float timer, TransX, TransY, timeFire;
    int countStabbed = 0, ratio;
    Vector3 temp;
    Vector3 scale;
    // Use this for initialization
	void Start () {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        DoorController.instance.enemyCount++;
    }
	
	// Update is called once per frame
	void Update () {
        
        Move();
        Slash();
        ChangeDirection();
        AudioControl();
        BeStabbed();
        Died();
        temp = transform.position;
        TransX = temp.x;
        TransY = temp.y;
        
        if (PlayerController.instance.died || GamePlayController.instance.pauseGame)
        {
            audioSource.enabled = false;
        }
        else
        {
            audioSource.enabled = true;
        }
        
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
    void Move()
    {
        myBody.velocity = new Vector2(transform.localScale.x, 0) * speed;
        if (Mathf.Abs(TransX - PlayerController.instance.playerTransX) > 0.5f && !stab)
        {
            speed = 0.5f;
        }
        else if (Mathf.Abs(TransY - PlayerController.instance.playerTransY) < 0.5f)
        {
            speed = 0f;
        }
    }
    void Slash()
    {
        if (stab)
        {
            
            timer += UnityEngine.Time.deltaTime * 10;
            if (timer > 1f && timer < 4f && !PlayerController.instance.slash && countStabbed < 50f * ratio * 1.2f
                && Mathf.Abs(TransY - PlayerController.instance.playerTransY) < 0.5f)
            {
                anim.SetBool("Slash", true);
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(audioStab);
                }
                PlayerController.instance.beStabbed = true;
                GameObject.Find("GamePlayController").GetComponent<SliderController>().hp -= 0.7f;
            }
            else if (timer >= 10f)
            {
                anim.SetBool("Slash", false);
                PlayerController.instance.beStabbed = false;
                timer = 0f;
            }
            else
            {
                anim.SetBool("Slash", false);
                PlayerController.instance.beStabbed = false;
            }
            if (Mathf.Abs(TransX - PlayerController.instance.playerTransX) > 1.6f)
            {
                stab = false;
                anim.SetBool("Slash", false);
                PlayerController.instance.beStabbed = false;
            }
        }

    }
    void ChangeDirection()
    {
        if (Mathf.Abs(temp.x- PlayerController.instance.playerTransX) < 1f && stab
            && Mathf.Abs(TransY - PlayerController.instance.playerTransY) < 0.5f)
        {
            
            if (temp.x - PlayerController.instance.playerTransX <= 0)
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
        if (true)
        {

            scale = transform.localScale;
            if (trigger && scale.x == 2f || rotateLeft == true)
            {
                trigger = false;
                scale.x = -2f;
                rotateLeft = false;
            }
            else if (trigger && scale.x == -2f || rotateRight == true)
            {
                trigger = false;
                scale.x = 2f;
                rotateRight = false;
            }
            transform.localScale = scale;
        }
    }
    bool checkFaceToFace()
    {
        if (Mathf.Abs(TransX - PlayerController.instance.playerTransX) < 1.6f && Mathf.Abs(scale.x + PlayerController.instance.ScaleX) < 2f
            && Mathf.Abs(TransY - PlayerController.instance.playerTransY) < 0.5f)
        {
            return true;
        }
        return false;
    }
    bool checkStandNear()
    {
        if (Mathf.Abs(TransX - PlayerController.instance.playerTransX) < 0.2f && Mathf.Abs(TransY - PlayerController.instance.playerTransY) < 0.5f)
        {
            return true;
        }
        return false;
    }
    void BeStabbed()
    {
        if (checkFaceToFace()||checkStandNear())
        {
            //Debug.Log("Toi Gan Roi");
           
            if (PlayerController.instance.slash==true)
            {
                anim.SetBool("Stabbed", true);
                speed = 0f;
                
                countStabbed++;
                timeFire += UnityEngine.Time.deltaTime * 10;
                if (timeFire > 3f)
                {
                    Instantiate(fire, transform.position, Quaternion.identity);
                    timeFire = 0f;
                }
                //Debug.Log("Bi Dam" + countStabbed);
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
        if (countStabbed > 50f * ratio * 1.2f)
        {
            speed = 0f;
            anim.SetBool("Stabbed", false);
            if (!audioSource.isPlaying && aud)
            {
                audioSource.PlayOneShot(audioDied);
                aud=false;
            }
            anim.SetBool("Died", true);
            
            Destroy(gameObject, 1.6f);
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
            stab = true;
            speed = 0;
            
        }
        if (collision.gameObject.tag == "Redirect")
        {
            trigger = true;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            stab = true;
            speed = 0;

        }
        if (collision.gameObject.tag == "Redirect")
        {
            trigger = true;
        }
    }
}
