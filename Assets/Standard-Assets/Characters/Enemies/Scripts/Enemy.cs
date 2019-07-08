using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageableEnemy
{
    public delegate void DieCallback(Enemy dead);
    public enum Type {
        Spider,
        Spider_fast,
        Spider_large,
        Drone
    }

    public struct Loot {
        public Loot(Item.Type item, float perc, int am = 1) {
            itemType = item;
            percentage = perc;
            amount = am;
        }
        public Item.Type itemType;
        public int amount;
        public float percentage;
    }
    public struct LootPool {
        public Loot[] loot;
    }

    [SerializeField] private float[] baseStats = new float[] { 50, 10, 3, 3 };
    [SerializeField] private float[] lvlupStats = new float[] { 10, 1, 0, 0};
    [SerializeField] private float[] eliteAmplifier = new float[] { 5, 2, 1, 0.75f };
    [SerializeField] private int level = 0;
    [SerializeField] private bool elite = false;

    [SerializeField] protected float maxHealth = 50f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float range = 3f;
    [SerializeField] protected float speed = 3f;
    protected float health;

    protected float slowTime = -1;
    protected float slowAmount = 1;

    [SerializeField] protected LootPool[] lootPool;
    [SerializeField] protected Vector3 lootOffset;
    public DieCallback dieCallback;

    protected Transform healthCanvas;
    protected Transform playerCamera;
    [SerializeField] protected RawImage healthBar;
    [SerializeField] protected RawImage snowflake;
    protected bool showHealth = false;

    protected NavMeshAgent agent;
    protected IDamageableFriendly target;
    [SerializeField] protected Vector3 targetPosition;

    public virtual void Awake() {
        if (lootPool == null) {
            lootPool = new LootPool[4];

            lootPool[0].loot = new Loot[] {
                new Loot(Item.Type.Battery,0.25f),
                new Loot(Item.Type.CristalRed,0.1f,2),
                new Loot(Item.Type.CristalBlue,0.1f,1),
                new Loot(Item.Type.Ironplate, 0.25f, 2),
                new Loot(Item.Type.GoldPlate, 0.05f, 1),
                new Loot(Item.Type.Nut, 0.15f)
            };

            lootPool[1].loot = new Loot[] {
                new Loot(Item.Type.Battery,0.25f, 2),
                new Loot(Item.Type.CristalRed,0.15f,2),
                new Loot(Item.Type.CristalGreen,0.1f,2),
                new Loot(Item.Type.CristalBlue,0.1f,2),
                new Loot(Item.Type.Gunpowder,0.1f,1),
                new Loot(Item.Type.Ironplate, 0.25f, 5),
                new Loot(Item.Type.GoldPlate, 0.1f, 1),
                new Loot(Item.Type.Nut, 0.20f, 2)
            };

            lootPool[2].loot = new Loot[] {
                new Loot(Item.Type.Battery,0.25f, 3),
                new Loot(Item.Type.CristalRed,0.15f,4),
                new Loot(Item.Type.CristalBlue,0.1f,3),
                new Loot(Item.Type.CristalGreen,0.15f,3),
                 new Loot(Item.Type.Gunpowder,0.15f,2),
                new Loot(Item.Type.Ironplate, 0.25f, 10),
                new Loot(Item.Type.GoldPlate, 0.20f, 1),
                new Loot(Item.Type.Nut, 0.25f, 3)
            };

            lootPool[3].loot = new Loot[] {
                new Loot(Item.Type.Battery,0.25f, 5),
                new Loot(Item.Type.CristalRed,0.25f,5),
                new Loot(Item.Type.CristalGreen,0.2f,4),
                 new Loot(Item.Type.Gunpowder,0.2f,5),
                new Loot(Item.Type.Ironplate, 0.25f, 20),
                 new Loot(Item.Type.GoldPlate, 0.25f, 3),
                new Loot(Item.Type.Nut, 0.25f, 5)
            };
        }
        agent = GetComponent<NavMeshAgent>();
        setLevel(level);
        if (healthBar != null) showHealth = true;
        if (snowflake != null) snowflake.enabled = false;

        healthCanvas = GetComponentInChildren<Canvas>().transform;
        playerCamera = GameObject.Find("FirstPersonCharacter").transform;
    }

    public virtual void setLevel(int lvl, bool eliteEnemy = false) {
        level = lvl;
        maxHealth = baseStats[0] + lvlupStats[0]*lvl;
        health = maxHealth;
        damage = baseStats[1] + lvlupStats[1] * lvl;
        range = baseStats[2] + lvlupStats[2] * lvl;
        speed = baseStats[3] + lvlupStats[3] * lvl;

        if (eliteEnemy) {
            elite = true;
            maxHealth *= eliteAmplifier[0];
            health = maxHealth;
            damage *= eliteAmplifier[1];
            range *= eliteAmplifier[2];
            speed *= eliteAmplifier[3];
            transform.localScale = transform.localScale * 1.5f;
        }

        agent.speed = speed;
    }

    public virtual void Update() {
        if(showHealth) healthCanvas.LookAt(playerCamera, Vector3.up);
    }

    public virtual void FixedUpdate() {
        if (slowTime >= 0) {
            slowTime -= Time.fixedDeltaTime;
            if (slowTime <= 0) {
                slowTime = -1;
                agent.speed = speed;
                snowflake.enabled = false;
            }
        }
        
    }

    public virtual void slow(float amount, float time) {
        if (slowTime >= 0) {
            slowTime = Mathf.Max(slowTime, slowAmount);
            slowAmount = Mathf.Max(slowAmount, amount);
        } else {
            slowTime = time;
            slowAmount = amount;
            snowflake.enabled = true;
        }
        agent.speed = speed * slowAmount;
    }

    public virtual void TakeDamage(float damage) {
        //Debug.Log("ENEMY HEALTH: " + health);
        health -= damage;
        if (showHealth) {
            healthBar.transform.localScale = new Vector3(Mathf.Clamp01(health / maxHealth), 1, 1);
        }
        if(health <= 0f) {
            die();
        }
    }

    public void setTarget(GameObject neu) {
        targetPosition = neu.GetComponent<Collider>().bounds.center;
        //targetTransform = neu.transform;
        target = neu.GetComponent<IDamageableFriendly>();
    }

    public void die() {
        int lootTier;
        if (level > 30) {
            lootTier = 2;
        } else {
            if (level > 10) {
                lootTier = 1;
            } else {
                lootTier = 0;
            }
        }
        if (elite) lootTier++;
        if (lootTier >= lootPool.Length) lootTier = lootPool.Length - 1;

        if (lootPool[lootTier].loot != null) {
            for(int i = 0; i < lootPool[lootTier].loot.Length; i++) {
                if (lootPool[lootTier].loot[i].percentage > Random.value) {
                    Item.createItem(lootPool[lootTier].loot[i].itemType,lootPool[lootTier].loot[i].amount,new Vector3(transform.position.x+Random.value*0.5f,transform.position.y, transform.position.z + Random.value * 0.5f)+lootOffset);
                }
            }
        }
        if(dieCallback!=null) dieCallback(this);
        Destroy(gameObject);
    }
}
