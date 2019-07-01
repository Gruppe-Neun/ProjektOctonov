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
    [SerializeField] private float[] baseStats = new float[] { 50, 10, 3, 3 };
    [SerializeField] private float[] lvlupStats = new float[] { 10, 1, 0, 0};
    [SerializeField] private int level = 0;

    [SerializeField] protected float maxHealth = 50f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float range = 3f;
    [SerializeField] protected float speed = 3f;
    protected float health;

    [SerializeField] protected Loot[] lootPool;
    [SerializeField] protected Vector3 lootOffset;
    public DieCallback dieCallback;

    protected Transform healthCanvas;
    protected Transform playerCamera;
    protected RawImage healthBar;
    protected bool showHealth = false;

    protected NavMeshAgent agent;
    protected IDamageableFriendly target;
    [SerializeField] protected Vector3 targetPosition;

    public virtual void Awake() {
        if (lootPool == null) {
            lootPool = new Loot[] {
                new Loot(Item.Type.Battery,0.25f),
                new Loot(Item.Type.LaserRedLevel1, 0.1f, 10),
                new Loot(Item.Type.Flashlight, 0.25f),
                new Loot(Item.Type.Medkit, 0.05f),
                new Loot(Item.Type.Ironplate, 0.25f),
                new Loot(Item.Type.Nut, 0.25f)
            };
        }
        agent = GetComponent<NavMeshAgent>();
        setLevel(level);
        healthBar = GetComponentInChildren<RawImage>();
        if (healthBar != null) showHealth = true;

        healthCanvas = GetComponentInChildren<Canvas>().transform;
        playerCamera = GameObject.Find("FirstPersonCharacter").transform;
    }

    public virtual void setLevel(int lvl) {
        level = lvl;
        maxHealth = baseStats[0] + lvlupStats[0]*lvl;
        health = maxHealth;
        damage = baseStats[1] + lvlupStats[1] * lvl;
        range = baseStats[2] + lvlupStats[2] * lvl;
        speed = baseStats[3] + lvlupStats[3] * lvl;

        agent.speed = speed;
    }

    public virtual void Update() {
        if(showHealth) healthCanvas.LookAt(playerCamera, Vector3.up);
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
        if (lootPool != null) {
            for(int i = 0; i < lootPool.Length; i++) {
                if (lootPool[i].percentage > Random.value) {
                    Item.createItem(lootPool[i].itemType,lootPool[i].amount,new Vector3(transform.position.x+Random.value*0.5f,transform.position.y, transform.position.z + Random.value * 0.5f)+lootOffset);
                }
            }
        }
        if(dieCallback!=null) dieCallback(this);
        Destroy(gameObject);
    }
}
