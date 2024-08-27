using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ������Ʈ�� ����ϱ� ���� �߰�

public class HUDManager : MonoBehaviour
{
    // TODO : HUD ��ũ��Ʈ�� ���ſ� ������ �ϴ� �и��� ��

    //���� ����
    private Text lvText;
    private Text playerSpecText;
    private Text WeaponSpecText;
    private Text TowerSpecText;
    private Text levelupHintText;

    // ���ʽ� ����� ��ư ������Ʈ
    // TODO ���� �ϵ� �ڵ� ���� ���̹����� �ٲ�ߵ�
    // �ƴѰ� ���߿� �ٸ� �ɷ����� �ٲܰŶ� �ӽ÷δ� ��������
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

    // ����� �ؽ�Ʈȿ�� (��Ÿ���� �������)
    private float fadeDuration = 2f; // �ؽ�Ʈ�� ������ ������� �ð�
    private float displayDuration = 2f; // �ؽ�Ʈ�� ǥ�õǴ� �ð�
    private Coroutine fadeOutCoroutine; // ���� ���� ���� ���̵� �ƿ� �ڷ�ƾ�� ����

    // ������ ��Ʈ�� Arr
    private string[] specTextDir = { "EXP/LVText", "MENU/UI/StatGroup/PlayerSpec", "MENU/UI/StatGroup/WeaponSpec", "MENU/UI/StatGroup/TowerSpec" };
    private string levelUpHintDir = "LevelUPText";
    private string bonusLevelupMsterDir = "LevelUpBonus";
    private string[] bonusLevelupDir = { "LevelUpBonus/PowerUp", "LevelUpBonus/AttackSpeedUp", "LevelUpBonus/RangeUp", "LevelUpBonus/HPRecover", "LevelUpBonus/SpeedUp",
        "LevelUpBonus/PenetrationUp","LevelUpBonus/ProjectileUp","LevelUpBonus/HPAutoRecover","LevelUpBonus/CoinDropUp","LevelUpBonus/HiddenTower"};
    private string[] debugBtnDir = { "MENU/DebugBTN/�׼ӵ�--", "MENU/DebugBTN/�׼ӵ�++", "MENU/�����Ϳ���+", "MENU/�����Ͱ���", "MENU/������Ӿ�", "MENU/���ⷹ������", "MENU/Ÿ�����Ӿ�",
        "MENU/Ÿ����������", "MENU/�ǻ�", "MENU/���̽�Ʈ"};
    private string[] expDir = { "EXP/BaseBar", "EXP/BaseBar/RealBar" };

    // ���ʽ� ��ư ���� ����ġ
    private List<Vector2Int> bonusAllPairs;
    Dictionary<int, float> bonusAppearanceWeight;

    private void Awake()
    {
        HUDObjectSet();
    }

    private void Start()
    {
        InitializeButtons(); // ��ư �ʱ�ȭ
        GenerateAllPairs();  //���ʽ� �ɷ� �迭�� �̸� ����

        // �� ���ڿ� ���� ���� Ȯ���� �����մϴ�.
        bonusAppearanceWeight = new Dictionary<int, float>()
        {
            { 0, 18 },    // 0�� ���Ե� Ȯ���� x%
            { 1, 18 },    // 1�� ���Ե� Ȯ���� y%
            { 2, 18 },    
            { 3, 18 },    
            { 4, 18 },    
            { 5, 2 },     
            { 6, 2 },     
            { 7, 2 },     
            { 8, 2 },     
            { 9, 2 },     
        };

        // ������ ���� 100,000�� �����Ͽ� ���� Ȯ���� ��ġ�ϴ��� �׽�Ʈ.
        //TestProbabilities(100000);
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

        // ���� ī��Ʈ �ʱ�ȭ
        for (int i = 0; i <= 9; i++)
        {
            numberCounts[i] = 0;
        }

        // �׽�Ʈ Ƚ����ŭ ������ ���� �����ϰ� �� ���� ���� Ƚ���� ���
        for (int i = 0; i < testCount; i++)
        {
            Vector2Int pair = GetRandomPair(bonusAppearanceWeight);
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

    // Ư�� ���ڰ� ���õ� ���, �ش� ���ڸ� �����ϴ� ��� ���� �����մϴ�.
    public void ExcludePairsContaining(int number)
    {
        bonusAllPairs.RemoveAll(pair => pair.x == number || pair.y == number);
    }

    // ��� ������ ���� �����մϴ�.
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
            return new Vector2Int(-1, -1); // ���� ó��
        }

        // �� �ֿ� ���� ����ġ�� ����մϴ�.
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

    // TODO : 5~9�� ENUM ����

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

