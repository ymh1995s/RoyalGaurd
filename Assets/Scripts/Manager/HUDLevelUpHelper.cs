using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 추가

public class HUDLevelUpHelper
{
    private Dictionary<int, float> bonusAppearanceWeight;  // 각 보너스 선택지의 가중치(확률)
    private List<Vector2Int> bonusAllPairs;                // 각 보너스 선택지가 나오는 선택지 총 집합 
    private float tempTimeScale;                           // 보너스 선택지가 나올 때 멈추기 전의 timeScale
    private GameObject bonusLevelUpButtonGroup;
    private Button[] bonusLevelUpButtonArr;

    public HUDLevelUpHelper(GameObject buttonGroup, Button[] buttonArr)
    {
        if(buttonGroup==null || buttonArr == null)
        {
            Debug.LogError("HUDLevelupUpHelper 생성자 생성 실패");
        }

        bonusLevelUpButtonGroup = buttonGroup;
        bonusLevelUpButtonArr = buttonArr;

        InitializeBonusAppearanceWeight(); // 보너스 선택지의 가중치 설정
        GenerateAllPairs();                // 보너스 선택지가 등장할 모든 경우 생성

        // 무작위 쌍을 100,000번 생성하여 실제 확률과 일치하는지 테스트.
        //TestProbabilities(100000);
    }

    private void InitializeBonusAppearanceWeight()
    {
        bonusAppearanceWeight = new Dictionary<int, float>()
        {
            //{ 0, 18 },    // 0은 포함될 확률이 x%
            //{ 1, 18 },    // 1은 포함될 확률이 y%
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
        // 1. 게임 재생 멈춤
        tempTimeScale = Time.timeScale;
        Time.timeScale = 0;

        // 2. 선택지 Canvas 활성화
        bonusLevelUpButtonGroup.SetActive(true);

        // 3. (이전에 등장했던) 선택지 모두 비활성화
        HideAllBonusButtons();

        // 4. 이번에 보여줄 보너스 선택지를 고르고 보여줌
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

        // 각 쌍에 대한 가중치를 계산합니다.
        List<float> weights = new List<float>();
        foreach (var pair in bonusAllPairs)
        {
            float weight = 1.0f;
            weight *= bonusAppearanceWeight.ContainsKey(pair.x) ? bonusAppearanceWeight[pair.x] : 1.0f;
            weight *= bonusAppearanceWeight.ContainsKey(pair.y) ? bonusAppearanceWeight[pair.y] : 1.0f;
            weights.Add(weight);
        }

        // 가중치 기반으로 랜덤한 쌍을 선택합니다.
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

        // 예외 처리 (이 코드에 도달하지 않아야 함)
        return bonusAllPairs[0];
    }

    public void ApplyBonus(Action levelUpAction)
    {
        levelUpAction();

        // 선택지를 골랐으므로 모든 선택지 비활성화
        foreach (var button in bonusLevelUpButtonArr)
        {
            button.gameObject.SetActive(false);
        }

        // 게임 재생 속도 원위치
        Time.timeScale = tempTimeScale;
    }

    public void ExcludePairsContaining(int number)
    {
        // 유니크한 능력 (현재는 5~9)는 1회만 고를 수 있으므로 선택지에서 삭제
        bonusAllPairs.RemoveAll(pair => pair.x == number || pair.y == number);
    }

    // 실제 설정한 퍼센트로 나오는지 print 확인
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