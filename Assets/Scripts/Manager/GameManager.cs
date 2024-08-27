using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// TODO : ��� ��ũ��Ʈ���� public ���� ����
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float startTime; // ���� ���۵� �ð�

    public HUDManager hudManager;
    public BasePlayer player;

    void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        Time.timeScale += 0.25f;
    }

    void Update()
    {
        UpdateTimer(); // �� ������ �ð� ������Ʈ
    }

    // OnEnable : MonoBehavior�� Ȱ��ȭ�� �� ȣ��
    void OnEnable()
    {
        // �ش� ��(���ӸŴ����� �ִ� ��) �̺�Ʈ ����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // OnDisable : MonoBehavior�� ��Ȱ��ȭ�� �� ȣ��
    void OnDisable()
    {
        // �ش� ��(���ӸŴ����� �ִ� ��) �̺�Ʈ ���� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���ӸŴ������� �����ϴ� ������Ʈ ã��
        FindPlayer();

        startTime = Time.time; // �� ���� ������ ���

        // HUDManager�� �������� ã��
        hudManager = FindObjectOfType<HUDManager>();


        // TODO �Ź� ��µǴ� �����޽��� ���� ���� ���� �ȳ��� ó��
        if (hudManager != null)
        {
            // HUDManager�� ��ư �ʱ�ȭ �޼��� ȣ��
            hudManager.InitializeButtons();
        }
        else
        {
            Debug.LogError("HUDManager�� ã�� �� �����ϴ�.");
        }

        ResetStaticParameter();
    }

    void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ResetStaticParameter()
    {
        //TODO : Static �̻��

        // BaseTower
        BaseTower.fireRateMmul = 1.0f; // ���� �ӵ� ���׷��̵� ���� ����
        BaseTower.detectionRadius = 10f;  // Ÿ���� Ž�� �ݰ�
        BaseTower.detectionRadiusPlus = 1.0f;  // Ÿ���� ���� Ž�� �ݰ�

        // BasePlayer
        //BasePlayer.moveSpeed = 2.0f;
        //BasePlayer.maxHP = 20;
        //BasePlayer.currentHP = 20;

        // Projectile
        BaseProjectile.attackPowerUp = 0;
        BaseProjectile.maxPenetration = 1;

        // BaseWeapon
        BaseWeapon.fireRateMmul = 1.0f;
        BaseWeapon.detectionRadiusPlus = 0f;  // ������ Ž�� �ݰ�
        BaseWeapon.fireMultiple = 1; // 1ȸ �߻� �� ����ü

        // Monster
        BaseMonster.coinClassRangeCut = new int[3] { 70, 97, 100 };
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<BasePlayer>();
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }

    public void DebugBtnTimePlus()
    {
        Time.timeScale += 0.5f;
    }

    public void DebugBtnTimeMinus()
    {
        Time.timeScale -= 0.5f;
    }

    void UpdateTimer()
    {
        float elapsedTime = Time.time - startTime; // ��� �ð� ���
    }

    public void DebugWeaponMaster()
    {
        player.Debug_WeaponAdd();
    }

    public void DebugWeaponAtaackPowerUp()
    {
        //LevelUpHelper.WeaponAttackPowerUp();
        player.levelUpHelper.WeaponAttackPowerUp();
    }

    public void DebugWeaponAtaackSpeedUp()
    {
        // LevelUpHelper.WeaponAttackSpeedUp();
        player.levelUpHelper.WeaponAttackSpeedUp();
    }

    public void DebugWeaponRangeUp()
    {
        // LevelUpHelper.WeaponRangedUp();
        player.levelUpHelper.WeaponRangedUp();
    }

    public void DebugTowerAtaackSpeedUp()
    {
        // LevelUpHelper.TowerAttackSpeedUp();
        player.levelUpHelper.TowerAttackSpeedUp();
    }

    public void DebugTowerRangeUp()
    {
        // LevelUpHelper.TowerRangeUp();
        player.levelUpHelper.TowerRangeUp();
    }

    public void DebugPlayerHPUp()
    {
        // LevelUpHelper.PlayerHPUp();
        player.levelUpHelper.PlayerHPUp(player);
    }

    public void DebugPlayerSpeedUp()
    {
        // LevelUpHelper.PlayerSpeedUp();
        player.levelUpHelper.PlayerSpeedUp(player);
    }
}
