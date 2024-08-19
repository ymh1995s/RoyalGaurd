using UnityEngine;

public static class LevelUpHelper
{
    static void WeaponUpgrade(int index)
    {
        //TODO �÷��̾ �ִ°� ����� ������
    }

    static public void WeaponAttackPowerUp()
    {
        BaseProjectile.attackPowerUp += 2;
        GameManager.Instance.hudManager.LevelUpHintUpdate("���ݷ� ����!");
    }

    static public void WeaponAttackSpeedUp()
    {
        BaseWeapon.fireRateMmul *= 0.97f;
        GameManager.Instance.hudManager.LevelUpHintUpdate("���� ���� �ӵ� ����!");
    }

    static public void WeaponRangedUp()
    {
        BaseWeapon.detectionRadiusMul += 0.15f;
        GameManager.Instance.hudManager.LevelUpHintUpdate("���� ��Ÿ� ����!");
    }

    //�̰͵� �б��� ���� �ȵǳ�
    static public void TowerUpgrade(int index)
    {
        if (index == 0) TowerAttackSpeedUp();
        else if (index == 1) TowerRangeUp();
    }

    static public void TowerAttackSpeedUp()
    {
        BaseTower.fireRateMmul *= 0.95f;
        GameManager.Instance.hudManager.LevelUpHintUpdate("Ÿ�� ���� �ӵ� ����!");
    }

    static public void TowerRangeUp()
    {
        BaseTower.detectionRadius += 0.2f;
        GameManager.Instance.hudManager.LevelUpHintUpdate("Ÿ�� ��Ÿ� ����!");
    }

    static public void PlayerUpgrade(int index)
    {
        if (index == 0)  PlayerHPUp(); 
        else if (index == 1)  PlayerSpeedUp();
    }

    static public void PlayerHPUp()
    {
        BasePlayer.maxHP += 1;
        BasePlayer.currentHP += BasePlayer.maxHP;
        GameManager.Instance.hudManager.LevelUpHintUpdate("�÷��̾� ü�� ����!");
    }

    static public void PlayerSpeedUp()
    {
        BasePlayer.moveSpeed += 0.2f;
        GameManager.Instance.hudManager.LevelUpHintUpdate("�÷��̾� �̵��ӵ� ����!");
    }
}
