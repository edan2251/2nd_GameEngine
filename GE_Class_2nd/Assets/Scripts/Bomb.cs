using UnityEngine;
using System.Collections; 

public class Bomb : MonoBehaviour
{
    public int explosionDamage = 3;        
    public float detonationDelay = 0.3f;      // 땅에 닿은 후 터지기까지의 시간
    public float explosionRadius = 5f;      // 폭발의 범위
    public GameObject explosionPrefab;      

    
    private bool fuseStarted = false; // 퓨즈가 이미 시작되었는지

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !fuseStarted)
        {
            fuseStarted = true;
            StartCoroutine(DetonateAfterDelay(detonationDelay));
        }


        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    Enemy hitEnemy = (collision.gameObject.GetComponent<Enemy>());
        //    if (hitEnemy != null)
        //    {
        //        hitEnemy.HP -= 1;       //적한테 직접 맞으면 체력 1깍음
        //        if (hitEnemy.HP <= 0)
        //        {
        //            Destroy(collision.gameObject.gameObject);
        //        }
        //    }
        //    Destroy(gameObject);
        //}
    }

    IEnumerator DetonateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Explode();
    }

    void Explode()
    {
        // 1. 폭발 이펙트 생성 (시각 및 청각 효과)
        if (explosionPrefab != null)
        {
            // 폭탄이 터진 위치에 이펙트를 생성합니다.
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // 2. 폭발 범위 내의 모든 충돌체 (Collider)를 감지
        // Physics.OverlapSphere(중심 위치, 반경)를 사용하여 폭발 범위 내의 모든 충돌체를 배열로 가져옵니다.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            // 3. 피해 적용 (범위 내의 적에게)
            // 충돌체가 Enemy 컴포넌트를 가지고 있는지 확인합니다.
            Enemy hitEnemy = hit.GetComponent<Enemy>();

            if (hitEnemy != null)
            {
                // 적의 HP를 직접 감소시킵니다.
                hitEnemy.HP -= explosionDamage;

                //Enemy 스크립트에서 효과 실행.
                hitEnemy.FlashOnHit();

                // 체력이 0 이하면 적을 파괴합니다.
                if (hitEnemy.HP <= 0)
                {
                    Destroy(hit.gameObject);
                }
            }
        }

        // 4. 폭탄 오브젝트 자체를 파괴하여 제거
        Destroy(gameObject);
    }

    // (선택 사항) Editor에서 폭발 반경을 시각적으로 확인
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // 폭발 반경을 구 형태로 시각화합니다.
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}