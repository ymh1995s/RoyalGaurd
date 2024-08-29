using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class GameSceneVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer를 할당할 변수

    void Start()
    {
        StartCoroutine(PlayVideoAfterDelay(5f)); // X초 후에 비디오 재생
    }

    IEnumerator PlayVideoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        videoPlayer.Play(); // 비디오 재생
    }
}
