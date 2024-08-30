using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour
{
    // TODO : GPT�� ���ִ� �����丵 �����ϰ� �ٽ� �����丵

    private Rigidbody2D rigid;
    private bool isDying = false;   // ���� ���� ����
    protected enum Level { LV1, LV2, LV3, LV4, LV5, BOSS }
    private Coroutine attackCoroutine;

    //���� ������Ʈ�� �ִϸ����� (�����)
    //protected Animator animator;

    //Ž�� ����
    protected GameObject target; // ��ǥ ����
    protected GameObject commandCenter; //����Ʈ ��ǥ
    private float DetectRange = 5; // ��ǥ�� Ž���ϱ� ���� ����
    private LayerMask[] targetLayer;    // �� ���̾�

    //���� ����
    [SerializeField] protected int hp;
    protected float speed = 1f;
    public int attackPower = 1;
    private float attackInterval = 1.0f;
    private float lastAttackTime = 0.0f;
    //protected int[] master_Hp = new int[6] { 10, 80, 130, 150, 160, 15000 };
    protected int[] master_Hp = new int[6] { 10, 50, 90, 180, 250, 25000 };

    //����� ����
    public AudioClip[] deathSound = new AudioClip[5]; // ��� ���� ����
    private Transform playerTransform; // �÷��̾� �Ÿ� ��� ���� ����
    private AudioSource audioSource; // ������Ʈ
    private Renderer monsterRenderer; // ��� �� ���� ��� ó���� ���� ������ ������
    CircleCollider2D monsterCollider;

    // ������ ��Ʈ�� Arr
    string[] deathSoundName = new string[5] { "Sounds/SFX/���̰�1", "Sounds/SFX/���̰�2", "Sounds/SFX/���̰�3", "Sounds/SFX/���̰�4", "Sounds/SFX/���̰�5", };
    string[] coinName = new string[3] { "Coin/Bronze", "Coin/Silver", "Coin/Gold"};
    static public int[] coinClassRangeCut = new int[3] { 70, 97, 100 };
    int[] tempCoinClassRangeCut_Up = new int[3] { 60, 95, 100 };

    protected virtual void Start()
    {
        commandCenter = GameObject.Find("CommandCenter");

        //// UnitRoot��� �̸��� �ڽ� ��ü���� Animator ������Ʈ�� ã��
        //Transform unitRootTransform = transform.Find("UnitRoot");
        //if (unitRootTransform != null)
        //{
        //    animator = unitRootTransform.GetComponent<Animator>();
        //}

        //// UnitRoot���� Animator�� ã�� ������ ��� HorseRoot���� �ٽ� ã��
        //if (animator == null)
        //{
        //    Transform horseRootTransform = transform.Find("HorseRoot");
        //    if (horseRootTransform != null)
        //    {
        //        animator = horseRootTransform.GetComponent<Animator>();
        //    }
        //}

        //// Animator�� ������ null�̶�� ��� ����ϰų� �⺻ �ִϸ����͸� ����
        //if (animator == null)
        //{
        //    Debug.LogWarning("Animator�� ã�� ���߽��ϴ�. �⺻ Animator�� ��ü�ϰų� �ٸ� ó���� �����մϴ�.");
        //    // �ʿ��� ��� �⺻ �ִϸ����͸� �Ҵ��ϰų� �߰� ������ ������ �� ����
        //    animator = GetComponent<Animator>();

        //    if (animator == null)
        //    {
        //        Debug.LogError("�⺻ Animator�� �������� �ʽ��ϴ�. �ִϸ����Ͱ� ��� �������� ������ �Ұ����մϴ�.");
        //        // �ʿ� �� �߰� ó���� ���⿡ ����
        //    }
        //}

        //// �ִϸ����Ͱ� ��ȿ�� ��쿡�� Ʈ���Ÿ� ����
        //if (animator != null)
        //{
        //    animator.SetTrigger("Run");
        //}

        monsterCollider = GetComponent<CircleCollider2D>();

        // ������ ����
        SetAudio();
        SetSearch();
    }

    protected void Update()
    {
        ChooseTarget();
    }

    void FixedUpdate()
    {
        Move();
    }
    
    void SetAudio()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        audioSource = gameObject.AddComponent<AudioSource>();
        monsterRenderer = GetComponent<Renderer>();
        audioSource.spatialBlend = 1.0f; // 3D ���� ����
        audioSource.minDistance = 1.0f; // n�� �� �Ҹ� �ּ�, �� ���� �Ҹ� ����
        audioSource.maxDistance = 5.0f; // n�� �� �Ҹ� �ִ�, �� �̻� �Ҹ� �ִ�
        LoadAudioClips();
    }

    void SetSearch()
    {
        target = GameObject.Find("tempCC");
        rigid = GetComponent<Rigidbody2D>();
        targetLayer = new LayerMask[3];
        targetLayer[0] = LayerMask.GetMask("Player");
        targetLayer[1] = LayerMask.GetMask("Tower");
        targetLayer[2] = LayerMask.GetMask("CommanCenter");
    }

    void LoadAudioClips()
    {
        for (int i = 0; i < deathSound.Length; i++)
        {
            deathSound[i] = Resources.Load<AudioClip>(deathSoundName[i]);
        }
    }

    protected virtual void ChooseTarget()
    {
        // �÷��̾� > Ÿ�� > ���� �� ��׷�
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, DetectRange, targetLayer[0]);
        if (targets.Length != 0)
        {
            target = targets[0].gameObject;
            return;
        }

        targets = Physics2D.OverlapCircleAll(transform.position, DetectRange, targetLayer[1]);
        if (targets.Length != 0)
        {
            target = targets[0].gameObject;
            return;
        }

        target = commandCenter;
    }

    void Move()
    {
        if (target != null && rigid!=null)
        {
            Vector2 goal = new Vector2(target.transform.position.x, target.transform.position.y);
            Vector2 direction = (goal - (Vector2)transform.position).normalized;

            // �̵� ���� ��
            float randomOffsetX = Random.Range(-1f, 1f); // X�� ����
            float randomOffsetY = Random.Range(-1f, 1f); // Y�� ����
            Vector2 randomOffset = new Vector2(randomOffsetX, randomOffsetY);

            // �̵��� ���� ����
            Vector2 finalDirection = (direction + randomOffset).normalized;

            rigid.velocity = finalDirection * speed;
        }
    }


    protected virtual void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0) StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        //animator.SetTrigger("Death");
        DropItem();
        PlayDeathSound();

        isDying = true;

        //�±�, ���̾� ����
        gameObject.tag = "Untagged";
        gameObject.layer = 0;

        // �Ҹ��� ����Ǵ� ���� �ݶ��̴��� �±� ����
        if (monsterCollider != null)
        {
            Destroy(monsterCollider); // �ݶ��̴� ����
        }

        if(rigid!=null)
        {            
           Destroy(rigid);
        }

        if (monsterRenderer != null)
        {
            monsterRenderer.enabled = false; // ������ ��Ȱ��ȭ
        }

        // �Ҹ��� ������ ������Ʈ�ϴ� ����
        while (audioSource.isPlaying)
        {
            UpdateSoundVolume();
            yield return null; // �� �����Ӹ��� ������ ������Ʈ�մϴ�.
        }

        Destroy(gameObject);


        //StartCoroutine(DeathRoutine());
    }

    //IEnumerator DeathRoutine()
    //{
    //    yield return new WaitForSeconds(1f);
    //    Destroy(gameObject);
    //}


    void UpdateSoundVolume()
    {
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            float volume = Mathf.Clamp01(1 - (distance / audioSource.maxDistance));
            volume = Mathf.Max(volume, audioSource.volume);

            audioSource.volume = volume * 0.8f; //�ò������� �� ����
        }
    }

    void PlayDeathSound()
    {
        int index = Random.Range(0, deathSound.Length);
        AudioClip clip = deathSound[index];

        audioSource.clip = clip;
        audioSource.Play();
    }

    void DropItem()
    {
        // �ߺ� ���� ����
        if (isDying == true) return;

        int index = Random.Range(1, 101); //1���� 100
        string coin;

        if (index < coinClassRangeCut[0]) coin = coinName[0];
        else if (index < coinClassRangeCut[1]) coin = coinName[1];
        else coin = coinName[2];

        // Resources �������� ������ �������� �ε�
        GameObject itemPrefab = Resources.Load<GameObject>(coin);

        if (itemPrefab != null)
        {
            // ���� ������Ʈ�� ��ġ�� ������ �������� ����
            Instantiate(itemPrefab, transform.position, transform.rotation);            
        }
        else
        {
            print(" �߸��� ����");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDying == true) return;

        if (collision.CompareTag("Bullet"))
        {
            BaseProjectile projectile = collision.GetComponent<BaseProjectile>();
            if (projectile != null)
            {
                TakeDamage(projectile.attackPower);
                projectile.CheckDestroy();
            }
        }
        else if (collision.CompareTag("Player") || collision.CompareTag("Tower") || collision.CompareTag("CommandCenter"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }
                attackCoroutine = StartCoroutine(Attack(damageable));
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Tower") || collision.CompareTag("CommandCenter"))
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator Attack(IDamageable target)
    {
        while (target != null)
        {
            if (Time.time - lastAttackTime >= attackInterval)
            {
                target.TakeDamage(attackPower);
                lastAttackTime = Time.time;
            }
            yield return null;
        }

        attackCoroutine = null; // �ڷ�ƾ ���� �� ��������� null ����
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectRange);
    }
}
