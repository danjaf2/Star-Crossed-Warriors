using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField] public GameObject explisionPrefab;

    protected override void Awake() {
        Instance = this;
    }

    public void InstantiateExplosion(GameObject target)
    {
        GameObject inst = GameObject.Instantiate(explisionPrefab, target.transform);

        GameObject.Destroy(inst, 2f);
    }
}
