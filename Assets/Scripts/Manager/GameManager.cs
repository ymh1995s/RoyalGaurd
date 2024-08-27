using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// TODO : 모든 스크립트에서 public 참조 자제
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float startTime; // 씬이 시작된 시간

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
        UpdateTimer(); // 매 프레임 시간 업데이트
    }

    // OnEnable : MonoBehavior가 활성화될 때 호출
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


        // TODO 매번 출력되는 에러메시지 씬에 따라 에러 안나게 처리
        if (hudManager != null)
        {
            // HUDManager의 버튼 초기화 메서드 호출
            hudManager.InitializeButtons();
        }
        else
        {
            Debug.LogError("HUDManager를 찾을 수 없습니다.");
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
        //TODO : Static 미사용

        // BaseTower
        BaseTower.fireRateMmul = 1.0f; // 공격 속도 업그레이드 곱셈 적용
        BaseTower.detectionRadius = 10f;  // 타워의 탐지 반경
        BaseTower.detectionRadiusPlus = 1.0f;  // 타워의 투가 탐지 반경

        // BasePlayer
        //BasePlayer.moveSpeed = 2.0f;
        //BasePlayer.maxHP = 20;
        //BasePlayer.currentHP = 20;

        // Projectile
        BaseProjectile.attackPowerUp = 0;
        BaseProjectile.maxPenetration = 1;

        // BaseWeapon
        BaseWeapon.fireRateMmul = 1.0f;
        BaseWeapon.detectionRadiusPlus = 0f;  // 무기의 탐지 반경
        BaseWeapon.fireMultiple = 1; // 1회 발사 당 투사체

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
        float elapsedTime = Time.time - startTime; // 경과 시간 계산
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
