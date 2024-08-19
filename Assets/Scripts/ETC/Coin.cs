using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //��� ����Ʈ
    private float floatHeight = 0.5f; // �������� ����
    private float floatDuration = 0.25f; // �������� �ð�
    private float landDuration = 0.125f; // �����ϴ� �ð�
    private Vector3 originalPosition; // �ʱ� ��ġ
    private float startTime; // ���� �ð�

    void Start()
    {
        DropEffect();
        Destroy(gameObject, 15); //X���� ����
    }

    void DropEffect()
    {
        originalPosition = transform.position;
        startTime = Time.time;

        // �������� �ִϸ��̼� ����
        StartCoroutine(FloatAndLand());
    }

    IEnumerator FloatAndLand()
    {
        // �������� �ִϸ��̼�
        while (Time.time - startTime < floatDuration)
        {
            float newY = originalPosition.y + floatHeight * Mathf.Sin((Time.time - startTime) * Mathf.PI / floatDuration);
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
            yield return null;
        }

        // ���� �ִϸ��̼�
        float startFloatTime = Time.time;
        float endY = originalPosition.y;
        while (Time.time - startFloatTime < landDuration)
        {
            float newY = Mathf.Lerp(transform.position.y, endY, (Time.time - startFloatTime) / landDuration);
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
            yield return null;
        }

        // ���� ��ġ ����
        transform.position = originalPosition;
    }
}
