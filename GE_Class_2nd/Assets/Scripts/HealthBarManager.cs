using UnityEngine;
using System.Collections.Generic;

public class HealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab; 
    public Camera mainCamera; 

    // ���� Ȱ��ȭ�� ü�� �� UI�� �� ������ �����ϴ� ��ųʸ�
    private Dictionary<Enemy, GameObject> activeHealthBars = new Dictionary<Enemy, GameObject>();

    public void UpdateEnemyHealth(Enemy enemy)
    {
        // ��ųʸ����� �ش� ���� ü�� �� ������Ʈ�� ã���ϴ�.
        if (activeHealthBars.TryGetValue(enemy, out GameObject healthBarObj))
        {
            HealthBarUI healthBarUI = healthBarObj.GetComponent<HealthBarUI>();
            if (healthBarUI != null)
            {
                // ã�� HealthBarUI���� ���� ü�� ������ ������Ʈ�϶�� ����մϴ�.
                healthBarUI.UpdateHealth(enemy.currentHP, enemy.maxHp);
            }
        }
    }

    // Enemy���� ȣ��: ü�� �ٸ� �����ϰ� ��ųʸ��� �߰�
    public void RegisterEnemy(Enemy enemy)
    {
        if (activeHealthBars.ContainsKey(enemy)) return; // �̹� ��ϵ�

        // 1. ü�� �� UI ����
        GameObject healthBarObj = Instantiate(healthBarPrefab, transform);

        // 2. HealthBarUI ��ũ��Ʈ ���� �� �ʱ� ����
        HealthBarUI healthBarUI = healthBarObj.GetComponent<HealthBarUI>();
        if (healthBarUI != null)
        {
            healthBarUI.target = enemy.transform; // ���� ��� ����
            healthBarUI.targetEnemy = enemy;      // Enemy ��ũ��Ʈ ��ü �Ҵ�
            healthBarUI.uiCamera = mainCamera;    // ī�޶� �Ҵ�
            healthBarUI.UpdateHealth(enemy.currentHP, enemy.maxHp); // �ʱ� ü�� ����
        }

        // 3. ��ųʸ��� ���
        activeHealthBars.Add(enemy, healthBarObj);
    }

    // �ܺ�(Enemy ��� ��)���� ȣ��: ü�� �� ����
    public void UnregisterEnemy(Enemy enemy)
    {
        if (activeHealthBars.TryGetValue(enemy, out GameObject healthBarObj))
        {
            // 1. UI ������Ʈ �ı�
            Destroy(healthBarObj);

            // 2. ��ųʸ����� ����
            activeHealthBars.Remove(enemy);
        }
    }
}