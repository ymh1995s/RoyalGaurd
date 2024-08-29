using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class EndSceneVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer�� �Ҵ��� ����
    public RenderTexture renderTexture; // ���� RenderTexture

    void Start()
    {        
        videoPlayer = GetComponent<VideoPlayer>();

        // ���� RenderTexture ����
        if (videoPlayer.targetTexture != null)
        {
            videoPlayer.targetTexture.Release();
        }

        // ���� �÷��̾��� targetTexture�� ���� RenderTexture�� ����
        videoPlayer.targetTexture = renderTexture;

        // ������ ���߰� �� ���� �ҽ��� �غ�
        videoPlayer.Stop();
        videoPlayer.Prepare();

        StartCoroutine(PlayVideoAfterDelay(10f)); // X�� �Ŀ� ���� ���
    }

    IEnumerator PlayVideoAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        videoPlayer.Play(); // ���� ���
    }
}
