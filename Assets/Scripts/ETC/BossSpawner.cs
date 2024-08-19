using System.Collections;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private string[] prefabNames = { "Monster/BOSS"};

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(SpawnObjectAfterDelay(600f));
        StartCoroutine(SpawnObjectAfterDelay(6f));
    }

    IEnumerator SpawnObjectAfterDelay(float delay)
    {
        print("소환대기");
        yield return new WaitForSeconds(delay);
        print("보스소환");
        GameObject objectToSpawn = Resources.Load<GameObject>(prefabNames[0]);
        Instantiate(objectToSpawn, transform.position, transform.rotation);
    }
}
