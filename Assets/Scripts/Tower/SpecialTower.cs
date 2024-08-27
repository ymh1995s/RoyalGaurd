using UnityEngine;

public class SpecialTower : BaseTower
{
    protected override void Start()
    {
        base.Start();
        bulletPrefab = Resources.Load<GameObject>(prefabNames[3]);
    }
}
