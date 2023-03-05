using UnityEngine;

namespace AI
{
    public class Arrive : AIMovement
    {
        public float slowRadius;
        public float stopRadius;

        private void DrawDebug(AIAgent agent)
        {
            if (debug)
            {
                DebugUtil.DrawCircle(agent.TargetPosition, transform.up, Color.yellow, stopRadius);
                DebugUtil.DrawCircle(agent.TargetPosition, transform.up, Color.magenta, slowRadius);
            }
        }

        public override SteeringOutput GetKinematic(AIAgent agent)
        {
            DrawDebug(agent);

            var output = base.GetKinematic(agent);

            // TODO: calculate linear component
            Vector3 desiredVelocity = agent.TargetPosition - this.transform.position;
            float distance = desiredVelocity.magnitude;
            desiredVelocity = desiredVelocity.normalized * agent.maxSpeed;

            if (distance <= stopRadius)
            {
                desiredVelocity *= 0;
            } else if (distance < slowRadius)
            {
                desiredVelocity *= (distance / slowRadius);
            }

            output.linear = desiredVelocity;



			
            return output;
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            DrawDebug(agent);

            var output = base.GetSteering(agent);
            output.linear = GetKinematic(agent).linear - agent.Velocity;
            if (debug) Debug.DrawRay(transform.position + agent.Velocity, output.linear, Color.green);

            return output;
        }
    }
}
