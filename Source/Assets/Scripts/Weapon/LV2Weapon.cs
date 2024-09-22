using UnityEngine;

public class LV2Weapon : BaseWeapon
{    
    protected override void Start()
    {
        base.Start();
        detectionRadius = detectionRadius_[(int)Level.LV2];  // Ÿ���� Ž�� �ݰ�
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
        fireRate = fireRates[(int)Level.LV2]; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�)
        base.CheckIsReadyFire();
    }
}
