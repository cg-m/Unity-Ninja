using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirectionController : MonoBehaviour {

    public BoxCollider2D boxCollider2d;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player"|| collision.gameObject.tag == "Knife")
        {
            boxCollider2d.isTrigger = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Knife")
        {
            boxCollider2d.isTrigger = true;
        }
    }
}
