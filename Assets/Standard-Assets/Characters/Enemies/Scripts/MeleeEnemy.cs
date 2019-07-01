using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    public float attackDelay = 1;

    private float attackTime = 0f;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        setTarget(GameObject.FindGameObjectWithTag("Player"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!agent.isOnNavMesh) {
            DestroyImmediate(GetComponent<NavMeshAgent>());
            gameObject.AddComponent(typeof(NavMeshAgent));
            agent = GetComponent<NavMeshAgent>();
            agent.agentTypeID = -1372625422;
        } else {
            if (Vector3.Distance(agent.transform.position, targetPosition) < range) {
                agent.isStopped=true;
                attack();
            } else {
                agent.isStopped = false;
                agent.SetDestination(targetPosition);
            }
        }

       
    }

    private void attack() {
        if (Time.time >= attackTime) {
            target.TakeDamage(damage);
            attackTime = Time.time + attackDelay;
        }
    }
}
