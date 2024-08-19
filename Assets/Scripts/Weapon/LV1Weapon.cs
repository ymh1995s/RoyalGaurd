using UnityEngine;

public class LV1Weapon : BaseWeapon
{
    protected override void Start()
    {
        base.Start();
        detectionRadius = detectionRadius_[(int)Level.LV1];  // Ÿ���� Ž�� �ݰ�
        bulletPrefab = Resources.Load<GameObject>(prefabNames[(int)Level.LV1]);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Fire();
    }

    protected override void Fire()
    {
        fireRate = fireRates[(int)Level.LV1]; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�)
        base.Fire();
    }
}
