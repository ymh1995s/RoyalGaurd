using UnityEngine;

public class BossMonster : BaseMonster
{
    //체력바 영역
    private RectTransform healthBarForeground;
    private Vector3 originalScale;

    protected override void Start()
    {
        base.Start();

        animator.speed = 0.5f;

        hp = master_Hp[(int)Level.BOSS];
        speed = 0.5f;

        //체력바 영역
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
        // 체력 비율 계산
        float healthPercent = (float)hp / master_Hp[(int)Level.BOSS];

        print(healthPercent);

        // 체력바의 스케일 조정
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }
}

