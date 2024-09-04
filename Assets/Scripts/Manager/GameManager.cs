using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// TODO : 모든 스크립트에서 public 참조 자제
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private float startTime; // 씬이 시작된 시간
    private const float defaultTimeScale = 1.5f; // 게임속도 기본 1.5배


    public HUDManager hudManager;
    public BasePlayer player;


    void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        Time.timeScale = defaultTimeScale;
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
        startTime = Time.time; // 씬 시작 시점을 기록

        FindPlayer(); // 플레이어 찾아서 할당
        hudManager = FindObjectOfType<HUDManager>(); // HUDManager 할당

        ResetStaticParameter();
    }

    void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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
        player.Debug_WeaponAdd(2);
    }

    public void DebugWeaponAtaackPowerUp()
    {
        player.levelUpHelper.WeaponAttackPowerUp();
    }

    public void DebugWeaponAtaackSpeedUp()
    {
        player.levelUpHelper.WeaponAttackSpeedUp();
    }

    public void DebugWeaponRangeUp()
    {
        player.levelUpHelper.WeaponRangedUp();
    }

    public void DebugTowerAtaackSpeedUp()
    {
        player.levelUpHelper.TowerAttackSpeedUp();
    }

    public void DebugTowerRangeUp()
    {
        player.levelUpHelper.TowerRangeUp();
    }

    public void DebugPlayerHPUp()
    {
        player.levelUpHelper.PlayerHPUp(player);
    }

    public void DebugPlayerSpeedUp()
    {
        player.levelUpHelper.PlayerSpeedUp(player);
    }

    public void DebugBonus()
    {
        hudManager.BonusLevelUp();
    }

    public void DebugWeapon2()
    {
        player.Debug_WeaponAdd(1);
    }

    public void GameClear()
    {
        SceneLoader.SceneLoad_ClearScene();
    }
}
