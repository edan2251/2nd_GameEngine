using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int Damage = 3;

    public Enemy enemy;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            Enemy hitEnemy = other.GetComponent<Enemy>();
            if (hitEnemy != null)
            {
                hitEnemy.HP -= Damage;
                if (hitEnemy.HP <= 0)
                {
                    Destroy(other.gameObject);
                }
            }
            Destroy(gameObject);
        }
    }
}