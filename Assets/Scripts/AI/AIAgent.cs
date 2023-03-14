using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AI
{
    public class AIAgent : MonoBehaviour
    {
        public float maxSpeed;
        public float maxDegreesDelta;
        public bool lockY = true;
        public bool debug;

        public AIAgent AIAgentTarget;

        public GameObject SeekerPrefab;

        public Vehicle vehicle; 

        public enum EBehaviorType { Kinematic, Steering }
        public EBehaviorType behaviorType;

        private Animator animator;

        [SerializeField] private Transform trackedTarget;
        [SerializeField] private Vector3 targetPosition;
        public Vector3 TargetPosition
        {
            get => trackedTarget != null ? trackedTarget.position : targetPosition;
        }
        public Vector3 TargetForward
        {
            get => trackedTarget != null ? trackedTarget.forward : Vector3.forward;
        }
        public Vector3 TargetVelocity
        {
            get
            {
                Vector3 v = Vector3.zero;
                if (trackedTarget != null)
                {
                    AIAgent targetAgent = trackedTarget.GetComponent<AIAgent>();
                    if (targetAgent != null)
                        v = targetAgent.Velocity;
                }

                return v;
            }
        }

        public Vector3 Velocity { get; set; }

        public void TrackTarget(Transform targetTransform)
        {
            trackedTarget = targetTransform;
        }

        public void UnTrackTarget()
        {
            trackedTarget = null;
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            vehicle = GetComponent<Vehicle>(); 
        }


        private void Update()
        {
            Vector3 currentPos = this.transform.position;
            Quaternion currentRot = this.transform.rotation; 
          
                if (debug)
                    Debug.DrawRay(transform.position, Velocity, Color.red);

                Vector3 finalVelocity = new Vector3();
                Quaternion finalRotation = new Quaternion();
                if (behaviorType == EBehaviorType.Kinematic)
                {
                        GetKinematicAvg(out finalVelocity, out finalRotation);
                        Velocity = finalVelocity * maxSpeed;
                        //TODO calculate sum 
                        currentPos += Velocity * Time.deltaTime;
                        currentRot = finalRotation;

              
                } else
                {
                    GetSteeringSum(out finalVelocity, out finalRotation);
                    Vector3 acc = finalVelocity / 3;//This has not been tested in depth, the value is just to determine an accelatation for testing
                    Velocity += acc * Time.deltaTime;
                    Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);
                    currentPos += Velocity * Time.deltaTime;
                    currentRot = finalRotation;
                }
            //animator.SetBool("walking", Velocity.magnitude > 0.1);
            //animator.SetBool("running", Velocity.magnitude > maxSpeed / 2);

            //TODO 
            //vehicle.ControllerProcessing(Vector3 translation, Quaternion rotation, bool throttleOn, bool brakingOn)

        }

        private void GetKinematicAvg(out Vector3 kinematicAvg, out Quaternion rotation)
        {
            kinematicAvg = Vector3.zero;
            AIMovement[] movements = GetComponents<AIMovement>();
            Vector4 cummulitiveRot = Vector4.zero;
            Quaternion firstRotation= Quaternion.identity;
            Quaternion finalRotation = transform.rotation;
            int count = 0;
            int currentRotCount = 0;

            foreach (AIMovement movement in movements)
            {
                if (movement.canPerform) { 
                kinematicAvg += movement.GetKinematic(this).linear;
                ++count;
                if (movement.GetKinematic(this).angular != Quaternion.identity)
                {
                    if(currentRotCount== 0)
                    {
                        firstRotation = movement.GetKinematic(this).angular;
                    }
                    currentRotCount++;
                    finalRotation = AverageQuaternion(ref cummulitiveRot, movement.GetKinematic(this).angular, firstRotation, currentRotCount);
                }
                }
            }

            if (count > 0)
            {
                kinematicAvg = Vector3.ClampMagnitude(kinematicAvg, maxSpeed);
                rotation = finalRotation;
            }
            else
            {
                kinematicAvg = Velocity;
                rotation = transform.rotation;
            }
        }

        private void GetSteeringSum(out Vector3 steeringForceSum, out Quaternion rotation)
        {
            steeringForceSum = Vector3.zero;
            rotation = Quaternion.identity;
            Quaternion firstRotation = Quaternion.identity;
            Vector4 cummulitiveRot = Vector4.zero;
            int currentRotCount = 0;
            AIMovement[] movements = GetComponents<AIMovement>();
            foreach (AIMovement movement in movements)
            {
                if (movement.canPerform)
                {
                    steeringForceSum += movement.GetSteering(this).linear;
                    if (movement.GetSteering(this).angular != Quaternion.identity)
                    {
                        if (currentRotCount == 0)
                        {
                            firstRotation = movement.GetKinematic(this).angular;
                        }
                        currentRotCount++;
                        rotation = AverageQuaternion(ref cummulitiveRot, movement.GetKinematic(this).angular, firstRotation, currentRotCount);
                    }
                }

            }
        }

        //Usage:
        //Cumulative is an external Vector4 which holds all the added x y z and w components.
        //newRotation is the next rotation to be added to the average pool
        //firstRotation is the first quaternion of the array to be averaged
        //addAmount holds the total amount of quaternions which are currently added
        //This function returns the current average quaternion (needs to be incrementally added)
        public static Quaternion AverageQuaternion(ref Vector4 cumulative, Quaternion newRotation, Quaternion firstRotation, int addAmount)
        {

            float w = 0.0f;
            float x = 0.0f;
            float y = 0.0f;
            float z = 0.0f;

            if (!AreQuaternionsClose(newRotation, firstRotation))
            {

                newRotation = InverseSignQuaternion(newRotation);
            }

            //Average the values
            float addDet = 1f / (float)addAmount;
            cumulative.w += newRotation.w;
            w = cumulative.w * addDet;
            cumulative.x += newRotation.x;
            x = cumulative.x * addDet;
            cumulative.y += newRotation.y;
            y = cumulative.y * addDet;
            cumulative.z += newRotation.z;
            z = cumulative.z * addDet;

            return NormalizeQuaternion(x, y, z, w);
        }

        public static Quaternion NormalizeQuaternion(float x, float y, float z, float w)
        {

            float lengthD = 1.0f / (w * w + x * x + y * y + z * z);
            w *= lengthD;
            x *= lengthD;
            y *= lengthD;
            z *= lengthD;

            return new Quaternion(x, y, z, w);
        }

        public static Quaternion InverseSignQuaternion(Quaternion q)
        {

            return new Quaternion(-q.x, -q.y, -q.z, -q.w);
        }

        public static bool AreQuaternionsClose(Quaternion q1, Quaternion q2)
        {

            float dot = Quaternion.Dot(q1, q2);

            if (dot < 0.0f)
            {

                return false;
            }

            else
            {

                return true;
            }
        }


    }

}