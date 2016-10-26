using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public static GameObject CurPlayer;
	// Use this for initialization
	void Start () {
        PlayerScript.CurPlayer = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
