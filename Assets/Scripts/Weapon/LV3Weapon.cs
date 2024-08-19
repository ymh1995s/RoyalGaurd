using UnityEngine;

public class LV3Weapon : BaseWeapon
{
    protected override void Start()
    {
        base.Start();
        detectionRadius = detectionRadius_[(int)Level.LV3];  // Ÿ���� Ž�� �ݰ�
        bulletPrefab = Resources.Load<GameObject>(prefabNames[(int)Level.LV3]);
    }

    protected override void Update()
    {
        base.Update();
        Fire();
    }

    protected override void Fire()
    {
        fireRate = fireRates[(int)Level.LV3]; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�)
        base.Fire();
    }
}
