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
    protected float bulletSpeed = 15f;  // �Ѿ� �ӵ�
    protected float fireCountdown = 0f;// �߻� ������ üũ�ϱ� ���� ī��Ʈ�ٿ� ����
    protected float fireRate; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�)
    protected float[] fireRates = { 1f, 0.6f, 0.4f }; 
    public static float fireRateMmul = 1.0f;

    // Ž�� ����
    static public float detectionRadius;  // Ÿ���� Ž�� �ݰ�
    protected float[] detectionRadius_ = { 4f, 5f, 6f };
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

        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, (detectionRadius), enemyLayer);
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
                GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);

                // �Ѿ��� Z�� ��ġ�� �����Ͽ� Grid���� �տ� ��ġ
                Vector3 bulletPosition = bulletGO.transform.position;
                bulletPosition.z = -3; // �ʿ信 ���� ����
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