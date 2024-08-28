using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class EndSceneVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer�� �Ҵ��� ����

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(PlayVideoAfterDelay(15f)); // X�� �Ŀ� ���� ���
    }

    IEnumerator PlayVideoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        videoPlayer.Play(); // ���� ���
    }
}
