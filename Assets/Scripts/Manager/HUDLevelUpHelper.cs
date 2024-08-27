using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ������Ʈ�� ����ϱ� ���� �߰�

public class HUDLevelUpHelper
{
    private Dictionary<int, float> bonusAppearanceWeight;  // �� ���ʽ� �������� ����ġ(Ȯ��)
    private List<Vector2Int> bonusAllPairs;                // �� ���ʽ� �������� ������ ������ �� ���� 
    private float tempTimeScale;                           // ���ʽ� �������� ���� �� ���߱� ���� timeScale
    private GameObject bonusLevelUpButtonGroup;
    private Button[] bonusLevelUpButtonArr;

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
            //{ 0, 18 },    // 0�� ���Ե� Ȯ���� x%
            //{ 1, 18 },    // 1�� ���Ե� Ȯ���� y%
            //{ 2, 18 },
            //{ 3, 18 },
            //{ 4, 18 },
            //{ 5, 2 },
            //{ 6, 2 },
            //{ 7, 2 },
            //{ 8, 2 },
            //{ 9, 2 }
            { 0, 1 },
            { 1, 1 },
            { 2, 1 },
            { 3, 1 },
            { 4, 1 },
            { 5, 19 },
            { 6, 19 },
            { 7, 19 },
            { 8, 19 },
            { 9, 19 }
        };
    }

    private void GenerateAllPairs()
    {
        bonusAllPairs = new List<Vector2Int>();
        for (int i = 0; i <= 9; i++)
        {
            for (int j = i + 1; j <= 9; j++)
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