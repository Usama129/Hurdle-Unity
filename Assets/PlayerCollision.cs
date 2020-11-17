// maaz
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    void OnCollisionEnter(Collision collisionInfo)
    {
        //to view which is the player colliding with ground or Obstac

        if (collisionInfo.collider.name == "Obstacle" || collisionInfo.collider.name == "Obstacle(Clone)")
        {
            Debug.Log("I just hit an obstacle ");
        }

    }
}