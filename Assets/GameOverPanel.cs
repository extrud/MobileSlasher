using UnityEngine;
using System.Collections;

public class GameOverPanel : MonoBehaviour {
    public GameObject rt;
	// Use this for initialization
	void Awake () {
        
        GameControl.GameOverPanel = rt;
       
    }
	
	
}
