using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    private static int speedobs = -500; 
    private Rigidbody rg;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, -20);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddForce(0f, 0f, speedobs * Time.deltaTime);
        GetComponent<Rigidbody>().velocity = velocity;


    }

    public void renew() {
        Vector3 newPosition = transform.position; // We store the current position
        newPosition.z = 50; // We set a axis, in this case the z axis
        transform.position = newPosition; // We pass it back
    }

    public static void IncreaseSpeed(){
        speedobs-=1000;
    }

}
