using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollisionAvoidance : AIMovement
{
    public LayerMask layerMask;
    public float avoidanceDistanceFactor = 0.1f;
    public float avoidanceDistanceMinimum = 250;
    public GameObject objectToAvoid=null;
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
        
        Vector3 desiredVelocity = Vector3.zero;
        RaycastHit hit;
        //Debug.DrawRay(transform.position, rb.velocity * 20, Color.yellow);
        //Debug.Log(Vector3.Magnitude(rb.velocity)/avoidanceDistanceFactor);
        float distance;
        Vector3 checkDirection;
        
        {
            distance = Vector3.Magnitude(rb.velocity) * avoidanceDistanceFactor;
            checkDirection = rb.velocity;
        }
        if (Physics.CapsuleCast(transform.position, transform.position + checkDirection.normalized, 20, checkDirection, out hit, distance, layerMask, QueryTriggerInteraction.Collide))
        {
            if (avoiding&&hit.transform.gameObject==objectToAvoid)
            {
                output.linear = prevDirection;
                output.angular = prevRotation;
                Debug.DrawRay(transform.position, output.linear * 20, Color.green);
                return output;
            }

            desiredVelocity = Vector3.Cross(hit.normal, transform.forward);
            objectToAvoid = hit.transform.gameObject;
            output.linear = desiredVelocity * weight;
            output.angular = Quaternion.LookRotation(desiredVelocity);
            prevRotation = output.angular; 
            prevDirection = desiredVelocity;
            StartCoroutine(Avoiding());
            //Debug.Log(hit.transform.name);
        }
     
        Debug.DrawRay(transform.position, output.linear * 20, Color.magenta);


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
        GetComponent<FaceTarget>().canPerform = false;
        yield return new WaitForSeconds(avoidanceTime);
        objectToAvoid= null;
        avoiding= false;
        GetComponent<Seek>().canPerform = true;
        GetComponent<FaceTarget>().canPerform = true;
        prevRotation = Quaternion.identity;
        prevDirection = Vector3.zero;
    }
}

