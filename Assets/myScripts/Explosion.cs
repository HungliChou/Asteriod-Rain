using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
