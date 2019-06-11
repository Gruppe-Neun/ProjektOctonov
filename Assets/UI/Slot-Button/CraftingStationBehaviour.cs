using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingStationBehaviour : ItemButtonBehavior {

    private InventoryBehavior inventory;


    protected override void Awake() {
        base.Awake();
        inventory = GameObject.FindGameObjectWithTag("UI").GetComponent<InventoryBehavior>();
    }

    // Update is called once per frame
    void Update() {
        setItem(Crafting.GetStationItemType(inventory.currentStation));
    }
}
