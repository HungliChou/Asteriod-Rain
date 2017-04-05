using UnityEngine;
using System.Collections;

public class MovingBackGround : MonoBehaviour {

    private Material myMat; 
    private float offSet;
    public float movingSpeed;

    void Awake(){
        myMat = GetComponent<Renderer>().material;
    }
	
    void Start()
    {
        movingSpeed = 0.003f;
        offSet = myMat.GetTextureOffset("_MainTex").x;
    }

	// Update is called once per frame
    void Update () {
        if(GameManager.GetInstance.PlayerState==PlayerStates.Alive)
        {
            offSet = offSet+movingSpeed;
            myMat.SetTextureOffset("_MainTex",new Vector2(offSet,0));
        }
	}
}
