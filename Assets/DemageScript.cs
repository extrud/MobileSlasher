using UnityEngine;
using System.Collections;

public class DemageScript : MonoBehaviour {
    public delegate void dmgDlg();
    public float HP;
    public float MaxHP;
    public bool Invulnerable = false;
    public event dmgDlg OnDMG;
    public event dmgDlg OnDeath;
    public GameObject BloodParticle;
    Coroutine BloodCoroutone;
    ParticleSystem BloodPs;
    public void Start()
    {
        BloodPs = ((GameObject)Instantiate(BloodParticle, transform.position, BloodParticle.transform.rotation)).GetComponent<ParticleSystem>();
        BloodPs.Stop();
        BloodPs.transform.SetParent(this.transform);


    }
    IEnumerator Blood()
    {
        BloodPs.Play();
        yield return new WaitForSeconds(0.3f);
        BloodPs.Stop();

    }
    public void TakeDMG(float count)
    {
        if (Invulnerable)
        {
            return;
        }
        if (BloodCoroutone == null)
          BloodCoroutone= StartCoroutine(Blood());
        else
        {
            StopCoroutine(BloodCoroutone);
            BloodCoroutone = StartCoroutine(Blood());
        }
        HP -= count;
        if (HP < 0)
        {
            HP = 0;
            Invulnerable = true;
            
            if (OnDeath != null)
            {
                OnDeath.Invoke();
            }

        }
        else
        {
            if (OnDMG != null)
            {
                OnDMG.Invoke();
            }
        }
    }
}
