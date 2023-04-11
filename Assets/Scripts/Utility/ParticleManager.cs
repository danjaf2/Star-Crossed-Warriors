using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField] public GameObject explisionPrefab;
    [SerializeField] public GameObject EMPPrefab;
    [SerializeField] public GameObject explisionPrefab2;

    protected override void Awake() {
        Instance = this;
    }

    public void InstantiateExplosion(GameObject target)
    {
        GameObject inst = GameObject.Instantiate(explisionPrefab, target.transform);

        GameObject.Destroy(inst, 2f);
    }

    public void InstantiateExplosion2(GameObject target)
    {
        GameObject inst = GameObject.Instantiate(explisionPrefab2, target.transform);

        GameObject.Destroy(inst, 2f);
    }

    public void InstantiateEMP(GameObject target)
    {
        GameObject inst = GameObject.Instantiate(EMPPrefab, target.transform);

        GameObject.Destroy(inst, 2f);
    }
}
