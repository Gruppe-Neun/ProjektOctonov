using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PA_WarriorBehavior : Enemy
{
    [SerializeField] private Light weaponLight;
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 5; //per Attack
    //[SerializeField] private float attackDelay = 1; //max attacks per second


    private Animation animation;
    private NavMeshAgent agent;
    private IDamageableFriendly target;
    private Transform targetTransform;
    private LineRenderer laser;
    

    private int status = 0;//0 undef, 1 walking, 2 attacking, 3 stunned, 4 death

    //attack animation time: 2.917
    private float attackTime = 0f;


    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        setTarget(GameObject.FindGameObjectWithTag("Player"));
        animation = GetComponent<Animation>();
        laser = GetComponent<LineRenderer>();
        animation.wrapMode = WrapMode.Loop;
    }

    // Update is called once per frame
    void Update() {

    }

    void FixedUpdate() {
        switch (status) {
            case 0 :
                if (!agent.isOnNavMesh) {
                    DestroyImmediate(GetComponent<NavMeshAgent>());
                    gameObject.AddComponent(typeof(NavMeshAgent));
                    agent = GetComponent<NavMeshAgent>();
                    agent.agentTypeID = -1372625422;
                } else {
                    status = 1;
                }
                break;

            case 1 :
                if (Vector3.Distance(agent.transform.position, targetTransform.position) < attackRange+agent.stoppingDistance) {
                    float rotToTarget = Quaternion.FromToRotation(Vector3.forward, new Vector3(targetTransform.position.x - transform.position.x, 0, targetTransform.position.z - transform.position.z)).eulerAngles.y;
                    if (Mathf.DeltaAngle(transform.eulerAngles.y, rotToTarget) > 30) {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.MoveTowardsAngle(transform.eulerAngles.y, rotToTarget, Time.deltaTime * agent.angularSpeed), transform.eulerAngles.z);
                        if (!animation.IsPlaying("PA_WarriorRight_Clip")) {
                            animation.Play("PA_WarriorRight_Clip");
                        }
                    } else {
                        if (Mathf.DeltaAngle(transform.eulerAngles.y, rotToTarget) < -30) {
                            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.MoveTowardsAngle(transform.eulerAngles.y, rotToTarget, Time.deltaTime * agent.angularSpeed), transform.eulerAngles.z);
                            if (!animation.IsPlaying("PA_WarriorLeft_Clip")) {
                                animation.Play("PA_WarriorLeft_Clip");
                            }
                        } else {
                            attack();
                        }
                    }
                } else {
                    if (!animation.IsPlaying("PA_WarriorForward_Clip")) {
                        animation.Play("PA_WarriorForward_Clip");
                    }
                    agent.SetDestination(targetTransform.position);
                }
                break;

            case 2:
                attackTime += Time.deltaTime;
                if (attackTime < 0.84f) {

                } else {
                    if (attackTime < 0.417f + 0.84f) {
                        //prepare
                        float targetHorizontal = Quaternion.FromToRotation(transform.forward, new Vector3(targetTransform.position.x - transform.position.x, 0, targetTransform.position.z - transform.position.z)).eulerAngles.y;
                        transform.eulerAngles += new Vector3(0, Mathf.MoveTowardsAngle(0,targetHorizontal,Time.deltaTime*agent.angularSpeed/2), 0);
                        weaponLight.gameObject.SetActive(true);
                        weaponLight.intensity = attackTime * 240;
                    } else {
                        if (attackTime < 1.667f + 0.84f) {
                            //shoot laser
                            RaycastHit hit;
                            if (Physics.Raycast(laserOrigin.position, laserOrigin.forward, out hit, attackRange)) {
                                IDamageableFriendly target = hit.transform.GetComponent<IDamageableFriendly>();
                                    if (target != null) {
                                        target.TakeDamage(attackDamage*Time.deltaTime/1.25f);
                                    }

                            }
                            weaponLight.intensity = 100;
                            laser.enabled = true;
                            laser.SetPositions(new Vector3[] { laserOrigin.position, laserOrigin.position + laserOrigin.forward * attackRange });
                        } else {
                            if (attackTime < 2.917 + 0.84f) {
                                //cooldown
                                weaponLight.intensity = 100 - (attackTime - 1.1f) * 50;
                                laser.enabled = false;
                            } else {
                                //attack animation time: 2.917
                                status = 1;
                                weaponLight.gameObject.SetActive(false);
                                agent.isStopped = false;
                                animation.wrapMode = WrapMode.Loop;
                                animation.Play();
                            }
                        }
                    }
                }
                break;

            case 3:
                status = 1;
                break;

            case 4:
                attackTime += Time.deltaTime;
                if (attackTime > 0.833) {
                    die();
                }
                break;
        }
    }

    public void setTarget(GameObject neu) {
        targetTransform = neu.transform;
        target = (GameObject.FindGameObjectWithTag("Player").GetComponent<IDamageableFriendly>());
    }

    private void attack() {
        animation.wrapMode = WrapMode.Once;
        status = 2;
        if (agent.hasPath) {
            attackTime = 0;
            animation.Play("PA_WarriorStop_Clip");
            animation.PlayQueued("PA_WarriorAttack_Clip");
            agent.ResetPath();
        } else {
            attackTime = 0.84f;
            animation.Play("PA_WarriorAttack_Clip");
        }
        agent.isStopped = true;
    }

    public override void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0f && status!=4) {
            agent.isStopped = true;
            agent.ResetPath();
            status = 4;
            animation.wrapMode = WrapMode.Once;
            animation.Play("PA_WarriorDeath_Clip");
            attackTime = 0;
        }
    }








}
