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

    }

    IEnumerator DetonateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Explode();
    }

    void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Enemy hitEnemy = hit.GetComponent<Enemy>();

            if (hitEnemy != null)
            {
                hitEnemy.TakeDamage(explosionDamage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}