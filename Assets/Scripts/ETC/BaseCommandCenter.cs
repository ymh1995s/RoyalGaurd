using UnityEngine;

public class BaseCommandCenter : MonoBehaviour, IDamageable
{
    //스텟 영역
    [SerializeField] private int maxHP = 30;
    [SerializeField] private int currentHp;

    //체력바 영역
    private RectTransform healthBarForeground;
    private Vector3 originalScale;

    void Start()
    {
        Init();
    }

    void Init()
    {
        currentHp = maxHP;
        healthBarForeground = transform.Find("HPBar/RED").GetComponent<RectTransform>();
        originalScale = healthBarForeground.localScale;

        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHP);
        UpdateHealthBar();

        if (currentHp <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        SceneLoader.SceneLoad_OverScene();
    }

    void UpdateHealthBar()
    {
        // 체력 비율 계산
        float healthPercent = (float)currentHp / maxHP;

        // 체력바의 스케일 조정
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }
}
