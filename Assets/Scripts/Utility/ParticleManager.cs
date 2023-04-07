using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] public GameObject explisionPrefab; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateExplosion(GameObject target)
    {
        GameObject inst = GameObject.Instantiate(explisionPrefab, target.transform);

        GameObject.Destroy(inst, 2f);
    }
}
