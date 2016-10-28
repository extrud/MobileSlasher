using UnityEngine;
using System.Collections;

public class TouchScreenMove : MonoBehaviour {
    bool touch;
    bool Live = true;
    public float MaxSTM;
    public float STMReg;
    DemageScript dms;
    public float CurrentSTM;
    public float DashSTMCoast;
    public float AttackSTMCoast;
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
        Input.simulateMouseWithTouches = true;
        ms = GetComponent<MoveScript>();
        AtS = GetComponent<AttackScript>();
        
        dms = GetComponent<DemageScript>();
        dms.OnDeath += OnDie;
        dms.OnDMG += Dmg;
    }
    void OnDie()
    {
        anim.Play("Die");
        Live = false;
    }
    IEnumerator DeshCD()
    {
        DeshInCD = true;
        yield return new WaitForSeconds(DeshCoolDown);
        DeshInCD = false;

    }
    IEnumerator PainC()
    {
        dms.Invulnerable = true;
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
        dms.Invulnerable = false;
    }
    void Dmg()
    {
        if (PainCur != null)
            StopCoroutine(PainCur);
       PainCur=StartCoroutine(PainC());
    }
    IEnumerator DashCur(Vector2 dir)
    {
        dms.Invulnerable = true;
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
                        time = 0;
                        Attack(dir);
                    }
                }

            }
        }
        if(!attack)
        StartCoroutine(DeshCD());
        CamControl.blur.enabled = false;
        anim.SetBool("Dash", false);
        dms.Invulnerable = false;
        indash = false;
    }
    void Attack(Vector2 dir)
    {
        if (CurrentSTM >= AttackSTMCoast)
        {
            CurrentSTM -= AttackSTMCoast;
            attack = true;
            AtS.Attack(2, new Vector2(transform.position.x, transform.position.y) + dir * 2, 2);
            anim.Play("Slash", 0, 0);
        }
    }
    void Desh(Vector2 dir)
    {
        if (CurrentSTM >= DashSTMCoast)
        {
            
            CurrentSTM -= DashSTMCoast;
            DeshCur = StartCoroutine(DashCur(dir));
        }
        
    }

    void Update () {

        if (!Live)
        {
            return;
        }
        if (inpain)
        {
            return;
                }
        if (CurrentSTM < MaxSTM)
        {
            if (!indash)
            {
                CurrentSTM += STMReg * Time.deltaTime;
                if (CurrentSTM > MaxSTM)
                {
                    CurrentSTM = MaxSTM;
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                attack = true;
                startTouchpos = Input.mousePosition;
                curentTouchpos = Input.mousePosition;
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
                if (Input.mousePosition !=oldmousePosition)
                       {
                    curentTouchpos = Input.mousePosition;
                }
                if ((curentTouchpos - startTouchpos).magnitude > 20)
                {
                    anim.SetBool("Move", true);
                    ms.Move((curentTouchpos - startTouchpos).normalized * speed * Time.deltaTime);
                }
            }
            
        }
        else
        {
            
            if (attack)
            {
                if ((curentTouchpos - startTouchpos).magnitude > 5)
                {
                    if (!indash && !DeshInCD)
                    {
                        Desh((curentTouchpos - startTouchpos).normalized);
                        attack = false;
                    }
                }
            }
            anim.SetBool("Move", false);
            curentTouchpos = startTouchpos;
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

