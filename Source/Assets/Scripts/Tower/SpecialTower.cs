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
            // �Ѿ� ���� ��ġ ���
            Vector3 spawnPosition = transform.position + offset;

            base.Fire(spawnPosition);
        }
    }
}
