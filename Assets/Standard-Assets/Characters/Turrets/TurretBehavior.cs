using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour, IInteractable {
    public enum Type { UNDEF, BlueLaser, RedLaser, GreenLaser }
    public enum Upgrade { UNDEF, Damage, Firerate, Range }
    private delegate void Shoot();
    private struct TurretStats {
        public TurretStats(float bDamage, float uDamage, float bFireRate, float uFirerate, float bRange, float uRange) {
            baseDamage = bDamage;
            baseFireRate = bFireRate;
            baseRange = bRange;

            upgradeDamage = uDamage;
            upgradeFireRate = uFirerate;
            upgradeRange = uRange;
        }

        public float baseDamage;
        public float baseFireRate;
        public float baseRange;

        public float upgradeDamage;
        public float upgradeFireRate;
        public float upgradeRange;
    }

    private TurretStats[] turretStats = new TurretStats[]{
        new TurretStats(0,0,0,0,0,0),
        new TurretStats(3,1,0.2f,2,15,5),
        new TurretStats(10,7,0.5f,2,20,10),
        new TurretStats(1,1,0.1f,2,15,7)
        };

    public Item.Type[,] upgradeItems = new Item.Type[,]{
        { Item.Type.UNDEF, Item.Type.UNDEF, Item.Type.UNDEF, Item.Type.UNDEF },
        { Item.Type.TurretBlueCoreLevel1, Item.Type.TurretBlueCoreLevel2, Item.Type.TurretBlueCoreLevel3, Item.Type.TurretBlueCoreLevel4 },
        { Item.Type.TurretRedCoreLevel1, Item.Type.TurretRedCoreLevel2, Item.Type.TurretRedCoreLevel3, Item.Type.TurretRedCoreLevel4 },
        { Item.Type.TurretGreenCoreLevel1, Item.Type.TurretGreenCoreLevel2, Item.Type.TurretGreenCoreLevel3, Item.Type.TurretGreenCoreLevel4 },
        };
    public int maxLevel = 4;

    public GameObject Bone_Barrel;
    public GameObject Bone_Upper;
    public BulletBehavior laserBullet;
    public Type turretType = Type.UNDEF;

    public float damage = 2;
    public float fireRate = 2;
    public float range = 10;

    public float horizontalSpeed = 30;
    public float verticalSpeed = 30;

    private Upgrade[] LevelUps = new Upgrade[0];



    private GameObject target;

    private Transform origin;
    private float rotationHorizontal = 0f;
    private float rotationVertical = 0f;
    private float targetHorizontal = 0f;
    private float targetVertical = 0f;

    private Shoot shootType;
    private float fireTime = 0f;
    private float fireTimeSincePress = 0f;
    private LineRenderer lineRenderer;
    private SphereCollider viewRange;

    // Start is called before the first frame update
    void Start() {
        origin = Bone_Barrel.transform;
        lineRenderer = GetComponent<LineRenderer>();
        viewRange = GetComponentInChildren<SphereCollider>();
        setTurretType(this.turretType);
        updateStats();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (target != null) {
            targetPosition(target.transform.position);
            rotationHorizontal = Mathf.MoveTowardsAngle(rotationHorizontal, targetHorizontal, horizontalSpeed * Time.deltaTime);
            rotationVertical = Mathf.MoveTowardsAngle(rotationVertical, targetVertical, verticalSpeed * Time.deltaTime);
            Bone_Upper.transform.localRotation = Quaternion.AngleAxis(rotationHorizontal, Vector3.left);
            Bone_Barrel.transform.localRotation = Quaternion.AngleAxis(rotationVertical, Vector3.forward);
            if (Mathf.Abs(Mathf.DeltaAngle(rotationHorizontal, targetHorizontal)) < 20 && Mathf.Abs(Mathf.DeltaAngle(rotationVertical, targetVertical)) < 20) {
                shootType();
            } else {
                lineRenderer.enabled = false;
            }
        } else {
            lineRenderer.enabled = false;
        }
        viewRange.radius = range;
    }

    private void setTurretType(Type type) {
        this.turretType = type;
        switch (type) {
            case Type.BlueLaser:
                shootType = Shoot_LaserBlue;
                break;

            case Type.RedLaser:
                shootType = Shoot_LaserRed;
                break;

            case Type.GreenLaser:
                shootType = Shoot_LaserGreen;
                break;
        }

    }

    private void updateStats() {
        damage = turretStats[(int)turretType].baseDamage;
        fireRate = turretStats[(int)turretType].baseFireRate;
        range = turretStats[(int)turretType].baseRange;
        for (int i = 0; i < LevelUps.Length; i++) {
            switch (LevelUps[i]) {
                case Upgrade.Damage:
                    damage += turretStats[(int)turretType].upgradeDamage;
                    break;
                case Upgrade.Firerate:
                    damage += turretStats[(int)turretType].upgradeFireRate;
                    break;
                case Upgrade.Range:
                    range += turretStats[(int)turretType].upgradeRange;
                    break;
            }
        }
        viewRange.radius = range;
    }

    public void levelUp(Upgrade choice) {
        Upgrade[] neu = new Upgrade[LevelUps.Length + 1];
        for (int i = 0; i < LevelUps.Length; i++) neu[i] = LevelUps[i];
        neu[LevelUps.Length] = choice;
        LevelUps = neu;
        updateStats();
    }

    public int getLevel() {
        return LevelUps.Length;
    }

    public Item.Type getUpgradeItem() {
        if (LevelUps.Length < maxLevel) return upgradeItems[(int)turretType, LevelUps.Length];
        else return Item.Type.UNDEF;
    }

    private void targetPosition(Vector3 pos) {
        targetHorizontal = Quaternion.FromToRotation(Vector3.left, new Vector3(pos.x-origin.position.x,0, pos.z - origin.position.z)).eulerAngles.y;
        float distY = Vector2.Distance(new Vector2(pos.x,pos.z), new Vector2(origin.position.x,origin.position.z));
        targetVertical = Vector2.Angle(new Vector2(distY, pos.y-origin.position.y), Vector2.up);
    }


    private void Shoot_LaserRed() {
        if (Time.time >= fireTime) {
            BulletBehavior bullet;
            bullet = Instantiate(laserBullet, origin.position-(origin.right*0.5f), Quaternion.FromToRotation(Vector3.back, origin.right)).GetComponent<BulletBehavior>();
            bullet.damage = damage;
            fireTime = Time.time + fireRate;
        }
    }

    private void Shoot_LaserBlue() {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, origin.position);
        RaycastHit hit;
        if (Physics.Raycast(origin.position, -origin.right, out hit, range)) {
            lineRenderer.SetPosition(1, hit.point);

            IDamageable target = hit.transform.GetComponent<IDamageable>();
            if (Time.time >= fireTime) {
                fireTime = Time.time + fireRate;
                if (target != null) {
                    target.TakeDamage(damage);
                }
            }

        } else {
            lineRenderer.SetPosition(1, origin.position + (origin.up * range));
            if (Time.time >= fireTime) {
                fireTime = Time.time + fireRate;
            }
        }
    }

    private void Shoot_LaserGreen() {
        if (Time.time >= fireTime) {
            BulletBehavior bullet;
            bullet = Instantiate(laserBullet, origin.position - (origin.right * 0.5f), Quaternion.FromToRotation(Vector3.back, origin.right)).GetComponent<BulletBehavior>();
            bullet.damage = damage;
            fireTime = Time.time + fireRate / Mathf.Clamp01(fireTimeSincePress / 15 + 0.4f);
        }
        fireTimeSincePress += Time.fixedDeltaTime;
    }

    public void setTarget(GameObject target) {
        this.target = target;
        fireTimeSincePress = 0;
    }

    public void Interact() {
        GameObject.Find("UI").GetComponent<UIBehavior>().openTurret(this);
    }
}
