using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform player;
    public int HP;


    public float flashDuration = 0.1f; // 빨갛게 유지되는 시간
    private Renderer enemyRenderer;
    private Color originalColor;


    // Start is called before the first frame update
    void Start()
    {
        HP = 5;
        player = GameObject.FindGameObjectWithTag("Player").transform;

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

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(player.position);
    }

    public void FlashOnHit()
    {
        // 중복 실행 방지를 위해 이미 코루틴이 실행 중이면 정지 후 다시 시작
        StopAllCoroutines();
        StartCoroutine(FlashColor());
    }

    // 피격 시 빨갛게 깜빡이는 코루틴
    IEnumerator FlashColor()
    {
        // 피격 시 색상을 빨간색으로 변경
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = Color.red;
        }

        // flashDuration 만큼 대기
        yield return new WaitForSeconds(flashDuration);

        // 원래 색상으로 복구
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = originalColor;
        }
    }

}
