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
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    protected float bulletSpeed = 15f;  // 총알 속도
    protected float fireCountdown = 0f;// 발사 간격을 체크하기 위한 카운트다운 변수
    protected float fireRate; // 발사 간격을 초 단위로 설정 (X초에 한 번 발사)
    protected float[] fireRates = { 1f, 0.6f, 0.4f }; 
    public static float fireRateMmul = 1.0f;

    // 탐지 영역
    static public float detectionRadius;  // 타워의 탐지 반경
    protected float[] detectionRadius_ = { 4f, 5f, 6f };
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

        // TODO
        // SPUM으로 넘어오면서 y오프셋 조정 임시 코드
        // 플레이어의 Y 축 중심 조정 (필요에 따라 Y 오프셋을 추가)
        offset.y += 0.5f; // 필요하다면 yOffset을 통해 조정

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

        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, (detectionRadius), enemyLayer);
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

    protected virtual void Fire()
    {
        fireCountdown -= Time.deltaTime;

        if (enemyCollider != null)
        {
            target = enemyCollider.transform;

            if (fireCountdown <= 0f)
            {
                GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);

                // 총알의 Z축 위치를 조정하여 Grid보다 앞에 배치
                Vector3 bulletPosition = bulletGO.transform.position;
                bulletPosition.z = -3; // 필요에 따라 조정
                bulletGO.transform.position = bulletPosition;

                Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();

                Vector3 direction = (target.position - transform.position).normalized;
                rb.velocity = direction * bulletSpeed;

                fireCountdown = (fireRate * fireRateMmul);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, (detectionRadius));
    }
}