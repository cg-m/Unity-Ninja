using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector2 velocity;

    private float smoothTimeX = 0.5f;
    private float smoothTimeY = 0.5f;

    private GameObject player;

    float minX = 0f, maxX = 110f;

    float posX, posY;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void FixedUpdate()
    {
        if(player.transform.position.x < minX)
        {
            posX = Mathf.SmoothDamp(transform.position.x, minX, ref velocity.x, smoothTimeX);
        }
        else if (player.transform.position.x > maxX)
        {
            float posX = Mathf.SmoothDamp(transform.position.x, maxX, ref velocity.x, smoothTimeX);
        }
        else
        {
            posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        }
        
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}
   