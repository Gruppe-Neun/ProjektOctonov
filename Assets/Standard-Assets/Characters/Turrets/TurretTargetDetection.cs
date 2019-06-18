using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTargetDetection : MonoBehaviour
{
    private GameObject target;
    private TurretBehavior parent;

    private void Start() {
        parent = GetComponentInParent<TurretBehavior>();
    }

    public void OnTriggerEnter(Collider other) {
        if (target == null && other.GetComponent<IDamageableEnemy>() != null) {
            target = other.gameObject;
            parent.setTarget(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other) {
        if (target == null || other.gameObject == target.gameObject) {
            target = null;
            parent.setTarget(null);
        }
    }
}
