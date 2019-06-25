using UnityEngine;

public class Lasergun : MonoBehaviour {
    public float fireTime = 0f;
    public float range = 50f;
    public AmmoBehavior ammo = null;
    public BulletBehavior laserBullet;
    public GrenadeBehavior grenade;

    private InventoryBehavior inventory;
    private LineRenderer lineRenderer;
    private GameObject laserSource;
    private GameObject viewSource;
    private int raycastLayerMask;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        laserSource = GameObject.FindGameObjectWithTag("LaserSource");
        viewSource = GameObject.FindGameObjectWithTag("MainCamera");
        inventory = GameObject.Find("UI").GetComponent<InventoryBehavior>();
        raycastLayerMask = 1 << 8;
        raycastLayerMask = ~raycastLayerMask;
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
            inventory.scrollAmmo(1);
        } else {
            switch (ammo.type) {
                case Item.Type.LaserBlue:
                    Shoot_LaserBlue();
                    break;

                case Item.Type.LaserRed:
                    Shoot_LaserRed();
                    break;

                case Item.Type.GrenadeLauncher:
                    Shoot_GrenadeLauncher();
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
        if (Physics.Raycast(viewSource.transform.position, viewSource.transform.forward, out hit, range, raycastLayerMask)) {
            lineRenderer.SetPosition(1, hit.point);

            IDamageableEnemy target = hit.transform.GetComponent<IDamageableEnemy>();
            if (Time.time >= fireTime) {
                fireTime = Time.time + ammo.fireRate;
                if (target != null) {
                    target.TakeDamage(ammo.damage);
                }
                ammo.use();
            }
            
        } else {
            lineRenderer.SetPosition(1, viewSource.transform.position + (viewSource.transform.forward * range));
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
            if(Physics.Raycast(viewSource.transform.position, viewSource.transform.forward, out hit, range, raycastLayerMask)) {
                Vector3 rotation = hit.point - laserSource.transform.position; 
                bullet = Instantiate(laserBullet, laserSource.transform.position, Quaternion.FromToRotation(Vector3.forward, rotation)).GetComponent<BulletBehavior>();
            } else {
                bullet = Instantiate(laserBullet, laserSource.transform.position, Quaternion.LookRotation(laserSource.transform.up, laserSource.transform.forward * -1)).GetComponent<BulletBehavior>();
            }

            bullet.damage = ammo.damage;

            fireTime = Time.time + ammo.fireRate;
            ammo.use();
        }
    }

    private void Shoot_GrenadeLauncher() {
        if (Time.time >= fireTime) {
            GrenadeBehavior neu;
            RaycastHit hit;
            if (Physics.Raycast(viewSource.transform.position, viewSource.transform.forward, out hit, range, raycastLayerMask)) {
                Vector3 rotation = hit.point - laserSource.transform.position;
                neu = Instantiate(grenade, laserSource.transform.position, Quaternion.FromToRotation(Vector3.forward, rotation));
            } else {
                neu = Instantiate(grenade, laserSource.transform.position, Quaternion.LookRotation(laserSource.transform.up, laserSource.transform.forward * -1));
            }
            neu.damage = ammo.damage;
            neu.velocity = neu.transform.forward * 30;
            fireTime = Time.time + ammo.fireRate;
            ammo.use();
        }
    }
}
