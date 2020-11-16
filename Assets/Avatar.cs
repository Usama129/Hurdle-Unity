using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    private Animator jumpAnim;
    private GameObject ground;

    private float jumpForce = 5;

    public static bool kinectJump;

    void Start()
    {
        ground = GameObject.Find("Ground");
        jumpAnim = GetComponent<Animator>();
        kinectJump = false;
    }

    public float forwardForce = 2000f;
    public float sidewayForce = 500f;

    void FixedUpdate()
    {
        


    void FixedUpdate()
    {

        if ((Input.GetKey(KeyCode.UpArrow) || kinectJump) && IsGrounded() )
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpAnim.Play("Start Jump");
            kinectJump = false;
        }

        // The transition from Start Jump to Midair is not in code: it is in the Unity Animator view

        if (currentStateMatches("Midair"))
        {
            if (DistanceToGround() < 2.5 && VerticalVelocity() < 0)
            {
                jumpAnim.Play("End Jump");
            }
        }


        double mostLeft = -4.3;
        double mostRight = 4.3;
        Vector3 p = transform.position;


        if (Input.GetKey(KeyCode.RightArrow) && IsGrounded())
        {
            //GetComponent<Rigidbody>().AddForce(Vector3.right * 1, ForceMode.Impulse);
            GetComponent<Rigidbody>().AddForce(sidewayForce * Time.deltaTime,0,0);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && IsGrounded() )
        {
            GetComponent<Rigidbody>().AddForce(-sidewayForce * Time.deltaTime, 0, 0);
        }

        //UnityEngine.Debug.Log(GetComponent<Rigidbody>().position.x);
    }

   


    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("ground"))
        {

        }
    }

    private float DistanceToGround()
    {
       return GetComponent<Rigidbody>().position.y - ground.transform.position.y;
    }

    private bool IsGrounded()
    {
        return DistanceToGround() < 0.6; // When Avatar is on the ground, DistanceToGround() returns around 0.54
    }

    private float VerticalVelocity()
    {
        return GetComponent<Rigidbody>().velocity.y;
    }

    private bool currentStateMatches(string state) // checks if a state matches the current state in the Jump Animator
    {
        return jumpAnim.GetCurrentAnimatorStateInfo(0).IsName(state);
    }

}
