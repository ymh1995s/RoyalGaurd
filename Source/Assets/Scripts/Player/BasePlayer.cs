using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer : MonoBehaviour, IBaseAllyUnit
{
    Rigidbody2D rigid;
    Vector2 input;
    private SpriteRenderer spriteRenderer;

    //하위 오브젝트의 애니메이터 (수출용)
    //private Animator animator;

    // 코드 분산용 레밸업 헬퍼
    public PlayerLevelUpHelper levelUpHelper;

    //스텟 영역
    public float moveSpeed = 2.0f;
    public int maxHP = 20;
    public int currentHP = 20;
    int hpAutoRecoverInterval = 3;
    public bool isObtainedAutoRecover = false;

    //무기 관리
    public List<GameObject> obtainedWeapon;
    //public GameObject[] obtainedWeapon;
    public GameObject[] weaponPrefab;

    //아이템 자석 효과
    float attractionRange = 0.8f;   // 아이템 자석 효과 범위 (플레이어 기준)
    float attractionSpeed = 2f;     // 아이템 끌려들어가는 속도
    LayerMask itemLayer;            // 아이템 레이어

    //체력바 영역
    private RectTransform healthBarForeground;
    private Vector3 originalScale;

    //선언과 동시에 초기화
    private int playerLv = 0;
    private int gatcha_weaponIndex = 0;
    private int gatcha_towerIndex = 0;
    private int gatcha_playerIndex = 0;
    private int maxExp = 10;
    private int curExp = 0;
    
    //하위 스크립트
    ItemCollector itemcollector;
    Gatcha gatcha;

    // 사운드 영역
    AudioSource audio;

    // 참조용 스트링 Arr
    string[] weaponName = new string[3] { "Weapon/유튜브쟁이", "Weapon/숲에남을걸", "Weapon/치지직갈걸" };
    string[] coinName = new string[3] { "Bronze(Clone)", "Silver(Clone)", "Gold(Clone)" };
    int[] coinExpQuantity = new int[3] { 1, 3, 10 };
    public int[] weaponAddClassCut = new int[3] { 60, 90, 100 };

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        itemLayer = LayerMask.GetMask("Item");
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 하위 스크립트 로드
        itemcollector = new ItemCollector();
        gatcha = new Gatcha();
        levelUpHelper = new PlayerLevelUpHelper(this);

        // 애니메이터 로드 - UnitRoot라는 이름의 자식 객체에서 Animator 컴포넌트를 찾아 할당
        //animator = transform.Find("UnitRoot").GetComponent<Animator>();

        // 오디오 로드
        audio = GetComponent<AudioSource>();

        // 무기 프리펩 로드
        weaponPrefab = new GameObject[weaponName.Length];
        for (int i = 0; i < weaponName.Length; i++)
        {
            weaponPrefab[i] = Resources.Load<GameObject>(weaponName[i]);
        }

        // 무기 1개 기본 제공
        //obtainedWeapon = new GameObject[maxWeaponCount];
        levelUpHelper.WeaponAdd();

        // 체력바 최초 세팅
        currentHP = maxHP;
        healthBarForeground = transform.Find("HPBar/RED").GetComponent<RectTransform>();
        originalScale = healthBarForeground.localScale;
        UpdateHealthBar();

        // 자동 체력 회복 코루티
        StartCoroutine(AutoHpRecover());
    }

    private void Update()
    {
        itemcollector.CollectItem(transform.position, attractionRange, attractionSpeed, itemLayer);

        // HUD 업데이트
        GameManager.Instance.hudManager.PlayerHUDUpdate(playerLv, curExp, maxExp, currentHP, moveSpeed);
        GameManager.Instance.hudManager.WeaponHUDUpdate(BaseProjectile.attackPowerUp, BaseWeapon.detectionRadiusPlus, BaseWeapon.fireRateMmul);
        GameManager.Instance.hudManager.TowerHUDUpdate(BaseProjectile.attackPowerUp, BaseTower.detectionRadiusPlus, BaseTower.fireRateMmul);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 코인 획득
        if (collision.CompareTag("Item"))
        {
            if (collision.gameObject.name == coinName[0]) curExp += coinExpQuantity[0];
            else if (collision.gameObject.name == coinName[1]) curExp += coinExpQuantity[1];
            else if (collision.gameObject.name == coinName[2]) curExp += coinExpQuantity[2];

            Destroy(collision.gameObject);
            CheckLevelUp();
        }

        // 장외로 나갈 수 없음
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
        //TODO : 애니메이터 버그(움직였는데 Idle 모션) 픽스해야됨
        input = value.Get<Vector2>();
        //animator.Play("1_Run");
        if (input.x < 0)
        {
            // 왼쪽으로 이동할 때 좌우 반전
            Vector3 scale = transform.localScale;
            //scale.x = 1; 
            spriteRenderer.flipX = false;
            transform.localScale = scale;
        }
        else if (input.x > 0)
        {
            // 오른쪽으로 이동할 때 좌우 반전 해제
            Vector3 scale = transform.localScale;
            //scale.x = -1; 
            spriteRenderer.flipX = true;
            transform.localScale = scale;
        }
        // SPUM 애니메이션을 위해 추가
        //else if (input.y != 0)
        //{
        //    animator.Play("1_Run");
        //}
        //else
        //{
        //    animator.Play("0_idle");
        //}
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        UpdateHealthBar();
        audio.Play();

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
        // 체력 비율 계산
        float healthPercent = (float)currentHP / maxHP;

        // 체력바의 스케일 조정
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }

    void CheckLevelUp()
    {
        if (curExp < maxExp) return;

        //Level up
        curExp = System.Math.Max(0, curExp - maxExp);
        maxExp += 1;
        LevelUp();

        if (playerLv % 5 == 0)
        {
            // 플레이어 스크립트에서는 해당 함수 호출만 하고 종결(래핑)
            GameManager.Instance.hudManager.BonusLevelUp();
        }
    }

    public void Debug_WeaponAdd(int no)
    {
        GameObject weapon = Instantiate(weaponPrefab[no], transform.position, Quaternion.identity);
        weapon.transform.parent = transform; // 현재 플레이어를 부모로 설정
        obtainedWeapon.Add(weapon);
        levelUpHelper.WeaponSort();
        GameManager.Instance.hudManager.LevelUpHintUpdate("무기 추가!");
    }

    void LevelUp()
    {
        int index = gatcha.levelUpGatcha[playerLv++];

        if (index == 0)
        {
            int _weaponIndex = gatcha.weaponGatcha[gatcha_weaponIndex++];

            if (_weaponIndex == 0) levelUpHelper.WeaponAdd();
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
            levelUpHelper.PlayerUpgrade(_playerIndex);
            UpdateHealthBar(); //체력 업그레이드 시 업그레이드 된 체력 반영
        }
        else
        {
            print("LEVELUP ERROR");
        }
    }

    private IEnumerator AutoHpRecover()
    {
        while (true)
        {
            if(isObtainedAutoRecover == true)
            {
                // 변수를 1씩 증가
                currentHP = System.Math.Min(currentHP + 1, maxHP);
                UpdateHealthBar();
            }

            // 지정된 시간(N초) 동안 대기
            yield return new WaitForSeconds(hpAutoRecoverInterval);
        }
    }

    // 자석효과 범위 디버그
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }
}
