using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance; //BGMManager�� �̱������� ����

    AudioSource audioSource;
    AudioClip[] Audio_BGM;
    string[] bgmDir = { "Sounds/BGM/bgm1", "Sounds/BGM/bgm2", "Sounds/BGM/bgm3", "Sounds/BGM/bgm4" };

    private int currentBGMIndex = 0; // ���� ��� ���� BGM�� �ε���

    [Range(0.0f, 1.0f)]
    float bgmVolume = 0.5f; // �⺻ ���� ���� (0.0 ~ 1.0)

    void Awake()
    {
        SetSingleton();
    }

    void Start()
    {
        // ���� �ε�� �� �̺�Ʈ�� �����մϴ�.
        SceneManager.sceneLoaded += OnSceneLoaded;

        // �ʱ⿡ ù ������ BGM ���
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void Update()
    {
        // ���� ���� ��� ������ �ʴٸ� ���� ���� ���
        if (!audioSource.isPlaying)
        {
            PlayNextBGM();
        }
    }

    void OnDisable()
    {
        // �� �ε� �̺�Ʈ ������ �����մϴ�.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //TODO ���� �߰� ����
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //switch (scene.name)
        //{
        //    case "StartScene":
        //        //PlayBGM(0); //������ ��ü
        //        break;
        //    case "GameScene":
        //        //PlayBGM(1);
        //        PlayBGM();
        //        break;
        //    case "GameOverScene":
        //        break;
        //    case "GameClearScene":
        //        break;
        //    default:
        //        break;
        //}
    }

    void SetSingleton()
    {
        // Singleton ������ �����Ͽ� ����� �Ŵ����� �ν��Ͻ��� �����մϴ�.
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

    void LoadAudioClips()
    {
        // BGM ���� ����
        Audio_BGM = new AudioClip[bgmDir.Length];

        for (int i = 0; i < bgmDir.Length; i++)
        {
            Audio_BGM[i] = Resources.Load<AudioClip>(bgmDir[i]);
        }
    }

    // TODO �߰� ����
    // TODO ��� �ٱ� �� ������ �Ƚ��ϱ�
    void PlayBGM(int index)
    {
        if (index >= 0 && index < Audio_BGM.Length && Audio_BGM[index] != null)
        {
            audioSource.clip = Audio_BGM[index];
            audioSource.loop = true;
            audioSource.volume = bgmVolume; // ���� ����
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Invalid BGM index or AudioClip not loaded.");
        }
    }

    void PlayBGM()
    {
        if (Audio_BGM.Length == 0)
        {
            Debug.LogError("No BGM clips loaded.");
            return;
        }

        PlayNextBGM(); // ù �� ��� ����
    }

    void PlayNextBGM()
    {
        if (Audio_BGM.Length == 0) return;

        audioSource.clip = Audio_BGM[currentBGMIndex];
        audioSource.loop = false; // �ڵ� ���� ����
        audioSource.volume = bgmVolume; // ���� ����
        audioSource.Play();

        // ���� �� �ε����� ���� (����)
        currentBGMIndex = (currentBGMIndex + 1) % Audio_BGM.Length;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0.0f, 1.0f); // ���� �� ���� (0.0 ~ 1.0)
        audioSource.volume = bgmVolume; // AudioSource�� ���� ������Ʈ
    }

}
