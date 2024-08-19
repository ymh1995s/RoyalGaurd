using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private GameObject objectToSpawn;  // 생성할 오브젝트 프리팹
    private float spawnInterval = 1f; // 생성 간격(초)
    private float changeInterval = 120f; // 프리팹 변경 간격(초) 2분씩 5개
    private float spawnInterval_changeInterval; // 생성 간격 변경 => changeInterval/3
    private int currentPrefabIndex = 0;
    private string[] prefabNames = { "Monster/LV1", "Monster/LV2", "Monster/LV3", "Monster/LV4", "Monster/LV5"};


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
            // TODO 오브젝트 풀링

            // (윗쪽 스폰) 범위 내에서 랜덤한 위치를 설정
            float randomX = Random.Range(1f, 30f);
            float randomY = Random.Range(-6f, 7f);

            // 오브젝트의 위치를 설정
            Vector3 position = new Vector3(randomX, randomY, transform.position.z);
            Instantiate(objectToSpawn, position, transform.rotation);

            // 나중에 함수로 구분해서 코드로 줄여놓을까?
            // 하드코딩도 줄일 수 있으면 최소화
            // (아래쪽 스폰) 범위 내에서 랜덤한 위치를 설정
            randomX = Random.Range(-16f, 30f);
            randomY = Random.Range(-6f, -22f);

            // 오브젝트의 위치를 설정
            position = new Vector3(randomX, randomY, transform.position.z);
            Instantiate(objectToSpawn, position, transform.rotation);

            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private IEnumerator ChangeSpawnInterval()
    {
        while (true)
        {
            float[] multipliers = { 0.75f, 0.5f, 1f }; // 반복될 생성 간격 배열
            int index = 0;

            while (true)
            {
                yield return new WaitForSeconds(spawnInterval_changeInterval); // 20초 대기
                spawnInterval = 1f * multipliers[index]; // 원래 spawnInterval(1f)에 배수를 곱함
                index = (index + 1) % multipliers.Length; // 인덱스를 순환시킴
            }
        }
    }

        private IEnumerator ChangePrefabPeriodically()
    {
        while (true)
        {
            // changeInterval 초 동안 대기
            yield return new WaitForSeconds(changeInterval);
            // 현재 프리팹 인덱스를 다음으로 변경
            currentPrefabIndex = (currentPrefabIndex + 1) % prefabNames.Length;
            // 새로운 프리팹 로드
            objectToSpawn = Resources.Load<GameObject>(prefabNames[currentPrefabIndex]);

            // 프리팹이 로드되지 않았을 경우 에러 로그 출력
            if (objectToSpawn == null)
            {
                Debug.LogError("프리팹을 찾을 수 없습니다: " + prefabNames[currentPrefabIndex]);
            }

            // 배열의 마지막 인덱스에 도달하면 코루틴 종료
            if (currentPrefabIndex == prefabNames.Length - 1)
            {
                Debug.Log("배열의 마지막 프리팹에 도달했습니다. 코루틴을 종료합니다.");
                yield break;
            }
        }
    }
}
