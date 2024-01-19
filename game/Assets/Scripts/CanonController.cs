using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float bulletSpeed = 10f;
    public Transform firePoint;
    private readonly float rotationOffsetDegrees = -90f;
    public GameObject bulletPrefab;
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 10f;
    private bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;   
    }

    // Update is called once per frame
    void Update()
    {
        if(isReloading) {
            return;
        }
        MoveCanon();

        if (Input.GetMouseButtonDown(0)) {
            FireCanon();
        }

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading");

        yield return new WaitForSeconds(reloadTime);
        
        currentAmmo = maxAmmo;
        isReloading = false;
    }
    private void MoveCanon()
    {
        transform.rotation = GetRotationTowardsMouse(rotationOffsetDegrees);
    }

    private void FireCanon()
    {
        currentAmmo--;
        
        Vector3 bulletDirection = GetRotationTowardsMouse() * Vector3.right;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * bulletSpeed, ForceMode2D.Impulse);

        Destroy(bullet, 10);
    }

    private Quaternion GetRotationTowardsMouse(float offset = 0f)
    {
        // Get mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the angle in radians and convert it to degrees
        float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        // Rotate the object towards the mouse
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle + offset);

        return rotation;
    }
}
