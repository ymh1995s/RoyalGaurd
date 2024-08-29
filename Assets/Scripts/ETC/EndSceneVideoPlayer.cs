using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class EndSceneVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer�� �Ҵ��� ����

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(PlayVideoAfterDelay(10f)); // X�� �Ŀ� ���� ���
    }

    IEnumerator PlayVideoAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        videoPlayer.Play(); // ���� ���
    }
}
