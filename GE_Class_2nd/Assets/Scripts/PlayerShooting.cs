/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootFront();
            //Shoot();
        }
    }
    void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;
        targetPoint = ray.GetPoint(50f);
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
    }

    void ShootFront()
    {
        Vector3 direction = firePoint.forward;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;   
    public GameObject bombPrefab;         

    public Transform firePoint;
    Camera cam;

    private bool useBomb = false;         // ¹«±â ÀüÈ¯ »óÅÂ (false = ÃÑ¾Ë, true = ÆøÅº)

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // ¹«±â ÀüÈ¯ (Z Å°)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            useBomb = !useBomb;
            Debug.Log(useBomb ? "ÆøÅº ¸ðµå" : "ÃÑ¾Ë ¸ðµå");
        }

        // ¹ß»ç
        if (Input.GetMouseButtonDown(0))
        {
            if (useBomb)
                ThrowBomb();
            else
                ShootFront();
        }
    }

    void ShootFront()
    {
        Vector3 direction = firePoint.forward;
        Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
    }

    void ThrowBomb()
    {
        // ÆøÅº »ý¼º
        GameObject bomb = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);

        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 throwForce = firePoint.forward * 10f + firePoint.up * 5f;
            rb.AddForce(throwForce, ForceMode.Impulse);
        }
    }
}