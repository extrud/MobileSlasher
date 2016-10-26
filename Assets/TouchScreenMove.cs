using UnityEngine;
using System.Collections;

public class TouchScreenMove : MonoBehaviour {
    bool touch;
    DemageScript dms;
    Vector2 startTouchpos;  
    Vector2 curentTouchpos;
    MoveScript ms;
    AttackScript AtS;
    float touchtime = 0f;
    bool inpain = false;
    bool indash = false;
    bool attack = false;
    bool DeshInCD;
    public float DeshCoolDown;
    public float painTime = 1f;
    public float speed = 2f;
    public float deshspeed = 4f;
    public float timetoatack;
    public float deshtime;
    bool indesh;
    Coroutine DeshCur;
    Coroutine PainCur;
    Vector3 oldmousePosition;
    Animator anim;
    Vector2 CurentPos;
    // Use this for initialization
    void Start () {
   
        anim = GetComponent<Animator>();
        Input.simulateMouseWithTouches = false;
        ms = GetComponent<MoveScript>();
        AtS = GetComponent<AttackScript>();
        
        dms = GetComponent<DemageScript>();
        dms.OnDeath += OnDie;
        dms.OnDMG += Dmg;
    }
    void OnDie()
    {
        Destroy(this.gameObject);
    }
    IEnumerator DeshCD()
    {
        DeshInCD = true;
        yield return new WaitForSeconds(DeshCoolDown);
        DeshInCD = false;

    }
    IEnumerator PainC()
    {
        if (DeshCur != null)
            StopCoroutine(DeshCur);
        CamControl.blur.enabled = false;
        anim.SetBool("Dash", false);
        indash = false;

        anim.SetBool("Painv", true);
        anim.Play("Pain",0,0);
      
        inpain = true;
        yield return new WaitForSeconds(painTime);
        inpain = false;
        anim.SetBool("Painv", false);
    }
    void Dmg()
    {
        if (PainCur != null)
            StopCoroutine(PainCur);
       PainCur=StartCoroutine(PainC());
    }
    IEnumerator DashCur(Vector2 dir)
    {
        bool attack = false;
        CamControl.blur.enabled = true;
        indash = true;
        anim.SetBool("Dash", true);
        anim.Play("Dash");
        float time = deshtime;

        while (time > 0)
        {
           ms.Move(dir*deshspeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
            RaycastHit2D[] rch2d = Physics2D.CircleCastAll( new Vector2(transform.position.x,transform.position.y)+dir*1, 1, Vector2.zero);
            foreach (var h in rch2d)
            {
                if (h.collider.gameObject != this.gameObject)
                {
                    DemageScript ds = h.collider.gameObject.GetComponent<DemageScript>();
                    if (ds != null)
                    {
                        attack = true;
                        time = 0;
                        AtS.Attack(2, new Vector2(transform.position.x, transform.position.y) + dir * 2, 2);
                        anim.Play("Slash", 0, 0);
                    }
                }

            }
        }
        if(!attack)
        StartCoroutine(DeshCD());
        CamControl.blur.enabled = false;
        anim.SetBool("Dash", false);  
        indash = false;
    }
    void Desh(Vector2 dir)
    {
        DeshCur= StartCoroutine(DashCur(dir));
    }

    void Update () {
        if (inpain)
        {
            return;
                }
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                attack = true;
                startTouchpos = Input.mousePosition;
                touchtime = 0f;
            }
            else
            {
                if (attack)
                {
                    touchtime += Time.deltaTime;
                    if (touchtime > timetoatack)
                    {
                        attack = false; touchtime = 0f;
                    }
                }
                if (oldmousePosition != Input.mousePosition)
                {
                    curentTouchpos = Input.mousePosition;
                }
            }
            anim.SetBool("Move", true);
            ms.Move((curentTouchpos - startTouchpos).normalized * speed * Time.deltaTime);
        }
        else
        {
            if (attack)
            {
               
                if (!indash&& !DeshInCD)
                Desh((curentTouchpos - startTouchpos).normalized);
                attack = false;
            }
            anim.SetBool("Move", false);
        }

        oldmousePosition = Input.mousePosition;
        //    //Debug.Log(oldmousePosition);
        //    if (Input.touchCount > 0)
        //    {
        //        if (Input.GetTouch(0).phase == TouchPhase.Began)
        //        {
        //            attack = true;
        //            startTouchpos = Input.GetTouch(0).position;
        //            touchtime = 0f;
        //        }
        //        else
        //        {
        //            if (attack)
        //            {
        //                touchtime += Time.deltaTime;
        //                if (touchtime > timetoatack)
        //                {
        //                    attack = false; touchtime = 0f;
        //                }
        //            }
        //            if (Input.GetTouch(0).phase == TouchPhase.Moved)
        //            {
        //                curentTouchpos = Input.GetTouch(0).position;
        //            }
        //        }
        //        if (!attack)
        //        {
        //            anim.SetBool("Move", true);
        //            Move((curentTouchpos - startTouchpos).normalized * speed * Time.deltaTime);
        //        }
        //        else
        //        {
        //            anim.SetBool("Move", false);
        //        }

        //    }
        //    else
        //    {
        //        if (attack)
        //        {
        //            Desh((curentTouchpos - startTouchpos).normalized);
        //            attack = false;
        //        }
        //        anim.SetBool("Move", false);
        //    }
     
    }
	}

