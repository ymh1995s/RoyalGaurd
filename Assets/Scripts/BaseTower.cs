using UnityEngine;
using UnityEngine.UIElements;

public class BaseTower : MonoBehaviour, IDamageable
{
    //���� ����
    public int maxHP = 100;
    [SerializeField] private int currentHp;
    [SerializeField] private int attackRange;
    [SerializeField] private int attackCoolTime;
    private GameObject bulletPrefab; // �߻��� �Ѿ� ������
    private float bulletSpeed = 5f;  // �Ѿ� �ӵ�
    private float fireRate = 1f; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�)
    public static float fireRateMmul = 1.0f; // ���� �ӵ� ���׷��̵� ���� ����
    public static float detectionRadius = 10f;  // Ÿ���� Ž�� �ݰ�
    private float fireCountdown = 0f;// �߻� ������ üũ�ϱ� ���� ī��Ʈ�ٿ� ����

    //ü�¹� ����
    private RectTransform healthBarForeground;
    private Vector3 originalScale;

    //Ž�� ����
    public LayerMask enemyLayer;    // �� ���̾�
    Collider2D enemyCollider;       // �� �ݶ��̴�
    Transform target;               // Ÿ���õ� ��

    // ������ ��Ʈ�� Arr
    protected string[] prefabNames = { "Projectile/Basic", "Projectile/ADVBasic", "Projectile/ICE", "Projectile/FIRE", "Projectile/Special2" }; // ����� ������ �̸���

    void Start()
    {
        bulletPrefab = Resources.Load<GameObject>(prefabNames[0]);
        currentHp = maxHP;
        healthBarForeground = transform.Find("HPBar/RED").GetComponent<RectTransform>();
        originalScale = healthBarForeground.localScale;
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemy();
        Rotate();
        Fire();
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
        //TODO : ������ ó��
        gameObject.SetActive(false);
    }

    void UpdateHealthBar()
    {
        // ü�� ���� ���
        float healthPercent = (float)currentHp / maxHP;

        // ü�¹��� ������ ����
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }


    void DetectEnemy()
    {
        // �ʱ�ȭ
        target = null;
        enemyCollider = null;
        float closestDistance = Mathf.Infinity;

        // Ÿ���� ��ġ
        Vector2 towerPosition = transform.position;

        // Ž�� ���� ���� ��� Collider �˻�
        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, detectionRadius, enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            Renderer enemyRenderer = collider.GetComponent<Renderer>();
            if (enemyRenderer == null || !enemyRenderer.enabled)
            {
                continue; // �������� ���ų� ��Ȱ��ȭ�� ���� �����մϴ�.
            }

            // Ÿ���� �� ������ �Ÿ� ���
            float distanceToEnemy = Vector2.Distance(towerPosition, collider.transform.position);

            // �� ����� ������ Ȯ��
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                enemyCollider = collider;
            }
        }        
    }

    void Rotate()
    {
        // ���� ����� ���� �ִٸ� ó��
        if (enemyCollider != null)
        {
            target = enemyCollider.transform;

            // Ÿ���� ���� ȸ��
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void Fire()
    {         
        // �߻� ���� üũ
        fireCountdown -= Time.deltaTime;

        if (target != null)
        {
           
            if (fireCountdown <= 0f)
            {
                // �Ѿ� ����
                GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
                Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();
                rb.velocity = transform.right * bulletSpeed;

                fireCountdown = (fireRate* fireRateMmul);
            }
        }
    }

    // Ž���ݰ� �����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
