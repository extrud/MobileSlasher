using UnityEngine;
using System.Collections;
public enum enamyState
{
    stay =1,
    moveTo,
    moveFrom,
    load,
    attack,
    pain,
    RandomMove
}
public class MonsterScript : MonoBehaviour {
    DemageScript ds;
    Animator anim;
    public float timetopain;
    public float timetoload;
    public float attackCD;
    public float DMG;
    public float minidistancetoAttack;
    enamyState CurentState = enamyState.stay;
    Vector2 Target;
    AttackScript ats;
    Coroutine AICour;
    MoveScript mv;
    public float movespeed = 2f;
    float loadtime;
    float paintime;
	// Use this for initialization
	void Start () {
        ats = GetComponent<AttackScript>();
        
        anim = GetComponent<Animator>();
        ds = GetComponent<DemageScript>();
        ds.OnDMG += OnDmg;
        ds.OnDeath += OnDie;
        mv = GetComponent<MoveScript>();
        AICour= StartCoroutine(AI());
    }
    void OnDie()
    {
        Destroy(this.gameObject);
    }
    void OnDmg()
    {
        anim.Play("Pain",0,0);
        CurentState = enamyState.pain;
        paintime = timetopain;
        StopCoroutine(AICour);
    }
    IEnumerator AI(bool skipfirst =false)
    {
        
        while (true)
        {
            float time = Random.Range(0.3f, 1.5f);
            int rnom = Random.Range((int)0, (int)11);
            if (rnom > 1)
            {
                CurentState = enamyState.moveTo;
                if (rnom > 8)
                {
                    CurentState = enamyState.stay;
         
                }
            }
            else
            {
                
                CurentState = enamyState.moveFrom;
            }
            yield return new WaitForSeconds(time);
        }
    }
    void ResetAI()
    {
        if (AICour != null)
            StopCoroutine(AICour);
        AICour=StartCoroutine(AI());
    }
    IEnumerator CoolDownAtk()
    {
        yield return new WaitForSeconds(attackCD);
        ResetAI();
        
    }
	// Update is called once per frame
	void Update () {
        if (PlayerScript.CurPlayer!=null)
        {
            Target = PlayerScript.CurPlayer.transform.position;
        }
        if (CurentState == enamyState.moveTo)
        {
            if ((Target - new Vector2(transform.position.x, transform.position.y)).magnitude <= minidistancetoAttack)
            {
                StopCoroutine(AICour);
                CurentState = enamyState.load;
                loadtime = timetoload;
                anim.Play("Load", 0, 0);
            }
        }
       
        switch (CurentState)
        {
            case enamyState.stay:
                anim.SetBool("Move", false);
                break;
            case enamyState.moveTo:
                anim.SetBool("Move", true);
                mv.Move((Target - new Vector2(transform.position.x,transform.position.y)).normalized*movespeed*Time.deltaTime);
                break;
            case enamyState.moveFrom:
                anim.SetBool("Move", true);
                mv.Move((Target - new Vector2(transform.position.x, transform.position.y)).normalized *-1* movespeed * Time.deltaTime);
                break;
            case enamyState.attack:
                anim.SetBool("Move", false);
                break;
            case enamyState.pain:
                anim.SetBool("Move", false);
                paintime -= Time.deltaTime;
                if (paintime <= 0)
                {
                    ResetAI();
                }
                break;
            case enamyState.load:
                anim.SetBool("Move", false);
                loadtime -= Time.deltaTime;
                if (loadtime <= 0)
                {
                    CurentState = enamyState.attack;
                    ats.Attack(DMG, new Vector2(transform.position.x, transform.position.y) + (Target - new Vector2(transform.position.x, transform.position.y)).normalized*1,2);
                    anim.Play("Slash", 0, 0);
                    StartCoroutine(CoolDownAtk());
                }
                break;
            case enamyState.RandomMove:
                break;
            default:
                break;
        }

        
	}
}
