using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DoorController : MonoBehaviour {

    public static DoorController instance;
    private BoxCollider2D boxCollider2D;
    public GameObject killAllEnemy, victoryPanel;

    

    public bool victory;
    [HideInInspector]
    public int enemyCount;

    float timeVictoryDelay;

    string nameLevel;
    public int level;

    // Use this for initialization
    void Awake () {
        MakeInstance();
        boxCollider2D = GetComponent<BoxCollider2D>();

        Scene scene = SceneManager.GetActiveScene();

        //Debug.Log("scene is: " + scene.name);
        string strNumber;
        if (GameMangerController.instance.GetLevel() == 0)
        {
            level = 1;
        }
        else
        {
            level = GameMangerController.instance.GetLevel();
        }
        //if (scene.name == "GamePlayLevel1")
        //{
            //strNumber = scene.name.Substring(13, 1);
            //level = int.Parse(strNumber);
          
            
            //Debug.Log("number is: " + nameLevel);
        //}
    }
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
	public void DecrementEnemy()
    {
        enemyCount--;
        if (enemyCount == 0)
        {
            StartCoroutine(OpenDoor());
        }
        GameMangerController.instance.score++;
    }

    IEnumerator OpenDoor()
    {
        killAllEnemy.SetActive(false);
        yield return new WaitForSeconds(.7f);
        boxCollider2D.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            victoryPanel.SetActive(true);
            victory = true;
            level++;
        }
    }
    // Update is called once per frame
    void Update () {
        //Debug.Log("So luong Enemy" + enemyCount);
        GameMangerController.instance.level = level;
        Victory();
        
        //Debug.Log("" + GameMangerController.instance.GetLevel());
	}

    void Victory()
    {
        if (victory)
        {
            timeVictoryDelay += Time.deltaTime * 10;
            if (timeVictoryDelay > 13f)
            {
                nameLevel = string.Concat("GamePlayLevel", GameMangerController.instance.GetLevel());
                
               // Debug.Log("Door Up Level" + nameLevel);
                victory = false;
                Application.LoadLevel(nameLevel);
                
            }
        }
        
    }
}
