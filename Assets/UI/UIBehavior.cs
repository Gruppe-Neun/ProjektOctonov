using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System;

public class UIBehavior : MonoBehaviour
{
    [SerializeField] private ItemButtonBehavior craftingStationButton;
    [SerializeField] private ItemButtonBehavior itemButton;
    [SerializeField] private ItemButtonBehavior itemButtonLeftOuter;
    [SerializeField] private ButtonBehavior leftArrowButton;
    [SerializeField] private ButtonBehavior rightArrowButton;

    [SerializeField] private Texture[] Crosshairs;
    [SerializeField] private ButtonBehavior[] menuButtons; 
    

    private RawImage UI_Top;
    private RawImage UI_Right;
    private RawImage UI_Left;
    private RawImage UI_Life;
    private RawImage UI_Armor;
    private RawImage UI_Crosshair; 
    private RawImage UI_Cursor;
    private RawImage UI_Container;
    private RawImage UI_Menu;
    private RawImage UI_Warning;
    private Text UI_WarningText;
    private RawImage UI_WaveCounter;
    private Text UI_WaveCounterText;

    private FirstPersonController player;
    private InventoryBehavior inventory;
    private float pos = 1.0f;
    private int status = 0;
    private float warningStatus = -1f;

    //recipeBook
    private Crafting.Recipe[] recipeBuffer;
    private int recipeBookPage = 0;
    private ItemButtonBehavior[] recipeItemButton;

    // Start is called before the first frame update
    void Awake()
    {
        UI_Top = GameObject.Find("UI_Top").GetComponent<RawImage>();
        UI_Right = GameObject.Find("UI_Right").GetComponent<RawImage>();
        UI_Left = GameObject.Find("UI_Left").GetComponent<RawImage>();
        UI_Life = GameObject.Find("UI_LifeFull").GetComponent<RawImage>();
        UI_Armor = GameObject.Find("UI_ArmorFull").GetComponent<RawImage>();
        UI_Crosshair = GameObject.Find("UI_Crosshair").GetComponent<RawImage>();
        UI_Container = GameObject.Find("UI_Container").GetComponent<RawImage>();
        UI_Cursor = GameObject.Find("UI_Cursor").GetComponent<RawImage>();
        UI_Menu = GameObject.Find("UI_Menu").GetComponent<RawImage>();
        UI_Warning = GameObject.Find("UI_Warning").GetComponent<RawImage>();
        UI_WarningText = UI_Warning.GetComponentInChildren<Text>();
        UI_WaveCounter = GameObject.Find("UI_WaveCounter").GetComponent<RawImage>();
        UI_WaveCounterText = UI_WaveCounter.GetComponentInChildren<Text>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        inventory = GetComponent<InventoryBehavior>();

        UI_Crosshair.gameObject.SetActive(true);
        UI_Cursor.gameObject.SetActive(false);

        //setup recipeBook
        GameObject.Find("UI_RecipeBookButton").GetComponent<ButtonBehavior>().clickEvent = openRecipeBook;
        leftArrowButton.clickEvent = leftArrowPressed;
        rightArrowButton.clickEvent = rightArrowPressed;
        recipeItemButton = new ItemButtonBehavior[14*6];
        for(int i = 0; i < 14; i++) {
            float posX = -710 + 830 * (int)(i/7), posY = 300 - 100*(i%7), posZ = 0;
            recipeItemButton[i * 6 + 0] = Instantiate<ItemButtonBehavior>(itemButtonLeftOuter, UI_Menu.transform);
            recipeItemButton[i * 6 + 0].clickEvent = searchRecipeResult;
            recipeItemButton[i * 6 + 1] = Instantiate<ItemButtonBehavior>(itemButton, UI_Menu.transform);
            recipeItemButton[i * 6 + 1].clickEvent = searchRecipeResult;
            recipeItemButton[i * 6 + 2] = Instantiate<ItemButtonBehavior>(itemButton, UI_Menu.transform);
            recipeItemButton[i * 6 + 2].clickEvent = searchRecipeResult;
            recipeItemButton[i * 6 + 3] = Instantiate<ItemButtonBehavior>(itemButton, UI_Menu.transform);
            recipeItemButton[i * 6 + 3].clickEvent = searchRecipeResult;
            recipeItemButton[i * 6 + 4] = Instantiate<ItemButtonBehavior>(craftingStationButton, UI_Menu.transform);
            //recipeItemButton[i * 6 + 4].clickEvent = searchRecipeStation;
            recipeItemButton[i * 6 + 5] = Instantiate<ItemButtonBehavior>(itemButtonLeftOuter, UI_Menu.transform);
            recipeItemButton[i * 6 + 5].clickEvent = chooseRecipe;
            recipeItemButton[i * 6 + 5].clickEventParam = i;
            recipeItemButton[i * 6 + 5].clickEventItemParam = false;
            recipeItemButton[i * 6 + 5].mirror();

            recipeItemButton[i * 6 + 0].transform.localPosition = new Vector3(posX, posY, 0);
            recipeItemButton[i * 6 + 1].transform.localPosition = new Vector3(posX + 110, posY, posZ);
            recipeItemButton[i * 6 + 2].transform.localPosition = new Vector3(posX + 220, posY, posZ);
            recipeItemButton[i * 6 + 3].transform.localPosition = new Vector3(posX + 330, posY, posZ);
            recipeItemButton[i * 6 + 4].transform.localPosition = new Vector3(posX + 460, posY, posZ);
            recipeItemButton[i * 6 + 5].transform.localPosition = new Vector3(posX + 590, posY, posZ);
        }
        menuButtons[0].clickEvent = openRecipeBook;
        menuButtons[1].clickEvent = searchRecipeResultUseType;
        menuButtons[1].clickEventParam = (int)Item.useType.AMMO;
        menuButtons[2].clickEvent = searchRecipeResultUseType;
        menuButtons[2].clickEventParam = (int)Item.useType.ACTIVE;
        menuButtons[3].clickEvent = searchRecipeResultUseType;
        menuButtons[3].clickEventParam = (int)Item.useType.CORE;

        menuButtons[8].clickEvent = closeRecipeBook;
        UI_Menu.gameObject.SetActive(false);
        UI_Warning.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (status != 0){
            pos += status * Time.deltaTime * 3;
            if (pos >= 1|| pos<=0) {
                pos = (int)pos;
                status = 0;
            }
            UI_Top.transform.localPosition = new Vector3(0, 250 * pos + 1, 0);
            UI_Right.transform.localPosition = new Vector3(100 * pos + 1, -250 * pos , 0);
            UI_Left.transform.localPosition = new Vector3(-100 * pos - 1, -250 * pos , 0);
        } else {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                if (pos == 0f) {
                    //close Inventory
                    closeAll();
                } else {
                    //open Inventory
                    openInventory();
                }

            }
        }
        if (warningStatus != -1) {
            if (warningStatus >= 3) {
                warningStatus = -1;
                UI_Warning.gameObject.SetActive(false);
                UI_WarningText.color = new Color(0.45f, 0, 0, 0);
                UI_Warning.color = new Color(1, 1, 1, 0);
            } else {
                if (warningStatus >= 2) {
                    warningStatus += Time.deltaTime;
                    UI_WarningText.color = new Color(0.45f, 0, 0, -warningStatus + 3);
                    UI_Warning.color = new Color(1, 1, 1, -warningStatus + 3 / 4);
                } else {
                    if(warningStatus >= 1) {
                        warningStatus += Time.deltaTime;
                        UI_WarningText.color = new Color(0.45f, 0, 0, 1);
                        UI_Warning.color = new Color(1, 1, 1, 0.25f);
                    } else {
                        warningStatus += Time.deltaTime;
                        UI_WarningText.color = new Color(0.45f, 0, 0, warningStatus);
                        UI_Warning.color = new Color(1, 1, 1, warningStatus / 4);
                    }
                }
            }
        }
    }

    public void openInventory() {
        status = -1;
        UI_Cursor.gameObject.SetActive(true);
        UI_Crosshair.gameObject.SetActive(false);
        player.InverntoryOpened();
    }
    
    public void openContainer(ContainerBehavior container) {
        inventory.openContainer(container);
        openInventory();
    }

    public void openConstruct(ConstructBehavior construct) {
        inventory.openConstruct(construct);
        openInventory();
    }

    public void openTurret(TurretBehavior turret) {
        inventory.openTurret(turret);
        openInventory();
    }

    public void searchRecipeResultUseType(int itemUseType) {
        recipeBuffer = Crafting.getByResult((Item.useType)itemUseType);
        loadRecipePage(0);
    }

    public void searchRecipeResult(int itemType) {
        recipeBuffer = Crafting.getByResult((Item.Type)itemType);
        loadRecipePage(0);
    }

    public void searchRecipeIngredient(int itemType) {
        recipeBuffer = Crafting.getByIgredient((Item.Type)itemType);
        loadRecipePage(0);
    }

    public void chooseRecipe(int pagePos) {
        inventory.setUpRecipe(recipeBuffer[pagePos + recipeBookPage * 14]);
    }

    public void openRecipeBook(int nothing=0) {
        recipeBuffer = Crafting.getAll();
        UI_Menu.gameObject.SetActive(true);
        loadRecipePage(0);
    }

    public void closeRecipeBook(int nothing=0) {
        UI_Menu.gameObject.SetActive(false);
    }

    public void rightArrowPressed(int nothing) {
        loadRecipePage(recipeBookPage + 1);
    }

    public void leftArrowPressed(int nothing) {
        loadRecipePage(recipeBookPage - 1);
    }

    private void loadRecipePage(int page) {

        if (page < 0) { recipeBookPage = (int)(recipeBuffer.Length / 14); } else {
            if (page > (int)((recipeBuffer.Length) / 14)) recipeBookPage = 0;
            else recipeBookPage = page;
        }
        int count = recipeBuffer.Length - recipeBookPage * 14;
        if (count > 14) count = 14;
        for(int i = 0;i < count; i++) {
            recipeItemButton[i * 6 + 0].gameObject.SetActive(true);
            recipeItemButton[i * 6 + 1].gameObject.SetActive(true);
            recipeItemButton[i * 6 + 2].gameObject.SetActive(true);
            recipeItemButton[i * 6 + 3].gameObject.SetActive(true);
            recipeItemButton[i * 6 + 4].gameObject.SetActive(true);
            recipeItemButton[i * 6 + 5].gameObject.SetActive(true);

            recipeItemButton[i * 6 + 0].setItem(recipeBuffer[i + recipeBookPage * 14].ingredients[3], recipeBuffer[i + recipeBookPage * 14].amount[3]);
            recipeItemButton[i * 6 + 1].setItem(recipeBuffer[i + recipeBookPage * 14].ingredients[2], recipeBuffer[i + recipeBookPage * 14].amount[2]);
            recipeItemButton[i * 6 + 2].setItem(recipeBuffer[i + recipeBookPage * 14].ingredients[1], recipeBuffer[i + recipeBookPage * 14].amount[1]);
            recipeItemButton[i * 6 + 3].setItem(recipeBuffer[i + recipeBookPage * 14].ingredients[0], recipeBuffer[i + recipeBookPage * 14].amount[0]);
            recipeItemButton[i * 6 + 4].setItem(Crafting.GetStationItemType(recipeBuffer[i + recipeBookPage * 14].craftingstation), 0);
            recipeItemButton[i * 6 + 5].setItem(recipeBuffer[i + recipeBookPage * 14].result, recipeBuffer[i + recipeBookPage * 14].resultAmount);

            recipeItemButton[i * 6 + 0].clickAble = recipeBuffer[i + recipeBookPage * 14].ingredients[3] != Item.Type.UNDEF;
            recipeItemButton[i * 6 + 1].clickAble = recipeBuffer[i + recipeBookPage * 14].ingredients[2] != Item.Type.UNDEF;
            recipeItemButton[i * 6 + 2].clickAble = recipeBuffer[i + recipeBookPage * 14].ingredients[1] != Item.Type.UNDEF;
            recipeItemButton[i * 6 + 3].clickAble = recipeBuffer[i + recipeBookPage * 14].ingredients[0] != Item.Type.UNDEF;
            recipeItemButton[i * 6 + 4].clickAble = false;
            recipeItemButton[i * 6 + 5].clickAble = recipeBuffer[i + recipeBookPage * 14].result != Item.Type.UNDEF;
        }
        for(int i = count; i < 14; i++) {
            recipeItemButton[i * 6 + 0].gameObject.SetActive(false);
            recipeItemButton[i * 6 + 1].gameObject.SetActive(false);
            recipeItemButton[i * 6 + 2].gameObject.SetActive(false);
            recipeItemButton[i * 6 + 3].gameObject.SetActive(false);
            recipeItemButton[i * 6 + 4].gameObject.SetActive(false);
            recipeItemButton[i * 6 + 5].gameObject.SetActive(false);
        }
    }

    public void closeAll() {
        status = 1;
        UI_Menu.gameObject.SetActive(false);
        UI_Cursor.gameObject.SetActive(false);
        UI_Crosshair.gameObject.SetActive(true);
        inventory.closeInventory();
        player.InventoryClosed();
    }

    public void updateHealth(float percent) {

        if (percent >= 1) percent = 1;
        if (percent <= 0) percent = 0;

        percent *= 0.546f;
        percent += 0.227f;

        Rect newUV = UI_Life.uvRect;
        newUV.width = percent;
        UI_Life.uvRect = newUV;
        UI_Life.rectTransform.localPosition = new Vector3(-960+960*percent,0,0);
        UI_Life.rectTransform.sizeDelta = new Vector2(1920*percent,1080);
    }

    public void updateAmor(float armorPercent) {
        if (armorPercent >= 1) armorPercent = 1;
        if (armorPercent <= 0) armorPercent = 0;

        armorPercent *= 0.510f;
        armorPercent += 0.245f;

        Rect newUV = UI_Armor.uvRect;
        newUV.width = armorPercent;
        UI_Armor.uvRect = newUV;
        UI_Armor.rectTransform.localPosition = new Vector3(-960 + 960 * armorPercent, 0, 0);
        UI_Armor.rectTransform.sizeDelta = new Vector2(1920 * armorPercent, 1080);
    }

    public void setCrosshairType(int type) {
        if (type == -1) {
            UI_Crosshair.enabled = false;
        } else {
            if (type < Crosshairs.Length) {
                UI_Crosshair.texture = Crosshairs[type];
                UI_Crosshair.enabled = true;
            }
        }
    }
    
    public void sendWarning(string message= "Olli Ordner is\ntaking Damage!") {
        UI_WarningText.text = message;
        if (warningStatus == -1) {
            warningStatus = 0;
            UI_Warning.gameObject.SetActive(true);
        } else {
            /*
            if (warningStatus > 2) {
                warningStatus -= 3;
                warningStatus = -warningStatus;
            } else {
                if (warningStatus > 1) {
                    warningStatus = 1;
                }
            }
            */
        }
       
    }

    public void setWaveCounter(bool show, int wave = 0, float countdown = 0) {
        if (show) {
            UI_WaveCounter.gameObject.SetActive(true);
            UI_WaveCounterText.text = "Wave " + wave + " in:\n" + (int)countdown;
        } else {
            UI_WaveCounter.gameObject.SetActive(false);
        }
    }
}
