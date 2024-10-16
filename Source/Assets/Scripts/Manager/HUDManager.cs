using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video; // UI 컴포넌트를 사용하기 위해 추가

public class HUDManager : MonoBehaviour
{
    //스텟 영역
    private Text lvText;
    private Text playerSpecText;
    private Text WeaponSpecText;
    private Text TowerSpecText;
    private Text levelupHintText;

    // 보너스 레밸업 버튼 컴포넌트
    private GameObject BonusLevelUpButtonGroup;
    private Button[] BonusLevelUpButtonArr;
    private Button BonusLevelUpButton1;
    private Button BonusLevelUpButton2;
    private Button BonusLevelUpButton3;
    private Button BonusLevelUpButton4;
    private Button BonusLevelUpButton5;
    private Button BonusLevelUpButton6;
    private Button BonusLevelUpButton7;
    private Button BonusLevelUpButton8;
    private Button BonusLevelUpButton9;
    private Button BonusLevelUpButton10;

    // 디버그 버튼 컴포넌트
    private Button[] debugButtonArr;
    private Button debugGsmeSpeedMinusButton;
    private Button debugGsmeSpeedButton;
    private Button debugWeaponAddButton;
    private Button debugAttackPowerUpButton;
    private Button debugWeaponAttackSpeedUpButton;
    private Button debugWeaponRangeUpButton;
    private Button debugTowerAttackSpeedUpButton;
    private Button debugTowerRangeUpButton;
    private Button debugPlayerHPUpButton;
    private Button debugPlayerSpeedUpButton;
    private Button debugBonusButton;
    private Button debugWeapon2AddButton;
    private Button debugGameClear;

    // 내수용 재촉 비디오
    private GameObject video;
    private Coroutine choiceHurryCoroutine;

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
        "MENU/타워레인지업", "MENU/피뻥", "MENU/헤이스트", "MENU/보너스능력", "MENU/레밸2무기", "MENU/게임클리어"};
    private string[] expDir = { "EXP/BaseBar", "EXP/BaseBar/RealBar" };
    private string videoDir = "VideoPlayer";

    // 하위 스크립트
    private HUDLevelUpHelper levelUpHelper;

    private void Awake()
    {
        HUDObjectSet();
    }

    private void Start()
    {
        InitializeButtons(); // 버튼 초기화    
    }

    private void InitializeButtons()
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

        // 동영상 영역
        Transform videoTransform = transform.Find(videoDir);
        if (videoTransform != null)
        {
            video = videoTransform.gameObject;
        }
        else
        {
            Debug.LogError("Canvas/LevelUpBonus 오브젝트를 찾을 수 없습니다.");
        }

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
        tempBool = true;
        levelUpHelper.BonusLevelUp();

        // 동영상 재생
        choiceHurryCoroutine = StartCoroutine(ActivateAfterDelay(4f));
    }

    bool tempBool = false;
    IEnumerator ActivateAfterDelay(float delay)
    {
        // 지정된 시간만큼 대기
        yield return new WaitForSecondsRealtime(delay);

        if(tempBool==true)
        {
            // VideoPlayer 객체를 활성화
            video.SetActive(true);
        }
    }

    public void StopVideo()
    {
        // VideoPlayer 객체를 비활성화
        tempBool = false;
        StopCoroutine(choiceHurryCoroutine);
        video.SetActive(false);
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
            debugPlayerSpeedUpButton,
            debugBonusButton,
            debugWeapon2AddButton,
            debugGameClear,
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
            GameManager.Instance.DebugPlayerSpeedUp,
            GameManager.Instance.DebugBonus,
            GameManager.Instance.DebugWeapon2,
            GameManager.Instance.GameClear
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

        levelUpHelper = new HUDLevelUpHelper(BonusLevelUpButtonGroup, BonusLevelUpButtonArr);

        Action[] bonusActions = new Action[]
        {
            levelUpHelper.BonusPowerUp,
            levelUpHelper.BonusAttackSpeedUp,
            levelUpHelper.BonusRangeUp,
            levelUpHelper.BonusHPUp,
            levelUpHelper.BonusMoveSpeedUp,
            levelUpHelper.BonusPenetraionUp,
            levelUpHelper.BonusProjectileUp,
            levelUpHelper.BonusHPAutoRecover,
            levelUpHelper.BonusCoinDropUp,
            levelUpHelper.BonusHiddenTower,
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

        // 버튼 매칭(링크)
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

    // 현재 경험치와 최대 경험치를 사용해 게이지 바를 업데이트하는 함수
    private void UpdateExperienceBar(float currentExperience, float maxExperience)
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

