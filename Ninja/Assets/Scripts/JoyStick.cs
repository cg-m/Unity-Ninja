using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class JoyStick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    private PlayerController player;
    //int keyCode;
    // Use this for initialization
    int rd;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (gameObject.name == "Left Button")
        {
            //Debug.Log("Left");
            player.SetMoveLeft(true);           
           
        }
        else if(gameObject.name == "Right Button")
        {
            player.SetMoveRight(true);
            //Debug.Log ("Right");
        }
        else if(gameObject.name=="Jump Button")
        {
            player.SetJump(true);
        }
        else
        {
            //Debug.Log("" + PlayerController.instance.timeSlash);
            rd = Random.RandomRange(1, 4);
            switch (rd)
            {
                case 1:
                    player.SetSlash(1);
                    break;
                case 2:
                    player.SetSlash(2);
                    break;
                case 3:
                    player.SetSlash(3);
                    break;
            }
                
            
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if(gameObject.name == "Left Button" || gameObject.name == "Right Button")
        {
            player.StopMoving();
        }
        
    }
}
