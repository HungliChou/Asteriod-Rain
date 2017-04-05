﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public float waitForSpawn;

    //speed of spawing objects
    public float spawningSpeed;

    [SerializeField]
    private int score;
    public int Score{get{return score;} set{score = value;}}
    [SerializeField]
    private int lives;
    public int Lives{get{return lives;} set{score = lives;}}

    private bool checkFlash;
    private float flashTimer;
    public float flashTimeValue;


    public SpriteRenderer hitSignRenderer;

    void Awake()
    {
        instance = this;
        score = 0;
        lives = 5;
        checkFlash = false;
        flashTimer = 0;
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
        yield return new WaitForSeconds(waitForSpawn);
        while(true)
        {
            objRandom = 1;
            Vector2 spawnPos = new Vector2(Random.Range(-objSpawnPos.x, objSpawnPos.x), objSpawnPos.y);
            InstantiateObject(objRandom, spawnPos);
            yield return new WaitForSeconds(spawningSpeed); 
        }
    }

    public void InstantiateObject(int type,Vector2 pos)
    {
        GameObject obj = Instantiate(objects[type], pos, Quaternion.identity) as GameObject;
    }


    public void Behit()
    {
        lives -= 1;
        if(lives==0)
            print("d");

        hitSignRenderer.enabled = false; //reset renderer to cause flase, no matter it is flashing red or not.
        hitSignRenderer.enabled = true;
        checkFlash = true;
        flashTimer = flashTimeValue;
    }
}
