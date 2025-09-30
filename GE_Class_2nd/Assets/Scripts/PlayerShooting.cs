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

    private bool useBomb = false;         // 무기 전환 상태 (false = 총알, true = 폭탄)

    void Start()
    {
        cam = Camera.main;

        if (handedBomb != null)
        {
            handedBomb.SetActive(useBomb); // useBomb이 false라면 비활성화된 상태로 시작
        }
        if (handedGun != null)
        {
            handedGun.SetActive(useBomb == false); 
        }
    }

    void Update()
    {
        // 무기 전환 (Z 키)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            useBomb = !useBomb;
            Debug.Log(useBomb ? "폭탄 모드" : "총알 모드");

            // --- handedBomb 활성화/비활성화 로직 추가 ---
            if (handedBomb != null) // null 체크는 안전을 위해 권장됩니다.
            {
                handedBomb.SetActive(useBomb);
            }
            // ------------------------------------------
            // --- handedGun 활성화/비활성화 로직 추가 ---
            if (handedGun != null) // null 체크는 안전을 위해 권장됩니다.
            {
                handedGun.SetActive(useBomb == false);
            }
            // ------------------------------------------
        }

        // 발사
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
        // 폭탄 생성
        GameObject bomb = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);

        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 throwForce = firePoint.forward * 10f + firePoint.up * 5f;
            rb.AddForce(throwForce, ForceMode.Impulse);
        }
    }
}