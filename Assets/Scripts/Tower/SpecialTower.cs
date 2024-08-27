using UnityEngine;

public class SpecialTower : BaseTower
{
    protected override void Start()
    {
        base.Start();

        fireRate = 0.5f;
        bulletPrefab = Resources.Load<GameObject>(prefabNames[3]);
    }
}
