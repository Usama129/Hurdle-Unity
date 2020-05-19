using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Animator anim;
    private Rigidbody rg;
    private float ms;
    private float pivotms;
    private bool onGround = false;
    void Start()
    {
        anim = GetComponent<Animator>();
        rg = GetComponent<Rigidbody>();
        ms = 5000f;
        pivotms = 200f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float pivot = Input.GetAxis("Horizontal");

        //float walkInput = Input.GetAxis("Vertical");
        //if (walkInput>0)
        //{  
        //    //anim.SetBool("walking", true);

        rg.AddForce(0f, 0f, ms * Time.deltaTime);

        //}
        //else
        //{
        //    anim.SetBool("walking", false);
        //}
        Debug.Log(onGround);
        if (onGround && Input.GetKey("space"))
        {
            Debug.Log("jumping1");
            anim.SetBool("jumping", true);

            jump();
        }
        if (onGround == false)
        {
            rg.drag = 0f;
        }
        if (onGround == true)
        {
            anim.SetBool("jumping", false);
        }
        if (pivot > 0)
        {
            rg.AddForce(pivotms, 0, 0);
        }
        else if (pivot < 0)
        {
            Debug.Log("left arrow");
            rg.AddForce(-pivotms, 0, 0);
        }

    }

    void jump()
    {
        Debug.Log("jumping2");
        onGround = false;
        rg.AddForce(0, 300f, 0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            Debug.Log("onground");
            rg.drag = 10f;
            onGround = true;

        }


    }
}
