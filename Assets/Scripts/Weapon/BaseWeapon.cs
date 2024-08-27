using System.Collections;
using UnityEngine;
// HIT & FLASH
// ����ü ����
// 1. �⺻(4 yellow arrow > Basic) 
// 2. �⺻ ��ȭ (11 orange arrow > ADVBasic)
// 3. ���Ӽ� (6 blue fire > ICE) //�̰� �Ⱥ���
// 4. ȭ�Ӽ� (16 red fire > FIRE)
// 5. EVT 22(STAT) or 27(HEART) > Special

public abstract class BaseWeapon : MonoBehaviour
{
    protected Transform playerTransform;  // �÷��̾��� Transform

    // ���� ����
    protected float orbitRadius = 1f;  // ���� ������
    protected float orbitSpeed = 120;   // ���� �ӵ� (���� ����)
    public float currentAngle { get; set; }   // ���� ȸ�� ����
    
    // ���� ����
    public GameObject bulletPrefab; // �߻��� �Ѿ� ������
    protected float bulletSpeed = 8f;  // �Ѿ� �ӵ�
    protected float fireCountdown = 0f;// �߻� ������ üũ�ϱ� ���� ī��Ʈ�ٿ� ����
    protected float fireRate; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�)
    protected float[] fireRates = { 1f, 0.8f, 0.6f };
    static public float fireRateMmul = 1.0f;
    static public int fireMultiple = 1;
    float fireMultipleInterval = 0.2f;

    // Ž�� ����
    public float detectionRadius;  // ������ Ž�� �ݰ�
    protected float[] detectionRadius_ = { 4f, 6f, 8f };
    static public float detectionRadiusPlus = 0f;
    protected LayerMask enemyLayer;           // �� ���̾�
    protected Collider2D enemyCollider;       // �� �ݶ��̴�
    [SerializeField] protected Transform target;               // Ÿ���õ� ��

    // ������ ��Ʈ�� Arr
    protected string[] prefabNames = { "Projectile/Basic", "Projectile/ADVBasic", "Projectile/FIRE"}; // ����� ������ �̸���

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

        // ���� ��ġ ����.
        Vector3 playerPosition = playerTransform.position;
        currentAngle += orbitSpeed * Time.deltaTime;
        float radians = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * orbitRadius;

        // TODO
        // SPUM���� �Ѿ���鼭 y������ ���� �ӽ� �ڵ�
        // �÷��̾��� Y �� �߽� ���� (�ʿ信 ���� Y �������� �߰�)
        offset.y += 0.5f; // �ʿ��ϴٸ� yOffset�� ���� ����

        // ������Ʈ�� ��ġ�� �÷��̾� ������ �����մϴ�.
        transform.position = playerPosition + offset;
    }

    protected void DetectEnemy()
    {
        // �ʱ�ȭ
        target = null;
        enemyCollider = null;
        float closestDistance = Mathf.Infinity;

        Vector2 towerPosition = transform.position;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, (detectionRadius + detectionRadiusPlus), enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            float distanceToEnemy = Vector2.Distance(towerPosition, collider.transform.position);

            // ���� ����� �� Ž��
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
                // FireMultipleTimes �ڷ�ƾ�� �����ϰ�, fireCountdown�� �ٷ� ����
                Transform thistarget = target;
                StartCoroutine(FireMultipleTimes(thistarget));
                fireCountdown = fireRate * fireRateMmul; // fireCountdown ����
            }
        }
    }

    // TODO �Լ��� ������
    void RealFire(Transform thisTarget)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);

        // �Ѿ��� Z�� ��ġ�� �����Ͽ� Grid���� �տ� ��ġ (�������� �ǵ���)
        Vector3 bulletPosition = bulletGO.transform.position;
        bulletPosition.z = -3; // �ʿ信 ���� ����
        bulletGO.transform.position = bulletPosition;

        Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();

        // ������ ����� �� Y�࿡ �ణ�� ���� ������ �߰�
        Vector3 offset = new Vector3(0, 0.5f, 0); // Y������ 0.5��ŭ ������, �ʿ信 ���� ����
        Vector3 direction = (thisTarget.position + offset - transform.position).normalized;

        // ������ ������ �߰��ϱ� ���� ���� ���Ϳ� ȸ���� ����
        float angleVariance = 5.0f; // ���� ���� ���� (����)
        float randomAngle = Random.Range(-angleVariance, angleVariance);
        direction = Quaternion.Euler(0, 0, randomAngle) * direction;

        rb.velocity = direction * bulletSpeed;
    }

    private IEnumerator FireMultipleTimes(Transform thisTarget)
    {
        for (int i = 0; i < fireMultiple; i++)
        {
            RealFire(thisTarget);

            // ���� �߻� ������ ��� (0.1��)
            yield return new WaitForSeconds(fireMultipleInterval * fireRateMmul);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, (detectionRadius + detectionRadiusPlus));
    }
}