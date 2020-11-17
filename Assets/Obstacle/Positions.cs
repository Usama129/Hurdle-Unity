using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : MonoBehaviour
{

    public GameObject myPrefab;
    public GameObject[] obstacles;

    // Start is called before the first frame update
    void Start()
    {
        obstacles = new GameObject[200];

        int z = 35;

        for (int i = 0; i < 5; i++)
        {
            obstacles[i] = Instantiate(myPrefab, new Vector3(0, 0.75F, z), Quaternion.identity);
            z += 25;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 5; i++) {
            if (obstacles[i].GetComponent<Rigidbody>().position.z < Avatar.pos.z - 5)
            {
                Obstacle obj = (Obstacle)obstacles[i].GetComponent(typeof(Obstacle));
                obj.renew();
            }
        }
        
    }
}
