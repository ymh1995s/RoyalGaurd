using UnityEngine;

public class BossMonster : BaseMonster
{
    //체력바 영역
    private RectTransform healthBarForeground;
    private Vector3 originalScale;
    private float bossMoveSpeed = 0.5f;

    protected override void Start()
    {
        base.Start();
 
        //animator.speed = 0.8f;

        hp = master_Hp[(int)Level.BOSS];
        speed = bossMoveSpeed;

        //체력바 영역
        healthBarForeground = transform.Find("HPBar/RED").GetComponent<RectTransform>();
        originalScale = healthBarForeground.localScale;
        UpdateHealthBar();
    }

    protected override void ChooseTarget()
    {
        target = commandCenter; //보스는 무조건 기지를 향해 이동
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
        // 체력 비율 계산
        float healthPercent = (float)hp / master_Hp[(int)Level.BOSS];

        // 체력바의 스케일 조정
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }
}

