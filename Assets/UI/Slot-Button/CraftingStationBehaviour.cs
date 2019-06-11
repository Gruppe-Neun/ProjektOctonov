using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingStationBehaviour : ItemButtonBehavior {

    private InventoryBehavior inventory;


    protected override void Awake() {
        base.Awake();
        inventory = GameObject.FindGameObjectWithTag("UI").GetComponent<InventoryBehavior>();
        gameObject.transform.localScale = new Vector3(1,1,1);
    }

    // Update is called once per frame
    void Update() {
        setItem(Crafting.GetStationItemType(inventory.currentStation));
    }
}
