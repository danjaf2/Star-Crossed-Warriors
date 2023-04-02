using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour
{
    [SerializeField]
    private float tumble;
    Vector3 angularVelocity = Vector3.zero;
    

    void Start()
    {
        angularVelocity = Random.insideUnitSphere * tumble;
        
    }
    private void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(angularVelocity);
    }
}