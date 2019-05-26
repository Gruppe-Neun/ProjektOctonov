using UnityEngine;

public class Lasergun : MonoBehaviour {
    private float fireRate = 0.5f;
    private float fireTime = 0f;
    private float damage = 5f;
    private float range = 50f;
    private bool laserBlue;

    private LineRenderer lineRenderer;
    GameObject laserSource;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        laserSource = GameObject.FindGameObjectWithTag("LaserSource");
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.white;
        laserBlue = true; //laser is blue at the start
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

            IDamageable target = hit.transform.GetComponent<IDamageable>();
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

    public void SwitchLaser() {
        laserBlue = !laserBlue;
        if (laserBlue) {
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.white;
            range = 50f;
            damage = 5f;
        }
        else {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            range = 10f;
            damage = 10f;
        }
    }
}
