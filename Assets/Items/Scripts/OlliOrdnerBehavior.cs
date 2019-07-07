using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OlliOrdnerBehavior : ContainerBehavior,IInteractable, IDamageableFriendly {
    [SerializeField] public GameObject arm_left;
    [SerializeField] public GameObject arm_right;
    [SerializeField] public GameObject leg_left;
    [SerializeField] public GameObject leg_right;
    [SerializeField] public GameObject body;

    [SerializeField] private GameObject explosion;
    [SerializeField] private float baseHealth = 100;
    [SerializeField] private float baseShield = 100;

    public float level = 1;
    private float maxHealth = 100;
    private float maxShield = 100;
    private bool[] partSlot = new bool[] { false, false, false, false, false };

    private UIBehavior ui;

    private Transform healthCanvas;
    private Transform playerCamera;
    private MeshRenderer forceField;

    private float health;
    private float shield;
    private RawImage healthBar;
    private Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        this.type = ContainerType.Olli;
        this.content = new ItemBehavior[5];
        this.name = "3D Drucker";
        maxHealth = baseHealth;
        maxShield = baseShield;
        health = maxHealth;
        shield = maxShield;
        //updateBody();
        healthCanvas = GetComponentInChildren<Canvas>().transform;
        playerCamera = GameObject.Find("FirstPersonCharacter").transform;
        healthBar = GetComponentInChildren<RawImage>();
        healthText = GetComponentInChildren<Text>();
        forceField = GetComponentInChildren<SphereCollider>().GetComponent<MeshRenderer>(); ;
        ui = GameObject.Find("UI").GetComponent<UIBehavior>();

        healthText.text = health + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthCanvas.LookAt(playerCamera, Vector3.up);
    }
    
    private void updateBody() {
        arm_left.SetActive(partSlot[0]);
        arm_right.SetActive(partSlot[1]);
        leg_left.SetActive(partSlot[2]);
        leg_right.SetActive(partSlot[3]);
        body.SetActive(partSlot[4]);

        int levelneu = 1;
        foreach(bool i in partSlot) 
            if (i) level++;
        if (levelneu != level) {
            maxHealth = baseHealth * level;
            maxShield = baseShield * level;
            level = levelneu;

            ui.updateHealth(health / maxHealth);
            healthText.text = (int)health + "/" + (int)maxHealth;
            healthBar.transform.localPosition = new Vector3(health / maxHealth * 2.5f - 2.5f, 0, 0);
            healthBar.transform.localScale = new Vector3(health / maxHealth, 1, 1);
            ui.updateAmor(shield / maxShield);
            forceField.material.color = new Color(1, 1, 1, shield / maxShield);
        }
        
    } 

    public override void updateContainer() {
        base.updateContainer();
        partSlot[0] = (content[0] != null && (content[0].type == Item.Type.Olli_Arm));
        partSlot[1] = (content[1] != null && (content[1].type == Item.Type.Olli_Arm));
        partSlot[2] = (content[2] != null && (content[2].type == Item.Type.Olli_Leg));
        partSlot[3] = (content[3] != null && (content[3].type == Item.Type.Olli_Leg));
        partSlot[4] = (content[4] != null && content[4].type == Item.Type.Olli_Body);
        updateBody();
    }

    public void TakeDamage(float dmg) {
        shield -= dmg;
        if (shield < 0) {
            health += shield;
            shield = 0;
            health = Mathf.Clamp(health, 0, maxHealth);
            ui.updateHealth(health / maxHealth);
            healthText.text = (int)health + "/" + (int)maxHealth;
            healthBar.transform.localPosition = new Vector3(health / maxHealth * 2.5f - 2.5f, 0, 0);
            healthBar.transform.localScale = new Vector3(health / maxHealth, 1, 1);
            if (health < 0) die();
        }
        forceField.material.color = new Color(1, 1, 1, shield / maxShield);
        ui.updateAmor(shield / maxShield);
        ui.sendWarning();
    }

    public void healLife(float healPoints) {
        health += healPoints;
        if (health > maxHealth) health = maxHealth;
        ui.updateHealth(health / maxHealth);
        healthText.text = (int)health + "/" + (int)maxHealth;
        healthBar.transform.localPosition = new Vector3(health / maxHealth * 2.5f - 2.5f, 0, 0);
        healthBar.transform.localScale = new Vector3(health / maxHealth, 1, 1);
    }

    public void healShield(float healPoints) {
        shield += healPoints;
        if (shield > maxShield) shield = maxShield;
        ui.updateAmor(shield / maxShield);
        forceField.material.color = new Color(1, 1, 1, shield / maxShield);
    }

    public void explode(float radius, float damage) {
        Destroy(Instantiate(explosion,this.transform),5);
        Collider[] hit = Physics.OverlapSphere(this.transform.position, radius);
        foreach (Collider i in hit) {
            if (i.GetComponent<IDamageableEnemy>() != null) {
                i.GetComponent<IDamageableEnemy>().TakeDamage(damage);
            }
        }
    }

    private void die() {

    }
}
