using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : Enemy
{
    public BulletBehavior laserBullet;

    public float moveSpeed = 10;
    public float rotationSpeed = 1;

    public Vector3 targetPosition;
    public GameObject target;
    public AmmoBehavior ammo;


    private float fireTime = 0f;
    private Transform origin;
    private Rigidbody rb;
    private int state = -1; //-1 = undef, 0 = idle, 1 = fly, 2 = attack

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        origin = transform;
        rb = GetComponent<Rigidbody>();
        state = 2;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state) {
            case 0:

                break;

            case 1:

                break;

            case 2:
                float targetHorizontal = Quaternion.FromToRotation(Vector3.forward, new Vector3(target.transform.position.x - origin.position.x, 0, target.transform.position.z - origin.position.z)).eulerAngles.y;
                float distY = Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.z), new Vector2(origin.position.x, origin.position.z));
                float targetVertical = Vector2.Angle(new Vector2(distY, target.transform.position.y - origin.position.y), Vector2.right);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x + Mathf.DeltaAngle(transform.eulerAngles.x, targetVertical) * Time.deltaTime, transform.eulerAngles.y + Mathf.DeltaAngle(transform.eulerAngles.y, targetHorizontal) * rotationSpeed * Time.deltaTime, 0);
                if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.x, targetVertical)) < 10 && Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetHorizontal)) < 10) {
                    Shoot();
                }
                break;
        }
        
    }

    //reroute if collision
    public void OnCollisionEnter(Collision collision) {
        
    }

    private void Shoot() {
        if (ammo == null) {

        } else {
            switch (ammo.type) {
                case Item.Type.LaserBlue:
                    Shoot_LaserBlue();
                    break;

                case Item.Type.LaserRed:
                    Shoot_LaserRed();
                    break;
            }
        }
    }

    private void Shoot_LaserRed() {
        if (Time.time >= fireTime) {
            BulletBehavior bullet;
            bullet = Instantiate(laserBullet, origin.position, transform.rotation).GetComponent<BulletBehavior>();
            bullet.friendly = false;
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
        if (Physics.Raycast(origin.position, -origin.right, out hit, range)) {
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
}
