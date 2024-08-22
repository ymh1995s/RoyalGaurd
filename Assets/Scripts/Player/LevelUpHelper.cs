using UnityEngine;

public static class LevelUpHelper
{
    static void WeaponUpgrade(int index)
    {
        //TODO 플레이어에 있는거 여기로 빼오기
    }

    static public void WeaponAttackPowerUp(int value = 2 )
    {
        BaseProjectile.attackPowerUp += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("공격력 증가!");
    }

    static public void WeaponAttackSpeedUp(float value = 0.97f)
    {
        BaseWeapon.fireRateMmul *= value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("무기 공격 속도 증가!");
    }

    static public void WeaponRangedUp(float value  = 0.15f)
    {
        BaseWeapon.detectionRadiusPlus += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("무기 사거리 증가!");
    }

    //이것도 분기좀 빼면 안되나
    static public void TowerUpgrade(int index)
    {
        if (index == 0) TowerAttackSpeedUp();
        else if (index == 1) TowerRangeUp();
    }

    static public void TowerAttackSpeedUp(float value = 0.95f)
    {
        BaseTower.fireRateMmul *= value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("타워 공격 속도 증가!");
    }

    static public void TowerRangeUp(float value = 0.2f)
    {
        BaseTower.detectionRadiusPlus += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("타워 사거리 증가!");
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
        GameManager.Instance.hudManager.LevelUpHintUpdate("플레이어 체력 증가!");
        GameManager.Instance.player.UpdateHealthBar();
    }

    static public void PlayerSpeedUp(float value =0.2f)
    {
        BasePlayer.moveSpeed += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("플레이어 이동속도 증가!");
    }
}
