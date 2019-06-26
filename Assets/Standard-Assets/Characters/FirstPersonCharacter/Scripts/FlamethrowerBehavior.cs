using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerBehavior : MonoBehaviour
{
    public float damage = 10;
    List<IDamageableEnemy> inRange;
    private bool active = false;
    ParticleSystem flame;
    // Start is called before the first frame update
    void Start()
    {
        inRange = new List<IDamageableEnemy>();
        flame = GetComponent<ParticleSystem>();
    }

    public void setActive(bool act) {
        active = act;
        if (act) {
            flame.Play();
        } else {
            flame.Stop();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active) {
            foreach(IDamageableEnemy e in inRange) {
                if (e == null) {
                    inRange.Remove(e);
                } else {
                    e.TakeDamage(damage * Time.deltaTime);
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (!inRange.Contains(other.GetComponent<IDamageableEnemy>())) {
            inRange.Add(other.GetComponent<IDamageableEnemy>());
        }
    }

    public void OnTriggerExit(Collider other) {
        if (inRange.Contains(other.GetComponent<IDamageableEnemy>())) {
            inRange.Remove(other.GetComponent<IDamageableEnemy>());
        }
    }
}
