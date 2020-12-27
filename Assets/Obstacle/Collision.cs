using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y > 2)
            gameObject.transform.position = new Vector3(0f, 0.5f, 90f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Avatar")
        {
            
        }
    }
}
