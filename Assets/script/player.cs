using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class player : MonoBehaviour
{
    public Animator anim;
    private Rigidbody rg;
    private float ms;
    private float pivotms;
    private bool onGround = false;
    private bool gotHit = false;
    private bool finishLine = false;
    void Start()
    {
        anim = GetComponent<Animator>();
        rg = GetComponent<Rigidbody>();
        ms = 40000f;
        pivotms = 200f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float pivot = Input.GetAxis("Horizontal");
        float jumps = Input.GetAxis("Vertical");

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
        //jumping by pressing space
        if (onGround && jumps>0)
        {
            
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
        //turning left and right
        if (pivot > 0)
        {
            rg.AddForce(pivotms, 0, 0);
        }
        else if (pivot < 0)
        {
            
            rg.AddForce(-pivotms, 0, 0);
        }

        if (gotHit == true)
        {
            rg.AddForce(0f, 0f, 0f);
            Debug.Log("got hit before");
            anim.SetBool("gotHit", true);
            Debug.Log("got hit after");
            
        }
        //stop the crouching loop
        if (Input.GetKey("r"))
        {
            gotHit = false;
            Debug.Log("cancel hit before");
            anim.SetBool("gotHit", false);
            Debug.Log("cancel hit after");
        }
        if (onGround==true &&jumps<0)
        {
           
            anim.SetBool("crouching", true);
            onGround = false;
        }

        if (finishLine == true)
        {
            anim.SetBool("finished", true);
        }
        //if (onGround == false)
        //{
        //    anim.SetBool("crouching", false);
        //}
        //anim.SetBool("crouching", false);
    }
    //jumping function
    void jump()
    {
        
        onGround = false;
        rg.AddForce(0, 300f, 0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            Debug.Log("grouund hit");
            rg.drag = 10f;
            onGround = true;

        }

        if (collision.gameObject.CompareTag("obstacles"))
        {
           
            gotHit = true;
           
           
        }
        if (collision.gameObject.CompareTag("FinishLine"))
        {
            Debug.Log("won");
            print("congratulations!! you won the first level!!");
            finishLine = true;
        }


    }
}
