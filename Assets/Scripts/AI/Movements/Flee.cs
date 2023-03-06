using UnityEngine;

namespace AI
{
    public class Flee : AIMovement
    {
        public override SteeringOutput GetKinematic(AIAgent agent)
        {
            var output = base.GetKinematic(agent);
            Vector3 desiredVelocity = this.transform.position - agent.TargetPosition;
            desiredVelocity = desiredVelocity.normalized * agent.maxSpeed;
            output.linear = desiredVelocity;

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
