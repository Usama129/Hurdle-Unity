using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    private Animator jumpAnim;
    private GameObject ground;
    private float jumpForce = 8;

    void Start()
    {
        ground = GameObject.Find("Ground");
        jumpAnim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow) && IsGrounded())
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpAnim.Play("Start Jump");
        }

        if (currentStateMatches("Start Jump"))
        {
            if (VerticalVelocity() < 0.5 && VerticalVelocity() > 0)
            {
                jumpAnim.Play("Midair");
            }
        }

        if (currentStateMatches("Midair"))
        {
            if (DistanceToGround() < 2.5 && GetComponent<Rigidbody>().velocity.y < 0)
            {
                jumpAnim.Play("End Jump");
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.right * 1, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.left * 1, ForceMode.Impulse);
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
        return Vector3.Distance(GetComponent<Rigidbody>().position, ground.transform.position);
    }

    private bool IsGrounded()
    {
        return DistanceToGround() < 0.6; // When Avatar is on the ground, DistanceToGround() returns around 0.54
    }

    private float VerticalVelocity()
    {
        return GetComponent<Rigidbody>().velocity.y;
    }

    private bool currentStateMatches(string state)
    {
        return jumpAnim.GetCurrentAnimatorStateInfo(0).IsName(state);
    }
}
