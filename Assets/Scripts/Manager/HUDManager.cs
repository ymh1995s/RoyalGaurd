using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 추가

public class HUDManager : MonoBehaviour
{
    //스텟 영역
    private Text lvText;
    private Text playerSpecText;
    private Text WeaponSpecText;
    private Text TowerSpecText;
    private Text levelupHintText;

    // 버튼 컴포넌트
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

    //레밸업 텍스트효과 (나타났다 사라지기)
    private float fadeDuration = 2f; // 텍스트가 서서히 사라지는 시간
    private float displayDuration = 2f; // 텍스트가 표시되는 시간
    private Coroutine fadeOutCoroutine; // 현재 실행 중인 페이드 아웃 코루틴을 추적

    // 참조용 스트링 Arr
    protected string[] specTextDir = { "EXP/LVText", "MENU/UI/StatGroup/PlayerSpec", "MENU/UI/StatGroup/WeaponSpec", "MENU/UI/StatGroup/TowerSpec" };
    protected string levelUpHintDir = "LevelUPText";
    protected string[] debugBtnDir = { "MENU/DebugBTN/겜속도--", "MENU/DebugBTN/겜속도++", "MENU/마스터웨폰+", "MENU/마스터공업", "MENU/무기공속업", "MENU/무기레인지업", "MENU/타워공속업",
        "MENU/타워레인지업", "MENU/피뻥", "MENU/헤이스트"};
    protected string[] expDir = {"EXP/BaseBar", "EXP/BaseBar/RealBar" };//나중에 변수명 지어주기

    private void Awake()
    {
        TextObjectSet();
        InitializeButtons(); // 버튼 초기화
    }

    private void Start()
    {
        InitializeButtons(); // 버튼 초기화??이건가?
    }

    private void TextObjectSet()
    {
        // 경험지 영역
        backgroundPanel = FindChild<RectTransform>(transform, expDir[0]);
        experienceBar = FindChild<RectTransform>(transform, expDir[1]);

        //스텟 영역
        lvText = FindChild<Text>(transform, specTextDir[0]);
        playerSpecText = FindChild<Text>(transform, specTextDir[1]);
        WeaponSpecText = FindChild<Text>(transform, specTextDir[2]);
        TowerSpecText = FindChild<Text>(transform, specTextDir[3]);
        levelupHintText = FindChild<Text>(transform, levelUpHintDir);
    }

    public void InitializeButtons()
    {
        debugGsmeSpeedMinusButton = FindChild<Button>(transform, debugBtnDir[0]);
        debugGsmeSpeedButton = FindChild<Button>(transform, debugBtnDir[1]);
        debugWeaponAddButton = FindChild<Button>(transform, debugBtnDir[2]);
        debugAttackPowerUpButton = FindChild<Button>(transform, debugBtnDir[3]);
        debugWeaponAttackSpeedUpButton = FindChild<Button>(transform, debugBtnDir[4]);
        debugWeaponRangeUpButton = FindChild<Button>(transform, debugBtnDir[5]);
        debugTowerAttackSpeedUpButton = FindChild<Button>(transform, debugBtnDir[6]);
        debugTowerRangeUpButton = FindChild<Button>(transform, debugBtnDir[7]);
        debugPlayerHPUpButton = FindChild<Button>(transform, debugBtnDir[8]);
        debugPlayerSpeedUpButton = FindChild<Button>(transform, debugBtnDir[9]);

        // 버튼 초기화
        if (debugGsmeSpeedMinusButton != null)
        {
            debugGsmeSpeedMinusButton.onClick.RemoveAllListeners();
            debugGsmeSpeedMinusButton.onClick.AddListener(() => GameManager.Instance.DebugBtnTimeMinus());
        }
        else
        {
            Debug.LogError("debugGsmeSpeedMinusButton이 할당되지 않았습니다.");
        }

        if (debugGsmeSpeedButton != null)
        {
            debugGsmeSpeedButton.onClick.RemoveAllListeners();
            debugGsmeSpeedButton.onClick.AddListener(() => GameManager.Instance.DebugBtnTimePlus());
        }
        else
        {
            Debug.LogError("debugGsmeSpeedButton이 할당되지 않았습니다.");
        }

        if (debugWeaponAddButton != null)
        {
            debugWeaponAddButton.onClick.RemoveAllListeners();
            debugWeaponAddButton.onClick.AddListener(() => GameManager.Instance.DebugWeaponMaster());
        }
        else
        {
            Debug.LogError("debugWeaponAddButton이 할당되지 않았습니다.");
        }

        if (debugAttackPowerUpButton != null)
        {
            debugAttackPowerUpButton.onClick.RemoveAllListeners();
            debugAttackPowerUpButton.onClick.AddListener(() => GameManager.Instance.DebugWeaponAtaackPowerUp());
        }
        else
        {
            Debug.LogError("debugAttackPowerUpButton이 할당되지 않았습니다.");
        }

        if (debugWeaponAttackSpeedUpButton != null)
        {
            debugWeaponAttackSpeedUpButton.onClick.RemoveAllListeners();
            debugWeaponAttackSpeedUpButton.onClick.AddListener(() => GameManager.Instance.DebugWeaponAtaackSpeedUp());
        }
        else
        {
            Debug.LogError("debugWeaponAttackSpeedUpButton이 할당되지 않았습니다.");
        }

        if (debugWeaponRangeUpButton != null)
        {
            debugWeaponRangeUpButton.onClick.RemoveAllListeners();
            debugWeaponRangeUpButton.onClick.AddListener(() => GameManager.Instance.DebugWeaponRangeUp());
        }
        else
        {
            Debug.LogError("debugWeaponRangeUpButton이 할당되지 않았습니다.");
        }

        if (debugTowerAttackSpeedUpButton != null)
        {
            debugTowerAttackSpeedUpButton.onClick.RemoveAllListeners();
            debugTowerAttackSpeedUpButton.onClick.AddListener(() => GameManager.Instance.DebugTowerAtaackSpeedUp());
        }
        else
        {
            Debug.LogError("debugTowerAttackSpeedUpButton이 할당되지 않았습니다.");
        }

        if (debugTowerRangeUpButton != null)
        {
            debugTowerRangeUpButton.onClick.RemoveAllListeners();
            debugTowerRangeUpButton.onClick.AddListener(() => GameManager.Instance.DebugTowerRangeUp());
        }
        else
        {
            Debug.LogError("debugTowerRangeUpButton이 할당되지 않았습니다.");
        }

        if (debugPlayerHPUpButton != null)
        {
            debugPlayerHPUpButton.onClick.RemoveAllListeners();
            debugPlayerHPUpButton.onClick.AddListener(() => GameManager.Instance.DebugPlayerHPUp());
        }
        else
        {
            Debug.LogError("debugPlayerHPUpButton이 할당되지 않았습니다.");
        }

        if (debugPlayerSpeedUpButton != null)
        {
            debugPlayerSpeedUpButton.onClick.RemoveAllListeners();
            debugPlayerSpeedUpButton.onClick.AddListener(() => GameManager.Instance.DebugPlayerSpeedUp());
        }
        else
        {
            Debug.LogError("debugPlayerSpeedUpButton이 할당되지 않았습니다.");
        }
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
        lv+=1; //배열 관리상 +1
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
}