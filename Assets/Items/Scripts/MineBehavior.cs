using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehavior : ContainerBehavior, IInteractable
{
    [SerializeField] Item.Type production;
    [SerializeField] int amount;


    // Start is called before the first frame update
    void Start()
    {
        type = ContainerBehavior.ContainerType.Mine;
        content = new ItemBehavior[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void waveClear() {
        if (content[0] != null && content[0].type == production) content[0].amount += amount;
        else if (content[0] == null) content[0] = Item.createItem(production, amount, new Vector3(0, 0, 0));
    }

}
