using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuefteltisch : MonoBehaviour, IInteractable {

    GameObject uiGo;
    InventoryBehavior inventory;
    UIBehavior ui;

    void Start()
    {
        uiGo = GameObject.FindGameObjectWithTag("UI");
        inventory = uiGo.GetComponent<InventoryBehavior>();
        ui = uiGo.GetComponent<UIBehavior>();
    }

    public void Interact() {
        ui.openInventory();
        inventory.setCraftingStation(Crafting.CraftingStationType.Tuefteltisch);
    }
}