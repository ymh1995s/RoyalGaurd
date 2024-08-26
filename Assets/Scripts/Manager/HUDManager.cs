using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ������Ʈ�� ����ϱ� ���� �߰�

public class HUDManager : MonoBehaviour
{
    // TODO : ������Ʈ �迭 ������ �׷� �����ϱ�

    //���� ����
    private Text lvText;
    private Text playerSpecText;
    private Text WeaponSpecText;
    private Text TowerSpecText;
    private Text levelupHintText;

    // ���ʽ� ����� ��ư ������Ʈ
    public GameObject BonusLevelUpButtonGroup;
    public Button[] BonusLevelUpButtonArr;

    // TODO ���� �ϵ� �ڵ� ���� ���̹����� �ٲ�ߵ�
    // �ƴѰ� ���߿� �ٸ� �ɷ����� �ٲܰŶ� �ӽ÷δ� ��������
    public Button BunusLevelUpButton1;
    public Button BunusLevelUpButton2;
    public Button BunusLevelUpButton3;
    public Button BunusLevelUpButton4;
    public Button BunusLevelUpButton5;
    public Button BunusLevelUpButton6;

    // ���ʽ� ����� ���� ���� ����
    float tempTimeScale = 0f;

    // ����� ��ư ������Ʈ
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

    // ����� ������ ȿ��
    private RectTransform backgroundPanel; // ������ �� ���
    private RectTransform experienceBar; // ������ ��

    //����� �ؽ�Ʈȿ�� (��Ÿ���� �������)
    private float fadeDuration = 2f; // �ؽ�Ʈ�� ������ ������� �ð�
    private float displayDuration = 2f; // �ؽ�Ʈ�� ǥ�õǴ� �ð�
    private Coroutine fadeOutCoroutine; // ���� ���� ���� ���̵� �ƿ� �ڷ�ƾ�� ����

    // ������ ��Ʈ�� Arr
    private string[] specTextDir = { "EXP/LVText", "MENU/UI/StatGroup/PlayerSpec", "MENU/UI/StatGroup/WeaponSpec", "MENU/UI/StatGroup/TowerSpec" };
    private string levelUpHintDir = "LevelUPText";
    private string bonusLevelupMsterDir = "LevelUpBonus";
    private string[] bonusLevelupDir = { "LevelUpBonus/PowerUp", "LevelUpBonus/AttackSpeedUp", "LevelUpBonus/RangeUp", "LevelUpBonus/HPRecover", "LevelUpBonus/SpeedUp", "LevelUpBonus/TODO" };
    private string[] debugBtnDir = { "MENU/DebugBTN/�׼ӵ�--", "MENU/DebugBTN/�׼ӵ�++", "MENU/�����Ϳ���+", "MENU/�����Ͱ���", "MENU/������Ӿ�", "MENU/���ⷹ������", "MENU/Ÿ�����Ӿ�",
        "MENU/Ÿ����������", "MENU/�ǻ�", "MENU/���̽�Ʈ"};
    private string[] expDir = { "EXP/BaseBar", "EXP/BaseBar/RealBar" };//���߿� ������ �����ֱ�

    private void Awake()
    {
        HUDObjectSet();
    }

    private void Start()
    {
        InitializeButtons(); // ��ư �ʱ�ȭ
    }

    public void InitializeButtons()
    {
        DebugButtonInit();
        BonusButtonInit();
    }

    private void HUDObjectSet()
    {
        // ������ ����
        backgroundPanel = FindChild<RectTransform>(transform, expDir[0]);
        experienceBar = FindChild<RectTransform>(transform, expDir[1]);

        // ���� ����
        lvText = FindChild<Text>(transform, specTextDir[0]);
        playerSpecText = FindChild<Text>(transform, specTextDir[1]);
        WeaponSpecText = FindChild<Text>(transform, specTextDir[2]);
        TowerSpecText = FindChild<Text>(transform, specTextDir[3]);
        levelupHintText = FindChild<Text>(transform, levelUpHintDir);

        // ���ʽ� ����� ����
        Transform bonusLevelUpTransform = transform.Find(bonusLevelupMsterDir);
        if (bonusLevelUpTransform != null)
        {
            BonusLevelUpButtonGroup = bonusLevelUpTransform.gameObject;
        }
        else
        {
            Debug.LogError("Canvas/LevelUpBonus ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    public void BonusLevelUp()
    {
        // 0. ���� ��ž
        tempTimeScale = Time.timeScale;
        Time.timeScale = 0;

        // 1. ��ư �̹����� ���̰��Ѵ�.
        BonusLevelUpButtonGroup.SetActive(true);

        // 2. ���� ��ư�� ��� ����
        for (int i = 0; i < BonusLevelUpButtonArr.Length; i++)
        {
            BonusLevelUpButtonArr[i].gameObject.SetActive(false);
        }

        // 3. ������ X(temp : 2)���� ���� �� ���̰�
        Vector2Int pair = GetRandomPair();

        for (int i = 0; i < BonusLevelUpButtonArr.Length; i++)
        {
            if (i == pair.x || i == pair.y)
            {
                BonusLevelUpButtonArr[i].gameObject.SetActive(true);
            }
        }

        // TEMP : ������ �ӽ� ������ ������ �ְ�
        BonusLevelUpButtonArr[BonusLevelUpButtonArr.Length - 1].gameObject.SetActive(true);
    }

    public static Vector2Int GetRandomPair()
    {
        // ������ ��� ���� �����մϴ�.
        List<Vector2Int> pairs = new List<Vector2Int>();

        // TODO : �ϵ� �ڵ� ����
        for (int i = 0; i <= 4; i++)
        {
            for (int j = i + 1; j <= 4; j++)
            {
                pairs.Add(new Vector2Int(i, j));
            }
        }

        // �������� �ϳ��� ���� �����մϴ�.
        int index = UnityEngine.Random.Range(0, pairs.Count); // UnityEngine.Random�� ����Ͽ� ������ �ε��� ����
        return pairs[index];
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

        InitializeButtons(BonusLevelUpButtonArr, bonusLevelupDir, bonusActions, "Bonus");
    }


    void InitializeButtons(Button[] buttonArr, string[] buttonDir, Action[] actions, string errorMessagePrefix)
    {
        // ��ư�� �׼��� ���̰� ��ġ�ϴ��� Ȯ��
        if (buttonArr.Length != actions.Length)
        {
            Debug.LogError($"{errorMessagePrefix} Button array and action array lengths do not match.");
            return;
        }

        // ��ư �迭 �ʱ�ȭ
        for (int i = 0; i < buttonDir.Length; i++)
        {
            buttonArr[i] = FindChild<Button>(transform, buttonDir[i]);
        }

        // ��ư �ʱ�ȭ �� �׼� �Ҵ�
        for (int i = 0; i < buttonArr.Length; i++)
        {
            if (buttonArr[i] != null)
            {
                int index = i; // �ε����� ���� ������ �����Ͽ� �ùٸ��� ����
                buttonArr[index].onClick.RemoveAllListeners();
                buttonArr[index].onClick.AddListener(() => actions[index]());
            }
            else
            {
                Debug.LogError($"{errorMessagePrefix} {buttonDir[i]}�� �ش��ϴ� ��ư�� �Ҵ���� �ʾҽ��ϴ�.");
            }
        }
    }

    void ApplyBonus(Action levelUpAction)
    {
        // ������ ��� ����
        levelUpAction();

        // ��� ���ʽ� ��ư ��Ȱ��ȭ
        foreach (var button in BonusLevelUpButtonArr)
        {
            button.gameObject.SetActive(false);
        }

        // �ð� �������� ������� ����
        Time.timeScale = tempTimeScale;
    }


    void BonusPowerUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.WeaponAttackPowerUp(1));
        //ApplyBonus(() => LevelUpHelper.WeaponAttackPowerUp(1));

        //LevelUpHelper.WeaponAttackPowerUp(1);

        //for (int i = 0; i < BonusLevelUpButtonArr.Length; i++)
        //{
        //    BonusLevelUpButtonArr[i].gameObject.SetActive(false);
        //}

        //Time.timeScale = tempTimeScale;
    }

    void BonusAttackSpeedUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.WeaponAttackSpeedUp(0.98f));
    }

    void BonusRangeUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.WeaponRangedUp(0.1f));
    }

    void BonusHPUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.PlayerHPUp(GameManager.Instance.player, 0));
    }

    void BonusMoveSpeedUp()
    {
        ApplyBonus(() => GameManager.Instance.player.levelUpHelper.PlayerSpeedUp(GameManager.Instance.player, 0.1f));
    }

    void BonusNothing()
    {

    }

    // ���� ����ġ�� �ִ� ����ġ�� ����� ������ �ٸ� ������Ʈ�ϴ� �Լ�
    public void UpdateExperienceBar(float currentExperience, float maxExperience)
    {
        if (backgroundPanel == null || experienceBar == null)
        {
            Debug.LogError("ExperienceBar: ��� �г� �Ǵ� ������ �ٰ� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ��� �г��� �ʺ� �������� ä���� �ʺ� ����մϴ�.
        float maxWidth = backgroundPanel.rect.width;
        float width = (currentExperience / maxExperience) * maxWidth;

        // ������ ���� �ʺ� �����մϴ�.
        experienceBar.sizeDelta = new Vector2(width, experienceBar.sizeDelta.y);
    }

    public void PlayerHUDUpdate(int lv, int curExp, int maxExp, int currentHP, float moveSpeed)
    {
        lv += 1; //�迭 ������ +1
        lvText.text = $"LV {lv} EXP {curExp}/{maxExp}";
        playerSpecText.text = $"ü��/�̵��ӵ� : {currentHP}/{moveSpeed}";
        UpdateExperienceBar(curExp, maxExp);
    }

    public void WeaponHUDUpdate(int attackPower, float attackRange, float attackSpeed)
    {
        // attackRange�� �Ҽ��� �� �ڸ����� ������
        string formattedAttackSpeed = attackSpeed.ToString("F2");

        WeaponSpecText.text = $"���� AP/AS/R : +{attackPower} / x{formattedAttackSpeed} / +{attackRange}";
    }

    public void TowerHUDUpdate(int attackPower, float attackRange, float attackSpeed)
    {
        // attackRange�� �Ҽ��� �� �ڸ����� ������
        string formattedAttackSpeed = attackSpeed.ToString("F2");

        TowerSpecText.text = $"Ÿ�� AP/AS/R : +{attackPower} / x{formattedAttackSpeed} / +{attackRange}";
    }

    public void LevelUpHintUpdate(string msg)
    {
        // ���� ���̵� �ƿ� �ڷ�ƾ�� ���� ���̶�� ����
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            ResetTextAlpha(); // �ؽ�Ʈ�� ������ �����ϰ� ����
        }

        // �ؽ�Ʈ�� �������� ����ü�� ����
        levelupHintText.text = $"{msg}";
        // �ʱ� ������ ������ �������ϰ� ����
        levelupHintText.color = new Color(1f, 0f, 0f, 1f);

        // ���̵� �ƿ� �ڷ�ƾ ����
        fadeOutCoroutine = StartCoroutine(FadeOutText());
    }

    private void ResetTextAlpha()
    {
        // �ؽ�Ʈ�� ���� ���� 0���� �����Ͽ� ��� �����ϰ� ����ϴ�.
        levelupHintText.color = new Color(levelupHintText.color.r, levelupHintText.color.g, levelupHintText.color.b, 0f);
    }

    private IEnumerator FadeOutText()
    {
        // �ؽ�Ʈ ǥ�� �� ���
        yield return new WaitForSeconds(displayDuration);

        // �ʱ� ������ �����´�
        Color originalColor = levelupHintText.color;
        float elapsedTime = 0f;

        // ���̵� �ƿ� ����
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            levelupHintText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���������� ������ �����ϰ� ����
        levelupHintText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // �ڷ�ƾ�� �������� ǥ���ϱ� ���� null�� ����
        fadeOutCoroutine = null;
    }

    // ����� ã�� ��ƿ��Ƽ �Լ�
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
                Debug.LogWarning($"���: '{path}' ��ο��� '{typeof(T)}' ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning($"���: '{path}' ��ο��� �ڽ� ������Ʈ�� ã�� �� �����ϴ�.");
        }
        return null;
    }
}

