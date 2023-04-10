using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrackEntitiesInArea : MonoBehaviour
{
    [Header("Note: Collider needs to be a trigger.")]
    public List<Entity> _inRange;
    public bool ally = true;
    public float range=5000;
    public float sensitivity = 1f;

    private void Awake()
    {
        _inRange = new List<Entity>();
    }

    public bool HasAny() { return _inRange.Count > 0; }
    public bool HasAny(out Entity pick)
    {
        pick = _inRange.GetRandom();
        return HasAny();
    }

    public bool Contains(Entity obj) { return _inRange.Contains(obj); }

    private void FixedUpdate()
    {//Triggers seemed to slow the bullets and was too hard to run
        if (ally)
        {

            foreach (GameObject enemy in TeamManager.Instance.enemyList)
            {
                if (enemy != null)
                {
                    if (enemy.TryGetComponent<Entity>(out var ent))
                    {
                        if (Vector3.Dot(this.transform.forward.normalized, (enemy.transform.position - this.transform.position).normalized) > sensitivity)
                        {

                            RaycastHit hit;
                            if (Physics.Raycast(transform.position, enemy.transform.position - this.transform.position, out hit, range, LayerMask.GetMask("Character", "Obstacle")))
                            {
                                if (hit.transform.gameObject == enemy)
                                {
                                    _inRange.AddUnique(enemy.GetComponent<Entity>());
                                    continue;
                                }
                            }
                        }
                        _inRange.Remove(enemy.GetComponent<Entity>());
                    }

                }
            }

        }else if (!ally)
        {
            foreach (GameObject enemy in TeamManager.Instance.alliesList)
            {
                if (enemy != null) {
                if (enemy.TryGetComponent<Entity>(out var ent))
                {
                    if (Vector3.Dot(this.transform.forward.normalized, (enemy.transform.position - this.transform.position).normalized) > sensitivity)
                    {

                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, enemy.transform.position - this.transform.position, out hit, range, LayerMask.GetMask("Character", "Obstacle")))
                        {
                            if (hit.transform.gameObject == enemy)
                            {
                                _inRange.AddUnique(enemy.GetComponent<Entity>());
                                continue;
                            }
                        }
                    }
                    _inRange.Remove(enemy.GetComponent<Entity>());
                }

            }
            }
        }
    }

        void OnTriggerEnter(Collider other)
        {
            //if(other.TryGetComponent<PlayerShip>(out var entered)) {
            //_inRange.AddUnique(entered);
            // }
        }

        private void OnTriggerExit(Collider other)
        {
            //if (other.TryGetComponent<PlayerShip>(out var exited))
            //{
            //_inRange.Remove(exited);
            //}
        }

    }