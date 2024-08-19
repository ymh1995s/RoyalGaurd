using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //드롭 이펙트
    private float floatHeight = 0.5f; // 떠오르는 높이
    private float floatDuration = 0.25f; // 떠오르는 시간
    private float landDuration = 0.125f; // 착지하는 시간
    private Vector3 originalPosition; // 초기 위치
    private float startTime; // 시작 시간

    void Start()
    {
        DropEffect();
        Destroy(gameObject, 15); //X초후 삭제
    }

    void DropEffect()
    {
        originalPosition = transform.position;
        startTime = Time.time;

        // 떠오르는 애니메이션 시작
        StartCoroutine(FloatAndLand());
    }

    IEnumerator FloatAndLand()
    {
        // 떠오르는 애니메이션
        while (Time.time - startTime < floatDuration)
        {
            float newY = originalPosition.y + floatHeight * Mathf.Sin((Time.time - startTime) * Mathf.PI / floatDuration);
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
            yield return null;
        }

        // 착지 애니메이션
        float startFloatTime = Time.time;
        float endY = originalPosition.y;
        while (Time.time - startFloatTime < landDuration)
        {
            float newY = Mathf.Lerp(transform.position.y, endY, (Time.time - startFloatTime) / landDuration);
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
            yield return null;
        }

        // 최종 위치 보정
        transform.position = originalPosition;
    }
}
