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
        maxHP = 100;
        base.Start();

        fireRate = 1.5f;
        bulletSpeed = 8f;

        bulletPrefab = Resources.Load<GameObject>(prefabNames[3]);
    }

    // TODO 부모함수 재사용으로 변경
    protected override void Fire()
    {
        foreach (var offset in spawnOffsets)
        {
            // 총알 생성 위치 계산
            Vector3 spawnPosition = transform.position + offset;

            // 총알 생성
            GameObject bulletGO = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

            // 총알의 Z축 위치를 조정하여 Grid보다 앞에 배치
            Vector3 bulletPosition = bulletGO.transform.position;
            bulletPosition.z = -3; // 필요에 따라 조정
            bulletGO.transform.position = bulletPosition;

            // 목표 방향 계산
            Vector3 directionToTarget = (target.position - bulletGO.transform.position).normalized;

            // 오차 범위 설정 (예: -5도에서 +5도까지)
            float deviationAngle = Random.Range(-5f, 5f);
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg + deviationAngle;

            // 오차를 적용한 방향 계산
            Vector3 deviationDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0).normalized;

            // 총알의 방향을 목표로 설정 (회전 설정)
            bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Rigidbody2D에 방향 속도 설정
            Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();
            rb.velocity = deviationDirection * bulletSpeed;
        }
    }
}
