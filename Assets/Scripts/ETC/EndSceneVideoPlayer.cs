using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class EndSceneVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer를 할당할 변수
    public RenderTexture renderTexture; // 기존 RenderTexture

    void Start()
    {        
        videoPlayer = GetComponent<VideoPlayer>();

        // 기존 RenderTexture 해제
        if (videoPlayer.targetTexture != null)
        {
            videoPlayer.targetTexture.Release();
        }

        // 비디오 플레이어의 targetTexture에 기존 RenderTexture를 설정
        videoPlayer.targetTexture = renderTexture;

        // 비디오를 멈추고 새 비디오 소스를 준비
        videoPlayer.Stop();
        videoPlayer.Prepare();

        StartCoroutine(PlayVideoAfterDelay(10f)); // X초 후에 비디오 재생
    }

    IEnumerator PlayVideoAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        videoPlayer.Play(); // 비디오 재생
    }
}
