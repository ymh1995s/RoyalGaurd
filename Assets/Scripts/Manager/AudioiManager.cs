using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioiManager : MonoBehaviour
{
    public static AudioiManager Instance;

    AudioSource audioSource;
    AudioClip[] Audio_BGM = new AudioClip[3];
    [Range(0.0f, 1.0f)]
    float bgmVolume = 0.4f; // 기본 볼륨 설정 (0.0 ~ 1.0)

    void Awake()
    {
        // Singleton 패턴을 적용하여 오디오 매니저의 인스턴스를 유지합니다.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            LoadAudioClips();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 씬이 로드될 때 이벤트를 구독합니다.
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 초기에 첫 씬에서 BGM 재생
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnDestroy()
    {
        // 씬 로드 이벤트 구독을 해제합니다.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "StartScene":
                //PlayBGM(0); //동영상 대체
                break;
            case "GameScene":
                PlayBGM(1);
                break;
            case "GameOverScene":
                break;
            case "GameClearScene":
                break;
            default:
                break;
        }
    }

    void LoadAudioClips()
    {
        Audio_BGM[0] = Resources.Load<AudioClip>("Sounds/BGM/뭉탱이서바이벌");
        Audio_BGM[1] = Resources.Load<AudioClip>("Sounds/BGM/뭉탱이서바이벌");
        Audio_BGM[2] = Resources.Load<AudioClip>("Sounds/BGM/뭉탱이서바이벌"); //일단 여기까진 사용 안함
    }

    void PlayBGM(int index)
    {
        if (index >= 0 && index < Audio_BGM.Length && Audio_BGM[index] != null)
        {
            audioSource.clip = Audio_BGM[index];
            audioSource.loop = true;
            audioSource.volume = bgmVolume; // 볼륨 설정
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Invalid BGM index or AudioClip not loaded.");
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0.0f, 1.0f); // 볼륨 값 제한 (0.0 ~ 1.0)
        audioSource.volume = bgmVolume; // AudioSource의 볼륨 업데이트
    }

}
