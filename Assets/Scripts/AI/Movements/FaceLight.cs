
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace AI
{
    public class FaceLight : AIMovement
    {
        public GameObject lightSource;
        public override SteeringOutput GetKinematic(AIAgent agent)
        {
            var output = base.GetKinematic(agent);
        //Seems to be broken, still needs work, so it is disabled for now
            //float AngleRad = Mathf.Atan2(lightSource.transform.position.y - this.transform.position.y, lightSource.transform.position.x - this.transform.position.x);
            //float AngleDeg = (180 / Mathf.PI) * AngleRad;
           // Vector3 direction = Quaternion.Euler(0, 0, AngleDeg)*Vector3.forward;
            //output.angular = Quaternion.LookRotation(direction, Vector3.right) ;
            //output.angular = quaternion;
            return output;
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            var output = base.GetSteering(agent);

            // TODO: calculate angular component
            output.angular = Quaternion.FromToRotation(transform.forward, GetKinematic(agent).angular* UnityEngine.Vector3.forward);
            
            return output;
        }
    }
}
