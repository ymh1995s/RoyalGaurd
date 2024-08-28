using UnityEngine;

public class SpecialTower : BaseTower
{
    // �� ���� ���� �ٸ� ��ġ�κ��� �Ѿ��� �����ϰ� �߻�
    Vector3[] spawnOffsets = new Vector3[]
    {
        new Vector3(-0.75f, 0.75f, 0),  // ���� ��
        new Vector3(0.75f, 0.75f, 0),   // ������ ��
        new Vector3(-0.75f, -0.75f, 0), // ���� �Ʒ�
        new Vector3(0.75f, -0.75f, 0)   // ������ �Ʒ�
    };

    protected override void Start()
    {
        maxHP = 100;
        base.Start();

        fireRate = 1.5f;
        bulletSpeed = 8f;

        bulletPrefab = Resources.Load<GameObject>(prefabNames[3]);
    }

    // TODO �θ��Լ� �������� ����
    protected override void Fire()
    {
        foreach (var offset in spawnOffsets)
        {
            // �Ѿ� ���� ��ġ ���
            Vector3 spawnPosition = transform.position + offset;

            // �Ѿ� ����
            GameObject bulletGO = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

            // �Ѿ��� Z�� ��ġ�� �����Ͽ� Grid���� �տ� ��ġ
            Vector3 bulletPosition = bulletGO.transform.position;
            bulletPosition.z = -3; // �ʿ信 ���� ����
            bulletGO.transform.position = bulletPosition;

            // ��ǥ ���� ���
            Vector3 directionToTarget = (target.position - bulletGO.transform.position).normalized;

            // ���� ���� ���� (��: -5������ +5������)
            float deviationAngle = Random.Range(-5f, 5f);
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg + deviationAngle;

            // ������ ������ ���� ���
            Vector3 deviationDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0).normalized;

            // �Ѿ��� ������ ��ǥ�� ���� (ȸ�� ����)
            bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Rigidbody2D�� ���� �ӵ� ����
            Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();
            rb.velocity = deviationDirection * bulletSpeed;
        }
    }
}
