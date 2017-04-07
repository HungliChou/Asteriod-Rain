using UnityEngine;
using System.Collections;

public class MovingBackGround : MonoBehaviour {

    private Material myMat;                     //Background: material
    private float offSet;                       //Background: offset
    private const float movingSpeed = 0.003f;   //Background: moving spead

    void Awake(){
        myMat = GetComponent<Renderer>().material;
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
