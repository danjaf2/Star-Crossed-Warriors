using UnityEngine;

namespace AI
{
    public class Wander : AIMovement
    {
        public float wanderDegreesDelta = 45;
        [Min(0)] public float wanderInterval = 0.75f;
        protected float wanderTimer = 0;

        private Vector3 lastWanderDirection;
        private Vector3 lastDisplacement;

        public override SteeringOutput GetKinematic(AIAgent agent)
        {
            var output = base.GetKinematic(agent);
            wanderTimer += Time.deltaTime;

            Vector3 desiredVelocity = output.linear;
            // TODO: calculate linear component
            wanderTimer += Time.deltaTime;

            if (lastWanderDirection == Vector3.zero)
            {
                lastWanderDirection = transform.forward.normalized * agent.maxSpeed;
            }

            if (lastDisplacement == Vector3.zero)
            {
                lastWanderDirection = transform.forward;
            }

            desiredVelocity = lastDisplacement;
            if (wanderTimer > wanderInterval)
            {
                float angle = (Random.value - Random.value) * wanderDegreesDelta;
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * lastWanderDirection.normalized;
                Vector3 circleCenter = transform.position + lastDisplacement;
                Vector3 destination = circleCenter + direction.normalized;

                desiredVelocity = destination - transform.position;
                desiredVelocity = desiredVelocity.normalized * agent.maxSpeed;

                lastDisplacement = desiredVelocity;
                lastWanderDirection = direction;
                wanderTimer = 0;
            }

            output.linear = desiredVelocity;

			
			if (debug) Debug.DrawRay(transform.position, output.linear, Color.cyan);
			
            return output;
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            var output = base.GetSteering(agent);

            // TODO: calculate linear component
            output.linear = GetKinematic(agent).linear - agent.Velocity;

            if (debug) Debug.DrawRay(transform.position + agent.Velocity, output.linear, Color.green);

            return output;
        }
    }
}
