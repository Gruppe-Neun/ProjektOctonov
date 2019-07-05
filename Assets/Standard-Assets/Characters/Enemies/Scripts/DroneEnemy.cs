using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneEnemy : Enemy
{
    public BulletBehavior laserBullet;
    public float rotationSpeed = 1;
    public float fireRate = 0.5f;

    private float fireTime = 0f;
    private Transform origin;
    private int status = 0; //0 = undef, 1 = fly, 2 = attack

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        origin = transform;
        
    }

    // Update is called once per frame
    public override void FixedUpdate() {
        base.FixedUpdate();
        switch (status) {
            case 0:
                if (!agent.isOnNavMesh) {
                    DestroyImmediate(GetComponent<NavMeshAgent>());
                    gameObject.AddComponent(typeof(NavMeshAgent));
                    agent = GetComponent<NavMeshAgent>();
                    agent.agentTypeID = -1372625422;
                } else {
                    status = 1;
                }
                break;

            case 1:
                if (Vector3.Distance(agent.transform.position, new Vector3(targetPosition.x , agent.transform.position.y, targetPosition.z)) < range + agent.stoppingDistance) {
                    status = 2;
                    agent.isStopped = true;
                    agent.enabled = false;
                } else {
                    agent.SetDestination(targetPosition);
                    
                }
                break;

            case 2:
                if (Vector3.Distance(agent.transform.position, new Vector3(targetPosition.x, agent.transform.position.y, targetPosition.z)) > range + agent.stoppingDistance + 2) {
                    status = 1;
                    agent.isStopped = false;
                    agent.enabled = true;
                } else {
                    Debug.Log("attacking");
                    float targetHorizontal = Quaternion.FromToRotation(Vector3.forward, new Vector3(targetPosition.x - origin.position.x, 0, targetPosition.z - origin.position.z)).eulerAngles.y;
                    float distY = Vector2.Distance(new Vector2(targetPosition.x, targetPosition.z), new Vector2(origin.position.x, origin.position.z));
                    float targetVertical = Vector2.Angle(new Vector2(distY, targetPosition.y - origin.position.y), Vector2.right);
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x + Mathf.DeltaAngle(transform.eulerAngles.x, targetVertical) * Time.deltaTime, transform.eulerAngles.y + Mathf.DeltaAngle(transform.eulerAngles.y, targetHorizontal) * rotationSpeed * Time.deltaTime, 0);
                    if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.x, targetVertical)) < 10 && Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetHorizontal)) < 10) {
                        Shoot();
                    }
                }
                break;
        }
        
    }

    private void Shoot() {
        if (Time.time >= fireTime) {
            BulletBehavior bullet;
            bullet = Instantiate(laserBullet, origin.position, transform.rotation).GetComponent<BulletBehavior>();
            bullet.friendly = false;
            bullet.damage = damage;
            fireTime = Time.time + fireRate;
        }
    }

}
