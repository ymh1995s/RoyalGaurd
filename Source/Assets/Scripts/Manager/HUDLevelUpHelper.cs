using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // UI ������Ʈ�� ����ϱ� ���� �߰�

public class HUDLevelUpHelper
{
    private Dictionary<int, float> bonusAppearanceWeight;  // �� ���ʽ� �������� ����ġ(Ȯ��)
    private List<Vector2Int> bonusAllPairs;                // �� ���ʽ� �������� ������ ������ �� ���� 
    private float tempTimeScale;                           // ���ʽ� �������� ���� �� ���߱� ���� timeScale
    private GameObject bonusLevelUpButtonGroup;
    private Button[] bonusLevelUpButtonArr;
    private int allOptionCount = 10; // ���� �غ��� �������� �� 10��

    public HUDLevelUpHelper(GameObject buttonGroup, Button[] buttonArr)
    {
        if(buttonGroup==null || buttonArr == null)
        {
            Debug.LogError("HUDLevelupUpHelper ������ ���� ����");
        }

        bonusLevelUpButtonGroup = buttonGroup;
        bonusLevelUpButtonArr = buttonArr;

        InitializeBonusAppearanceWeight(); // ���ʽ� �������� ����ġ ����
        GenerateAllPairs();                // ���ʽ� �������� ������ ��� ��� ����

        // ������ ���� 100,000�� �����Ͽ� ���� Ȯ���� ��ġ�ϴ��� �׽�Ʈ.
        //TestProbabilities(100000);
    }

    private void InitializeBonusAppearanceWeight()
    {
        bonusAppearanceWeight = new Dictionary<int, float>()
        {
            { 0, 19.5f },    // 0�� ���Ե� Ȯ���� x%
            { 1, 19.2f },    // 1�� ���Ե� Ȯ���� y%
            { 2, 17 },
            { 3, 18 },
            { 4, 18.5f },
            { 5, 0.5f },   // ����
            { 6, 0.8f },   // ����ü ���� 
            { 7, 3f },   // ü�� �ڵ� ȸ��
            { 8, 2f },   // ���� Ȯ��
            { 9, 1.5f }    // ����� Ÿ��

            //{ 0, 0 },    // 0�� ���Ե� Ȯ���� x%
            //{ 1, 19f },    // 1�� ���Ե� Ȯ���� y%
            //{ 2, 17 },
            //{ 3, 17 },
            //{ 4, 0 },
            //{ 5, 20f }, // ����
            //{ 6, 1f },  // ����ü ���� 
            //{ 7, 3f },  // ü�� �ڵ� ȸ��
            //{ 8, 3f },  // ���� Ȯ��
            //{ 9, 20f }  // ����� Ÿ��

            //{ 0, 2 }, // �׽�Ʈ��
            //{ 1, 2 },
            //{ 2, 2 },
            //{ 3, 2 },
            //{ 4, 2 },
            //{ 5, 20 },    // ����
            //{ 6, 0},    // ����ü ���� 
            //{ 7, 20 },    // ü�� �ڵ� ȸ��
            //{ 8, 20 },    // ���� Ȯ��
            //{ 9, 20 }     // ����� Ÿ��
        };

        float sum = 0;

        foreach (var kvp in bonusAppearanceWeight)
        {
            sum += kvp.Value;
        }

        // ���� 100�� �ƴϸ� ���
        if (Math.Abs(sum - 100) <= 0.0001)
        {
             //  Console.WriteLine($"�� ��° ������ ���� 100�Դϴ�. ���� ��: {sum}");
        }
        else
        {
             Debug.LogError($"�� ��° ������ ���� 100�� �ƴմϴ�. ���� ��: {sum}");
        }
    }

    private void GenerateAllPairs()
    {
        bonusAllPairs = new List<Vector2Int>();
        for (int i = 0; i <= allOptionCount-1; i++)
        {
            for (int j = i + 1; j <= allOptionCount - 1; j++)
            {
                bonusAllPairs.Add(new Vector2Int(i, j));
            }
        }
    }

    public void BonusLevelUp()
    {
        // 1. ���� ��� ����
        tempTimeScale = Time.timeScale;
        Time.timeScale = 0;

        // 2. ������ Canvas Ȱ��ȭ
        bonusLevelUpButtonGroup.SetActive(true);

        // 3. (������ �����ߴ�) ������ ��� ��Ȱ��ȭ
        HideAllBonusButtons();

        // 4. �̹��� ������ ���ʽ� �������� ���� ������
        Vector2Int pair = GetRandomPair();
        ShowBonusButtons(pair);
    }

    private void HideAllBonusButtons()
    {
        foreach (var button in bonusLevelUpButtonArr)
        {
            button.gameObject.SetActive(false);
        }
    }

    private void ShowBonusButtons(Vector2Int pair)
    {
        for (int i = 0; i < bonusLevelUpButtonArr.Length; i++)
        {
            if (i == pair.x || i == pair.y)
            {
                bonusLevelUpButtonArr[i].gameObject.SetActive(true);
            }
        }
    }

    private Vector2Int GetRandomPair()
    {
        if (bonusAllPairs.Count == 0)
        {
            Debug.LogWarning("No more pairs available!");
            return new Vector2Int(-1, -1); // ���� ó��
        }

        // �� �ֿ� ���� ����ġ�� ����մϴ�.
        List<float> weights = new List<float>();
        foreach (var pair in bonusAllPairs)
        {
            float weight = 1.0f;
            weight *= bonusAppearanceWeight.ContainsKey(pair.x) ? bonusAppearanceWeight[pair.x] : 1.0f;
            weight *= bonusAppearanceWeight.ContainsKey(pair.y) ? bonusAppearanceWeight[pair.y] : 1.0f;
            weights.Add(weight);
        }

        // ����ġ ������� ������ ���� �����մϴ�.
        float totalWeight = 0f;
        foreach (var weight in weights)
        {
            totalWeight += weight;
        }

        float randomWeightPoint = UnityEngine.Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < bonusAllPairs.Count; i++)
        {
            cumulativeWeight += weights[i];
            if (randomWeightPoint <= cumulativeWeight)
            {
                return bonusAllPairs[i];
            }
        }

        // ���� ó�� (�� �ڵ忡 �������� �ʾƾ� ��)
        return bonusAllPairs[0];
    }

    public void ApplyBonus(Action levelUpAction)
    {
        levelUpAction();

        // �������� ������Ƿ� ��� ������ ��Ȱ��ȭ
        foreach (var button in bonusLevelUpButtonArr)
        {
            button.gameObject.SetActive(false);
        }

        // ���� ��� �ӵ� ����ġ
        Time.timeScale = tempTimeScale;

        // ���� ������ ����
        GameManager.Instance.hudManager.StopVideo();
    }

    public void BonusPowerUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.WeaponAttackPowerUp(2));
    }

    public void BonusAttackSpeedUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.WeaponAttackSpeedUp(0.98f));
    }

    public void BonusRangeUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.WeaponRangedUp(0.2f));
    }

    public void BonusHPUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.PlayerHPUp(0));
    }

    public void BonusMoveSpeedUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.PlayerSpeedUp(0.2f));
    }

    // TODO : 5~9�� ENUM ����
    public void BonusPenetraionUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.PenetrationUp());
        ExcludePairsContaining(5);
    }

    public void BonusProjectileUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.ProjectileUp());
        ExcludePairsContaining(6);
    }

    public void BonusHPAutoRecover()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.HPAutoRecover());
        ExcludePairsContaining(7);
    }

    public void BonusCoinDropUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.CoinDropUp());
        ExcludePairsContaining(8);
    }

    public void BonusHiddenTower()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.HiddenTowerSpawn());
        ExcludePairsContaining(9);
    }

    public void ExcludePairsContaining(int number)
    {
        // ����ũ�� �ɷ� (����� 5~9)�� 1ȸ�� �� �� �����Ƿ� ���������� ����
        bonusAllPairs.RemoveAll(pair => pair.x == number || pair.y == number);
    }

    // ���� ������ �ۼ�Ʈ�� �������� print Ȯ��
    public void TestProbabilities(int testCount)
    {
        Dictionary<int, int> numberCounts = new Dictionary<int, int>();

        // ���� ī��Ʈ �ʱ�ȭ
        for (int i = 0; i <= 9; i++)
        {
            numberCounts[i] = 0;
        }

        // �׽�Ʈ Ƚ����ŭ ������ ���� �����ϰ� �� ���� ���� Ƚ���� ���
        for (int i = 0; i < testCount; i++)
        {
            Vector2Int pair = GetRandomPair();
            numberCounts[pair.x]++;
            numberCounts[pair.y]++;
        }

        // ��� ���
        Debug.Log("Number Appearance Counts:");
        foreach (var kvp in numberCounts)
        {
            float appearanceRate = (float)kvp.Value / (testCount * 2) * 100f;
            Debug.Log($"Number {kvp.Key}: {kvp.Value} times, Appeared in {appearanceRate}% of pairs");
        }
    }
}