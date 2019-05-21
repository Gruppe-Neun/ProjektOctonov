using UnityEngine;

public class Lasergun : MonoBehaviour {
    public float damage = 5f;
    public float fireRate = 10f;
    public float fireTime = 0f;
    public float range = 100f;

    public Camera fpsCam;
    private LineRenderer lineRenderer;
    GameObject laserSource;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        laserSource = GameObject.FindGameObjectWithTag("LaserSource");
    }

    void Update() {
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

            Target target = hit.transform.GetComponent<Target>();
            if (target != null) {
                lineRenderer.SetPosition(1, laserSource.transform.position + (laserSource.transform.up * hit.distance));

                if (Time.time >= fireTime) {
                    fireTime = Time.time + fireRate;
                    target.TakeDamage(damage);
                }
            }
            else {
                lineRenderer.SetPosition(1, laserSource.transform.position + (laserSource.transform.up * range));
            }
        }
        else {
            lineRenderer.SetPosition(1, laserSource.transform.position + (laserSource.transform.up * range));
        }
    }
}
