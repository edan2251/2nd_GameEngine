using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform player;
    public int HP;


    public float flashDuration = 0.1f; // ������ �����Ǵ� �ð�
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

}
