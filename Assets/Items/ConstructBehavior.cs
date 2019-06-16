using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructBehavior : MonoBehaviour, IInteractable
{
    public TurretBehavior redTurret;
    public TurretBehavior blueTurret;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool construct(Item.Type item) {
        /*
        if (Item.getUseType(item) != Item.useType.CORE) {
            return false;
        }
        */
        switch ((int)item) {
            case (int)Item.Type.CristalRed:
                Instantiate(redTurret, new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z), new Quaternion());
                Destroy(this.gameObject);
                return true;

            case (int)Item.Type.CristalBlue:
                Instantiate(blueTurret, new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z), new Quaternion());
                Destroy(this.gameObject);
                return true;

            default:
                return false;
        }


        return false;
    }

    public void Interact() {
        GameObject.Find("UI").GetComponent<UIBehavior>().openConstruct(this);
    }
}
