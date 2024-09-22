using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 추가

public class HUDLevelUpHelper
{
    private Dictionary<int, float> bonusAppearanceWeight;  // 각 보너스 선택지의 가중치(확률)
    private List<Vector2Int> bonusAllPairs;                // 각 보너스 선택지가 나오는 선택지 총 집합 
    private float tempTimeScale;                           // 보너스 선택지가 나올 때 멈추기 전의 timeScale
    private GameObject bonusLevelUpButtonGroup;
    private Button[] bonusLevelUpButtonArr;
    private int allOptionCount = 10; // 현재 준비한 선택지는 총 10개

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
            { 0, 19.5f },    // 0은 포함될 확률이 x%
            { 1, 19.2f },    // 1은 포함될 확률이 y%
            { 2, 17 },
            { 3, 18 },
            { 4, 18.5f },
            { 5, 0.5f },   // 관통
            { 6, 0.8f },   // 투사체 증가 
            { 7, 3f },   // 체력 자동 회복
            { 8, 2f },   // 코인 확률
            { 9, 1.5f }    // 스페셜 타워

            //{ 0, 0 },    // 0은 포함될 확률이 x%
            //{ 1, 19f },    // 1은 포함될 확률이 y%
            //{ 2, 17 },
            //{ 3, 17 },
            //{ 4, 0 },
            //{ 5, 20f }, // 관통
            //{ 6, 1f },  // 투사체 증가 
            //{ 7, 3f },  // 체력 자동 회복
            //{ 8, 3f },  // 코인 확률
            //{ 9, 20f }  // 스페셜 타워

            //{ 0, 2 }, // 테스트용
            //{ 1, 2 },
            //{ 2, 2 },
            //{ 3, 2 },
            //{ 4, 2 },
            //{ 5, 20 },    // 관통
            //{ 6, 0},    // 투사체 증가 
            //{ 7, 20 },    // 체력 자동 회복
            //{ 8, 20 },    // 코인 확률
            //{ 9, 20 }     // 스페셜 타워
        };

        float sum = 0;

        foreach (var kvp in bonusAppearanceWeight)
        {
            sum += kvp.Value;
        }

        // 합이 100이 아니면 경고
        if (Math.Abs(sum - 100) <= 0.0001)
        {
             //  Console.WriteLine($"두 번째 인자의 합이 100입니다. 현재 합: {sum}");
        }
        else
        {
             Debug.LogError($"두 번째 인자의 합이 100이 아닙니다. 현재 합: {sum}");
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

        // 재촉 동영상 정지
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

    // TODO : 5~9는 ENUM 관리
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