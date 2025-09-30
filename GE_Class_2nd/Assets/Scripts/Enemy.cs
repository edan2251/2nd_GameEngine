using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Trace, Attack, RunAway }
    public EnemyState state = EnemyState.Idle;

    public float moveSpeed = 2f;
    public float traceRange = 15f;
    public float attackRange = 6f;
    public float attackCooldown = 1.5f;


    private HealthBarManager healthBarManager;

    public GameObject projectilePrefab;
    public Transform firePoint;

    public Transform player;

    private float lastAttackTime;
    public int maxHp = 5;
    public int currentHP;

    public float flashDuration = 0.1f; // ������ �����Ǵ� �ð�
    private Renderer enemyRenderer;
    private Color originalColor;


    //---------------AI ����------------------
    private UnityEngine.AI.NavMeshAgent agent;
    //----------------------------------------

    // Start is called before the first frame update
    public void Start()
    {
        currentHP = maxHp;

        healthBarManager = FindObjectOfType<HealthBarManager>();
        if (healthBarManager != null)
        {
            healthBarManager.RegisterEnemy(this); // �Ŵ����� �ڽ�(Enemy)�� ���
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;

        lastAttackTime = -attackCooldown;


        //---------------AI ����------------------
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed; // �̵� �Ӽ��� ��ũ��Ʈ ������ ����
        }
        //----------------------------------------


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
        //----------------�⺻�ڵ�-----------------------
        //Vector3 dir = (player.position - transform.position).normalized;
        //transform.position += dir * -moveSpeed * 2 * Time.deltaTime;

        //Vector3 oppositeDirection = -dir;

        //transform.rotation = Quaternion.LookRotation(oppositeDirection);
        //-----------------------------------------------

        //-------------------------------AI����---------------------------------------------
        if (agent == null) return;

        if (agent.speed != moveSpeed * 2f)
        {
            agent.speed = moveSpeed * 2f;
        }

        Vector3 runDirection = transform.position - player.position;
        Vector3 destination = transform.position + runDirection.normalized * traceRange;
        agent.SetDestination(destination);
        //--------------------------------------------------------------------------------
    }

    void TracePlayer()
    {
        //-----------------------�⺻�ڵ�-------------------------------
        //Vector3 dir = (player.position - transform.position).normalized;
        //transform.position += dir * moveSpeed * Time.deltaTime;
        //transform.LookAt(player.position);
        //--------------------------------------------------------------

        //-------------------------AI-------------------------------------
        if (agent == null) return;

        if (agent.speed != moveSpeed)
        {
            agent.speed = moveSpeed;
        }

        agent.SetDestination(player.position);
        //--------------------------------------------------------------
    }

    void AttackPlayer()
    {
        //���� ��ٿ�� �߻�
        if (Time.time >= lastAttackTime + attackCooldown)
        {

            if (agent == null) return;

            if (agent.speed != moveSpeed)
            {
                agent.speed = moveSpeed;
            }

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
        if (enemyRenderer != null)
        {
            // �ǰ� �� ������ ���������� ����
            enemyRenderer.material.SetColor("_BaseColor", Color.red);

            yield return new WaitForSeconds(flashDuration);

            // ���� �������� ����
            enemyRenderer.material.SetColor("_BaseColor", originalColor);
        }
    }


    public void TakeDamage(int damage)
    {
        FlashOnHit();
        currentHP -= damage;

        if (healthBarManager != null)
        {
            healthBarManager.UpdateEnemyHealth(this);
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (healthBarManager != null)
        {
            healthBarManager.UnregisterEnemy(this);
        }
        Destroy(gameObject);
    }
}
