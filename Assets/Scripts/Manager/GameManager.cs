using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float startTime; // ���� ���۵� �ð�

    public HUDManager hudManager;
    public BasePlayer player;

    void Awake()
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

        FindPlayer();


        startTime = Time.time; // �� ���� ������ ���
        Gatcha.GameStart();

        // HUDManager�� �������� ã��
        hudManager = FindObjectOfType<HUDManager>();

        if (hudManager == null)
        {
            Debug.LogError("HUDManager�� ã�� �� �����ϴ�.");
            return;
        }
    }



    void Start()
    {
        Time.timeScale += 0.5f; //����� ���� (1.5���)
    }

    void Update()
    {
        UpdateTimer(); // �� ������ �ð� ������Ʈ
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
        LevelUpHelper.WeaponAttackPowerUp();
    }

    public void DebugWeaponAtaackSpeedUp()
    {
        LevelUpHelper.WeaponAttackSpeedUp();
    }

    public void DebugWeaponRangeUp()
    {
        LevelUpHelper.WeaponRangedUp();
    }

    public void DebugTowerAtaackSpeedUp()
    {
        LevelUpHelper.TowerAttackSpeedUp();
    }

    public void DebugTowerRangeUp()
    {
        LevelUpHelper.TowerRangeUp();
    }

    public void DebugPlayerHPUp()
    {
        LevelUpHelper.PlayerHPUp();
    }

    public void DebugPlayerSpeedUp()
    {
        LevelUpHelper.PlayerSpeedUp();
    }
}
