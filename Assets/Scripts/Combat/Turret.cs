using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class Turret : ScoutShip
{
    [SerializeField] public Transform target;
    [SerializeField]public List<Collider> validTargets = new List<Collider> ();
    [SerializeField] public GameObject anchor;
    [SerializeField] public Vector3 offset = Vector3.zero; 
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        base._Rbody = gameObject.GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = anchor.transform.position + offset;
        transform.rotation = anchor.transform.rotation; 
        FindTarget();
        if(target != null)
        {
            //FaceTarget();
            FaceTargetFull(); 
            Shoot(); 
        }
    }

    

    public override void HandleAbility(bool input)
    {
        //Do nothing
    }



    public void FindTarget()
    {
        if(target == null)
        {
            
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1000, 6);
            foreach (Collider collider in hitColliders)
            {
                if (collider.transform.CompareTag("Plane") || collider.transform.CompareTag("Player"))
                {
                    validTargets.Add(collider);
                }

            }

            Collider min = GetClosestTarget(validTargets);
            if(min != null)
            {
                Transform t = GetClosestTarget(validTargets).transform;

                target = t;

                Debug.Log("Turret Found Target");
            }
           
        }
    }

    public void FaceTarget()
    {
        if (target == null)
        {
            return; 
        }
        transform.LookAt(target);
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles.x = 0;
        eulerAngles.z = 0;
        transform.rotation = Quaternion.Euler(eulerAngles);
    }

    public void FaceTargetFull()
    {
        if (target == null)
        {
            return;
        }
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        float rotstrength = Mathf.Min(360 * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotstrength);
    }

    public void Shoot()
    {
        base.HandleShoot(true); 
    }



    Collider GetClosestTarget(List<Collider> targets)
    {
        Collider tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos =transform.position;
        foreach (Collider t in targets)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

   protected override void OnDeath()
    {
        ParticleManager.Instance.InstantiateExplosion(this.gameObject);
        gameObject.SetActive(false);

    }
}
