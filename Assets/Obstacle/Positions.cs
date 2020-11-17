using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : MonoBehaviour
{

    public GameObject myPrefab;
    public GameObject[] obstacles;
    int z = 25;
    int j = 0;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        obstacles = new GameObject[200];

        obstacles[j] = Instantiate(myPrefab, new Vector3(0, 0.75F, z), Quaternion.identity);
        z += 25;
    }

    // Update is called once per frame
    void Update()
    {
        float[] x = { -4.3f, 0f, 4.3f };
        float[] y = { 0.75f, 2.85f };

        System.Random rd = new System.Random();
        System.Random rd1 = new System.Random();
        int randomX = rd.Next(0, 3);
        int randomY = rd1.Next(0, 2);


        if (Avatar.pos.z > obstacles[j].GetComponent<Rigidbody>().position.z)
        {
            obstacles[j + 1] = Instantiate(myPrefab, new Vector3(x[randomX], y[randomY], z), Quaternion.identity);
            j += 1;
            count += 1;
            if (count % 5 == 0)
            {
                Obstacle.IncreaseSpeed();
            }
            UnityEngine.Debug.Log(z);
        }
    }
}
/* foreach (GameObject obs in obstacles) {
        if (obs.GetComponent<Rigidbody>().position.z < Avatar.pos.z - 5)
        {
            Obstacle obj = (Obstacle)obs.GetComponent(typeof(Obstacle));
            obj.renew();
        }
    }
    */