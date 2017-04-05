using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum PlayerStates{Alive, Dead};

public class GameManager : MonoBehaviour {

    //Singleton private instance
    private static GameManager instance;
   
    //Singleton instance getter
    public static GameManager GetInstance{get{return instance;}}

    //Lists of prefabs
    public List<GameObject> objects = new List<GameObject>();

    //random kind of the asteriod
    int objRandom;

    //spawning position of asteriods
    public Vector2 objSpawnPos;

    //wait seconds before spwaning objects
    public int waitForSpawn;

    //speed of spawing objects
    public float spawningSpeed;

    [SerializeField]
    private int score;
    public int Score{get{return score;} set{score = value;}}

    [SerializeField]
    private int lives;
    public int Lives{get{return lives;} set{score = lives;}}

    [SerializeField]
    private PlayerStates playerState;
    public PlayerStates PlayerState{get{return playerState;}set{playerState = value;}}

    private bool checkFlash;
    private float flashTimer;
    public float flashTimeValue;


    public SpriteRenderer hitSignRenderer;

    public GameObject gameoverPanel;
    public Text readyCountDownText;
    public Text currentScoreText;
    public Text finalScoreText;
    public Slider livesSlider;

    void Awake()
    {
        instance = this;
        score = 0;
        lives = 5;
        playerState = PlayerStates.Dead;
        checkFlash = false;
        flashTimer = 0;
        livesSlider.value = lives;
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnRandomObject());
	}
	
	// Update is called once per frame
	void Update () {
        if(checkFlash)
            CheckFlashTimer();
	}

    void CheckFlashTimer()
    {
        if(flashTimer>0)
        {
            flashTimer -= Time.deltaTime;
        }
        else
        {
            flashTimer = 0;
            hitSignRenderer.enabled = false;
        }
    }

    IEnumerator SpawnRandomObject()
    {
        yield return StartCoroutine(CountDownReady());   //Wait for seconds -> start playing

        while(true)
        {
            objRandom = 1;
            Vector2 spawnPos = new Vector2(Random.Range(-objSpawnPos.x, objSpawnPos.x), objSpawnPos.y);
            InstantiateObject(objRandom, spawnPos);
            yield return new WaitForSeconds(spawningSpeed); 
        }
    }
        
    IEnumerator CountDownReady()
    {
        readyCountDownText.gameObject.SetActive(true);
        for(int i = waitForSpawn; i>0; i--)
        {
            readyCountDownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        readyCountDownText.text = "Start!";
        yield return new WaitForSeconds(0.5f);
        readyCountDownText.gameObject.SetActive(false);
        playerState=  PlayerStates.Alive;
    }

    public void InstantiateObject(int type,Vector2 pos)
    {
        Instantiate(objects[type], pos, Quaternion.identity);
    }

    public void AddScore()
    {
        score += 1;
        currentScoreText.text = score.ToString();
    }

    public void Behit()
    {
        lives -= 1;
        livesSlider.value = lives;
        if(lives==0)
        {
            playerState = PlayerStates.Dead;
            finalScoreText.text = score.ToString();
            gameoverPanel.SetActive(true);
        }

        hitSignRenderer.enabled = false; //reset renderer to cause flase, no matter it is flashing red or not.
        hitSignRenderer.enabled = true;
        checkFlash = true;
        flashTimer = flashTimeValue;
    }
}
