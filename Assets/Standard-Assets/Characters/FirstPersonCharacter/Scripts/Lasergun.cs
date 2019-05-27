using UnityEngine;

public class Lasergun : MonoBehaviour {
    public float fireTime = 0f;
    public float range = 50f;
    public AmmoBehavior ammo = null;
    public BulletBehavior laserBullet;

    private LineRenderer lineRenderer;
    GameObject laserSource;
    GameObject viewSource;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        laserSource = GameObject.FindGameObjectWithTag("LaserSource");
        viewSource = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void Combat() {
        if (Input.GetButton("Fire1")) {
            Shoot();
        }
        else {
            lineRenderer.enabled = false;
        }
    }

    void Shoot() {
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

    private void Shoot_LaserBlue() {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, laserSource.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(laserSource.transform.position, laserSource.transform.up, out hit, range)) {
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
            lineRenderer.SetPosition(1, laserSource.transform.position + (laserSource.transform.up * range));
            if (Time.time >= fireTime) {
                fireTime = Time.time + ammo.fireRate;
                ammo.use();
            }
        }
    }

    private void Shoot_LaserRed() {
        if (Time.time >= fireTime) {
            BulletBehavior bullet;
            RaycastHit hit;
            if(Physics.Raycast(viewSource.transform.position, viewSource.transform.forward, out hit, range)) {
                Vector3 rotation = hit.point - viewSource.transform.position;
                
                bullet = Instantiate(laserBullet, laserSource.transform.position, Quaternion.FromToRotation(Vector3.forward, rotation)).GetComponent<BulletBehavior>();
            } else {
                bullet = Instantiate(laserBullet, laserSource.transform.position, Quaternion.LookRotation(laserSource.transform.up, laserSource.transform.forward * -1)).GetComponent<BulletBehavior>();
            }

            bullet.damage = ammo.damage;

            fireTime = Time.time + ammo.fireRate;
            ammo.use();
        }
    }
}
