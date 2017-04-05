using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    //the vector3 position of the button click
    Vector3 point;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            //point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
	}
}
