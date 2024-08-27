using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 추가

public class HUDLevelUpHelper
{
    private Dictionary<int, float> bonusAppearanceWeight;
    private List<Vector2Int> bonusAllPairs;
    private float tempTimeScale;
    private GameObject bonusLevelUpButtonGroup;
    private Button[] bonusLevelUpButtonArr;

    public HUDLevelUpHelper(GameObject buttonGroup, Button[] buttonArr)
    {
        bonusLevelUpButtonGroup = buttonGroup;
        bonusLevelUpButtonArr = buttonArr;

        InitializeBonusAppearanceWeight();
        GenerateAllPairs();

        // 무작위 쌍을 100,000번 생성하여 실제 확률과 일치하는지 테스트.
        //TestProbabilities(100000);
    }

    private void InitializeBonusAppearanceWeight()
    {
        bonusAppearanceWeight = new Dictionary<int, float>()
        {
            //{ 0, 18 },
            //{ 1, 18 },
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
        tempTimeScale = Time.timeScale;
        Time.timeScale = 0;
        bonusLevelUpButtonGroup.SetActive(true);
        HideAllBonusButtons();

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
            return new Vector2Int(-1, -1); // 예외 처리
        }

        List<float> weights = new List<float>();
        foreach (var pair in bonusAllPairs)
        {
            float weight = 1.0f;
            weight *= bonusAppearanceWeight.ContainsKey(pair.x) ? bonusAppearanceWeight[pair.x] : 1.0f;
            weight *= bonusAppearanceWeight.ContainsKey(pair.y) ? bonusAppearanceWeight[pair.y] : 1.0f;
            weights.Add(weight);
        }

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

        return bonusAllPairs[0];
    }

    public void ApplyBonus(Action levelUpAction)
    {
        levelUpAction();
        foreach (var button in bonusLevelUpButtonArr)
        {
            button.gameObject.SetActive(false);
        }
        Time.timeScale = tempTimeScale;
    }

    public void ExcludePairsContaining(int number)
    {
        bonusAllPairs.RemoveAll(pair => pair.x == number || pair.y == number);
    }

    public void TestProbabilities(int testCount)
    {
        Dictionary<int, int> numberCounts = new Dictionary<int, int>();

        // 숫자 카운트 초기화
        for (int i = 0; i <= 9; i++)
        {
            numberCounts[i] = 0;
        }

        // 테스트 횟수만큼 무작위 쌍을 생성하고 각 숫자 등장 횟수를 계산
        for (int i = 0; i < testCount; i++)
        {
            Vector2Int pair = GetRandomPair();
            numberCounts[pair.x]++;
            numberCounts[pair.y]++;
        }

        // 결과 출력
        Debug.Log("Number Appearance Counts:");
        foreach (var kvp in numberCounts)
        {
            float appearanceRate = (float)kvp.Value / (testCount * 2) * 100f;
            Debug.Log($"Number {kvp.Key}: {kvp.Value} times, Appeared in {appearanceRate}% of pairs");
        }
    }
}