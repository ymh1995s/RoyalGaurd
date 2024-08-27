using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 추가

public class HUDManager : MonoBehaviour
{
    // TODO : HUD 스크립트가 무거워 지려고 하니 분리할 것

    //스텟 영역
    private Text lvText;
    private Text playerSpecText;
    private Text WeaponSpecText;
    private Text TowerSpecText;
    private Text levelupHintText;

    // 보너스 레밸업 버튼 컴포넌트
    // TODO 숫자 하드 코딩 말고 네이밍으로 바꿔야됨
    // 아닌가 나중에 다른 능력으로 바꿀거라 임시로는 괜찮은가
    public GameObject BonusLevelUpButtonGroup;
    public Button[] BonusLevelUpButtonArr;
    public Button BonusLevelUpButton1;
    public Button BonusLevelUpButton2;
    public Button BonusLevelUpButton3;
    public Button BonusLevelUpButton4;
    public Button BonusLevelUpButton5;
    public Button BonusLevelUpButton6;
    public Button BonusLevelUpButton7;
    public Button BonusLevelUpButton8;
    public Button BonusLevelUpButton9;
    public Button BonusLevelUpButton10;

    // 보너스 레밸업 관려 전역 변수
    float tempTimeScale = 0f;

    // 디버그 버튼 컴포넌트
    public Button[] debugButtonArr;
    public Button debugGsmeSpeedMinusButton;
    public Button debugGsmeSpeedButton;
    public Button debugWeaponAddButton;
    public Button debugAttackPowerUpButton;
    public Button debugWeaponAttackSpeedUpButton;
    public Button debugWeaponRangeUpButton;
    public Button debugTowerAttackSpeedUpButton;
    public Button debugTowerRangeUpButton;
    public Button debugPlayerHPUpButton;
    public Button debugPlayerSpeedUpButton;

    // 레밸업 게이지 효과
    private RectTransform backgroundPanel; // 게이지 바 배경
    private RectTransform experienceBar; // 게이지 바

    // 레밸업 텍스트효과 (나타났다 사라지기)
    private float fadeDuration = 2f; // 텍스트가 서서히 사라지는 시간
    private float displayDuration = 2f; // 텍스트가 표시되는 시간
    private Coroutine fadeOutCoroutine; // 현재 실행 중인 페이드 아웃 코루틴을 추적

    // 참조용 스트링 Arr
    private string[] specTextDir = { "EXP/LVText", "MENU/UI/StatGroup/PlayerSpec", "MENU/UI/StatGroup/WeaponSpec", "MENU/UI/StatGroup/TowerSpec" };
    private string levelUpHintDir = "LevelUPText";
    private string bonusLevelupMsterDir = "LevelUpBonus";
    private string[] bonusLevelupDir = { "LevelUpBonus/PowerUp", "LevelUpBonus/AttackSpeedUp", "LevelUpBonus/RangeUp", "LevelUpBonus/HPRecover", "LevelUpBonus/SpeedUp",
        "LevelUpBonus/PenetrationUp","LevelUpBonus/ProjectileUp","LevelUpBonus/HPAutoRecover","LevelUpBonus/CoinDropUp","LevelUpBonus/HiddenTower"};
    private string[] debugBtnDir = { "MENU/DebugBTN/겜속도--", "MENU/DebugBTN/겜속도++", "MENU/마스터웨폰+", "MENU/마스터공업", "MENU/무기공속업", "MENU/무기레인지업", "MENU/타워공속업",
        "MENU/타워레인지업", "MENU/피뻥", "MENU/헤이스트"};
    private string[] expDir = { "EXP/BaseBar", "EXP/BaseBar/RealBar" };

    // 보너스 버튼 등장 가중치
    private List<Vector2Int> bonusAllPairs;
    Dictionary<int, float> bonusAppearanceWeight;

    private void Awake()
    {
        HUDObjectSet();
    }

    private void Start()
    {
        InitializeButtons(); // 버튼 초기화
        GenerateAllPairs();  //보너스 능력 배열을 미리 세팅

        // 각 숫자에 대한 포함 확률을 설정합니다.
        bonusAppearanceWeight = new Dictionary<int, float>()
        {
            { 0, 18 },    // 0은 포함될 확률이 x%
            { 1, 18 },    // 1은 포함될 확률이 y%
            { 2, 18 },    
            { 3, 18 },    
            { 4, 18 },    
            { 5, 2 },     
            { 6, 2 },     
            { 7, 2 },     
            { 8, 2 },     
            { 9, 2 },     
        };

        // 무작위 쌍을 100,000번 생성하여 실제 확률과 일치하는지 테스트.
        //TestProbabilities(100000);
    }

    public void InitializeButtons()
    {
        DebugButtonInit();
        BonusButtonInit();
    }

    private void HUDObjectSet()
    {
        // 경험지 영역
        backgroundPanel = FindChild<RectTransform>(transform, expDir[0]);
        experienceBar = FindChild<RectTransform>(transform, expDir[1]);

        // 스텟 영역
        lvText = FindChild<Text>(transform, specTextDir[0]);
        playerSpecText = FindChild<Text>(transform, specTextDir[1]);
        WeaponSpecText = FindChild<Text>(transform, specTextDir[2]);
        TowerSpecText = FindChild<Text>(transform, specTextDir[3]);
        levelupHintText = FindChild<Text>(transform, levelUpHintDir);

        // 보너스 레밸업 영역
        Transform bonusLevelUpTransform = transform.Find(bonusLevelupMsterDir);
        if (bonusLevelUpTransform != null)
        {
            BonusLevelUpButtonGroup = bonusLevelUpTransform.gameObject;
        }
        else
        {
            Debug.LogError("Canvas/LevelUpBonus 오브젝트를 찾을 수 없습니다.");
        }
    }

    public void BonusLevelUp()
    {
        // 0. 게임 스탑
        tempTimeScale = Time.timeScale;
        Time.timeScale = 0;

        // 1. 버튼 이미지를 보이게한다.
        BonusLevelUpButtonGroup.SetActive(true);

        // 2. 하위 버튼을 모두 가림
        for (int i = 0; i < BonusLevelUpButtonArr.Length; i++)
        {
            BonusLevelUpButtonArr[i].gameObject.SetActive(false);
        }

        // 3. 랜덤한 X(temp : 2)개는 선택 및 보이게
        Vector2Int pair = GetRandomPair(bonusAppearanceWeight);

        for (int i = 0; i < BonusLevelUpButtonArr.Length; i++)
        {
            if (i == pair.x || i == pair.y)
            {
                BonusLevelUpButtonArr[i].gameObject.SetActive(true);
            }
        }
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
            Vector2Int pair = GetRandomPair(bonusAppearanceWeight);
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

    // 특정 숫자가 선택된 경우, 해당 숫자를 포함하는 모든 쌍을 제외합니다.
    public void ExcludePairsContaining(int number)
    {
        bonusAllPairs.RemoveAll(pair => pair.x == number || pair.y == number);
    }

    // 모든 가능한 쌍을 생성합니다.
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

    public Vector2Int GetRandomPair(Dictionary<int, float> probabilities)
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

            if (probabilities.ContainsKey(pair.x))
                weight *= probabilities[pair.x];

            if (probabilities.ContainsKey(pair.y))
                weight *= probabilities[pair.y];

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

    void DebugButtonInit()
    {
        debugButtonArr = new Button[]
        {
            debugGsmeSpeedMinusButton,
            debugGsmeSpeedButton,
            debugWeaponAddButton,
            debugAttackPowerUpButton,
            debugWeaponAttackSpeedUpButton,
            debugWeaponRangeUpButton,
            debugTowerAttackSpeedUpButton,
            debugTowerRangeUpButton,
            debugPlayerHPUpButton,
            debugPlayerSpeedUpButton
        };

        Action[] debugActions = new Action[]
        {
            GameManager.Instance.DebugBtnTimeMinus,
            GameManager.Instance.DebugBtnTimePlus,
            GameManager.Instance.DebugWeaponMaster,
            GameManager.Instance.DebugWeaponAtaackPowerUp,
            GameManager.Instance.DebugWeaponAtaackSpeedUp,
            GameManager.Instance.DebugWeaponRangeUp,
            GameManager.Instance.DebugTowerAtaackSpeedUp,
            GameManager.Instance.DebugTowerRangeUp,
            GameManager.Instance.DebugPlayerHPUp,
            GameManager.Instance.DebugPlayerSpeedUp
        };

        InitializeButtons(debugButtonArr, debugBtnDir, debugActions, "Debug");
    }

    void BonusButtonInit()
    {
        BonusLevelUpButtonArr = new Button[]
        {
            BonusLevelUpButton1,
            BonusLevelUpButton2,
            BonusLevelUpButton3,
            BonusLevelUpButton4,
            BonusLevelUpButton5,
            BonusLevelUpButton6,
            BonusLevelUpButton7,
            BonusLevelUpButton8,
            BonusLevelUpButton9,
            BonusLevelUpButton10,
        };

        Action[] bonusActions = new Action[]
        {
            BonusPowerUp,
            BonusAttackSpeedUp,
            BonusRangeUp,
            BonusHPUp,
            BonusMoveSpeedUp,
            BonusPenetraionUp,
            BonusProjectileUp,
            BonusHPAutoRecover,
            BonusCoinDropUp,
            BonusHiddenTower,
        };

        InitializeButtons(BonusLevelUpButtonArr, bonusLevelupDir, bonusActions, "Bonus");
    }


    void InitializeButtons(Button[] buttonArr, string[] buttonDir, Action[] actions, string errorMessagePrefix)
    {
        // 버튼과 액션의 길이가 일치하는지 확인
        if (buttonArr.Length != actions.Length)
        {
            Debug.LogError($"{errorMessagePrefix} Button array and action array lengths do not match.");
            return;
        }

        // 버튼 배열 초기화
        for (int i = 0; i < buttonDir.Length; i++)
        {
            buttonArr[i] = FindChild<Button>(transform, buttonDir[i]);
        }

        // 버튼 초기화 및 액션 할당
        for (int i = 0; i < buttonArr.Length; i++)
        {
            if (buttonArr[i] != null)
            {
                int index = i; // 인덱스를 로컬 변수에 저장하여 올바르게 참조
                buttonArr[index].onClick.RemoveAllListeners();
                buttonArr[index].onClick.AddListener(() => actions[index]());
            }
            else
            {
                Debug.LogError($"{errorMessagePrefix} {buttonDir[i]}에 해당하는 버튼이 할당되지 않았습니다.");
            }
        }
    }

    void ApplyBonus(Action levelUpAction)
    {
        // 레벨업 기능 적용
        levelUpAction();

        // 모든 보너스 버튼 비활성화
        foreach (var button in BonusLevelUpButtonArr)
        {
            button.gameObject.SetActive(false);
        }

        // 시간 스케일을 원래대로 복원
        Time.timeScale = tempTimeScale;
    }


    void BonusPowerUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.WeaponAttackPowerUp(2));
    }

    void BonusAttackSpeedUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.WeaponAttackSpeedUp(0.98f));
    }

    void BonusRangeUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.WeaponRangedUp(0.2f));
    }

    void BonusHPUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.PlayerHPUp(GameManager.Instance.player, 0));
    }

    void BonusMoveSpeedUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.PlayerSpeedUp(GameManager.Instance.player, 0.2f));
    }

    // TODO : 5~9는 ENUM 관리

    void BonusPenetraionUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.PenetrationUp());
        ExcludePairsContaining(5);
    }

    void BonusProjectileUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.ProjectileUp());
        ExcludePairsContaining(6);
    }

    void BonusHPAutoRecover()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.HPAutoRecover());
        ExcludePairsContaining(7);
    }

    void BonusCoinDropUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.CoinDropUp());
        ExcludePairsContaining(8);
    }

    void BonusHiddenTower()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.HiddenTowerSpawn());
        ExcludePairsContaining(9);
    }

    // 현재 경험치와 최대 경험치를 사용해 게이지 바를 업데이트하는 함수
    public void UpdateExperienceBar(float currentExperience, float maxExperience)
    {
        if (backgroundPanel == null || experienceBar == null)
        {
            Debug.LogError("ExperienceBar: 배경 패널 또는 게이지 바가 설정되지 않았습니다.");
            return;
        }

        // 배경 패널의 너비를 기준으로 채워질 너비를 계산합니다.
        float maxWidth = backgroundPanel.rect.width;
        float width = (currentExperience / maxExperience) * maxWidth;

        // 게이지 바의 너비를 조절합니다.
        experienceBar.sizeDelta = new Vector2(width, experienceBar.sizeDelta.y);
    }

    public void PlayerHUDUpdate(int lv, int curExp, int maxExp, int currentHP, float moveSpeed)
    {
        lv += 1; //배열 관리상 +1
        lvText.text = $"LV {lv} EXP {curExp}/{maxExp}";
        playerSpecText.text = $"체력/이동속도 : {currentHP}/{moveSpeed}";
        UpdateExperienceBar(curExp, maxExp);
    }

    public void WeaponHUDUpdate(int attackPower, float attackRange, float attackSpeed)
    {
        // attackRange를 소수점 두 자리까지 포맷팅
        string formattedAttackSpeed = attackSpeed.ToString("F2");

        WeaponSpecText.text = $"무기 AP/AS/R : +{attackPower} / x{formattedAttackSpeed} / +{attackRange}";
    }

    public void TowerHUDUpdate(int attackPower, float attackRange, float attackSpeed)
    {
        // attackRange를 소수점 두 자리까지 포맷팅
        string formattedAttackSpeed = attackSpeed.ToString("F2");

        TowerSpecText.text = $"타워 AP/AS/R : +{attackPower} / x{formattedAttackSpeed} / +{attackRange}";
    }

    public void LevelUpHintUpdate(string msg)
    {
        // 기존 페이드 아웃 코루틴이 실행 중이라면 중지
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            ResetTextAlpha(); // 텍스트를 완전히 투명하게 설정
        }

        // 텍스트를 빨간색과 볼드체로 설정
        levelupHintText.text = $"{msg}";
        // 초기 색상을 완전히 불투명하게 설정
        levelupHintText.color = new Color(1f, 0f, 0f, 1f);

        // 페이드 아웃 코루틴 시작
        fadeOutCoroutine = StartCoroutine(FadeOutText());
    }

    private void ResetTextAlpha()
    {
        // 텍스트의 알파 값을 0으로 설정하여 즉시 투명하게 만듭니다.
        levelupHintText.color = new Color(levelupHintText.color.r, levelupHintText.color.g, levelupHintText.color.b, 0f);
    }

    private IEnumerator FadeOutText()
    {
        // 텍스트 표시 후 대기
        yield return new WaitForSeconds(displayDuration);

        // 초기 색상을 가져온다
        Color originalColor = levelupHintText.color;
        float elapsedTime = 0f;

        // 페이드 아웃 진행
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            levelupHintText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 완전히 투명하게 설정
        levelupHintText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // 코루틴이 끝났음을 표시하기 위해 null로 설정
        fadeOutCoroutine = null;
    }

    // 재귀적 찾기 유틸리티 함수
    T FindChild<T>(Transform parent, string path) where T : Component
    {
        Transform child = parent.Find(path);
        if (child != null)
        {
            T component = child.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
            else
            {
                Debug.LogWarning($"경고: '{path}' 경로에서 '{typeof(T)}' 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"경고: '{path}' 경로에서 자식 오브젝트를 찾을 수 없습니다.");
        }
        return null;
    }
}

