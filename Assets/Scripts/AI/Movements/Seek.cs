using UnityEngine;

namespace AI
{
    public class Seek : AIMovement
    {
        public override SteeringOutput GetKinematic(AIAgent agent)
        {
            var output = base.GetKinematic(agent);

            // TODO: calculate linear component
            Vector3 desiredVelocity = agent.TargetPosition - this.transform.position;
            desiredVelocity = desiredVelocity.normalized * agent.maxSpeed;
            output.linear = desiredVelocity;

            if (debug) Debug.DrawRay(transform.position, output.linear, Color.cyan);

            return output;
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            var output = base.GetSteering(agent);
            output.linear = GetKinematic(agent).linear - agent.Velocity;
            if (debug) Debug.DrawRay(transform.position + agent.Velocity, output.linear, Color.green);

            return output;
        }
    }
}
