using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 추가

public class HUDManager : MonoBehaviour
{
    // TODO : 컴포넌트 배열 등으로 그룹 관리하기
    // TODO : 디버그 그룹하고 스크립트 분리해야 할 듯
    // TODO : 더해서 여기는 너무 막해서 코드 정리 필요

    //스텟 영역
    private Text lvText;
    private Text playerSpecText;
    private Text WeaponSpecText;
    private Text TowerSpecText;
    private Text levelupHintText;

    // 보너스 레밸업 버튼 컴포넌트
    public GameObject BonusLevelUpButtonGroup;
    public Button[] BonusLevelUpButtonArr;
    // TODO 숫자 하드 코딩 말고 네이밍으로 바꿔야됨
    // 아닌가 나중에 다른 능력으로 바꿀거라 임시로는 괜찮은가
    public Button BunusLevelUpButton1;
    public Button BunusLevelUpButton2;
    public Button BunusLevelUpButton3;
    public Button BunusLevelUpButton4;
    public Button BunusLevelUpButton5;
    public Button BunusLevelUpButton6;

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

    //레밸업 텍스트효과 (나타났다 사라지기)
    private float fadeDuration = 2f; // 텍스트가 서서히 사라지는 시간
    private float displayDuration = 2f; // 텍스트가 표시되는 시간
    private Coroutine fadeOutCoroutine; // 현재 실행 중인 페이드 아웃 코루틴을 추적

    // 참조용 스트링 Arr
    private string[] specTextDir = { "EXP/LVText", "MENU/UI/StatGroup/PlayerSpec", "MENU/UI/StatGroup/WeaponSpec", "MENU/UI/StatGroup/TowerSpec" };
    private string levelUpHintDir = "LevelUPText";
    private string bonusLevelupMsterDir = "LevelUpBonus";
    private string[] bonusLevelupDir = { "LevelUpBonus/PowerUp", "LevelUpBonus/AttackSpeedUp", "LevelUpBonus/RangeUp", "LevelUpBonus/HPRecover", "LevelUpBonus/SpeedUp", "LevelUpBonus/TODO" };
    private string[] debugBtnDir = { "MENU/DebugBTN/겜속도--", "MENU/DebugBTN/겜속도++", "MENU/마스터웨폰+", "MENU/마스터공업", "MENU/무기공속업", "MENU/무기레인지업", "MENU/타워공속업",
        "MENU/타워레인지업", "MENU/피뻥", "MENU/헤이스트"};
    private string[] expDir = { "EXP/BaseBar", "EXP/BaseBar/RealBar" };//나중에 변수명 지어주기

    private void Awake()
    {
        TextObjectSet();
    }

    private void Start()
    {
        InitializeButtons(); // 버튼 초기화
    }

    public void InitializeButtons()
    {
        DebugButtonInit();
        BonusButtonInit();
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

        //나중에 옮기기
        // GameObject를 찾기 위해 Transform.Find를 사용합니다
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
        for(int i = 0; i< BonusLevelUpButtonArr.Length;i++)
        {
            BonusLevelUpButtonArr[i].gameObject.SetActive(false);
        }

        // 3. 랜덤한 X(temp : 2)개는 선택 및 보이게
        Vector2Int pair = GetRandomPair();
        
        for(int i=0; i< BonusLevelUpButtonArr.Length; i++)
        {
            if( i == pair.x || i==pair.y)
            {
                BonusLevelUpButtonArr[i].gameObject.SetActive(true);
            }
        }

        // TEMP : 마지막 임시 설명은 무조건 있게
        BonusLevelUpButtonArr[BonusLevelUpButtonArr.Length-1].gameObject.SetActive(true);
    }

    public static Vector2Int GetRandomPair()
    {
        // 가능한 모든 쌍을 생성합니다.
        List<Vector2Int> pairs = new List<Vector2Int>();

        for (int i = 0; i <= 4; i++)
        {
            for (int j = i + 1; j <= 4; j++)
            {
                pairs.Add(new Vector2Int(i, j));
            }
        }

        // 무작위로 하나의 쌍을 선택합니다.
        int index = UnityEngine.Random.Range(0, pairs.Count); // UnityEngine.Random을 사용하여 무작위 인덱스 선택
        return pairs[index];
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

    void DebugButtonInit()
    {
        // 배열로 관리하기 위해 연결
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

        // GameManager의 디버그 메서드를 배열로 관리
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

        // 버튼과 액션의 길이가 일치하는지 확인
        if (debugButtonArr.Length != debugActions.Length)
        {
            Debug.LogError("Button array and action array lengths do not match.");
            return;
        }

        for (int i = 0; i < debugBtnDir.Length; i++)
        {
            debugButtonArr[i] = FindChild<Button>(transform, debugBtnDir[i]);
        }

        // 디버그 버튼 초기화 및 재연결
        for (int i = 0; i < debugButtonArr.Length; i++)
        {
            if (debugButtonArr[i] != null)
            {
                int index = i; // 인덱스를 로컬 변수에 저장하여 올바르게 참조
                debugButtonArr[index].onClick.RemoveAllListeners();
                debugButtonArr[index].onClick.AddListener(() => debugActions[index]());
            }
            else
            {
                Debug.LogError($"{debugBtnDir[i]}에 해당하는 버튼이 할당되지 않았습니다.");
            }
        }
    }

    void BonusButtonInit()
    {
        BonusLevelUpButtonArr = new Button[]
        {
            BunusLevelUpButton1,
            BunusLevelUpButton2,
            BunusLevelUpButton3,
            BunusLevelUpButton4,
            BunusLevelUpButton5,
            BunusLevelUpButton6,
        };

        Action[] bonusActions = new Action[]
        {
            BonusPowerUp,
            BonusAttackSpeedUp,
            BonusRangeUp,
            BonusHPUp,
            BonusMoveSpeedUp,
            BonusNothing,
        };

        // 버튼과 액션의 길이가 일치하는지 확인
        if (BonusLevelUpButtonArr.Length != bonusActions.Length)
        {
            Debug.LogError("Button array and action array lengths do not match.");
            return;
        }

        for (int i = 0; i < bonusLevelupDir.Length; i++)
        {
            BonusLevelUpButtonArr[i] = FindChild<Button>(transform, bonusLevelupDir[i]);
        }

        // 보너스 버튼 초기화 및 재연결 
        for (int i = 0; i < BonusLevelUpButtonArr.Length; i++)
        {
            if (BonusLevelUpButtonArr[i] != null)
            {
                int index = i; // 인덱스를 로컬 변수에 저장하여 올바르게 참조
                BonusLevelUpButtonArr[index].onClick.RemoveAllListeners();
                BonusLevelUpButtonArr[index].onClick.AddListener(() => bonusActions[index]());
            }
            else
            {
                Debug.LogError($"{BonusLevelUpButtonArr[i].name}이 할당되지 않았습니다.");
            }
        }
    }

    void BonusPowerUp()
    {
        LevelUpHelper.WeaponAttackPowerUp(1);

        for (int i = 0; i < BonusLevelUpButtonArr.Length; i++)
        {
            BonusLevelUpButtonArr[i].gameObject.SetActive(false);
        }

        Time.timeScale = tempTimeScale;
    }

    void BonusAttackSpeedUp()
    {
        LevelUpHelper.WeaponAttackSpeedUp(0.98f);
        for (int i = 0; i < BonusLevelUpButtonArr.Length; i++)
        {
            BonusLevelUpButtonArr[i].gameObject.SetActive(false);
        }

        Time.timeScale = tempTimeScale;
    }

    void BonusRangeUp()
    {
        LevelUpHelper.WeaponRangedUp(0.1f);
        for (int i = 0; i < BonusLevelUpButtonArr.Length; i++)
        {
            BonusLevelUpButtonArr[i].gameObject.SetActive(false);
        }

        Time.timeScale = tempTimeScale;
    }

    void BonusHPUp()
    {
        LevelUpHelper.PlayerHPUp(0);
        for (int i = 0; i < BonusLevelUpButtonArr.Length; i++)
        {
            BonusLevelUpButtonArr[i].gameObject.SetActive(false);
        }

        Time.timeScale = tempTimeScale;
    }

    void BonusMoveSpeedUp()
    {
        LevelUpHelper.PlayerSpeedUp(0.1f);
        for (int i = 0; i < BonusLevelUpButtonArr.Length; i++)
        {
            BonusLevelUpButtonArr[i].gameObject.SetActive(false);
        }

        Time.timeScale = tempTimeScale;
    }

    void BonusNothing()
    {

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
}