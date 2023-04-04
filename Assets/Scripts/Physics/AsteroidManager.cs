using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Asteroid> others;

    public int amountOfSpaceRocks;
    public Transform bottomCornerWaypoint;
    public Transform topCornerWaypoint;
    public int minVelocityPerAxis=-6;
    public int maxVelocityPerAxis = 6;
    public GameObject prefab;
    void Start()
    {
        others = GameObject.FindObjectsOfType(typeof(Asteroid)).Cast<Asteroid>().Where(obj => obj.gameObject != this.gameObject).ToList();

        for(int i = 0; i < amountOfSpaceRocks; i++)
        {
            float x = Random.Range(bottomCornerWaypoint.position.x, topCornerWaypoint.position.x);
            float y = Random.Range(bottomCornerWaypoint.position.y, topCornerWaypoint.position.y);
            float z = Random.Range(bottomCornerWaypoint.position.z, topCornerWaypoint.position.z);

            float xVelocity = Random.Range(minVelocityPerAxis, maxVelocityPerAxis);
            float yVelocity = Random.Range(minVelocityPerAxis, maxVelocityPerAxis);
            float zVelocity = Random.Range(minVelocityPerAxis, maxVelocityPerAxis);

            Asteroid newRock = Instantiate(prefab, new Vector3(x,y,z), Quaternion.identity).GetComponent<Asteroid>();
            newRock.velocity = new Vector3(xVelocity,yVelocity,zVelocity);
            others.Add(newRock);
            newRock.transform.parent = transform;
        }


    }

    private void FixedUpdate()
    {
        PhysicsUpdate();
    }

    void PhysicsUpdate()
    {
        foreach (Asteroid obj1 in others)
        {
            foreach (Asteroid obj2 in others)
            {
                {
                    if (obj1 != obj2)
                    {


                        if (CheckCollision(obj1, obj2))
                        {
                            CollisionResponce(obj1, obj2);
                        }

                    }
                    }

                }
        }
    }

    bool CheckCollision(Asteroid obj1, Asteroid obj2)
    {
        Vector3 distance = obj1.transform.position - obj2.transform.position;

        float length = distance.magnitude;

        // Sum of the radiuses
        float sumradius = obj1.radius + obj2.radius - 0.1f;

        if (length <= sumradius)
        {
            print("We have collision");
            return true;
        }
        return false;
    }
   

    void CollisionResponce(Asteroid obj1, Asteroid obj2)
    {
        float m1, m2, x1, x2;
        m1 = obj1.mass;
        m2 = obj2.mass;
        Vector3 v1, v2, v1x, v2x, v1y, v2y;
        Vector3 x = (obj1.transform.position - obj2.transform.position);

        x= x.normalized;
        v1 = obj1.velocity;
        x1 = Vector3.Dot(x, v1);
        v1x = x * x1;
        v1y = v1 - v1x;

        x = x * -1;
        v2 = obj2.velocity;
        x2 = Vector3.Dot(x, v2);
        v2x = x * x2;   
        v2y = v2 - v2x;

        Vector3 velocity2 = (v1x * (m1 - m2) / (m1 + m2) + v2x * (2 * m2) / (m1 + m2) + v1y);
        Vector3 velocity1 = (v1x * (2 * m1) / (m1 + m2) + v2x * (m2 - m1) / (m1 + m2) + v2y);
        obj1.velocity = velocity1;
        obj2.velocity = -velocity2;
    }
}
