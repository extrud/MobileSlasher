using UnityEngine;
using System.Collections;

public class AttackScript : MonoBehaviour {
    

    public void Attack(float DMGCount,Vector2 pos, float radius)
    {
        RaycastHit2D[] rch2d = Physics2D.CircleCastAll(pos, radius, Vector2.zero);
        foreach (var h in rch2d)
        {
            DemageScript ds = h.collider.gameObject.GetComponent<DemageScript>();
            if (ds != null)
            {
                if(ds.gameObject != this.gameObject)
                ds.TakeDMG(DMGCount);
            }
            
        }
    }
	
}
