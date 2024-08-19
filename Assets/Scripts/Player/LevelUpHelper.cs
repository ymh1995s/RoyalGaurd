using UnityEngine;

public static class LevelUpHelper
{
    static void WeaponUpgrade(int index)
    {
        //TODO 플레이어에 있는거 여기로 빼오기
    }

    static public void WeaponAttackPowerUp()
    {
        BaseProjectile.attackPowerUp += 2;
        GameManager.Instance.hudManager.LevelUpHintUpdate("공격력 증가!");
    }

    static public void WeaponAttackSpeedUp()
    {
        BaseWeapon.fireRateMmul *= 0.97f;
        GameManager.Instance.hudManager.LevelUpHintUpdate("무기 공격 속도 증가!");
    }

    static public void WeaponRangedUp()
    {
        BaseWeapon.detectionRadiusMul += 0.15f;
        GameManager.Instance.hudManager.LevelUpHintUpdate("무기 사거리 증가!");
    }

    //이것도 분기좀 빼면 안되나
    static public void TowerUpgrade(int index)
    {
        if (index == 0) TowerAttackSpeedUp();
        else if (index == 1) TowerRangeUp();
    }

    static public void TowerAttackSpeedUp()
    {
        BaseTower.fireRateMmul *= 0.95f;
        GameManager.Instance.hudManager.LevelUpHintUpdate("타워 공격 속도 증가!");
    }

    static public void TowerRangeUp()
    {
        BaseTower.detectionRadius += 0.2f;
        GameManager.Instance.hudManager.LevelUpHintUpdate("타워 사거리 증가!");
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
        GameManager.Instance.hudManager.LevelUpHintUpdate("플레이어 체력 증가!");
    }

    static public void PlayerSpeedUp()
    {
        BasePlayer.moveSpeed += 0.2f;
        GameManager.Instance.hudManager.LevelUpHintUpdate("플레이어 이동속도 증가!");
    }
}
