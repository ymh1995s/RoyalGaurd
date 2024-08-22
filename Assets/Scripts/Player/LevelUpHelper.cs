using UnityEngine;

public static class LevelUpHelper
{
    static void WeaponUpgrade(int index)
    {
        //TODO �÷��̾ �ִ°� ����� ������
    }

    static public void WeaponAttackPowerUp(int value = 2 )
    {
        BaseProjectile.attackPowerUp += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("���ݷ� ����!");
    }

    static public void WeaponAttackSpeedUp(float value = 0.97f)
    {
        BaseWeapon.fireRateMmul *= value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("���� ���� �ӵ� ����!");
    }

    static public void WeaponRangedUp(float value  = 0.15f)
    {
        BaseWeapon.detectionRadiusPlus += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("���� ��Ÿ� ����!");
    }

    //�̰͵� �б��� ���� �ȵǳ�
    static public void TowerUpgrade(int index)
    {
        if (index == 0) TowerAttackSpeedUp();
        else if (index == 1) TowerRangeUp();
    }

    static public void TowerAttackSpeedUp(float value = 0.95f)
    {
        BaseTower.fireRateMmul *= value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("Ÿ�� ���� �ӵ� ����!");
    }

    static public void TowerRangeUp(float value = 0.2f)
    {
        BaseTower.detectionRadiusPlus += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("Ÿ�� ��Ÿ� ����!");
    }

    static public void PlayerUpgrade(int index)
    {
        if (index == 0)  PlayerHPUp(); 
        else if (index == 1)  PlayerSpeedUp();
    }

    static public void PlayerHPUp(int value = 1)
    {
        BasePlayer.maxHP += value;
        BasePlayer.currentHP = BasePlayer.maxHP;
        GameManager.Instance.hudManager.LevelUpHintUpdate("�÷��̾� ü�� ����!");
        GameManager.Instance.player.UpdateHealthBar();
    }

    static public void PlayerSpeedUp(float value =0.2f)
    {
        BasePlayer.moveSpeed += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("�÷��̾� �̵��ӵ� ����!");
    }
}
