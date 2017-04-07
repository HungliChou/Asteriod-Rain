using UnityEngine;
using System.Collections;

public enum Asteriods{
    normal, super
}

public class Objects : MonoBehaviour {

    public Asteriods asteriod;              //Type of Asteriod
    public GameObject explosionPrefab;      //Prefab of Explosion effect
    public AudioClip ExplosionSound;        //Audio clip of Explosion sound

    private Transform myTransform;          //Globle variable of Transform
    private float posY;                     //Position Y of this gameObject
    private const float DamagePosY = -5;    //Position Y threashold of getting hit
    public float speed;                    //Moving speed(current)
    private const float minSpeed = 0.3f;    //Moving speed(minimum)
    private const float maxSpeed = 0.7f;    //Moving speed(maximum)
    private float maxPosX;                  //Maximum Spawning value of position X: This value is used to prevent the asteriod out of screen.


    void Awake()
    {
        myTransform = transform;
        posY = myTransform.position.y;
    }

    void Start()
    {
        maxPosX = GameManager.GetInstance.objSpawnPos.x;
        speed = Random.Range(minSpeed, maxSpeed);
    }

	// Update is called once per frame
	void Update () {
        myTransform.position -= new Vector3(0,speed,0);
        posY -= speed;
        if(posY<DamagePosY)
        {
            GameManager.GetInstance.Behit();
            DestroyMyself();
        }
	}

    void OnMouseDown()
    {       
        SoundManager.GetInstance.PlaySingle(AudioSources.Expolsion, ExplosionSound);
        Instantiate(explosionPrefab, myTransform.position, Quaternion.identity);
        DetermineDestroy();
        DestroyMyself();
    }

    void DestroyMyself()
    {
        GameManager.GetInstance.DestroyObject(gameObject);
    }

    void DetermineDestroy()
    {
        if(asteriod == Asteriods.normal)
            GameManager.GetInstance.AddScore();
        else if(asteriod == Asteriods.super)
        {
            for(int i=0; i<2; i++)
            {
                Vector2 spawnPos = new Vector2(GetPosX(i), (posY + i*Random.Range(5,10)));
                GameManager.GetInstance.InstantiateObject((int)Asteriods.normal,spawnPos);
            }
        }
    }

    float GetPosX(int i)
    {
        float offset = i*Random.Range(5,10);
        float posX = myTransform.position.x + offset;

        if(posX > maxPosX)
            return myTransform.position.x - offset;
        else
            return posX;
    }
}
