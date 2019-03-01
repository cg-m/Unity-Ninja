using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowController : MonoBehaviour {
    public static EnemyThrowController instance;
    private Rigidbody2D myBody;
    private Animator anim;
    public GameObject knife, fire;

    public AudioSource audioSource;
    public AudioClip audioDied, audioThrow;

    public bool throww, rotateRight, rotateLeft, enemyDown, pauseThrow;
    float timeThrow, TransX, TransY, timeFire;
    Vector3 temp, scale;
    int slash = 0;
    bool startThrow, checkY, aud = true;
	// Use this for initialization
	void Start () {   
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        MakeInstance();
        DoorController.instance.enemyCount++;
    }
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
	
	// Update is called once per frame
	void Update () {
        scale = transform.localScale;
        temp = transform.position;
        TransX = temp.x;
        TransY = temp.y;
        Throw();
        ChangeDirection();
        AudioControl();
        BeStabbed();
        Died();
        StartActive();
        CheckY();
        if (PlayerController.instance.died || GamePlayController.instance.pauseGame)
        {
            audioSource.enabled = false;
        }
        else
        {
            audioSource.enabled = true;
        }
        //Debug.Log("EnemyThrow Right:"+rotateRight);


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
    void StartActive()
    {
        if (Mathf.Abs(TransX - PlayerController.instance.playerTransX) < 7f && Mathf.Abs(TransY-PlayerController.instance.playerTransY) < 0.5f)
        {
            if (!startThrow)
            {
                StartCoroutine(ThrowLoop());
            }
        }
    }
    void CheckY()
    {
        if (Mathf.Abs(TransY - PlayerController.instance.playerTransY) < 0.5f)
        {
            checkY = true;
        }

    }

    void Throw()
    {
        if (throww)
        {
            anim.SetBool("Throw", true);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioThrow);
            }
            timeThrow += UnityEngine.Time.deltaTime * 10;
            if (timeThrow > 5f)
            {
                anim.SetBool("Throw", false);
                throww = false;
                timeThrow = 0;
            }
        }

    }
    private void ChangeDirection()
    {     
        if (Mathf.Abs(TransX - PlayerController.instance.playerTransX) < 0.5f)
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
        if (true)
        {
            
            if (rotateLeft == true)
            {
                
                scale.x = -2.3f;
            }
            else if ( rotateRight == true)
            {
                
                scale.x = 2.3f;
            }
            transform.localScale = scale;
        }

    }
    bool checkFaceToFace()
    {
        if (Mathf.Abs(TransX - PlayerController.instance.playerTransX) < 0.8f && Mathf.Abs(scale.x + PlayerController.instance.ScaleX) < 2f
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

            if (PlayerController.instance.slash == true)
            {
                //anim.SetBool("Stabbed", true);
                slash++;
                pauseThrow = true;
                timeFire += UnityEngine.Time.deltaTime * 10;
                if (timeFire > 3f)
                {
                    Instantiate(fire, transform.position, Quaternion.identity);
                    timeFire = 0f;
                    pauseThrow = false;
                }
                //Debug.Log(slash.ToString());
            }
            else
            {
               // anim.SetBool("Stabbed", false);
            }

        }
    }
    void Died()
    {
        if (slash >= 1)
        {
            anim.SetBool("Died", true);
            if (!audioSource.isPlaying && aud)
            {
                audioSource.PlayOneShot(audioDied);
                aud = false;
            }
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
    IEnumerator ThrowLoop()
    {
        startThrow = true;
        yield return new WaitForSeconds(Random.RandomRange(1f, 3f));
        if (!pauseThrow && checkY)
        {
            Instantiate(knife, transform.position, Quaternion.identity);
            throww = true;
        }
        
        
        StartCoroutine(ThrowLoop());
    }
}
