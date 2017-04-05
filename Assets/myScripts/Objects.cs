using UnityEngine;
using System.Collections;

public enum Asteriods{
    normal, super
}

public class Objects : MonoBehaviour {

    public Asteriods asteriod;
    Transform myTransform;
    float posY;
    float speed;

    void Awake()
    {
        myTransform = transform;
        posY = myTransform.position.y;
        speed = 0.6f;
    }

    void Start()
    {
        
    }

	// Update is called once per frame
	void Update () {
        myTransform.position -= new Vector3(0,speed,0);
        posY -= speed;
        if(posY<-5)
        {
            GameManager.GetInstance.Behit();
            Destroy(gameObject);
        }
	}

    void OnMouseDown()
    {       
        DetermineDestroy();
        Destroy(gameObject);
    }

    void DetermineDestroy()
    {
        if(asteriod == Asteriods.normal)
            GameManager.GetInstance.Score += 1;
        else if(asteriod == Asteriods.super)
        {
            for(int i=0; i<2; i++)
            {
                Vector2 spawnPos = new Vector2((myTransform.position.x + i*5), (posY + i*5));
                GameManager.GetInstance.InstantiateObject((int)Asteriods.normal,spawnPos);
            }
        }
    }
}
