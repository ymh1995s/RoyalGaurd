using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float startTime; // 씬이 시작된 시간

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


        startTime = Time.time; // 씬 시작 시점을 기록
        Gatcha.GameStart();

        // HUDManager를 동적으로 찾음
        hudManager = FindObjectOfType<HUDManager>();

        if (hudManager == null)
        {
            Debug.LogError("HUDManager를 찾을 수 없습니다.");
            return;
        }
    }



    void Start()
    {
        Time.timeScale += 0.5f; //제출용 스겜 (1.5배속)
    }

    void Update()
    {
        UpdateTimer(); // 매 프레임 시간 업데이트
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
        float elapsedTime = Time.time - startTime; // 경과 시간 계산
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
