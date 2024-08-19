using UnityEngine;

public class BossMonster : BaseMonster
{
    //ü�¹� ����
    private RectTransform healthBarForeground;
    private Vector3 originalScale;

    protected override void Start()
    {
        base.Start();

        animator.speed = 0.5f;

        hp = master_Hp[(int)Level.BOSS];
        speed = 0.5f;

        //ü�¹� ����
        healthBarForeground = transform.Find("HPBar/RED").GetComponent<RectTransform>();
        originalScale = healthBarForeground.localScale;
        UpdateHealthBar();
    }

    protected override void ChooseTarget()
    {
        target = commandCenter;
    }

    protected override void TakeDamage(int damage)
    {
        hp -= damage;
        UpdateHealthBar();

        if (hp < 0)
        {
            SceneLoader.SceneLoad_ClearScene();
        }
    }

    void UpdateHealthBar()
    {
        // ü�� ���� ���
        float healthPercent = (float)hp / master_Hp[(int)Level.BOSS];

        print(healthPercent);

        // ü�¹��� ������ ����
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }
}

