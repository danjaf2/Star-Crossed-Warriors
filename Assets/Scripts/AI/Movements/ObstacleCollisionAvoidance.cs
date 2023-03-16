using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollisionAvoidance : AIMovement
{
    public LayerMask layerMask;
    public float avoidanceDistanceFactor = 0.1f;
    public float avoidanceDistanceMinimum = 250;
    public float avoidanceTime = 4f;
    bool avoiding = false;
    Quaternion prevRotation;
    Vector3 prevDirection;

    private Rigidbody rb;
    public override SteeringOutput GetKinematic(AIAgent agent)
    {
        if(rb == null)
        {
            rb= GetComponent<Rigidbody>();
        }
        var output = base.GetKinematic(agent);
        if(avoiding)
        {
            output.linear = prevDirection;
            output.angular = prevRotation;
            Debug.DrawRay(transform.position, output.linear * 20, Color.green);
            return output;
        }
        Vector3 desiredVelocity = Vector3.zero;
        RaycastHit hit;
        Debug.DrawRay(transform.position, rb.velocity * 20, Color.yellow);
        //Debug.Log(Vector3.Magnitude(rb.velocity)/avoidanceDistanceFactor);
        float distance;
        Vector3 checkDirection;
        if(Vector3.Magnitude(rb.velocity)< 50)
        {
            distance = avoidanceDistanceMinimum;
            checkDirection = transform.forward;
        }
        else
        {
            distance = Vector3.Magnitude(rb.velocity) / avoidanceDistanceFactor;
            checkDirection = rb.velocity;
        }
        //Will need to replace the Raycast to more like a Cylinder cast so like that the whole body is safe and not just the direction we are going
        if (Physics.Raycast(transform.position, checkDirection, out hit,distance, layerMask, QueryTriggerInteraction.Collide))
        {
            desiredVelocity = hit.normal;
            output.linear = desiredVelocity * weight;
            output.angular = Quaternion.LookRotation(desiredVelocity);
            prevRotation = output.angular; 
            prevDirection = desiredVelocity;
            StartCoroutine(Avoiding());
            Debug.Log(hit.transform.name);
        }
     
        Debug.DrawRay(transform.position, output.linear * 20, Color.green);


        return output;
    }

    public override SteeringOutput GetSteering(AIAgent agent)
    {
        var output = base.GetSteering(agent);

        // TODO: calculate linear component
        output.linear = GetKinematic(agent).linear - agent.Velocity;

        return output;
    }


    IEnumerator Avoiding()
    {
        avoiding= true;
        GetComponent<Seek>().canPerform = false;
        yield return new WaitForSeconds(avoidanceTime);
        avoiding= false;
        GetComponent<Seek>().canPerform = true;
        prevRotation = Quaternion.identity;
        prevDirection = Vector3.zero;
    }
}

