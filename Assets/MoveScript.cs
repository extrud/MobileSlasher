﻿using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour
{
    public bool inmove;
    Rigidbody2D rb2d;
    // Use this for initialization
    public void Move(Vector2 dir)
    {
        inmove = true;
      
        if (dir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);

        }

        rb2d.MovePosition(new Vector2(transform.position.x, transform.position.y) + dir);

    }
    
    void FixedUpdate()
    {
        if (!inmove)
        {
            rb2d.isKinematic = true;
        }
        else
        {
            rb2d.isKinematic = false;
        }
        inmove = false;
    }
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
}
