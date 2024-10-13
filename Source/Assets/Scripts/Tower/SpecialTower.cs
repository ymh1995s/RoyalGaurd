using UnityEngine;

public class SpecialTower : BaseTower
{
    // 네 개의 각기 다른 위치로부터 총알을 생성하고 발사
    Vector3[] spawnOffsets = new Vector3[]
    {
        new Vector3(-0.75f, 0.75f, 0),  // 왼쪽 위
        new Vector3(0.75f, 0.75f, 0),   // 오른쪽 위
        new Vector3(-0.75f, -0.75f, 0), // 왼쪽 아래
        new Vector3(0.75f, -0.75f, 0)   // 오른쪽 아래
    };

    protected override void Start()
    {
        maxHP = 150;
        base.Start();

        fireRate = 1.15f;
        bulletSpeed = 10f;

        bulletPrefab = Resources.Load<GameObject>(prefabNames[3]);
    }

    protected override void Fire(Vector3 spawnPoint)
    {
        foreach (var offset in spawnOffsets)
        {
            // 총알 생성 위치 계산
            Vector3 spawnPosition = transform.position + offset;

            base.Fire(spawnPosition);
        }
    }
}
