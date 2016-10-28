using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class StaminaBar : MonoBehaviour {
    TouchScreenMove tsm;
    DemageScript dms;
    RectTransform img;
    public RectTransform hp;
	// Use this for initialization
	void Start () {
        tsm = PlayerScript.CurPlayer.GetComponent<TouchScreenMove>();
        dms = PlayerScript.CurPlayer.GetComponent<DemageScript>();
        img = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {

        img.localScale = new Vector3(tsm.CurrentSTM / tsm.MaxSTM, img.localScale.y);
        hp.localScale = new Vector3(dms.HP / dms.MaxHP, hp.localScale.y);
	}
}
