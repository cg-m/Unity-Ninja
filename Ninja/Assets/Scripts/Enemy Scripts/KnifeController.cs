using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour {
    private float speed = 4f;
    bool stab, right;
    float timer, TransX;
    public GameObject fire;
    private Rigidbody2D myBody;
    Vector3 temp, tempLS;
    // Use this for initialization
    void Start () {
        myBody = GetComponent<Rigidbody2D>();
        temp = transform.position;
        tempLS = transform.localScale;
        TransX = temp.x;
        //Debug.Log("Transfom Player: " + PlayerController.instance.playerTransX);
       // Debug.Log("Kinife Player: " + TransX);
        Move();
    }
	
	// Update is called once per frame
	void Update () {
        //Move();
        Stab();
        Destroy(gameObject, 5f);
        temp = transform.position;
        tempLS = transform.localScale;
        TransX = temp.x;
        
        //Debug.Log("Knife Right:" + EnemyThrowController.instance.flag);
    }

    void Move()
    {
        if (TransX - PlayerController.instance.playerTransX <= 0)
        {
            myBody.velocity = new Vector2(2, 0) * 1.8f;
            tempLS.x = 2f;
            transform.localScale = tempLS;
        }
        else
        {
            myBody.velocity = new Vector2(-2, 0) * 1.8f;
            tempLS.x = -2f;
            transform.localScale = tempLS;
        }
        

    }
    void Stab()
    {
        if (stab)
        {
            PlayerController.instance.beStabbed = true;
            timer += UnityEngine.Time.deltaTime * 10;
            //Debug.Log("Dao dam vao nvc");
   
            if (timer > 5f)
            {
                PlayerController.instance.beStabbed = false;
                timer = 0f;
                stab = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            stab = true;
            Destroy(gameObject);
            GameObject.Find("GamePlayController").GetComponent<SliderController>().hp -= 5f;
            Instantiate(fire, transform.position, Quaternion.identity);

        }
        if(collision.gameObject.tag == "Wall" || collision.gameObject.tag =="Patch")
        {
            Instantiate(fire, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    } 
}
