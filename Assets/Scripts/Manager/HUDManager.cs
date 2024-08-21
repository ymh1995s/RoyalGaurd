using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI ������Ʈ�� ����ϱ� ���� �߰�

public class HUDManager : MonoBehaviour
{
    //���� ����
    private Text lvText;
    private Text playerSpecText;
    private Text WeaponSpecText;
    private Text TowerSpecText;
    private Text levelupHintText;

    // ��ư ������Ʈ
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
    protected string[] specTextDir = { "EXP/LVText", "MENU/UI/StatGroup/PlayerSpec", "MENU/UI/StatGroup/WeaponSpec", "MENU/UI/StatGroup/TowerSpec" };
    protected string levelUpHintDir = "LevelUPText";
    protected string[] debugBtnDir = { "MENU/DebugBTN/�׼ӵ�--", "MENU/DebugBTN/�׼ӵ�++", "MENU/�����Ϳ���+", "MENU/�����Ͱ���", "MENU/������Ӿ�", "MENU/���ⷹ������", "MENU/Ÿ�����Ӿ�",
        "MENU/Ÿ����������", "MENU/�ǻ�", "MENU/���̽�Ʈ"};
    protected string[] expDir = {"EXP/BaseBar", "EXP/BaseBar/RealBar" };//���߿� ������ �����ֱ�

    private void Awake()
    {
        TextObjectSet();
        InitializeButtons(); // ��ư �ʱ�ȭ
    }

    private void Start()
    {
        InitializeButtons(); // ��ư �ʱ�ȭ??�̰ǰ�?
    }

    private void TextObjectSet()
    {
        // ������ ����
        backgroundPanel = FindChild<RectTransform>(transform, expDir[0]);
        experienceBar = FindChild<RectTransform>(transform, expDir[1]);

        //���� ����
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

        // ��ư �ʱ�ȭ
        if (debugGsmeSpeedMinusButton != null)
        {
            debugGsmeSpeedMinusButton.onClick.RemoveAllListeners();
            debugGsmeSpeedMinusButton.onClick.AddListener(() => GameManager.Instance.DebugBtnTimeMinus());
        }
        else
        {
            Debug.LogError("debugGsmeSpeedMinusButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (debugGsmeSpeedButton != null)
        {
            debugGsmeSpeedButton.onClick.RemoveAllListeners();
            debugGsmeSpeedButton.onClick.AddListener(() => GameManager.Instance.DebugBtnTimePlus());
        }
        else
        {
            Debug.LogError("debugGsmeSpeedButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (debugWeaponAddButton != null)
        {
            debugWeaponAddButton.onClick.RemoveAllListeners();
            debugWeaponAddButton.onClick.AddListener(() => GameManager.Instance.DebugWeaponMaster());
        }
        else
        {
            Debug.LogError("debugWeaponAddButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (debugAttackPowerUpButton != null)
        {
            debugAttackPowerUpButton.onClick.RemoveAllListeners();
            debugAttackPowerUpButton.onClick.AddListener(() => GameManager.Instance.DebugWeaponAtaackPowerUp());
        }
        else
        {
            Debug.LogError("debugAttackPowerUpButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (debugWeaponAttackSpeedUpButton != null)
        {
            debugWeaponAttackSpeedUpButton.onClick.RemoveAllListeners();
            debugWeaponAttackSpeedUpButton.onClick.AddListener(() => GameManager.Instance.DebugWeaponAtaackSpeedUp());
        }
        else
        {
            Debug.LogError("debugWeaponAttackSpeedUpButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (debugWeaponRangeUpButton != null)
        {
            debugWeaponRangeUpButton.onClick.RemoveAllListeners();
            debugWeaponRangeUpButton.onClick.AddListener(() => GameManager.Instance.DebugWeaponRangeUp());
        }
        else
        {
            Debug.LogError("debugWeaponRangeUpButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (debugTowerAttackSpeedUpButton != null)
        {
            debugTowerAttackSpeedUpButton.onClick.RemoveAllListeners();
            debugTowerAttackSpeedUpButton.onClick.AddListener(() => GameManager.Instance.DebugTowerAtaackSpeedUp());
        }
        else
        {
            Debug.LogError("debugTowerAttackSpeedUpButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (debugTowerRangeUpButton != null)
        {
            debugTowerRangeUpButton.onClick.RemoveAllListeners();
            debugTowerRangeUpButton.onClick.AddListener(() => GameManager.Instance.DebugTowerRangeUp());
        }
        else
        {
            Debug.LogError("debugTowerRangeUpButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (debugPlayerHPUpButton != null)
        {
            debugPlayerHPUpButton.onClick.RemoveAllListeners();
            debugPlayerHPUpButton.onClick.AddListener(() => GameManager.Instance.DebugPlayerHPUp());
        }
        else
        {
            Debug.LogError("debugPlayerHPUpButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (debugPlayerSpeedUpButton != null)
        {
            debugPlayerSpeedUpButton.onClick.RemoveAllListeners();
            debugPlayerSpeedUpButton.onClick.AddListener(() => GameManager.Instance.DebugPlayerSpeedUp());
        }
        else
        {
            Debug.LogError("debugPlayerSpeedUpButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }
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
        lv+=1; //�迭 ������ +1
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
}