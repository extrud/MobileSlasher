using UnityEngine;
using System.Collections;

public static class CamControl 
    {
    public static Camera CurentCam;
    public static UnityStandardAssets.ImageEffects.CameraMotionBlur  blur;

}
public class CamInit : MonoBehaviour {

   
	// Use this for initialization
	void Start () {
        CamControl.CurentCam = GetComponent<Camera>();
        CamControl.blur = GetComponent<UnityStandardAssets.ImageEffects.CameraMotionBlur>();
    }
	
	// Update is called once per frame
	void Update () {
   
	}
}
