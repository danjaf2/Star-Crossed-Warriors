using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace AI
{
    public class Seek : AIMovement
    {
        public bool usePredictiveSeek = true;
        public float projectileSpeed = 2000;
        public override SteeringOutput GetKinematic(AIAgent agent)
        {
            var output = base.GetKinematic(agent);


            // TODO: calculate angular component
            //print("Turning?");
            Vector3 direction = Vector3.zero;
            if (!usePredictiveSeek)
            {
                direction = agent.TargetPosition - this.transform.position;
                direction = direction.normalized * agent.maxSpeed;
                output.linear = direction * weight;
                return output;
            }

            if (agent.trackedTarget != null)
            {
                if (agent.trackedTarget.TryGetComponent<Rigidbody>(out Rigidbody body))
                {
                    if(body.velocity.magnitude > 65) {
                        //print("Here");
                        Vector3 point = GetPredictedPoint(agent.TargetPosition, body.velocity, this.transform.position, projectileSpeed);
                        direction = point - this.transform.position;
                        Debug.DrawLine(this.transform.position, point);
                    }
                    else
                    {
                        direction = agent.TargetPosition - this.transform.position;
                    }

                }
            }
            else
            {
                direction = agent.TargetPosition - this.transform.position;
            }

            direction = direction.normalized * agent.maxSpeed;
            output.linear = direction * weight;
            if (debug) Debug.DrawRay(transform.position + agent.Velocity, output.linear, Color.green);

            return output;
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            var output = base.GetSteering(agent);
            output.linear = GetKinematic(agent).linear - agent.Velocity;
            if (debug) Debug.DrawRay(transform.position + agent.Velocity, output.linear, Color.green);

            return output;
        }


        Vector3 GetPredictedPoint(Vector3 targetPosition, Vector3 targetSpeed, Vector3 attackerPosition, float bulletSpeed)
        {//Quadratic formula
            Vector3 q = targetPosition - attackerPosition;
            float a = Vector3.Dot(targetSpeed, targetSpeed) - (bulletSpeed * bulletSpeed);
            float b = 2 * Vector3.Dot(targetSpeed, q);
            float c = Vector3.Dot(q, q);

            float D = Mathf.Sqrt((b * b) - 4 * a * c);

            float t1 = (-b + D) / (2 * a);
            float t2 = (-b - D) / (2 * a);
            float time = Mathf.Max(t1, t2);
            Vector3 ret = targetPosition + targetSpeed * time;
            return ret;
        }
    }
}
