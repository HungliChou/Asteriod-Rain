using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum PlayerStates{Alive, Dead};

public class GameManager : MonoBehaviour {


    private static GameManager instance;                                //Singleton private instance
    public static GameManager GetInstance{get{return instance;}}        //Singleton instance getter

    #region Spawning
    public List<GameObject> objects = new List<GameObject>();           //List of object prefabs
    public List<GameObject> objects_spawned = new List<GameObject>();   //List of spawned onjects
    public int waitForStart;                                            //Wait seconds before spwaning objects
    private Coroutine SpawnCoroutine;                                   //Globle variable of spawning coroutine   
    private int objRandom;                                              //Globle variable of random asteriod
    public Vector2 objSpawnPos;                                         //Spawning position of asteriods
    private const float originalSpawningSpeed = 1;                      //Speed of spawing objects (original)
    public float spawningSpeed;                                         //Speed of spawing objects (variable)
    public float difficulty;                                            //Difficulty: affect the speed of spawning
    #endregion

    #region Clock
    private const float originalClockTimer = 60;                        //Timer (original)
    private float clockTimer;                                           //Timer (current)
    private int clockTimer_int;                                         //Globle variable of timer in "Interger"
    #endregion

    #region Flash screen 
    private bool checkFlash;                                            //Flash screen boolean: To prevent from checking flash in Update() all the time.
    private float flashTimer;                                           //Flash screen timer
    public float flashTimeValue;                                        //Flash screen duration (Default is 1 second)  
    public SpriteRenderer hitSignRenderer;                              //Red sprite renderer can be enable and disable to create flash
    #endregion

    #region Data
    [SerializeField]
    private int score;                                                  //Private variable of game score
    public int Score{get{return score;} set{score = value;}}            //Game score Getter and Setter

    [SerializeField]
    private int lives;                                                  //Private variable of lives
    private const int originalLives = 5;                                //Lives (original)
    public int Lives{get{return lives;} set{score = lives;}}            //Lives Getter and Setter

    [SerializeField]
    private PlayerStates playerState;                                   //Prviate variable of current state (Alive,Dead)
    public PlayerStates PlayerState{get{return playerState;} set{playerState = value;}}  //Current state Getter and Setter
    #endregion

    #region UI 
    public GameObject gameoverPanel;                                    //Panel reference can be activate when the game is over 
    public Text readyCountDownText;                                     //Text reference for ready countdown
    public Text currentScoreText;                                       //Text reference for current score
    public Text finalScoreText;                                         //Text reference for final score
    public Text finalResultText;                                        //Text reference for final result (Congratulations/GameOver)
    public Slider livesSlider;                                          //slider reference for current lives
    public Text clockText;                                              //Text reference for clock
    public RectTransform clockTick;                                     //Transform reference for clock tick sprite
    public GameObject signPrefab;                                       //Prefab reference for sign (lives +1)
    #endregion

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
        
	void Start () {
        Initialize();
	}
	
	void Update () {
        if(playerState==PlayerStates.Alive)
        {
            CheckClockTimer();

            if(checkFlash)
                CheckFlashTimer();

            LinearDiffulty();
        }
	}

    void LinearDiffulty()
    {
        //spawn asteriods more frequently
        difficulty += 0.0002f;
        spawningSpeed = originalSpawningSpeed/difficulty;
    }

    void CheckClockTimer()
    {
        if(clockTimer>0)
        {
            clockTimer -= Time.deltaTime;
            clockTimer_int = Mathf.FloorToInt(clockTimer);
            clockText.text = clockTimer_int.ToString();
            clockTick.rotation = Quaternion.Euler(0,0,-(60-clockTimer_int)*360/60);
        }
        else
        {
            clockText.text = "0";
            GameOver(true);
        }
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
            objRandom = Random.Range(0,2);
            Vector2 spawnPos = new Vector2(Random.Range(-objSpawnPos.x, objSpawnPos.x), objSpawnPos.y);
            InstantiateObject(objRandom, spawnPos);
            yield return new WaitForSeconds(spawningSpeed); 
        }
    }
        
    public void InstantiateObject(int type,Vector2 pos)
    {
        GameObject obj = Instantiate(objects[type], pos, Quaternion.identity) as GameObject;
        objects_spawned.Add(obj);
    }

    IEnumerator CountDownReady()
    {
        readyCountDownText.gameObject.SetActive(true);
        for(int i = waitForStart; i>0; i--)
        {
            readyCountDownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        readyCountDownText.text = "Start!";
        yield return new WaitForSeconds(0.5f);
        readyCountDownText.gameObject.SetActive(false);
        playerState=  PlayerStates.Alive;
    }
        
    public void DestroyObject(GameObject obj)
    {
        objects_spawned.Remove(obj);
        Destroy(obj);
    }

    public void AddScore()
    {
        score += 1;
        currentScoreText.text = score.ToString();
        if(score%20==0)
        {
            AddHealth();
        }
    }

    IEnumerator ScoreAnimation()
    {
        currentScoreText.fontSize = 40;
        currentScoreText.color = Color.white;
        yield return new WaitForSeconds(1.5f);
        currentScoreText.fontSize = 25;
        currentScoreText.color = new Color32(23,23,23,255);
    }

    void AddHealth()
    {
        if(lives<originalLives)
        {
            lives += 1;
            SoundManager.GetInstance.PlaySingle(AudioSources.UI,SoundManager.GetInstance.RecoverySound);
            UpdateLives();
            GameObject sign = Instantiate(GameManager.GetInstance.signPrefab,transform.position,Quaternion.identity) as GameObject;
            sign.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        StartCoroutine(ScoreAnimation());
    }

    public void Behit()
    {
        if(playerState==PlayerStates.Dead)
            return;
        
        SoundManager.GetInstance.PlaySingle(AudioSources.Hit, SoundManager.GetInstance.hitSound);

        lives -= 1;
        UpdateLives();

        hitSignRenderer.enabled = false; //reset renderer to cause flase, no matter it is flashing red or not.
        hitSignRenderer.enabled = true;
        checkFlash = true;
        flashTimer = flashTimeValue;
    }

    void UpdateLives()
    {
        livesSlider.value = lives;
        if(lives==0)
        {
            GameOver(false);   
        }
    }


    void GameOver(bool pass)
    {
        StopCoroutine(SpawnCoroutine);
        playerState = PlayerStates.Dead;
        finalScoreText.text = score.ToString();
        if(pass)
        {
            SoundManager.GetInstance.PlaySingle(AudioSources.UI,SoundManager.GetInstance.VictorySound);
            finalResultText.text = "Congratulations!";
        }
        else
        {
            SoundManager.GetInstance.PlaySingle(AudioSources.UI,SoundManager.GetInstance.LoseSound);
            finalResultText.text = "Game Over";
        }
        gameoverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        //Play sound
        SoundManager.GetInstance.PlaySingle(AudioSources.UI, SoundManager.GetInstance.startSound);

        //Initialize variables and UI
        Initialize();
    }

    void Initialize()
    {
        //UI panel reset
        gameoverPanel.SetActive(false);

        //Clock reset
        clockTimer = originalClockTimer;
        clockText.text = clockTimer.ToString();
        clockTick.rotation = Quaternion.Euler(0,0,0);

        //Difficulty reset
        difficulty = 1;

        //Score reset
        score = 0;
        currentScoreText.text = score.ToString();

        //Lives reset
        lives = originalLives;
        livesSlider.value = lives;

        //PlayerState reset
        playerState = PlayerStates.Dead;

        //Flash timer reset
        checkFlash = false;
        flashTimer = 0;
        hitSignRenderer.enabled = false;

        //Clear all objects
        for(int i=0;i<objects_spawned.Count;i++)
        {
            Destroy(objects_spawned[i]);
        }
        objects_spawned.Clear();

        //Start spawning coroutine
        SpawnCoroutine = StartCoroutine(SpawnRandomObject());
    }
}
