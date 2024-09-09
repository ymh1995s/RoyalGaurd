using System.Collections.Generic;
using UnityEngine;

// HIT & FLASH
// ����ü ����
// 1. �⺻(4 yellow arrow > Basic) 
// 2. �⺻ ��ȭ (11 orange arrow > ADVBasic)
// 3. ���Ӽ� (6 blue fire > ICE) //�̰� �Ⱥ���
// 4. ȭ�Ӽ� (16 red fire > FIRE)
// 5. EVT 22(STAT) or 27(HEART) > Special

public class BaseProjectile : MonoBehaviour
{
    //������Ʈ ����
    private Rigidbody2D rb;  // 2D ������ٵ�
    private CircleCollider2D cc;  // circle collider

    // ���ݷ� ����
    public int attackPower = 1;
    public static int attackPowerUp = 0;
    protected int[] master_attackPower = { 10,15,18 };

    // ���� ���� ����
    public static int maxPenetration = 1; // ����ü�� ����� (������ �� �ִ� ���� ��)
    int currentpenetration;

    // ����Ʈ ����
    protected GameObject hit;  // �浹 ȿ�� ������Ʈ
    protected GameObject flash;  // �߻� ȿ�� ������Ʈ
    public GameObject[] Detached;  // �и��� ������Ʈ �迭

    // ������ ��Ʈ�� Arr
    //protected string[] hitFrefabNames = { "Projectile/BasicHit", "Projectile/ADVBasicHit", "Projectile/ICEHit", "Projectile/FIREHit", "Projectile/Special2Hit" };
    //protected string[] flashFrefabNames = { "Projectile/BasicFlash", "Projectile/ADVBasicFlash", "Projectile/ICEFlash", "Projectile/FIREFlash", "Projectile/Special2Flash" };
    protected string[] hitFrefabNames = { "Projectile/BasicHit", "Projectile/ADVBasicHit", "Projectile/FireHit" };
    protected string[] flashFrefabNames = { "Projectile/BasicFlash", "Projectile/ADVBasicFlash", "Projectile/FireFlash" };

    //ETC
    Dictionary<string, int> dictionary = new Dictionary<string, int>();
    const int destorywait = 2;

    protected enum Level { LV1, LV2, LV3}

    //��� ����
    //public float hitOffset = 0f;  // �浹 �� ������
    //public bool UseFirePointRotation;  // �浹 �� ȸ�� ���� ����
    //public Vector3 rotationOffset = new Vector3(0, 0, 0);  // ȸ�� ������

    protected virtual void Start()
    {
        // ����ü ũ�⸦ x��� ����
        transform.localScale *= 1.5f;

        // ���� �� �� �ִ� ���� ��
        currentpenetration = maxPenetration;

        rb = GetComponent<Rigidbody2D>();
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject, destorywait);
    }

    public void Destroy()
    {
        var hitInstance = Instantiate(hit, transform.position, transform.rotation);

        // ����Ʈ ũ�⸦ x��� ����
        hitInstance.transform.localScale *= 1.2f;

        //Destroy hit effects depending on particle Duration time
        var hitPs = hitInstance.GetComponent<ParticleSystem>();
        if (hitPs != null)
        {
            Destroy(hitInstance, hitPs.main.duration);
        }
        else
        {
            var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy(hitInstance, hitPsParts.main.duration);
        }

        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
                Destroy(detachedPrefab, 1);
            }
        }

        Destroy(gameObject);
    }

    public void CheckDestroy()
    {
        if (--currentpenetration <= 0) Destroy();
    }
}
