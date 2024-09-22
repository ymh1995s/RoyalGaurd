using UnityEngine;

public class LV2Weapon : BaseWeapon
{    
    protected override void Start()
    {
        base.Start();
        detectionRadius = detectionRadius_[(int)Level.LV2];  // 타워의 탐지 반경
        bulletPrefab = Resources.Load<GameObject>(prefabNames[(int)Level.LV2]);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        CheckIsReadyFire();
    }

    protected override void CheckIsReadyFire()
    {
        fireRate = fireRates[(int)Level.LV2]; // 발사 간격을 초 단위로 설정 (X초에 한 번 발사)
        base.CheckIsReadyFire();
    }
}
