using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class UIBehavior : MonoBehaviour
{

    private RawImage UI_Top;
    private RawImage UI_Right;
    private RawImage UI_Left;
    private RawImage UI_Life;
    private RawImage UI_Armor;
    private RawImage UI_Crosshair;
    private RawImage UI_Cursor;
    private FirstPersonController player;
    private InventoryBehavior inventory;
    private float pos = 1.0f;
    private int status = 0;
    private FirstPersonController fps;
    

    // Start is called before the first frame update
    void Start()
    {
        UI_Top = GameObject.Find("UI_Top").GetComponent<RawImage>();
        UI_Right = GameObject.Find("UI_Right").GetComponent<RawImage>();
        UI_Left = GameObject.Find("UI_Left").GetComponent<RawImage>();
        UI_Life = GameObject.Find("UI_LifeFull").GetComponent<RawImage>();
        UI_Armor = GameObject.Find("UI_ArmorFull").GetComponent<RawImage>();
        UI_Crosshair = GameObject.Find("UI_Crosshair").GetComponent<RawImage>();
        UI_Crosshair.enabled = false;
        UI_Cursor = GameObject.Find("UI_Cursor").GetComponent<RawImage>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        inventory = GetComponent<InventoryBehavior>();

        UI_Crosshair.gameObject.SetActive(true);
        UI_Cursor.gameObject.SetActive(false);

        fps = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
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
                    status = 1;
                    UI_Cursor.gameObject.SetActive(false);
                    UI_Crosshair.gameObject.SetActive(true);
                    inventory.closeInventory();
                    fps.InventoryClosed();
                } else {
                    //open Inventory
                    status = -1;
                    UI_Cursor.gameObject.SetActive(true);
                    UI_Crosshair.gameObject.SetActive(false);
                    fps.InverntoryOpened();
                }

            }
        }


        


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

    public void updateHealth(float healthPercent, float armorPercent) {
        updateHealth(healthPercent);

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
}
