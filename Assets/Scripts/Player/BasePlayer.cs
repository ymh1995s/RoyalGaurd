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

    //���� ������Ʈ�� �ִϸ�����
    private Animator animator;

    // �ڵ� �л�� ����� ����
    public TmpLevelUpHelper levelUpHelper;

    //���� ����
    //static public float moveSpeed = 2.0f;
    //static public int maxHP = 20;
    //static public int currentHP = 20;
    public float moveSpeed = 2.0f;
    public int maxHP = 20;
    public int currentHP = 20;

    //���� ����
    public int maxWeaponCount = 100;
    public GameObject[] obtainedWeapon;
    public GameObject[] weaponPrefab;

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
    Gatcha gatcha;

    // ������ ��Ʈ�� Arr
    string[] weaponName = new string[3] { "Weapon/��Ʃ������", "Weapon/ġ��������", "Weapon/����������" };
    string[] coinName = new string[3] { "Bronze(Clone)", "Silver(Clone)", "Gold(Clone)" };
    int[] coinExpQuantity = new int[3] { 1, 3, 10 };
    public int[] weaponAddClassCut = new int[3] { 60, 90, 100 };

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        itemLayer = LayerMask.GetMask("Item");
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ���� ��ũ��Ʈ �ε�
        itemcollector = new ItemCollector();
        gatcha = new Gatcha();
        levelUpHelper = new TmpLevelUpHelper();

        // �ִϸ����� �ε� - UnitRoot��� �̸��� �ڽ� ��ü���� Animator ������Ʈ�� ã�� �Ҵ�
        animator = transform.Find("UnitRoot").GetComponent<Animator>();

        // ���� ������ �ε�
        weaponPrefab = new GameObject[weaponName.Length];
        for (int i = 0; i < weaponName.Length; i++)
        {
            weaponPrefab[i] = Resources.Load<GameObject>(weaponName[i]);
        }

        // ���� 1�� �⺻ ����
        obtainedWeapon = new GameObject[maxWeaponCount];
        levelUpHelper.WeaponAdd(this, this.transform);

        // ü�¹� ���� ����
        currentHP = maxHP;
        healthBarForeground = transform.Find("HPBar/RED").GetComponent<RectTransform>();
        originalScale = healthBarForeground.localScale;
        UpdateHealthBar();
    }

    private void Update()
    {
        itemcollector.CollectItem(transform.position, attractionRange, attractionSpeed, itemLayer);

        // HUD ������Ʈ
        GameManager.Instance.hudManager.PlayerHUDUpdate(playerLv, curExp, maxExp, currentHP, moveSpeed);
        GameManager.Instance.hudManager.WeaponHUDUpdate(BaseProjectile.attackPowerUp, BaseWeapon.detectionRadiusPlus, BaseWeapon.fireRateMmul);
        GameManager.Instance.hudManager.TowerHUDUpdate(BaseProjectile.attackPowerUp, BaseTower.detectionRadiusPlus, BaseTower.fireRateMmul);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ȹ��
        if (collision.CompareTag("Item"))
        {
            if (collision.gameObject.name == coinName[0]) curExp += coinExpQuantity[0];
            else if (collision.gameObject.name == coinName[1]) curExp += coinExpQuantity[1];
            else if (collision.gameObject.name == coinName[2]) curExp += coinExpQuantity[2];

            Destroy(collision.gameObject);
            CheckLevelUp();
        }

        // ���
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
        //TODO : �ִϸ����� ����(�������µ� Idle ���) �Ƚ��ؾߵ�
        input = value.Get<Vector2>();
        animator.Play("1_Run");
        if (input.x < 0)
        {
            // �������� �̵��� �� �¿� ����
            Vector3 scale = transform.localScale;
            scale.x = 1; 
            transform.localScale = scale;
        }
        else if (input.x > 0)
        {
            // ���������� �̵��� �� �¿� ���� ����
            Vector3 scale = transform.localScale;
            scale.x = -1; 
            transform.localScale = scale;
        }
        // SPUM �ִϸ��̼��� ���� �߰�
        else if (input.y != 0)
        {
            animator.Play("1_Run");
        }
        else
        {
            animator.Play("0_idle");
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

    public void UpdateHealthBar()
    {
        // ü�� ���� ���
        float healthPercent = (float)currentHP / maxHP;

        // ü�¹��� ������ ����
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }

    void CheckLevelUp()
    {
        if (curExp < maxExp) return;

        //Level up
        curExp = System.Math.Max(0, curExp - maxExp);
        maxExp += 1; //������ �� �ּ� ����
        LevelUp();

        // ����� �ܿ��� ���� ����
        if (playerLv % 10 == 0)
        {
            BonusLevelUp();
        }
    }

    public void Debug_WeaponAdd()
    {
        for (int i = 0; i < maxWeaponCount; i++)
        {
            if (obtainedWeapon[i] == null)
            {
                GameObject weapon;

                weapon = Instantiate(weaponPrefab[2], transform.position, Quaternion.identity);

                weapon.transform.parent = transform; // ���� �÷��̾ �θ�� ����

                obtainedWeapon[i] = weapon;
                levelUpHelper.WeaponSort(this);
                GameManager.Instance.hudManager.LevelUpHintUpdate("���� �߰�!");
                return;
            }
        }
    }

    //TODO : ���׷��̵� ���� �� Helper������ ����
    //void WeaponAdd()
    //{
        //for (int i = 0; i < maxWeaponCount; i++)
        //{
        //    if (obtainedWeapon[i] == null)
        //    {
        //        GameObject weapon;

        //        int index = UnityEngine.Random.Range(1, 101); //1���� 101

        //        if (index < weaponAddClassCut[0]) weapon = Instantiate(weaponPrefab[0], transform.position, Quaternion.identity);
        //        else if (index < weaponAddClassCut[1]) weapon = Instantiate(weaponPrefab[1], transform.position, Quaternion.identity);
        //        else weapon = Instantiate(weaponPrefab[2], transform.position, Quaternion.identity);

        //        weapon.transform.parent = transform; // ���� �÷��̾ �θ�� ����

        //        obtainedWeapon[i] = weapon;
        //        WeaponSort();
        //        GameManager.Instance.hudManager.LevelUpHintUpdate("���� �߰�!");
        //        return;
        //    }
        //}
    //}

    void LevelUp()
    {
        int index = gatcha.levelUpGatcha[playerLv++];

        if (index == 0)
        {
            int _weaponIndex = gatcha.weaponGatcha[gatcha_weaponIndex++];

            if (_weaponIndex == 0) levelUpHelper.WeaponAdd(this, this.transform);
            else if (_weaponIndex == 1) levelUpHelper.WeaponAttackSpeedUp();
            else if (_weaponIndex == 2) levelUpHelper.WeaponRangedUp();
            else if (_weaponIndex == 3) levelUpHelper.WeaponAttackPowerUp();
        }
        else if (index == 1)
        {
            int _towerIndex = gatcha.towerGatcha[gatcha_towerIndex++];
            levelUpHelper.TowerUpgrade(_towerIndex);
        }
        else if (index == 2)
        {
            int _playerIndex = gatcha.playerGatcha[gatcha_playerIndex++];
            levelUpHelper.PlayerUpgrade(this, _playerIndex);
            UpdateHealthBar(); //ü�� ���׷��̵� �� ���׷��̵� �� ü�� �ݿ�
        }
        else
        {
            print("LEVELUP ERROR");
        }
    }

    void BonusLevelUp()
    {
        // �÷��̾� ��ũ��Ʈ������ �ش� �Լ� ȣ�⸸ �ϰ� ����(����)
        GameManager.Instance.hudManager.BonusLevelUp();
    }

    // �˵� ���⸦ ������ ���ݿ��� �����ϰ���
    //void WeaponSort()
    //{
        //GameObject[] weapons1 = GameObject.FindGameObjectsWithTag("LV1Weapon");
        //GameObject[] weapons2 = GameObject.FindGameObjectsWithTag("LV2Weapon");
        //GameObject[] weapons3 = GameObject.FindGameObjectsWithTag("LV3Weapon");

        //int weaponCount = weapons1.Length + weapons2.Length + weapons3.Length;

        //try
        //{
        //    int rad = 360 / weaponCount;

        //    for (int i = 0; i < weaponCount; i++)
        //    {
        //        BaseWeapon bweapon = obtainedWeapon[i].GetComponent<BaseWeapon>();
        //        bweapon.currentAngle = rad * i;
        //    }
        //}

        //catch (Exception ex)
        //{
        //    print("sort err");
        //    print(ex.ToString());
        //}
    //}

    // �ڼ�ȿ�� ���� �����
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }
}
