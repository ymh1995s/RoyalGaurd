using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private GameObject objectToSpawn;  // ������ ������Ʈ ������
    private float spawnInterval = 1f; // ���� ����(��)
    private float changeInterval = 120f; // ������ ���� ����(��) 2�о� 5��
    private float spawnInterval_changeInterval; // ���� ���� ���� => changeInterval/3
    private int currentPrefabIndex = 0;
    private string[] prefabNames = { "Monster/LV1", "Monster/LV2", "Monster/LV3", "Monster/LV4", "Monster/LV5"};
    bool isLastStage = false;

    float[] topSpawnPointRange = {1f,30f,-6f,7f };
    float[] bottomSpawnPointRange = {-16f, 30f, -6f, -22f };

    void Start()
    {
        objectToSpawn = Resources.Load<GameObject>(prefabNames[currentPrefabIndex]);
        spawnInterval_changeInterval = changeInterval / 3;
        StartCoroutine(SpawnObject());
        StartCoroutine(ChangeSpawnInterval());
        StartCoroutine(ChangePrefabPeriodically());
    }

    private IEnumerator SpawnObject()
    {
        while (true)
        {
            // TODO ������Ʈ Ǯ��

            // (���� ����) ���� ������ ������ ��ġ�� ����
            float randomX = Random.Range(topSpawnPointRange[0], topSpawnPointRange[1]);
            float randomY = Random.Range(topSpawnPointRange[2], topSpawnPointRange[3]);

            // ������Ʈ�� ��ġ�� ����
            Vector3 position = new Vector3(randomX, randomY, transform.position.z);
            Instantiate(objectToSpawn, position, transform.rotation);

            // (�Ʒ��� ����) ���� ������ ������ ��ġ�� ����
            randomX = Random.Range(bottomSpawnPointRange[0], bottomSpawnPointRange[1]);
            randomY = Random.Range(bottomSpawnPointRange[2], bottomSpawnPointRange[3]);

            // ������Ʈ�� ��ġ�� ����
            position = new Vector3(randomX, randomY, transform.position.z);
            Instantiate(objectToSpawn, position, transform.rotation);

            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private IEnumerator ChangeSpawnInterval()
    {
        while (true)
        {
            float[] multipliers = { 0.75f, 0.5f, 1f }; // ���� �ֱ⿡ ���� 
            int index = 0;

            while (true)
            {

                yield return new WaitForSeconds(spawnInterval_changeInterval); // 20�� ���
                spawnInterval = 1f * multipliers[index]; // ���� spawnInterval(1f)�� ����� ����
                index = (index + 1) % multipliers.Length; // �ε����� ��ȯ��Ŵ

                //������ ���������� �׻� �ְ� �ӵ� ����
                if (isLastStage == true)
                {
                    spawnInterval = 1f * 0.5f;
                }

                print("���� ���� �ֱ� " + spawnInterval);
            }
        }
    }

        private IEnumerator ChangePrefabPeriodically()
    {
        while (true)
        {
            // changeInterval �� ���� ���
            yield return new WaitForSeconds(changeInterval);
            // ���� ������ �ε����� �������� ����
            currentPrefabIndex = (currentPrefabIndex + 1) % prefabNames.Length;
            // ���ο� ������ �ε�
            objectToSpawn = Resources.Load<GameObject>(prefabNames[currentPrefabIndex]);

            // �������� �ε���� �ʾ��� ��� ���� �α� ���
            if (objectToSpawn == null)
            {
                Debug.LogError("�������� ã�� �� �����ϴ�: " + prefabNames[currentPrefabIndex]);
            }

            // �迭�� ������ �ε����� �����ϸ� �ڷ�ƾ ����
            if (currentPrefabIndex == prefabNames.Length - 1)
            {
                isLastStage = true;
                Debug.Log("�迭�� ������ �����տ� �����߽��ϴ�. �ڷ�ƾ�� �����մϴ�.");
                yield break;
            }
        }
    }
}
