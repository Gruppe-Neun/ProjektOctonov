using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehavior : MonoBehaviour
{
    [SerializeField] GameObject explosion;

    public bool friendly;
    public float damage;
    public float radius = 5;

    public Vector3 velocity;

    private bool impact = false;
    private float travelDistance = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!impact) {
            velocity += Vector3.down * Time.fixedDeltaTime * 9.81f;
            //velocity -= velocity/(1*Time.deltaTime);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, velocity, out hit, velocity.magnitude * Time.fixedDeltaTime)){
                transform.position = hit.point;
            } else {
                transform.position += velocity * Time.fixedDeltaTime;
            }

            travelDistance += velocity.magnitude * Time.fixedDeltaTime;
            if (travelDistance >= 100) Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.GetComponentInParent<TurretBehavior>()) {
            return;
        }
        impact = true;
        Destroy(Instantiate(explosion, this.transform.position, new Quaternion()), explosion.GetComponentInChildren<ParticleSystem>().duration);
        Collider[] hit = Physics.OverlapSphere(this.transform.position, radius);
        if (friendly) {
            foreach (Collider i in hit) {
                if (i.GetComponent<IDamageableEnemy>() != null) {
                    float dist = Vector3.Distance(transform.position, i.transform.position)-i.contactOffset;
                    if(dist < 0.5) {
                        i.GetComponent<IDamageableEnemy>().TakeDamage(damage);
                    } else {
                        i.GetComponent<IDamageableEnemy>().TakeDamage(dist / radius * damage);
                    }
                    
                    Debug.Log("damage applied");
                }
            }
        } else {
            foreach (Collider i in hit) {
                if (i.GetComponent<IDamageableFriendly>() != null) {
                    float dist = Vector3.Distance(transform.position, i.transform.position);
                    i.GetComponent<IDamageableFriendly>().TakeDamage(dist / radius * damage);
                }
            }
        }
        
        Destroy(gameObject);
    }
}
