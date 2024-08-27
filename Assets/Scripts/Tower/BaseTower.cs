using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseTower : MonoBehaviour, IDamageable
{
    //스텟 영역
    private int maxHP = 50;
    [SerializeField] private int currentHp;
    [SerializeField] private int attackRange;
    [SerializeField] private int attackCoolTime;
    protected GameObject bulletPrefab; // 발사할 총알 프리팹
    private float bulletSpeed = 5f;  // 총알 속도
    protected float fireRate = 1f; // 발사 간격을 초 단위로 설정 (X초에 한 번 발사)
    public static float fireRateMmul = 1.0f; // 공격 속도 업그레이드 곱셈 적용
    public static float detectionRadius = 10f;  // 타워의 탐지 반경
    public static float detectionRadiusPlus = 0f;  // 타워의 투가 탐지 반경
    private float fireCountdown = 0f;// 발사 간격을 체크하기 위한 카운트다운 변수
    bool isAlive = true;

    // 컴포넌트 영역
    BoxCollider2D collider;

    //하위 오브젝트의 애니메이터
    private Animator unitAnimator;
    public float fireDelayTime = 0.5f;     // 애니메이션 진행 시간 후 총알 발사

    //체력바 영역
    private RectTransform healthBarForeground;
    private Vector3 originalScale;

    //탐지 영역
    private LayerMask enemyLayer;    // 적 레이어
    Transform target;               // 타게팅된 적

    // 참조용 스트링 Arr
    protected string[] prefabNames = { "Projectile/Basic", "Projectile/ADVBasic", "Projectile/ICE", "Projectile/FIRE", "Projectile/Special2" }; // 사용할 프리팹 이름들

    protected virtual void Start()
    {
        // 애니메이터 로드 - UnitRoot라는 이름의 자식 객체에서 Animator 컴포넌트를 찾아 할당
        unitAnimator = transform.Find("UnitRoot").GetComponent<Animator>();

        collider = transform.GetComponent<BoxCollider2D>();

        //적 레이어 설정
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        bulletPrefab = Resources.Load<GameObject>(prefabNames[0]);
        currentHp = maxHP;
        healthBarForeground = transform.Find("HPBar/RED").GetComponent<RectTransform>();
        originalScale = healthBarForeground.localScale;
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive==true)
        {
            DetectEnemy();
            FireCheck();
        }
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
        gameObject.layer = 0;
        gameObject.tag = "Untagged";
        unitAnimator.SetTrigger("Death");
        Destroy(collider);
        isAlive = false;
    }

    void UpdateHealthBar()
    {
        // 체력 비율 계산
        float healthPercent = (float)currentHp / maxHP;

        // 체력바의 스케일 조정
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }


    void DetectEnemy()
    {
        // 초기화
        target = null;
        float closestDistance = Mathf.Infinity;

        // 타워의 위치
        Vector2 towerPosition = transform.position;

        // 탐지 범위 내의 모든 Collider 검사
        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, detectionRadius+ detectionRadiusPlus, enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            // 타워와 적 사이의 거리 계산
            float distanceToEnemy = Vector2.Distance(towerPosition, collider.transform.position);

            // 더 가까운 적인지 확인
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                target = collider.transform;
            }
        }
    }

    // 발사가 가능한 상태인지 체크
    private void FireCheck()
    {
        // 발사 간격 체크
        fireCountdown -= Time.deltaTime;

        if (target != null)
        {
            if (fireCountdown <= 0f)
            {
                fireCountdown = (fireRate * fireRateMmul);
                unitAnimator.speed = (1.5f / fireRateMmul);
                unitAnimator.SetTrigger("Attack");
                StartCoroutine(FireAfterAnimation());
            }
            else
            {
                unitAnimator.SetTrigger("Idle");
            }
        }
    }

    private IEnumerator FireAfterAnimation()
    {
        // 애니메이션의 현재 시간을 기다린 후 총알 발사
        yield return new WaitForSeconds(fireDelayTime * fireRateMmul);
        if (target != null) Fire();
    }

    protected virtual void Fire()
    {
        // 총알 생성
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity); // 회전 없이 생성

        // 총알의 Z축 위치를 조정하여 Grid보다 앞에 배치
        Vector3 bulletPosition = bulletGO.transform.position;
        bulletPosition.z = -3; // 필요에 따라 조정
        bulletGO.transform.position = bulletPosition;

        // 목표 방향 계산
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        // 오차 범위 설정 (예: -5도에서 +5도까지)
        float deviationAngle = Random.Range(-5f, 5f);
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg + deviationAngle;

        // 오차를 적용한 방향 계산
        Vector3 deviationDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0).normalized;

        // 총알의 방향을 목표로 설정 (회전 설정)
        bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Rigidbody2D에 방향 속도 설정
        Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();
        rb.velocity = deviationDirection * bulletSpeed;

    }

    // 탐지반경 기즈모
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius+ detectionRadiusPlus);
    }
}
