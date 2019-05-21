using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float damage) {

        Debug.Log("ENEMY HEALTH: " + health);
        health -= damage;
        if(health <= 0f) {
            Destroy(gameObject);
        }
    }
}
