using UnityEngine;

public class LV3Projectile : BaseProjectile
{
    static public int attackPower_ForUI;
    protected override void Start()
    {
        base.Start();
        attackPower = master_attackPower[(int)Level.LV3] + attackPowerUp;

        hit = Resources.Load<GameObject>(hitFrefabNames[(int)Level.LV3]);  // �浹 ȿ�� ������Ʈ
        flash = Resources.Load<GameObject>(flashFrefabNames[(int)Level.LV3]);  // �߻� ȿ�� ������Ʈ
    }

    private void Update()
    {
        attackPower_ForUI = attackPower + attackPowerUp; ;
    }
}
