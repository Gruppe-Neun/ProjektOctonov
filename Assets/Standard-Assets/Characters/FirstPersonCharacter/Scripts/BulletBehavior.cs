using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 1f;
    public bool friendly = true;
    public float slowAmount = 0.2f;
    public float slowTime = -1;

    private float distance = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FixedUpdate() {
        transform.position += transform.forward * speed * Time.deltaTime;
        distance -= speed * Time.deltaTime;
        if (distance <= 0) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(friendly && other.GetComponent<IDamageableEnemy>() != null){
            other.GetComponent<IDamageableEnemy>().TakeDamage(damage);
            if (slowTime > 0) {
                other.GetComponent<IDamageableEnemy>().slow(slowAmount, slowTime);
            }
            Destroy(this.gameObject);
        } else {
            if(!friendly &&  other.GetComponent<IDamageableFriendly>() != null) {
                other.GetComponent<IDamageableFriendly>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
