using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance; //BGMManager�� �̱������� ����

    AudioSource audioSource;
    AudioClip[] Audio_BGM;
    string[] bgmDir = { "Sounds/BGM/bgm1", "Sounds/BGM/bgm2", "Sounds/BGM/bgm3", "Sounds/BGM/bgm4", "Sounds/BGM/bgm5" };

    private int currentBGMIndex = 0; // ���� ��� ���� BGM�� �ε���

    [Range(0.0f, 1.0f)]
    float bgmVolume = 0.3f; // �⺻ ���� ���� (0.0 ~ 1.0)

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
        PlayBGM();
    }

    void SetSingleton()
    {
        // Singleton ������ �����Ͽ� ����� �Ŵ����� �ν��Ͻ��� �����մϴ�.
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
        // BGM ���� ����
        Audio_BGM = new AudioClip[bgmDir.Length];

        for (int i = 0; i < bgmDir.Length; i++)
        {
            Audio_BGM[i] = Resources.Load<AudioClip>(bgmDir[i]);
        }
    }

    // TODO ��� �ٱ� �� ������ �Ƚ��ϱ�

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

        if (currentBGMIndex == 4) bgmVolume = 0.6f; // �ش� ���� �Ҹ��� �۾� 2��
        else bgmVolume = 0.2f;

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
