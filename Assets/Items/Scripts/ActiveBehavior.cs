using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBehavior : ItemBehavior
{
    UnityStandardAssets.Characters.FirstPerson.FirstPersonController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }


    public override void use() {
        switch (this.type) {
            case Item.Type.Wrench:
                GameObject selection = playerController.getviewedObject(10);
                if (selection!=null && selection.GetComponentInParent<OlliOrdnerBehavior>()) {
                    selection.GetComponentInParent<OlliOrdnerBehavior>().healLife(100);
                } else {
                    return;
                }
                break;

            
            case Item.Type.RedButton:
                GameObject.Find("3D Drucker").GetComponent<OlliOrdnerBehavior>().explode(20, 100* GameObject.Find("3D Drucker").GetComponent<OlliOrdnerBehavior>().level);
                break;

        }
        if (amount == -1) {
            return;
        } else {
            if (amount != 0) {
                amount--;
                if (amount == 0) {
                    Destroy(this.gameObject);
                }
            } 
        }

    }
}
