using System;
using UnityEngine;

public class LevelUpHelper : MonoBehaviour
{
    //TODO : ����� ���ڵ��� �Լ� �Ķ���� ���� �迭�̶���� �ؼ� �����ؾ߰ڴ�.

    public void WeaponAdd(BasePlayer player, Transform parentTransform)
    {
        for (int i = 0; i < player.maxWeaponCount; i++)
        {
            if (player.obtainedWeapon[i] == null)
            {
                GameObject weapon;
                int index = UnityEngine.Random.Range(1, 101); // 1���� 101

                if (index < player.weaponAddClassCut[0])
                    weapon = GameObject.Instantiate(player.weaponPrefab[0], parentTransform.position, Quaternion.identity);
                else if (index < player.weaponAddClassCut[1])
                    weapon = GameObject.Instantiate(player.weaponPrefab[1], parentTransform.position, Quaternion.identity);
                else
                    weapon = GameObject.Instantiate(player.weaponPrefab[2], parentTransform.position, Quaternion.identity);

                weapon.transform.parent = parentTransform; // ���޵� Transform�� �θ�� ����

                player.obtainedWeapon[i] = weapon;
                WeaponSort(player);
                GameManager.Instance.hudManager.LevelUpHintUpdate("���� �߰�!");
                return;
            }
        }
    }

    public void WeaponSort(BasePlayer player)
    {
        GameObject[] weapons1 = GameObject.FindGameObjectsWithTag("LV1Weapon");
        GameObject[] weapons2 = GameObject.FindGameObjectsWithTag("LV2Weapon");
        GameObject[] weapons3 = GameObject.FindGameObjectsWithTag("LV3Weapon");

        int weaponCount = weapons1.Length + weapons2.Length + weapons3.Length;

        try
        {
            int rad = 360 / weaponCount;

            for (int i = 0; i < weaponCount; i++)
            {
                BaseWeapon bweapon = player.obtainedWeapon[i].GetComponent<BaseWeapon>();
                bweapon.currentAngle = rad * i;
            }
        }

        catch (Exception ex)
        {
            Debug.LogError("sort err");
            Debug.LogError(ex.ToString());
        }
    }

    public void WeaponAttackPowerUp(int value = 1)
    {
        BaseProjectile.attackPowerUp += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("���ݷ� ����!");
    }

    public void WeaponAttackSpeedUp(float value = 0.98f)
    {
        BaseWeapon.fireRateMmul *= value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("���� ���� �ӵ� ����!");
    }

    public void WeaponRangedUp(float value = 0.15f)
    {
        BaseWeapon.detectionRadiusPlus += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("���� ��Ÿ� ����!");
    }

    public void TowerUpgrade(int index)
    {
        if (index == 0) TowerAttackSpeedUp();
        else if (index == 1) TowerRangeUp();
    }

    public void TowerAttackSpeedUp(float value = 0.96f)
    {
        BaseTower.fireRateMmul *= value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("Ÿ�� ���� �ӵ� ����!");
    }

    public void TowerRangeUp(float value = 0.2f)
    {
        BaseTower.detectionRadiusPlus += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("Ÿ�� ��Ÿ� ����!");
    }

    public void PlayerUpgrade(BasePlayer player, int index)
    {
        if (index == 0) PlayerHPUp(player);
        else if (index == 1) PlayerSpeedUp(player);
    }

    public void PlayerHPUp(BasePlayer player, int value = 1)
    {
        player.maxHP += value;
        player.currentHP = player.maxHP;
        GameManager.Instance.hudManager.LevelUpHintUpdate("�÷��̾� ü�� ����!");
        GameManager.Instance.player.UpdateHealthBar();
    }

    public void PlayerSpeedUp(BasePlayer player, float value = 0.2f)
    {
        player.moveSpeed += value;
        GameManager.Instance.hudManager.LevelUpHintUpdate("�÷��̾� �̵��ӵ� ����!");
    }

    public void PenetrationUp(int value = 1)
    {
        BaseProjectile.maxPenetration += value;
    }

    public void ProjectileUp(int value = 1)
    {
        BaseWeapon.fireMultiple += value;
    }

    public void HPAutoRecover()
    {
        GameManager.Instance.player.isObtainedAutoRecover = true;
    }

    public void CoinDropUp()
    {
        BaseMonster.coinClassRangeCut = new int[3] { 50, 94, 100 };
    }

    public void HiddenTowerSpawn()
    {
        SpecialTowerSpawn();
    }

    void SpecialTowerSpawn()
    {
        // Resources �������� Tower �������� �ε�
        GameObject towerPrefab = Resources.Load<GameObject>("SpecialTower");

        // Ÿ�� �������� ����� �ε�Ǿ����� Ȯ��
        if (towerPrefab != null)
        {
            // Ÿ���� Ư�� ��ǥ�� Instantiate
            //Vector3 position = new Vector3(-9.5f, 1.1f, 0f); // �����
            Vector3 position = new Vector3(-9.8f, 1.88f, 0f);
            Instantiate(towerPrefab, position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Tower prefab could not be found in the Resources folder.");
        }
    }
}
