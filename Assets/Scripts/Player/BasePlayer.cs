using NUnit.Framework.Constraints;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer : MonoBehaviour, IDamageable
{
    Rigidbody2D rigid;
    Vector2 input;
    private SpriteRenderer spriteRenderer;
    public Vector2 minBounds; // �ּ� ���
    public Vector2 maxBounds; // �ִ� ���

    //���� ����
    static public float moveSpeed = 2.0f;
    static public int maxHP = 20;
    static public int currentHP;

    //���� ����
    const int maxWeaponCount = 100;
    public GameObject[] weapon1;
    public GameObject weaponPrefab; 
    GameObject LV1WeaponPrefab; 
    GameObject LV2WeaponPrefab; 
    GameObject LV3WeaponPrefab; 

    //������ �ڼ� ȿ��
    float attractionRange = 0.8f;   // ������ �ڼ� ȿ�� ���� (�÷��̾� ����)
    float attractionSpeed = 2f;     // ������ �������� �ӵ�
    LayerMask itemLayer;            // ������ ���̾�

    //ü�¹� ����
    private RectTransform healthBarForeground;
    private Vector3 originalScale;

    //����� ���ÿ� �ʱ�ȭ
    private int playerLv = 0;
    private int gatcha_weaponIndex = 0;
    private int gatcha_towerIndex = 0;
    private int gatcha_playerIndex = 0;
    private int maxExp = 10;
    private int curExp = 0;
    
    //���� ��ũ��Ʈ
    ItemCollector itemcollector;

    // ������ ��Ʈ�� Arr
    string[] weaponName = new string[3] { "Weapon/��Ʃ������", "Weapon/ġ��������", "Weapon/����������" };

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        itemLayer = LayerMask.GetMask("Item");
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHP = maxHP;

        // ���� ��ũ��Ʈ �ε�
        itemcollector = new ItemCollector();

        // ���� ������ �ε�
        LV1WeaponPrefab = Resources.Load<GameObject>(weaponName[0]); // Instantiate�� ������ �������� ����
        LV2WeaponPrefab = Resources.Load<GameObject>(weaponName[1]); // Instantiate�� ������ �������� ����
        LV3WeaponPrefab = Resources.Load<GameObject>(weaponName[2]); // Instantiate�� ������ �������� ����

        // ���� 1�� �⺻ ����
        weapon1 = new GameObject[maxWeaponCount];
        WeaponAdd();

        //ü�¹�
        healthBarForeground = transform.Find("HPBar/RED").GetComponent<RectTransform>();
        originalScale = healthBarForeground.localScale;
        UpdateHealthBar();

    }

    private void Update()
    {
        itemcollector.CollectItem(transform.position, attractionRange, attractionSpeed, itemLayer);

        // HUD ������Ʈ
        GameManager.Instance.hudManager.PlayerHUDUpdate(playerLv, curExp, maxExp, currentHP, moveSpeed);
        GameManager.Instance.hudManager.WeaponHUDUpdate(BaseProjectile.attackPowerUp, BaseWeapon.detectionRadiusMul, BaseWeapon.fireRateMmul);
        GameManager.Instance.hudManager.TowerHUDUpdate(BaseProjectile.attackPowerUp, BaseTower.detectionRadius, BaseTower.fireRateMmul);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            if (collision.gameObject.name == "Coin3(Clone)")
                curExp += 1;
            else if (collision.gameObject.name == "Coin2(Clone)")
                curExp += 3;
            else if (collision.gameObject.name == "Coin(Clone)")
                curExp += 10;
            Destroy(collision.gameObject);
            CheckLevelUp();
        }

        if (collision.gameObject.CompareTag("Boundary"))
        {
            rigid.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(input.x, input.y).normalized * moveSpeed;
    }

    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();

        if (input.x < 0)
        {
            // �������� �̵��� �� �¿� ����
            //spriteRenderer.flipX = false;
            Vector3 scale = transform.localScale;
            scale.x = 1;  // ���ϴ� ������ ����
            transform.localScale = scale;
        }
        else if (input.x > 0)
        {
            // ���������� �̵��� �� �¿� ���� ����
            //spriteRenderer.flipX = true;
            Vector3 scale = transform.localScale;
            scale.x = -1;  // ���ϴ� ������ ����
            transform.localScale = scale;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        UpdateHealthBar();

        if (currentHP <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        SceneLoader.SceneLoad_OverScene();
    }

    void UpdateHealthBar()
    {
        // ü�� ���� ���
        float healthPercent = (float)currentHP / maxHP;

        // ü�¹��� ������ ����
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }

    int templevel = 1;
    void CheckLevelUp()
    {
        if (curExp < maxExp) return;

        //Level up
        curExp = System.Math.Max(0, curExp - maxExp);
        maxExp += 1; //������ �� �ּ� ����
        LevelUp();
        templevel++;
    }

    public void Debug_WeaponAdd()
    {
        if (LV1WeaponPrefab != null)
        {
            for (int i = 0; i < maxWeaponCount; i++)
            {
                if (weapon1[i] == null)
                {
                    GameObject weapon;

                    weapon = Instantiate(LV3WeaponPrefab, transform.position, Quaternion.identity);

                    weapon.transform.parent = transform; // ���� �÷��̾ �θ�� ����

                    weapon1[i] = weapon;
                    WeaponSort();
                    GameManager.Instance.hudManager.LevelUpHintUpdate("���� �߰�!");
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning("���� �߰� ����");
        }
    }

    //TODO : ���׷��̵� ���� �� Helper������ ����
    void WeaponAdd()
    {
        if (LV1WeaponPrefab != null)
        {
            for (int i = 0; i < maxWeaponCount; i++)
            {
                if (weapon1[i] == null)
                {
                    GameObject weapon;

                    int index = UnityEngine.Random.Range(0, 100);

                    //TODO �ϵ��ڵ� ����
                    if (index <70) weapon = Instantiate(LV1WeaponPrefab, transform.position, Quaternion.identity);
                    else if (index < 95) weapon = Instantiate(LV2WeaponPrefab, transform.position, Quaternion.identity);
                    else weapon = Instantiate(LV3WeaponPrefab, transform.position, Quaternion.identity);

                    weapon.transform.parent = transform; // ���� �÷��̾ �θ�� ����

                    weapon1[i] = weapon;
                    WeaponSort();
                    GameManager.Instance.hudManager.LevelUpHintUpdate("���� �߰�!");
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning("���� �߰� ����");
        }
    }

    void LevelUp()
    {
        print("levelup �Ƿ���");
        //������Ϸ��� try ����
        try
        {
            int index = Gatcha.levelUpGatcha[playerLv++];

            if (index == 0)
            {
                int _weaponIndex = Gatcha.weaponGatcha[gatcha_weaponIndex++];

                if (_weaponIndex == 0) WeaponAdd();
                else if (_weaponIndex == 1) LevelUpHelper.WeaponAttackSpeedUp();
                else if (_weaponIndex == 2) LevelUpHelper.WeaponRangedUp();
                else if (_weaponIndex == 3) LevelUpHelper.WeaponAttackPowerUp(); 
            }
            else if (index == 1)
            {
                int _towerIndex = Gatcha.towerGatcha[gatcha_towerIndex++];
                LevelUpHelper.TowerUpgrade(_towerIndex);
            }
            else if (index == 2)
            {
                int _playerIndex = Gatcha.playerGatcha[gatcha_playerIndex++];
                LevelUpHelper.PlayerUpgrade(_playerIndex);
                UpdateHealthBar(); //ü�� ���׷��̵� �� ���׷��̵� �� ü�� �ݿ�
            }
            else
            {
                print("LEVELUP ERROR");
            }
        }
        catch (Exception ex)
        {
            print(ex.ToString());
        }
    }

    // �˵� ���⸦ ������ ���ݿ��� �����ϰ���
    void WeaponSort()
    {
        GameObject[] weapons1 = GameObject.FindGameObjectsWithTag("LV1Weapon");
        GameObject[] weapons2 = GameObject.FindGameObjectsWithTag("LV2Weapon");
        GameObject[] weapons3 = GameObject.FindGameObjectsWithTag("LV3Weapon");

        int weaponCount = weapons1.Length + weapons2.Length + weapons3.Length;

        try
        {
            int rad = 360 / weaponCount;

            for (int i = 0; i < weaponCount; i++)
            {
                BaseWeapon bweapon = weapon1[i].GetComponent<BaseWeapon>();
                bweapon.currentAngle = rad * i;
            }
        }

        catch (Exception ex)
        {
            print("sort err");
            print(ex.ToString());
        }
    }

    // �ڼ�ȿ�� ���� �����
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }
}
