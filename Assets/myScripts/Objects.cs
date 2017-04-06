using UnityEngine;
using System.Collections;

public enum Asteriods{
    normal, super
}

public class Objects : MonoBehaviour {

    public Asteriods asteriod;
    public GameObject explosionPrefab;
    public AudioClip ExplosionSound;

    private Transform myTransform;
    private float posY;
    private float speed;
    private float maxPosX;  //this value is used to prevent the asteriod out of screen.

    void Awake()
    {
        myTransform = transform;
        posY = myTransform.position.y;

    }

    void Start()
    {
        maxPosX = GameManager.GetInstance.objSpawnPos.x;
        speed = Random.Range(0.3f, 0.7f);
    }

	// Update is called once per frame
	void Update () {
        myTransform.position -= new Vector3(0,speed,0);
        posY -= speed;
        if(posY<-5)
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
