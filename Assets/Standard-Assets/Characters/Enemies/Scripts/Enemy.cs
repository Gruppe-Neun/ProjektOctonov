using UnityEngine;

public class Enemy : MonoBehaviour, IDamageableEnemy
{
    public struct Loot {
        public Loot(Item.Type item, float perc, int am = 1) {
            itemType = item;
            percentage = perc;
            amount = am;
        }
        public Item.Type itemType;
        public int amount;
        public float percentage;
    }
    public float health = 50f;
    public Loot[] lootPool;
    

    public virtual void Start() {
        if (lootPool == null) {
            lootPool = new Loot[] {
                new Loot(Item.Type.Battery,0.25f),
                new Loot(Item.Type.LaserRed, 0.1f),
                new Loot(Item.Type.Flashlight, 0.25f),
                new Loot(Item.Type.Medkit, 0.05f),
                new Loot(Item.Type.Ironplate, 0.25f),
                new Loot(Item.Type.Nut, 0.25f)
            };
        }
    }

    public void TakeDamage(float damage) {
        //Debug.Log("ENEMY HEALTH: " + health);
        health -= damage;
        if(health <= 0f) {
            die();
        }
    }

    public void die() {
        if (lootPool != null) {
            for(int i = 0; i < lootPool.Length; i++) {
                if (lootPool[i].percentage > Random.value) {
                    Item.createItem(lootPool[i].itemType,lootPool[i].amount,new Vector3(transform.position.x+Random.value*0.1f,transform.position.y, transform.position.z + Random.value * 0.1f));
                }
            }
        }
        Destroy(gameObject);
    }
}
