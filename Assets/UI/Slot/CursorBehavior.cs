using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorBehavior : SlotBehavior
{
    public float sensitivity = 1200;

    private InventoryBehavior inventory;

    private Collider2D hover;

    private float xPos = 0, yPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponentInParent<InventoryBehavior>();
        
        amount = GetComponentInChildren<Text>();
        slotImage = GetComponentsInChildren<RawImage>()[1];
        itemImage = GetComponentsInChildren<RawImage>()[2];
        itemImage.gameObject.SetActive(false);
        slotImage.gameObject.SetActive(false);
        amount.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        xPos += Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        yPos += Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        xPos = Mathf.Clamp(xPos, -960, 960);
        yPos = Mathf.Clamp(yPos, -540, 540);
        GetComponent<RectTransform>().localPosition = new Vector3(xPos,yPos,0);
        if (Input.GetKeyDown(KeyCode.Mouse0)&& hover != null) {
            inventory.switchCursor(hover.gameObject.GetComponent<SlotBehavior>());
        }
    }

    public override void updateSlot() {
        if (item == null) {
            amount.text = "";
            itemImage.texture = null;
            itemImage.gameObject.SetActive(false);
            slotImage.gameObject.SetActive(false);
            amount.gameObject.SetActive(false);
        } else {
            amount.text = "" + item.amount;
            itemImage.texture = Item.getSprite(item.type);
            itemImage.gameObject.SetActive(true);
            slotImage.gameObject.SetActive(true);
            amount.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        hover = collision;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision == hover) {
            hover = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {

    }
}
