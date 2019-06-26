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

    private float maxHealth = 100;
    private bool[] partSlot = new bool[] { false, false, false, false, false };

    private UIBehavior ui;

    private Transform healthCanvas;
    private Transform playerCamera;

    private float health;
    private RawImage healthBar;
    private Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        this.type = ContainerType.Olli;
        this.content = new ItemBehavior[5];
        this.name = "3D Drucker";
        health = maxHealth;
        //updateBody();
        healthCanvas = GetComponentInChildren<Canvas>().transform;
        playerCamera = GameObject.Find("FirstPersonCharacter").transform;
        healthBar = GetComponentInChildren<RawImage>();
        healthText = GetComponentInChildren<Text>();
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
    } 

    public void addPart(Item.Type part) {
        switch (part) {
            case Item.Type.Olli_ArmLeft:
                partSlot[0] = true;
                break;

            case Item.Type.Olli_ArmRight:
                partSlot[1] = true;
                break;

            case Item.Type.Olli_LegLeft:
                partSlot[2] = true;
                break;

            case Item.Type.Olli_LegRight:
                partSlot[3] = true;
                break;

            case Item.Type.Olli_Body:
                partSlot[4] = true;
                break;

        }
        updateBody();
    }

    public override void updateContainer() {
        base.updateContainer();
        partSlot[0] = (content[0] != null && content[0].type == Item.Type.Olli_ArmLeft);
        partSlot[1] = (content[1] != null && content[1].type == Item.Type.Olli_ArmRight);
        partSlot[2] = (content[2] != null && content[2].type == Item.Type.Olli_LegLeft);
        partSlot[3] = (content[3] != null && content[3].type == Item.Type.Olli_LegRight);
        partSlot[4] = (content[4] != null && content[4].type == Item.Type.Olli_Body);
        updateBody();
    }

    public void TakeDamage(float dmg) {
        health -= dmg;
        healthText.text = (int)health + "/" + (int)maxHealth;
        healthBar.transform.localPosition = new Vector3(health / maxHealth * 2.5f - 2.5f, 0, 0);
        healthBar.transform.localScale = new Vector3(health / maxHealth, 1, 1);
        ui.sendWarning(dmg);
    }
}
