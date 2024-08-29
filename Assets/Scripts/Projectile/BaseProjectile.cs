using System.Collections.Generic;
using UnityEngine;

// HIT & FLASH
// 투사체 고르기
// 1. 기본(4 yellow arrow > Basic) 
// 2. 기본 강화 (11 orange arrow > ADVBasic)
// 3. 수속성 (6 blue fire > ICE) //이거 안보임
// 4. 화속성 (16 red fire > FIRE)
// 5. EVT 22(STAT) or 27(HEART) > Special

public class BaseProjectile : MonoBehaviour
{
    //컴포넌트 영역
    private Rigidbody2D rb;  // 2D 리지드바디
    private CircleCollider2D cc;  // circle collider

    // 공격력 영역
    public int attackPower = 1;
    public static int attackPowerUp = 0;
    protected int[] master_attackPower = { 10,15,18 };

    // 관통 스텟 영역
    public static int maxPenetration = 1; // 투사체의 관통력 (관통할 수 있는 적의 수)
    int currentpenetration;

    // 이펙트 영역
    protected GameObject hit;  // 충돌 효과 오브젝트
    protected GameObject flash;  // 발사 효과 오브젝트
    public GameObject[] Detached;  // 분리된 오브젝트 배열

    // 참조용 스트링 Arr
    //protected string[] hitFrefabNames = { "Projectile/BasicHit", "Projectile/ADVBasicHit", "Projectile/ICEHit", "Projectile/FIREHit", "Projectile/Special2Hit" };
    //protected string[] flashFrefabNames = { "Projectile/BasicFlash", "Projectile/ADVBasicFlash", "Projectile/ICEFlash", "Projectile/FIREFlash", "Projectile/Special2Flash" };
    protected string[] hitFrefabNames = { "Projectile/BasicHit", "Projectile/ADVBasicHit", "Projectile/FireHit" };
    protected string[] flashFrefabNames = { "Projectile/BasicFlash", "Projectile/ADVBasicFlash", "Projectile/FireFlash" };

    //ETC
    Dictionary<string, int> dictionary = new Dictionary<string, int>();
    const int destorywait = 2;

    protected enum Level { LV1, LV2, LV3}

    //사용 보류
    //public float hitOffset = 0f;  // 충돌 시 오프셋
    //public bool UseFirePointRotation;  // 충돌 시 회전 적용 여부
    //public Vector3 rotationOffset = new Vector3(0, 0, 0);  // 회전 오프셋

    protected virtual void Start()
    {
        // 투사체 크기를 x배로 설정
        transform.localScale *= 1.5f;

        // 관통 할 수 있는 적의 수
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

        // 이펙트 크기를 x배로 설정
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
