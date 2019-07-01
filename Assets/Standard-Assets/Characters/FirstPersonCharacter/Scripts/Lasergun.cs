using UnityEngine;

public class Lasergun : MonoBehaviour {
    
    [SerializeField] private float range = 100f;
    [SerializeField] private BulletBehavior laserBulletRed;
    [SerializeField] private BulletBehavior laserBulletGreen;
    [SerializeField] private GrenadeBehavior grenade;

    public AmmoBehavior ammo = null;
    private float fireTime = 0f;
    private float fireTimeSincePress = 0f;

    private InventoryBehavior inventory;
    private LineRenderer lineRenderer;
    private GameObject laserSource;
    private GameObject viewSource;
    private FlamethrowerBehavior flamethrower;
    private int raycastLayerMask;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        laserSource = GameObject.FindGameObjectWithTag("LaserSource");
        viewSource = GameObject.FindGameObjectWithTag("MainCamera");
        inventory = GameObject.Find("UI").GetComponent<InventoryBehavior>();
        flamethrower = GetComponentInChildren<FlamethrowerBehavior>();
        raycastLayerMask = ~((1 << 8) + (1 << 2));

    }

    public void Combat() {
        if (Input.GetButtonDown("Fire1")) {
            ShootInitiate();
        } else {
            if (Input.GetButton("Fire1")) {
                Shoot();
            } else {
                lineRenderer.enabled = false;
                //flamethrower.setActive(false);
            }
        }
    }

    private void ShootInitiate() {
        if (ammo == null) {
            inventory.scrollAmmo(1);
        } else {
            switch (ammo.type) {
                case Item.Type.LaserBlue:
                    Shoot_LaserBlue();
                    break;
                case Item.Type.LaserRedLevel1:
                    Shoot_LaserRed();
                    break;
                case Item.Type.LaserRedLevel2:
                    Shoot_LaserRed();
                    break;
                case Item.Type.LaserGreenLevel1:
                    Initiate_LaserGreen();
                    break;
                case Item.Type.LaserGreenLevel2:
                    Initiate_LaserGreen();
                    break;
                case Item.Type.GrenadeLauncher:
                    Shoot_GrenadeLauncher();
                    break;
                case Item.Type.Flamethrower:
                    Shoot_Flamethrower();
                    break;
                default:

                    break;
            }
        }
    }

    private void Shoot() {
        if (ammo == null) {
            inventory.scrollAmmo(1);
        } else {
            switch (ammo.type) {
                case Item.Type.LaserBlue:
                    Shoot_LaserBlue();
                    break;
                case Item.Type.LaserRedLevel1:
                    Shoot_LaserRed();
                    break;
                case Item.Type.LaserRedLevel2:
                    Shoot_LaserRed();
                    break;
                case Item.Type.LaserGreenLevel1:
                    Shoot_LaserGreen();
                    break;
                case Item.Type.LaserGreenLevel2:
                    Shoot_LaserGreen();
                    break;
                case Item.Type.GrenadeLauncher:
                    Shoot_GrenadeLauncher();
                    break;
                case Item.Type.Flamethrower:
                    Shoot_Flamethrower();
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
            float dist = Vector3.Magnitude(lineRenderer.GetPosition(0) - lineRenderer.GetPosition(1));
            lineRenderer.material.mainTextureScale = new Vector2(dist/16,1);
            lineRenderer.material.mainTextureOffset = new Vector2(-Time.time*4, 0);
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
            float dist = Vector3.Magnitude(lineRenderer.GetPosition(0) - lineRenderer.GetPosition(1));
            lineRenderer.material.mainTextureScale = new Vector2(dist / 16, 1);
            lineRenderer.material.mainTextureOffset = new Vector2(-Time.time*4, 0);
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
                bullet = Instantiate(laserBulletRed, laserSource.transform.position, Quaternion.FromToRotation(Vector3.forward, rotation)).GetComponent<BulletBehavior>();
            } else {
                bullet = Instantiate(laserBulletRed, laserSource.transform.position, Quaternion.LookRotation(laserSource.transform.up, laserSource.transform.forward * -1)).GetComponent<BulletBehavior>();
            }

            bullet.damage = ammo.damage;

            fireTime = Time.time + ammo.fireRate;
            ammo.use();
        }
    }

    private void Initiate_LaserGreen() {
        fireTimeSincePress = 0f;
        fireTime = Time.time + 0.1f;
    }

    private void Shoot_LaserGreen() {
        if (Time.time >= fireTime) {
            BulletBehavior bullet;

            RaycastHit hit;
            if (Physics.Raycast(viewSource.transform.position, viewSource.transform.forward, out hit, range, raycastLayerMask)) {
                if (hit.distance > 5) {
                    float maxDeviation = 0.15f * Mathf.Clamp01(fireTimeSincePress / 10 + 0.5f);
                    Vector3 deviation = new Vector3(maxDeviation * Random.value - maxDeviation / 2, maxDeviation * Random.value - maxDeviation / 2, 0);
                    Quaternion rotation = Quaternion.FromToRotation(Vector3.forward + deviation, hit.point - laserSource.transform.position);
                    bullet = Instantiate(laserBulletGreen, laserSource.transform.position, rotation).GetComponent<BulletBehavior>();
                } else {
                    float maxDeviation = 0.15f * Mathf.Clamp01(fireTimeSincePress / 10 + 0.5f);
                    Vector3 deviation = new Vector3(maxDeviation * Random.value - maxDeviation / 2, maxDeviation * Random.value - maxDeviation / 2, 0);
                    Quaternion rotation = Quaternion.FromToRotation(Vector3.forward + deviation, viewSource.transform.forward * 5);
                    bullet = Instantiate(laserBulletGreen, laserSource.transform.position, rotation).GetComponent<BulletBehavior>();
                }
            } else {
                float maxDeviation = 0.15f * Mathf.Clamp01(fireTimeSincePress / 10 + 0.5f);
                Vector3 deviation = new Vector3(maxDeviation * Random.value - maxDeviation / 2, maxDeviation * Random.value - maxDeviation / 2, 0);
                Quaternion rotation = Quaternion.FromToRotation(Vector3.forward + deviation, viewSource.transform.forward * range);
                bullet = Instantiate(laserBulletGreen, laserSource.transform.position, rotation).GetComponent<BulletBehavior>();
            }
            bullet.damage = ammo.damage;
            fireTime = Time.time + ammo.fireRate/Mathf.Clamp01(fireTimeSincePress/10+0.5f);
            ammo.use();
        }
        fireTimeSincePress += Time.deltaTime;
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

    private void Shoot_Flamethrower() {
        flamethrower.setActive(true);
    }
}
