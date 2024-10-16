using System.Collections;
using UnityEngine;
// HIT & FLASH
// 투사체 고르기
// 1. 기본(4 yellow arrow > Basic) 
// 2. 기본 강화 (11 orange arrow > ADVBasic)
// 3. 수속성 (6 blue fire > ICE) //이거 안보임
// 4. 화속성 (16 red fire > FIRE)
// 5. EVT 22(STAT) or 27(HEART) > Special

public abstract class BaseWeapon : MonoBehaviour
{
    protected Transform playerTransform;  // 플레이어의 Transform

    // 공전 영역
    protected float orbitRadius = 1f;  // 공전 반지름
    protected float orbitSpeed = 120;   // 공전 속도 (각도 단위)
    public float currentAngle { get; set; }   // 현재 회전 각도

    // 스텟 영역
    protected GameObject bulletPrefab; // 발사할 총알 프리팹
    protected float bulletSpeed = 8f;  // 총알 속도
    protected float fireCountdown = 0f;// 발사 간격을 체크하기 위한 카운트다운 변수
    protected float fireRate; // 발사 간격을 초 단위로 설정 (X초에 한 번 발사)
    protected float[] fireRates = { 1f, 0.85f, 0.7f };
    static public float fireRateMmul = 1.0f;
    static public int fireMultiple = 1;
    float fireMultipleInterval = 0.2f;

    // 탐지 영역
    protected float detectionRadius;  // 무기의 탐지 반경
    protected float[] detectionRadius_ = { 4f, 4.5f, 5f };
    static public float detectionRadiusPlus = 0f;
    protected LayerMask enemyLayer;           // 적 레이어
    protected Collider2D enemyCollider;       // 적 콜라이더
    [SerializeField] protected Transform target;               // 타게팅된 적

    // 참조용 스트링 Arr
    protected string[] prefabNames = { "Projectile/Basic", "Projectile/ADVBasic", "Projectile/FIRE"}; // 사용할 프리팹 이름들

    // ETC
    protected enum Level { LV1, LV2, LV3 }

    protected virtual void Start()
    {
        bulletPrefab = Resources.Load<GameObject>(prefabNames[0]);

        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            playerTransform = parentTransform;
        }
        else
        {
            Debug.LogError("Parent transform not found.");
        }

        enemyLayer = LayerMask.GetMask("Enemy");
    }

    protected virtual void Update()
    {
        Orbit();
        DetectEnemy();
    }

    protected void Orbit()
    {
        if (playerTransform == null) return;

        // 공전 위치 설정.
        Vector3 playerPosition = playerTransform.position;
        currentAngle += orbitSpeed * Time.deltaTime;
        float radians = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * orbitRadius;

        // SPUM으로 넘어오면서 y오프셋 조정 임시 코드
        // 플레이어의 Y 축 중심 조정 (필요에 따라 Y 오프셋을 추가)
        //offset.y += 0.5f; // 필요하다면 yOffset을 통해 조정

        // 오브젝트의 위치를 플레이어 주위로 설정합니다.
        transform.position = playerPosition + offset;
    }

    protected void DetectEnemy()
    {
        // 초기화
        target = null;
        enemyCollider = null;
        float closestDistance = Mathf.Infinity;

        Vector2 towerPosition = transform.position;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, (detectionRadius + detectionRadiusPlus), enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            float distanceToEnemy = Vector2.Distance(towerPosition, collider.transform.position);

            // 가장 가까운 적 탐지
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                enemyCollider = collider;
            }
        }
    }

    protected virtual void CheckIsReadyFire()
    {
        fireCountdown -= Time.deltaTime;

        if (enemyCollider != null)
        {
            target = enemyCollider.transform;

            if (fireCountdown <= 0f)
            {
                // FireMultiple 코루틴을 시작하고, fireCountdown을 바로 갱신
                Transform thistarget = target;
                StartCoroutine(FireMultiple(thistarget));
                fireCountdown = fireRate * fireRateMmul; // fireCountdown 갱신
            }
        }
    }

    // 투사체 증가 코루틴
    private IEnumerator FireMultiple(Transform thisTarget)
    {
        for (int i = 0; i < fireMultiple; i++)
        {
            Fire(thisTarget);

            // 다음 발사 전까지 대기(fireMultipleInterval)
            // 여기서 fireRateMmul는 공속 보정용으로 쓰임
            yield return new WaitForSeconds(fireMultipleInterval * fireRateMmul);
        }
    }

    void Fire(Transform thisTarget)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);

        // 총알의 Z축 위치를 조정하여 Grid보다 앞에 배치 (랜더링이 되도록)
        Vector3 bulletPosition = bulletGO.transform.position;
        bulletPosition.z = -3; // 필요에 따라 조정
        bulletGO.transform.position = bulletPosition;

        Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();

        // 수출용
        //// 방향을 계산할 때 Y축에 약간의 상향 오프셋 추가
        //Vector3 offset = new Vector3(0, 0.5f, 0); // Y축으로 0.5만큼 오프셋, 필요에 따라 조정
        //Vector3 direction = (thisTarget.position + offset - transform.position).normalized;
        Vector3 direction = (thisTarget.position - transform.position).normalized;

        // 랜덤한 오차를 추가하기 위해 방향 벡터에 회전을 적용
        float angleVariance = 5.0f; // 오차 각도 범위 (각도)
        float randomAngle = Random.Range(-angleVariance, angleVariance);
        direction = Quaternion.Euler(0, 0, randomAngle) * direction;

        rb.velocity = direction * bulletSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, (detectionRadius + detectionRadiusPlus));
    }
}