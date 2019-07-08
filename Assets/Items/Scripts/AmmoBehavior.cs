using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBehavior : ItemBehavior
{

    public float fireRate = 0.1f;
    public float damage = 1f;
    public int crosshairType = -1;

    public void setValues() {
        switch (this.type) {
            case Item.Type.LaserBlue:
                fireRate = 0.1f;
                damage = 2f;
                crosshairType = 0;
                break;

            case Item.Type.LaserRedLevel1:
                fireRate = 0.3f;
                damage = 5f;
                crosshairType = 0;
                break;
            case Item.Type.LaserRedLevel2:
                fireRate = 0.25f;
                damage = 9f;
                crosshairType = 0;
                break;

            case Item.Type.LaserGreenLevel1:
                fireRate = 0.1f;
                damage = 3f;
                crosshairType = 2;
                break;
            case Item.Type.LaserGreenLevel2:
                fireRate = 0.07f;
                damage = 8f;
                crosshairType = 2;
                break;

            case Item.Type.LaserSlowLevel1:
                fireRate = 0.3f;
                damage = 2f;
                crosshairType = 1;
                break;
            case Item.Type.LaserSlowLevel2:
                fireRate = 0.2f;
                damage = 2.5f;
                crosshairType = 1;
                break;

            case Item.Type.GrenadeLauncher:
                fireRate = 2;
                damage = 50;
                crosshairType = 2;
                break;

            case Item.Type.Flamethrower:
                fireRate = 1;
                damage = 10;
                crosshairType = 2;
                break;
        }
    }

    // Start is called before the first frame update


    // Update is called once per frame

}
