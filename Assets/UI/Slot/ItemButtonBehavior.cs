using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonBehavior : ButtonBehavior
{
    
    [SerializeField] private RawImage itemImage;
    [SerializeField] private Text itemText;

    public Item.Type itemType = Item.Type.UNDEF;


    private void Awake() {
        setItem(Item.Type.UNDEF);
        buttonImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setItem(ItemBehavior item) {
        setItem(item.type, item.amount);
    }

    public void setItem(Item.Type type, int amount=0) {
        itemType = type;
        if(type == Item.Type.UNDEF) {
            itemImage.enabled = false;
            itemText.enabled = false;
        } else {
            itemImage.enabled = true;
            itemImage.texture = Item.getSprite(type);
            if (amount <= 0) {
                itemText.enabled = false;
            } else {
                itemText.enabled = true;
                itemText.text = ""+amount;
            }
        }
    }

    public override void click() {
        if (clickAble) {
            clickEvent((int)itemType);
        }
    }

    public override void mirror() {
        transform.localScale = new Vector3(transform.localScale.x  * - 1, transform.localScale.y , transform.localScale.z);
        itemImage.transform.localScale = new Vector3(itemImage.transform.localScale.x * - 1, itemImage.transform.localScale.y, itemImage.transform.localScale.z );
        itemText.transform.localScale = new Vector3(itemText.transform.localScale.x * - 1, itemText.transform.localScale.y, itemText.transform.localScale.z);
    }
}
