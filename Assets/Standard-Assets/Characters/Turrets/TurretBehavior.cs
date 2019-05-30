using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public GameObject Bone_Barrel;
    public GameObject Bone_Upper;
    public BulletBehavior laserBullet;

    public float horizontalSpeed = 20;
    public float verticalSpeed = 15;

    public GameObject target;
    public AmmoBehavior ammo;
    public float range = 30;

    private Transform origin;
    private float rotationHorizontal = 0f;
    private float rotationVertical = 0f;
    private float targetHorizontal = 0f;
    private float targetVertical = 0f;

    private float fireTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        origin = Bone_Barrel.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) {
            targetPosition(target.transform.position);
            rotationHorizontal = Mathf.MoveTowardsAngle(rotationHorizontal, targetHorizontal, horizontalSpeed * Time.deltaTime);
            rotationVertical = Mathf.MoveTowardsAngle(rotationVertical, targetVertical, verticalSpeed * Time.deltaTime);
            Bone_Upper.transform.localRotation = Quaternion.AngleAxis(rotationHorizontal, Vector3.left);
            Bone_Barrel.transform.localRotation = Quaternion.AngleAxis(rotationVertical, Vector3.forward);
            if (Mathf.Abs(Mathf.DeltaAngle(rotationHorizontal, targetHorizontal)) < 30 && Mathf.Abs(Mathf.DeltaAngle(rotationVertical, targetVertical)) < 20) {
                shoot();
            }
        }
        
    }


    private void targetPosition(Vector3 pos) {
        targetHorizontal = Quaternion.FromToRotation(Vector3.left, new Vector3(pos.x-origin.position.x,0, pos.z - origin.position.z)).eulerAngles.y;
        float distY = Vector2.Distance(new Vector2(pos.x,pos.z), new Vector2(origin.position.x,origin.position.z));
        targetVertical = Vector2.Angle(new Vector2(distY, pos.y-origin.position.y), Vector2.up);
    }

    public void shoot() {
        if (ammo == null) {

        } else {
            switch (ammo.type) {
                case Item.Type.LaserBlue:
                    Shoot_LaserBlue();
                    break;

                case Item.Type.LaserRed:
                    Shoot_LaserRed();
                    break;

                default:

                    break;
            }
        }
    }

    private void Shoot_LaserRed() {
        if (Time.time >= fireTime) {
            BulletBehavior bullet;
            bullet = Instantiate(laserBullet, origin.position-(origin.right*0.5f), Quaternion.FromToRotation(Vector3.back, origin.right)).GetComponent<BulletBehavior>();
            bullet.damage = ammo.damage;
            fireTime = Time.time + ammo.fireRate;
            ammo.use();
        }
    }

    private void Shoot_LaserBlue() {
        /*
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, origin.position);
        RaycastHit hit;
        if (Physics.Raycast(origin.position, origin.up, out hit, range)) {
            lineRenderer.SetPosition(1, hit.point);

            IDamageable target = hit.transform.GetComponent<IDamageable>();
            if (Time.time >= fireTime) {
                fireTime = Time.time + ammo.fireRate;
                if (target != null) {
                    target.TakeDamage(ammo.damage);
                }
                ammo.use();
            }

        } else {
            lineRenderer.SetPosition(1, origin.position + (origin.up * range));
            if (Time.time >= fireTime) {
                fireTime = Time.time + ammo.fireRate;
                ammo.use();
            }
        }
        */
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject == target.gameObject) {
            target = null;
        }
    }

    public void OnTriggerStay(Collider other) {
        if (target == null && other.GetComponent<IDamageable>()!=null) {
            target = other.gameObject;
        }
    }
}
