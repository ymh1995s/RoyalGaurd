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
        Fire();
    }

    protected override void Fire()
    {
        fireRate = fireRates[(int)Level.LV2]; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�)
        base.Fire();
    }
}
