using UnityEngine;

public class Lasergun : MonoBehaviour {
    public float damage = 5f;
    public float fireRate = 0.5f;
    public float fireTime = 0f;
    public float range = 50f;

    private LineRenderer lineRenderer;
    GameObject laserSource;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        laserSource = GameObject.FindGameObjectWithTag("LaserSource");
    }

    public void Combat() {
        if (Input.GetButton("Fire1")) {
            lineRenderer.enabled = true;
            Shoot();
        }
        else {
            lineRenderer.enabled = false;
        }
    }

    void Shoot() {
        lineRenderer.SetPosition(0, laserSource.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(laserSource.transform.position, laserSource.transform.up, out hit, range)) {
            lineRenderer.SetPosition(1, hit.point);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null) {
                if (Time.time >= fireTime) {
                    fireTime = Time.time + fireRate;
                    target.TakeDamage(damage);
                }
            }
        }
        else {
            lineRenderer.SetPosition(1, laserSource.transform.position + (laserSource.transform.up * range));
        }
    }
}
