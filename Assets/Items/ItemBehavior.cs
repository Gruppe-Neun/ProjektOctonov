using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public Item.Type type = Item.Type.UNDEF;
    public int amount = 1;


    public int status = 0; // 0 = undef, 1= in Inventory, 2 = dropped

    


    // Start is called before the first frame update
    void Start()
    {
        
        if (status == 2) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void drop() {
        gameObject.SetActive(false);
        status = 2;
    }

    public void take() {
        gameObject.SetActive(false);
        status = 1;
    }
}
