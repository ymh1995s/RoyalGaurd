using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioiManager : MonoBehaviour
{
    public static AudioiManager Instance;

    AudioSource audioSource;
    AudioClip[] Audio_BGM = new AudioClip[3];
    [Range(0.0f, 1.0f)]
    float bgmVolume = 0.4f; // �⺻ ���� ���� (0.0 ~ 1.0)

    void Awake()
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

    void Start()
    {
        // ���� �ε�� �� �̺�Ʈ�� �����մϴ�.
        SceneManager.sceneLoaded += OnSceneLoaded;

        // �ʱ⿡ ù ������ BGM ���
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ������ �����մϴ�.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "StartScene":
                //PlayBGM(0); //������ ��ü
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
        Audio_BGM[0] = Resources.Load<AudioClip>("Sounds/BGM/�����̼����̹�");
        Audio_BGM[1] = Resources.Load<AudioClip>("Sounds/BGM/�����̼����̹�");
        Audio_BGM[2] = Resources.Load<AudioClip>("Sounds/BGM/�����̼����̹�"); //�ϴ� ������� ��� ����
    }

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

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0.0f, 1.0f); // ���� �� ���� (0.0 ~ 1.0)
        audioSource.volume = bgmVolume; // AudioSource�� ���� ������Ʈ
    }

}
