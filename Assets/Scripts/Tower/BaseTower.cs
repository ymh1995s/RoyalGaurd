using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseTower : MonoBehaviour, IDamageable
{
    //���� ����
    private int maxHP = 50;
    [SerializeField] private int currentHp;
    [SerializeField] private int attackRange;
    [SerializeField] private int attackCoolTime;
    protected GameObject bulletPrefab; // �߻��� �Ѿ� ������
    private float bulletSpeed = 5f;  // �Ѿ� �ӵ�
    protected float fireRate = 1f; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�)
    public static float fireRateMmul = 1.0f; // ���� �ӵ� ���׷��̵� ���� ����
    public static float detectionRadius = 10f;  // Ÿ���� Ž�� �ݰ�
    public static float detectionRadiusPlus = 0f;  // Ÿ���� ���� Ž�� �ݰ�
    private float fireCountdown = 0f;// �߻� ������ üũ�ϱ� ���� ī��Ʈ�ٿ� ����
    bool isAlive = true;

    // ������Ʈ ����
    BoxCollider2D collider;

    //���� ������Ʈ�� �ִϸ�����
    private Animator unitAnimator;
    public float fireDelayTime = 0.5f;     // �ִϸ��̼� ���� �ð� �� �Ѿ� �߻�

    //ü�¹� ����
    private RectTransform healthBarForeground;
    private Vector3 originalScale;

    //Ž�� ����
    private LayerMask enemyLayer;    // �� ���̾�
    Transform target;               // Ÿ���õ� ��

    // ������ ��Ʈ�� Arr
    protected string[] prefabNames = { "Projectile/Basic", "Projectile/ADVBasic", "Projectile/ICE", "Projectile/FIRE", "Projectile/Special2" }; // ����� ������ �̸���

    protected virtual void Start()
    {
        // �ִϸ����� �ε� - UnitRoot��� �̸��� �ڽ� ��ü���� Animator ������Ʈ�� ã�� �Ҵ�
        unitAnimator = transform.Find("UnitRoot").GetComponent<Animator>();

        collider = transform.GetComponent<BoxCollider2D>();

        //�� ���̾� ����
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
        // ü�� ���� ���
        float healthPercent = (float)currentHp / maxHP;

        // ü�¹��� ������ ����
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }


    void DetectEnemy()
    {
        // �ʱ�ȭ
        target = null;
        float closestDistance = Mathf.Infinity;

        // Ÿ���� ��ġ
        Vector2 towerPosition = transform.position;

        // Ž�� ���� ���� ��� Collider �˻�
        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, detectionRadius+ detectionRadiusPlus, enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            // Ÿ���� �� ������ �Ÿ� ���
            float distanceToEnemy = Vector2.Distance(towerPosition, collider.transform.position);

            // �� ����� ������ Ȯ��
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                target = collider.transform;
            }
        }
    }

    // �߻簡 ������ �������� üũ
    private void FireCheck()
    {
        // �߻� ���� üũ
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
        // �ִϸ��̼��� ���� �ð��� ��ٸ� �� �Ѿ� �߻�
        yield return new WaitForSeconds(fireDelayTime * fireRateMmul);
        if (target != null) Fire();
    }

    protected virtual void Fire()
    {
        // �Ѿ� ����
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity); // ȸ�� ���� ����

        // �Ѿ��� Z�� ��ġ�� �����Ͽ� Grid���� �տ� ��ġ
        Vector3 bulletPosition = bulletGO.transform.position;
        bulletPosition.z = -3; // �ʿ信 ���� ����
        bulletGO.transform.position = bulletPosition;

        // ��ǥ ���� ���
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        // ���� ���� ���� (��: -5������ +5������)
        float deviationAngle = Random.Range(-5f, 5f);
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg + deviationAngle;

        // ������ ������ ���� ���
        Vector3 deviationDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0).normalized;

        // �Ѿ��� ������ ��ǥ�� ���� (ȸ�� ����)
        bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Rigidbody2D�� ���� �ӵ� ����
        Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();
        rb.velocity = deviationDirection * bulletSpeed;

    }

    // Ž���ݰ� �����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius+ detectionRadiusPlus);
    }
}
