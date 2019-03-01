using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {
    private Slider slider;
    public float hp = 100f, timer;
	// Use this for initialization
	void Start () {
        slider = GameObject.Find("HP Slider").GetComponent<Slider>();
        slider.maxValue = 100f;
        slider.minValue = 0f;
        slider.value = slider.maxValue;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //hp -= UnityEngine.Time.deltaTime * 10;
        if(hp != 100f)
        {
            //Debug.Log("" + hp);
            slider.value = hp;
        }
        if (hp <= 0f)
        {
            timer += UnityEngine.Time.deltaTime*10;
            PlayerController.instance.died = true;
            if (timer > 20f)
            {
                GetComponent<GamePlayController>().GameOver();
            }
            
        }
        
	}
}
