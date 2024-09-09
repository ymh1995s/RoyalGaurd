using UnityEngine;

public class BossMonster : BaseMonster
{
    //ü�¹� ����
    private RectTransform healthBarForeground;
    private Vector3 originalScale;
    private float bossMoveSpeed = 0.5f;

    protected override void Start()
    {
        base.Start();
 
        //animator.speed = 0.8f;

        hp = master_Hp[(int)Level.BOSS];
        speed = bossMoveSpeed;

        //ü�¹� ����
        healthBarForeground = transform.Find("HPBar/RED").GetComponent<RectTransform>();
        originalScale = healthBarForeground.localScale;
        UpdateHealthBar();
    }

    protected override void ChooseTarget()
    {
        target = commandCenter; //������ ������ ������ ���� �̵�
    }

    protected override void TakeDamage(int damage)
    {
        hp -= damage;
        UpdateHealthBar();

        Debug.Log($"BOSS HP {hp} damage {damage}");

        if (hp < 0)
        {
            SceneLoader.SceneLoad_ClearScene();
        }
    }

    void UpdateHealthBar()
    {
        // ü�� ���� ���
        float healthPercent = (float)hp / master_Hp[(int)Level.BOSS];

        // ü�¹��� ������ ����
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }
}

