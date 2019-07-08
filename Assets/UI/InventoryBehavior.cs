using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using System;

public class InventoryBehavior : MonoBehaviour
{

    public SlotBehavior slotPrefab;
    public SlotBehavior usableSlotPrefab;
    public SlotBehavior craftingResultPrefab;
    public SlotBehavior craftingSlotPrefab;
    public ItemButtonBehavior craftingStationPrefab;
    public SlotBehavior containerSlotPrefab;
    public Texture[] ContainerImage;

    public ItemBehavior itemPrefab;
    public CursorBehavior cursor;

    private UIBehavior ui;
    private Lasergun gun;
    private RawImage containerUI;
    private ButtonBehavior craftButton;
    private ButtonBehavior constructButton;
    private ButtonBehavior[] lvlUpButton;

    private SlotBehavior[] InventorySlot=new SlotBehavior[40];
    private SlotBehavior[] AmmoSlot = new SlotBehavior[5];
    private int activeAmmo = 0;
    private SlotBehavior[] ActiveSlot = new SlotBehavior[5];
    private int activeActive = 0;
    private SlotBehavior[] CraftingSlot = new SlotBehavior[5];
    private ItemButtonBehavior craftingStation;
    private SlotBehavior CursorSlot;
    private SlotBehavior[] ContainerSlot = new SlotBehavior[12];
   

    public Crafting.CraftingStationType currentStation;
    private ContainerBehavior activeContainer;
    private ConstructBehavior activeConstruct;
    private TurretBehavior activeTurret;

    // Start is called before the first frame update
    void Start()
    {
        gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Lasergun>();
        ui = GetComponent<UIBehavior>();
        containerUI = GameObject.Find("UI_Container").GetComponent<RawImage>();
        craftButton = GameObject.Find("UI_CraftButton").GetComponent<ButtonBehavior>();
        craftButton.clickEvent = craft;
        constructButton = GameObject.Find("UI_ConstructButton").GetComponent<ButtonBehavior>();
        constructButton.clickEvent = clickConstruct;

        lvlUpButton = new ButtonBehavior[3];
        lvlUpButton[0] = GameObject.Find("UI_LvlUpButton1").GetComponent<ButtonBehavior>();
        lvlUpButton[1] = GameObject.Find("UI_LvlUpButton2").GetComponent<ButtonBehavior>();
        lvlUpButton[2] = GameObject.Find("UI_LvlUpButton3").GetComponent<ButtonBehavior>();
        lvlUpButton[0].clickEvent = clickTurret;
        lvlUpButton[1].clickEvent = clickTurret;
        lvlUpButton[2].clickEvent = clickTurret;
        lvlUpButton[0].clickEventParam = 0;
        lvlUpButton[1].clickEventParam = 1;
        lvlUpButton[2].clickEventParam = 2;

        GameObject InventoryUI = GameObject.Find("UI_Top");
        GameObject ActiveUI = GameObject.Find("UI_Left");
        GameObject AmmoUI = GameObject.Find("UI_Right");

        CursorSlot = cursor;
        CursorSlot.useType = Item.useType.GENERIC;
       
        //crate inventory slots
        for(int i = 0; i < InventorySlot.Length; i++) {
            InventorySlot[i] = Instantiate(slotPrefab, InventoryUI.transform);
            InventorySlot[i].transform.localPosition = new Vector3(100 * (i % 10) - 631 + ((int)i / 10) * 34, 508 - ((int)i/10) * 61,0);
            InventorySlot[i].useType = Item.useType.GENERIC;
        }
        //create ammo slots
        for (int i = 0; i < AmmoSlot.Length; i++) {
            AmmoSlot[i] = Instantiate(usableSlotPrefab, AmmoUI.transform);
            AmmoSlot[i].transform.localPosition = new Vector3(-(14 * i) + 746, -(90 * i) - 97, 0);
            AmmoSlot[i].mirror();
            AmmoSlot[i].useType = Item.useType.AMMO;
        }
        AmmoSlot[0].addItem(Item.createItem(Item.Type.LaserBlue, -1, new Vector3(0, 0, 0)));
        AmmoSlot[0].accessible = SlotBehavior.AccesType.VIEWONLY;
        gun.ammo = (AmmoBehavior)AmmoSlot[0].viewItem();
        ui.setCrosshairType(((AmmoBehavior)AmmoSlot[0].viewItem()).crosshairType);
        //create active slots
        for (int i = 0; i < ActiveSlot.Length; i++) {
            ActiveSlot[i] = Instantiate(usableSlotPrefab, ActiveUI.transform);
            ActiveSlot[i].transform.localPosition = new Vector3((14 * i) - 746, -(90 * i) - 97, 0);
            ActiveSlot[i].useType = Item.useType.ACTIVE;
        }
        //create container slots
        for(int i = 0; i < ContainerSlot.Length; i++) {
            ContainerSlot[i] = Instantiate(containerSlotPrefab, containerUI.transform);
        }
        //create crafting slots
        CraftingSlot[0] = Instantiate(craftingResultPrefab, InventoryUI.transform);
        CraftingSlot[0].transform.localPosition = new Vector3(500, 325, 0);        
        CraftingSlot[0].accessible = SlotBehavior.AccesType.TAKEONLY;
        CraftingSlot[1] = Instantiate(craftingSlotPrefab, InventoryUI.transform);
        CraftingSlot[1].transform.localPosition = new Vector3(429, 412, 0);
        CraftingSlot[2] = Instantiate(craftingSlotPrefab, InventoryUI.transform);
        CraftingSlot[2].transform.localPosition = new Vector3(571, 412, 0);
        CraftingSlot[2].mirror();
        CraftingSlot[3] = Instantiate(slotPrefab, InventoryUI.transform);
        CraftingSlot[3].transform.localPosition = new Vector3(387, 473, 0);
        CraftingSlot[4] = Instantiate(slotPrefab, InventoryUI.transform);
        CraftingSlot[4].transform.localPosition = new Vector3(613, 473, 0);
        CraftingSlot[4].mirror();
        craftingStation = Instantiate(craftingStationPrefab, InventoryUI.transform);
        craftingStation.transform.localPosition = new Vector3(500, 473, 0);
        craftingStation.transform.localScale = new Vector3(1, 1, 1);
        craftingStation.clickAble = false;

        CraftingSlot[0].useType = Item.useType.GENERIC;
        for (int i = 1; i < CraftingSlot.Length; i++) {
            CraftingSlot[i].updateEvent = updateCraftingResult;
        }
        CraftingSlot[0].updateEvent = takeCraftingResult;

        constructButton.gameObject.SetActive(false);
        lvlUpButton[0].gameObject.SetActive(false);
        lvlUpButton[1].gameObject.SetActive(false);
        lvlUpButton[2].gameObject.SetActive(false);
        containerUI.gameObject.SetActive(false);

        currentStation = Crafting.CraftingStationType.NONE;
        giveItem(Item.Type.TurretBlueCoreLevel1, 4);
        giveItem(Item.Type.TurretGreenCoreLevel1, 4);
        giveItem(Item.Type.TurretRedCoreLevel1, 4);
        giveItem(Item.Type.TurretSlowCoreLevel1, 4);
        giveItem(Item.Type.RedButton, 1);
        giveItem(Item.Type.LaserRedLevel1, 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (CursorSlot.viewItem() != null && Input.GetKeyDown(KeyCode.Q)) {
            CursorSlot.forceTake().drop(true);
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            int itemNum = Enum.GetNames(typeof(Item.Type)).Length;
            for(int i = 1; i < itemNum; i++) {
                giveItem((Item.Type) i,50);
            }
            /*
            giveItem(Item.Type.Battery,20);
            giveItem(Item.Type.Nut, 20);
            giveItem(Item.Type.Ironplate, 20);
            giveItem(Item.Type.Flashlight, 20);
            giveItem(Item.Type.CristalBlue, 20);
            giveItem(Item.Type.CristalRed, 20);
            giveItem(Item.Type.Case, 20);
            giveItem(Item.Type.GrenadeLauncher, 20);
            */
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            scrollActive(1);
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            if(ActiveSlot[activeActive].viewItem()!=null) ActiveSlot[activeActive].viewItem().use();
        }

        //switch ammo
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            switchAmmo(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            switchAmmo(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            switchAmmo(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            switchAmmo(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            switchAmmo(4);
        }
        if (Input.mouseScrollDelta.y != 0) {
            scrollAmmo((int)Input.mouseScrollDelta.y);
        }
        AmmoSlot[activeAmmo].updateSlot();
        ActiveSlot[activeActive].updateSlot();
    }

    private void positionAmmoSlots() {
        for (int i = 0; i < AmmoSlot.Length; i++) {
            int p = (i + activeAmmo) % 5;
            AmmoSlot[p].transform.localPosition = new Vector3(-(14 * i) + 746, -(90 * i) - 97, 0);
        }
    }

    private void positionActiveSlots() {
        for (int i = 0; i < ActiveSlot.Length; i++) {
            int p = (i + activeActive) % 5;
            ActiveSlot[p].transform.localPosition = new Vector3((14 * i) - 746, -(90 * i) - 97, 0);
        }
    }

    /* 
    public void SetItem(ItemBehavior item,int slot) {
        InventorySlot[slot].setItem(item);
    }
    */

    public ItemBehavior takeItem(int slot) {
        return InventorySlot[slot].takeItem();
    }
    

    public void switchCursor(SlotBehavior slot) {
        CursorSlot.addItem(slot.switchItem(CursorSlot.takeItem(),true));
    }

    public void splitCursor(SlotBehavior slot) {
        if (CursorSlot.viewItem() == null) {
            CursorSlot.addItem(slot.splitItem(-1));
        } else {
            if (slot.viewItem()==null || slot.viewItem().type==CursorSlot.viewItem().type) {
                slot.addItem(CursorSlot.splitItem(1));
            }
        }
    }

    public bool pickUpItem(ItemBehavior item) {
        if (item == null) return true;
        for (int i = 0; i < InventorySlot.Length; i++) {
            if (InventorySlot[i].viewItem() != null && InventorySlot[i].viewItem().type == item.type) {
                return InventorySlot[i].addItem(item);
            }
        }

        for (int i = 0; i < InventorySlot.Length; i++) {
            if (InventorySlot[i].viewItem()==null) {
                return InventorySlot[i].addItem(item);
            }
        }
        return false;
    }

    public ItemBehavior scrollAmmo(int count) {
        if (count >= 0) {
            for (int i = 0; i < count; i++) {
                activeAmmo++;
                if (activeAmmo >= 5) {
                    activeAmmo = 0;
                } else {
                    if (AmmoSlot[activeAmmo].viewItem() == null) {
                        i--;
                    }
                }
            }
        } else {
            for (int i = 0; i > count; i--) {
                activeAmmo--;
                if (activeAmmo < 0) {
                    activeAmmo = 4;
                }
                if (AmmoSlot[activeAmmo].viewItem() == null) {
                    i++;
                }
            }
        }
        gun.ammo = (AmmoBehavior)AmmoSlot[activeAmmo].viewItem();
        ui.setCrosshairType(((AmmoBehavior)AmmoSlot[activeAmmo].viewItem()).crosshairType);
        positionAmmoSlots();
        return AmmoSlot[activeAmmo].viewItem();
    }

    public ItemBehavior switchAmmo(int num) {
        num = num % 5;
        if (AmmoSlot[num].viewItem() != null) {
            activeAmmo = num;
            ui.setCrosshairType(((AmmoBehavior)AmmoSlot[activeAmmo].viewItem()).crosshairType);
            gun.ammo = (AmmoBehavior)AmmoSlot[activeAmmo].viewItem();
            positionAmmoSlots();
        }
        return AmmoSlot[activeAmmo].viewItem();
    }

    public ItemBehavior scrollActive(int count) {
        if (count >= 0) {
            for (int i = 0; i < count; i++) {
                activeActive++;
                if (activeActive >= 5) {
                    activeActive = 0;
                } else {
                    if (ActiveSlot[activeActive].viewItem() == null) {
                        i--;
                    }
                }
            }
        } else {
            for (int i = 0; i > count; i--) {
                activeActive--;
                if (activeActive < 0) {
                    activeActive = 4;
                }
                if (ActiveSlot[activeActive].viewItem() == null) {
                    i++;
                }
            }
        }
        positionActiveSlots();
        return ActiveSlot[activeActive].viewItem();
    }

    public ItemBehavior switchActive(int num) {
        num = num % 5;
        if (ActiveSlot[num].viewItem() != null) {
            activeActive = num;
        }
        positionActiveSlots();
        return ActiveSlot[activeActive].viewItem();
    }

    public void giveItem(Item.Type type, int amount) {
        Item.createItem(type, amount, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 0, 0));
    }

    public void closeInventory() {
        if (CursorSlot.viewItem() != null) {
            pickUpItem(CursorSlot.takeItem());
        }
        for(int i = 0; i < 5; i++) {
            if(CraftingSlot[i].viewItem() != null) {
                pickUpItem(CraftingSlot[i].takeItem());
            }
        }
        closeContainer();
        closeConstruct();
        closeTurret();
        setCraftingStation(Crafting.CraftingStationType.NONE);
    }

    public void setCraftingStation(Crafting.CraftingStationType type) {
        currentStation = type;
        craftingStation.setItem(Crafting.GetStationItemType(currentStation));
    }

    public void closeContainer() {
        if (activeContainer != null) {
            for (int i = 0; i < activeContainer.content.Length; i++) {
                activeContainer.content[i] = ContainerSlot[i].viewItem();
                ContainerSlot[i].setItem(null);
            }
            activeContainer.updateContainer();
            activeContainer = null;
            containerUI.gameObject.SetActive(false);
        }
        ContainerSlot[0].accessible = SlotBehavior.AccesType.OPEN;
    }

    public void openContainer(ContainerBehavior container) {
        activeContainer = container;
        this.containerUI.texture = ContainerImage[(int)container.type];
        switch (container.type) {
            case ContainerBehavior.ContainerType.Tiny:
                for (int i = 0; i < 4; i++) {
                    ContainerSlot[i].gameObject.SetActive(true);
                    ContainerSlot[i].setItem(container.content[i]);
                    ContainerSlot[i].transform.localPosition = new Vector3(96 * (i % 2) - 48, 32 - ((int)i / 2) * 62, 0);
                }
                for (int i = 4; i < ContainerSlot.Length; i++) {
                    ContainerSlot[i].gameObject.SetActive(false);
                    ContainerSlot[i].setItem(null);
                }
                break;

            case ContainerBehavior.ContainerType.Medium:
                for (int i = 0; i < 8; i++) {
                    ContainerSlot[i].gameObject.SetActive(true);
                    ContainerSlot[i].setItem(container.content[i]);
                    ContainerSlot[i].transform.localPosition = new Vector3(96 * (i % 4) - 144, 32 - ((int)i / 4) * 62, 0);
                }
                for (int i = 8; i < ContainerSlot.Length; i++) {
                    ContainerSlot[i].gameObject.SetActive(false);
                    ContainerSlot[i].setItem(null);
                }
                break;

            case ContainerBehavior.ContainerType.Large:
                for (int i = 0; i < 12; i++) {
                    ContainerSlot[i].gameObject.SetActive(true);
                    ContainerSlot[i].setItem(container.content[i]);
                }
                for (int i = 12; i < ContainerSlot.Length; i++) {
                    ContainerSlot[i].gameObject.SetActive(false);
                    ContainerSlot[i].setItem(null);
                }
                break;

            case ContainerBehavior.ContainerType.Olli:
                for (int i = 0; i < 5; i++) {
                    ContainerSlot[i].gameObject.SetActive(true);
                    ContainerSlot[i].setItem(container.content[i]);
                }
                for (int i = 5; i < ContainerSlot.Length; i++) {
                    ContainerSlot[i].gameObject.SetActive(false);
                    ContainerSlot[i].setItem(null);
                }
                ContainerSlot[0].transform.localPosition = new Vector3(-96, 32, 0);
                ContainerSlot[1].transform.localPosition = new Vector3(96, 32, 0);
                ContainerSlot[2].transform.localPosition = new Vector3(-48, -30, 0);
                ContainerSlot[3].transform.localPosition = new Vector3(48, -30, 0);
                ContainerSlot[4].transform.localPosition = new Vector3(0, 32, 0);
                break;

            case ContainerBehavior.ContainerType.Mine:
                ContainerSlot[0].gameObject.SetActive(true);
                ContainerSlot[0].setItem(container.content[0]);
                for (int i = 1; i < ContainerSlot.Length; i++) {
                    ContainerSlot[i].gameObject.SetActive(false);
                    ContainerSlot[i].setItem(null);
                }
                ContainerSlot[0].transform.localPosition = new Vector3(0,0,0);
                ContainerSlot[0].accessible = SlotBehavior.AccesType.TAKEONLY;
                break;
        }
        containerUI.gameObject.SetActive(true);
    }

    public void openTurret(TurretBehavior turret) {
        this.containerUI.texture = ContainerImage[0];
        ContainerSlot[0].transform.localPosition = new Vector3(0, 32, 0);
        ContainerSlot[0].gameObject.SetActive(true);
        for (int i = 1; i < ContainerSlot.Length; i++) {
            ContainerSlot[i].gameObject.SetActive(false);
        }
        lvlUpButton[0].gameObject.SetActive(true);
        lvlUpButton[1].gameObject.SetActive(true);
        lvlUpButton[2].gameObject.SetActive(true);
        activeTurret = turret;
        containerUI.gameObject.SetActive(true);
    }

    public void clickTurret(int choice) {
        if (activeTurret.getUpgradeItem() == ContainerSlot[0].viewItem().type) {
            switch (choice) {
                case 0:
                    activeTurret.levelUp(TurretBehavior.Upgrade.Damage);
                    break;
                case 1:
                    activeTurret.levelUp(TurretBehavior.Upgrade.Firerate);
                    break;
                case 2:
                    activeTurret.levelUp(TurretBehavior.Upgrade.Range);
                    break;
            }
            ContainerSlot[0].viewItem().use();
            closeTurret();
        }
    }

    public void closeTurret() {
        if (activeTurret != null) {
            pickUpItem(ContainerSlot[0].forceTake());
            lvlUpButton[0].gameObject.SetActive(false);
            lvlUpButton[1].gameObject.SetActive(false);
            lvlUpButton[2].gameObject.SetActive(false);
            containerUI.gameObject.SetActive(false);
            activeTurret = null;
        }
    }

    public void openConstruct(ConstructBehavior construct) {
        this.containerUI.texture = ContainerImage[0];
        ContainerSlot[0].transform.localPosition = new Vector3(0, 32, 0);
        ContainerSlot[0].gameObject.SetActive(true);
        for(int i = 1; i<ContainerSlot.Length; i++) {
            ContainerSlot[i].gameObject.SetActive(false);
        }
        constructButton.gameObject.SetActive(true);
        activeConstruct = construct;
        containerUI.gameObject.SetActive(true);
    }

    public void clickConstruct(int nothing) {
        if (activeConstruct!=null && ContainerSlot[0].viewItem() != null && activeConstruct.construct(ContainerSlot[0].viewItem().type)) {
            ContainerSlot[0].viewItem().use();
            closeConstruct();
        }
    }

    public void closeConstruct() {
        if (activeConstruct != null) {
            pickUpItem(ContainerSlot[0].forceTake());
            constructButton.gameObject.SetActive(false);
            containerUI.gameObject.SetActive(false);
            activeConstruct = null;
        }
    }

    public ItemBehavior getActiveAmmo() {
        return AmmoSlot[activeAmmo].viewItem();
    }

    public ItemBehavior getActiveActive() {
        return ActiveSlot[activeActive].viewItem();
    }

    public bool setUpRecipe(Crafting.Recipe recipe) {
        for (int i = 0; i < 5; i++) {
            if (CraftingSlot[i].viewItem() != null) {
                pickUpItem(CraftingSlot[i].takeItem());
            }
        }

        SlotBehavior[] ingredients = new SlotBehavior[] { null, null, null, null };
        if (recipe.craftingstation != currentStation) return false;
        for(int i = 0; i < 4; i++) {
            if(recipe.ingredients[i] != Item.Type.UNDEF) {
                bool missing = true;
                for(int c = 0; c < InventorySlot.Length; c++) {
                    if(InventorySlot[c].viewItem() != null && InventorySlot[c].viewItem().type == recipe.ingredients[i]) {
                        ingredients[i] = InventorySlot[c];
                        missing = false;
                        break;
                    }
                }
                if (missing) return false;
            }
        }

        for(int i = 0; i < ingredients.Length; i++) {
            if(ingredients[i]!=null)
                if (!CraftingSlot[i + 1].addItem(ingredients[i].takeItem())) return false ;
        }
        return true;
    }

    public void updateCraftingResult() {
        ItemBehavior[] ingredients = new ItemBehavior[] { CraftingSlot[1].viewItem(), CraftingSlot[2].viewItem(), CraftingSlot[3].viewItem(), CraftingSlot[4].viewItem() };
        if (CraftingSlot[0].viewItem() == null || CraftingSlot[0].accessible == SlotBehavior.AccesType.CLOSED) {
            CraftingSlot[0].accessible = SlotBehavior.AccesType.CLOSED;
            CraftingSlot[0].setItem(Crafting.getResult(ingredients, currentStation));
        }
    }

    public void takeCraftingResult() {
        if(CraftingSlot[0].accessible != SlotBehavior.AccesType.CLOSED) {
            CraftingSlot[1].updateSlot();
            CraftingSlot[2].updateSlot();
            CraftingSlot[3].updateSlot();
            CraftingSlot[4].updateSlot();
        }
    }

    public void craft(int amount) {
        ItemBehavior[] ingredients = new ItemBehavior[] { CraftingSlot[1].viewItem() , CraftingSlot[2].viewItem() , CraftingSlot[3].viewItem() , CraftingSlot[4].viewItem() };
        ItemBehavior res = Crafting.getResult(ingredients, currentStation);
        if (CraftingSlot[0].accessible == SlotBehavior.AccesType.CLOSED) {
            if(CraftingSlot[0].viewItem()!=null) Destroy(CraftingSlot[0].forceTake().gameObject);
            CraftingSlot[0].accessible = SlotBehavior.AccesType.TAKEONLY;
            CraftingSlot[0].forceAdd(Crafting.craft(ingredients, currentStation));
        } else {
            if (CraftingSlot[0].viewItem() == null || CraftingSlot[0].viewItem().type == res.type) {
                CraftingSlot[0].forceAdd(Crafting.craft(ingredients, currentStation));
                CraftingSlot[1].updateSlot();
                CraftingSlot[2].updateSlot();
                CraftingSlot[3].updateSlot();
                CraftingSlot[4].updateSlot();
            }
        }
        
    }

}
