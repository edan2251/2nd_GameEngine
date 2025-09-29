using UnityEngine;
using System.Collections; 

public class Bomb : MonoBehaviour
{
    public int explosionDamage = 3;        
    public float detonationDelay = 0.3f;      // ���� ���� �� ����������� �ð�
    public float explosionRadius = 5f;      // ������ ����
    public GameObject explosionPrefab;      

    
    private bool fuseStarted = false; // ǻ� �̹� ���۵Ǿ�����

    
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
        //        hitEnemy.HP -= 1;       //������ ���� ������ ü�� 1����
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
        // 1. ���� ����Ʈ ���� (�ð� �� û�� ȿ��)
        if (explosionPrefab != null)
        {
            // ��ź�� ���� ��ġ�� ����Ʈ�� �����մϴ�.
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // 2. ���� ���� ���� ��� �浹ü (Collider)�� ����
        // Physics.OverlapSphere(�߽� ��ġ, �ݰ�)�� ����Ͽ� ���� ���� ���� ��� �浹ü�� �迭�� �����ɴϴ�.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            // 3. ���� ���� (���� ���� ������)
            // �浹ü�� Enemy ������Ʈ�� ������ �ִ��� Ȯ���մϴ�.
            Enemy hitEnemy = hit.GetComponent<Enemy>();

            if (hitEnemy != null)
            {
                // ���� HP�� ���� ���ҽ�ŵ�ϴ�.
                hitEnemy.HP -= explosionDamage;

                //Enemy ��ũ��Ʈ���� ȿ�� ����.
                hitEnemy.FlashOnHit();

                // ü���� 0 ���ϸ� ���� �ı��մϴ�.
                if (hitEnemy.HP <= 0)
                {
                    Destroy(hit.gameObject);
                }
            }
        }

        // 4. ��ź ������Ʈ ��ü�� �ı��Ͽ� ����
        Destroy(gameObject);
    }

    // (���� ����) Editor���� ���� �ݰ��� �ð������� Ȯ��
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // ���� �ݰ��� �� ���·� �ð�ȭ�մϴ�.
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}