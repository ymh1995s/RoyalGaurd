using UnityEngine;

public class LV1Projectile : BaseProjectile
{
    protected override void Start()
    {
        base.Start();
        attackPower = master_attackPower[(int)Level.LV1] + attackPowerUp;

        hit = Resources.Load<GameObject>(hitFrefabNames[(int)Level.LV1]);  // �浹 ȿ�� ������Ʈ
        flash = Resources.Load<GameObject>(flashFrefabNames[(int)Level.LV1]);  // �߻� ȿ�� ������Ʈ
    }
}
