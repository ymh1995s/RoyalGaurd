using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance; //BGMManager는 싱글톤으로 관리

    AudioSource audioSource;
    AudioClip[] Audio_BGM;
    string[] bgmDir = { "Sounds/BGM/bgm1", "Sounds/BGM/bgm2", "Sounds/BGM/bgm3", "Sounds/BGM/bgm4", "Sounds/BGM/bgm5" };

    private int currentBGMIndex = 0; // 현재 재생 중인 BGM의 인덱스

    [Range(0.0f, 1.0f)]
    float bgmVolume = 0.3f; // 기본 볼륨 설정 (0.0 ~ 1.0)

    void Awake()
    {
        SetSingleton();
    }

    void Start()
    {
        // 씬이 로드될 때 이벤트를 구독합니다.
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 초기에 첫 씬에서 BGM 재생
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void Update()
    {
        // 현재 곡이 재생 중이지 않다면 다음 곡을 재생
        if (!audioSource.isPlaying)
        {
            PlayNextBGM();
        }
    }

    void OnDisable()
    {
        // 씬 로드 이벤트 구독을 해제합니다.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //TODO 여기 추가 정립
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGM();
    }

    void SetSingleton()
    {
        // Singleton 패턴을 적용하여 오디오 매니저의 인스턴스를 유지합니다.
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            LoadAudioClips();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadAudioClips()
    {
        // BGM 개수 설정
        Audio_BGM = new AudioClip[bgmDir.Length];

        for (int i = 0; i < bgmDir.Length; i++)
        {
            Audio_BGM[i] = Resources.Load<AudioClip>(bgmDir[i]);
        }
    }

    // TODO 브금 바귈 때 순간렉 픽스하기

    void PlayBGM()
    {
        if (Audio_BGM.Length == 0)
        {
            Debug.LogError("No BGM clips loaded.");
            return;
        }

        PlayNextBGM(); // 첫 곡 재생 시작
    }

    void PlayNextBGM()
    {
        if (Audio_BGM.Length == 0) return;

        if (currentBGMIndex == 4) bgmVolume = 0.6f; // 해당 영상 소리가 작아 2배
        else bgmVolume = 0.2f;

        audioSource.clip = Audio_BGM[currentBGMIndex];
        audioSource.loop = false; // 자동 루프 해제
        audioSource.volume = bgmVolume; // 볼륨 설정
        audioSource.Play();


        // 다음 곡 인덱스를 설정 (루프)
        currentBGMIndex = (currentBGMIndex + 1) % Audio_BGM.Length;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0.0f, 1.0f); // 볼륨 값 제한 (0.0 ~ 1.0)
        audioSource.volume = bgmVolume; // AudioSource의 볼륨 업데이트
    }
}
