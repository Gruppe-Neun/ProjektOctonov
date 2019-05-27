using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 1f;


    private float distance = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        distance -= speed * Time.deltaTime;
        if (distance <= 0) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<IDamageable>() != null){
            other.GetComponent<IDamageable>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
