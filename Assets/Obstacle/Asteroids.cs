using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{

    public GameObject type1;
    public GameObject type2;
    public GameObject type3;
    public GameObject type4;

    public GameObject avatar;

    public GameObject[] asteroids;

    // Start is called before the first frame update
    void Start()
    {
        asteroids = new GameObject[4];
        asteroids[0] = Instantiate(type1, new Vector3(0f, 0.50F, 25f), Quaternion.identity);
        asteroids[1] = Instantiate(type2, new Vector3(0f, 0.50F, 50f), Quaternion.identity);
        asteroids[2] = Instantiate(type3, new Vector3(0f, 0.50F, 75f), Quaternion.identity);
        asteroids[3] = Instantiate(type4, new Vector3(0f, 0.50F, 90f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < asteroids.Length; i++) {
            if (asteroids[i].transform.position.z > - 3)
                asteroids[i].transform.position = new Vector3(0f, asteroids[i].transform.position.y, asteroids[i].transform.position.z - 0.5f);
            else
            {
                Destroy(asteroids[i]);
                asteroids[i] = Instantiate(type1, new Vector3(0f, 0.50F, 90f), Quaternion.identity);
            }
        }
    }
}
