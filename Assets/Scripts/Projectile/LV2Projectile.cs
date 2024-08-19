using UnityEngine;

public class LV2Projectile : BaseProjectile
{
    static public int attackPower_ForUI;
    protected override void Start()
    {
        base.Start();
        attackPower = master_attackPower[(int)Level.LV2] + attackPowerUp;

        hit = Resources.Load<GameObject>(hitFrefabNames[(int)Level.LV2]);  // �浹 ȿ�� ������Ʈ
        flash = Resources.Load<GameObject>(flashFrefabNames[(int)Level.LV2]);  // �߻� ȿ�� ������Ʈ
    }

    private void Update()
    {
        attackPower_ForUI = attackPower + attackPowerUp; ;
    }
}
