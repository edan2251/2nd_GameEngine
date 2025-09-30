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

    public GameObject handedBomb;
    public GameObject handedGun;

    public Transform firePoint;
    Camera cam;

    private bool useBomb = false;         // ���� ��ȯ ���� (false = �Ѿ�, true = ��ź)

    void Start()
    {
        cam = Camera.main;

        if (handedBomb != null)
        {
            handedBomb.SetActive(useBomb); // useBomb�� false��� ��Ȱ��ȭ�� ���·� ����
        }
        if (handedGun != null)
        {
            handedGun.SetActive(useBomb == false); 
        }
    }

    void Update()
    {
        // ���� ��ȯ (Z Ű)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            useBomb = !useBomb;
            Debug.Log(useBomb ? "��ź ���" : "�Ѿ� ���");

            // --- handedBomb Ȱ��ȭ/��Ȱ��ȭ ���� �߰� ---
            if (handedBomb != null) // null üũ�� ������ ���� ����˴ϴ�.
            {
                handedBomb.SetActive(useBomb);
            }
            // ------------------------------------------
            // --- handedGun Ȱ��ȭ/��Ȱ��ȭ ���� �߰� ---
            if (handedGun != null) // null üũ�� ������ ���� ����˴ϴ�.
            {
                handedGun.SetActive(useBomb == false);
            }
            // ------------------------------------------
        }

        // �߻�
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
        // ��ź ����
        GameObject bomb = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);

        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 throwForce = firePoint.forward * 10f + firePoint.up * 5f;
            rb.AddForce(throwForce, ForceMode.Impulse);
        }
    }
}