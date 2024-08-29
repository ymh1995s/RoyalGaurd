using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class GameSceneVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer�� �Ҵ��� ����

    void Start()
    {
        StartCoroutine(PlayVideoAfterDelay(5f)); // X�� �Ŀ� ���� ���
    }

    IEnumerator PlayVideoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        videoPlayer.Play(); // ���� ���
    }
}
