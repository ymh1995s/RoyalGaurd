using UnityEngine;

public class BaseCommandCenter : MonoBehaviour, IDamageable
{
    //���� ����
    [SerializeField] private int maxHP = 30;
    [SerializeField] private int currentHp;

    //ü�¹� ����
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
        // ü�� ���� ���
        float healthPercent = (float)currentHp / maxHP;

        // ü�¹��� ������ ����
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }
}
