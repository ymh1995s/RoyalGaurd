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
    }

    void Start()
    {
        Time.timeScale += 0.5f; //제출용 스겜 (1.5배속)
    }

    void Update()
    {
        UpdateTimer(); // 매 프레임 시간 업데이트
    }

    // OnEnxable : MonoBehavior가 활성화될 때 호출
    void OnEnable()
    {
        // 해당 씬(게임매니저가 있는 씬) 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // OnDisable : MonoBehavior가 비활성화될 때 호출
    void OnDisable()
    {
        // 해당 씬(게임매니저가 있는 씬) 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 게임매니저에서 관리하는 오브젝트 찾기
        FindPlayer();

        startTime = Time.time; // 씬 시작 시점을 기록

        // HUDManager를 동적으로 찾음
        hudManager = FindObjectOfType<HUDManager>();

        if (hudManager != null)
        {
            // HUDManager의 버튼 초기화 메서드 호출
            Debug.Log("hudManager를 찾았습니다.버튼 (재)설정");
            hudManager.InitializeButtons();
        }
        else
        {
            Debug.LogError("HUDManager를 찾을 수 없습니다.");
        }

        ResetStaticParameter();
    }

    private void ResetStaticParameter()
    {
        //TODO : Static 미사용

        // BaseTower
        BaseTower.fireRateMmul = 1.0f; // 공격 속도 업그레이드 곱셈 적용
        BaseTower.detectionRadius = 10f;  // 타워의 탐지 반경
        BaseTower.detectionRadiusPlus = 1.0f;  // 타워의 투가 탐지 반경

        // BasePlayer
        BasePlayer.moveSpeed = 2.0f;
        BasePlayer.maxHP = 20;
        BasePlayer.currentHP = 20;

        // Projectile
        BaseProjectile.attackPowerUp = 0;

        // BaseWeapon
        BaseWeapon.fireRateMmul = 1.0f;
        BaseWeapon.detectionRadiusPlus = 0f;  // 무기의 탐지 반경
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
