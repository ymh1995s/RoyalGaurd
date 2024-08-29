using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class EndSceneVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer를 할당할 변수

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(PlayVideoAfterDelay(10f)); // X초 후에 비디오 재생
    }

    IEnumerator PlayVideoAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        videoPlayer.Play(); // 비디오 재생
    }
}
