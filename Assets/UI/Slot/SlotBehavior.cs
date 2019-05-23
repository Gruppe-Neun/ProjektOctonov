using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotBehavior : MonoBehaviour
{
    protected ItemBehavior item;
    public Item.useType useType = Item.useType.GENERIC;

    protected Text amount;
    protected RawImage slotImage;
    protected RawImage itemImage;

    // Start is called before the first frame update
    void Start()
    {
        amount = GetComponentInChildren<Text>();
        slotImage = GetComponent<RawImage>();
        itemImage = GetComponentsInChildren<RawImage>()[1];
        itemImage.gameObject.SetActive(false);
        updateSlot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void updateSlot() {
        if (item == null) {
            amount.text = "";
            itemImage.texture = null;
            itemImage.gameObject.SetActive(false);
        } else {
            amount.text = "" + item.amount;
            itemImage.texture = Item.getSprite(item.type);
            itemImage.gameObject.SetActive(true);
        } 
    }

    public bool addItem(ItemBehavior neu) {
        if (neu == null) {
            return true;
        }
        if (item == null) {
            if (useType==Item.useType.GENERIC||useType==neu.useType) {
                item = neu;
                neu.take();
                updateSlot();
                return true;
            } else {
                return false;
            }
        }
        if (item.type == neu.type) {
            item.amount += neu.amount;
            Destroy(neu.gameObject);
            updateSlot();
            return true;
        }
        return false;
    }

    public void setItem(ItemBehavior neu) {
        item = neu;
        updateSlot();
    }

    public ItemBehavior viewItem() {
        return item;
    }

    public ItemBehavior takeItem() {
        if (item == null) return null;
        ItemBehavior ret = item;
        item = null;
        updateSlot();
        return ret;
    }

    public void mirror(bool set) {
        if(set) {
            transform.localScale = new Vector3(-1,1,1);
            GetComponentInChildren<Text>().transform.localScale = new Vector3(-1,1,1);
            GetComponentsInChildren<RawImage>()[1].transform.localScale = new Vector3(-1, 1, 1);
        } else {
            transform.localScale = new Vector3(1, 1, 1);
            GetComponentInChildren<Text>().transform.localScale = new Vector3(1, 1, 1);
            GetComponentsInChildren<RawImage>()[1].transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
