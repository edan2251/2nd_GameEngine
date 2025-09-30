using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Trace, Attack, RunAway }
    public EnemyState state = EnemyState.Idle;

    public float moveSpeed = 2f;
    public float traceRange = 15f;
    public float attackRange = 6f;
    public float attackCooldown = 1.5f;

    public GameObject projectilePrefab;
    public Transform firePoint;

    private Transform player;

    private float lastAttackTime;
    public int maxHp = 5;
    public int currentHP;

    private Slider HealthBar;

    public float flashDuration = 0.1f; // ������ �����Ǵ� �ð�
    private Renderer enemyRenderer;
    private Color originalColor;


    // Start is called before the first frame update
    void Start()
    {
        HealthBar.value = 1f;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        lastAttackTime = -attackCooldown;
        currentHP = maxHp;

        enemyRenderer = GetComponentInChildren<Renderer>();
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        //FSM ���� ��ȯ
        switch (state)
        {
            case EnemyState.Idle:
                if (dist < traceRange)
                    state = EnemyState.Trace;
                else if (currentHP <= maxHp / 5 * 2)
                    state = EnemyState.RunAway;
                break;

            case EnemyState.Trace:
                if (dist < attackRange)
                    state = EnemyState.Attack;
                else if (currentHP <= maxHp / 5 * 2)
                    state = EnemyState.RunAway;
                else if (dist > traceRange)
                    state = EnemyState.Idle;
                else
                    TracePlayer();
                    break;

            case EnemyState.Attack:
                if (dist > attackRange)
                    state = EnemyState.Trace;
                else if (currentHP <= maxHp / 5 * 2)
                    state = EnemyState.RunAway;
                else
                    AttackPlayer();
                break;

            case EnemyState.RunAway:
                if (dist > traceRange)
                    state = EnemyState.Idle;
                else
                    RunAway();
                    break;
        }
    }

    void RunAway()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * -moveSpeed * 2 * Time.deltaTime;

        Vector3 oppositeDirection = -dir;

        transform.rotation = Quaternion.LookRotation(oppositeDirection);
    }

    void TracePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.LookAt(player.position);
    }

    void AttackPlayer()
    {
        //���� ��ٿ�� �߻�
        if(Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if(projectilePrefab != null && firePoint != null)
        {
            transform.LookAt(player.position);
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
            if(ep != null)
            {
                Vector3 dir = (player.position - firePoint.position).normalized;
                ep.SetDirection(dir);
            }
        }
    }

    public void FlashOnHit()
    {
        // �ߺ� ���� ������ ���� �̹� �ڷ�ƾ�� ���� ���̸� ���� �� �ٽ� ����
        StopAllCoroutines();
        StartCoroutine(FlashColor());
    }

    // �ǰ� �� ������ �����̴� �ڷ�ƾ
    IEnumerator FlashColor()
    {
        // �ǰ� �� ������ ���������� ����
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = Color.red;
        }

        // flashDuration ��ŭ ���
        yield return new WaitForSeconds(flashDuration);

        // ���� �������� ����
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = originalColor;
        }
    }

    public void SetupHealthBar(Canvas canvas, Camera camera)
    {
        //HealthBar.transform.SetParent(Canvas.transform);
        //if(HealthBar.TryGetComponent<MainCamera>(out MainCamera faceCamera))
        //{
        //    faceCamera.Camera = Camera;
        //}
    }

    public void TakeDamage(int damage)
    {
        FlashOnHit();
        currentHP -= damage;

        HealthBar.value = (float)currentHP / maxHp;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(HealthBar.gameObject);
        Destroy(gameObject);
    }
}
