using UnityEngine;

namespace AI
{
    public class FaceTarget : AIMovement
    {
        public override SteeringOutput GetKinematic(AIAgent agent)
        {
            var output = base.GetKinematic(agent);

            // TODO: calculate angular component
            //print("Turning?");
            Vector3 direction = agent.TargetPosition - this.transform.position;
            output.angular = Quaternion.LookRotation(direction.normalized);
            return output;
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            var output = base.GetSteering(agent);

            // TODO: calculate angular component
            output.angular = Quaternion.FromToRotation(transform.forward, GetKinematic(agent).angular*Vector3.forward);
            
            return output;
        }
    }
}
