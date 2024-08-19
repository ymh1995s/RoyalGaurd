using UnityEngine;

public class LV2Monster : BaseMonster
{
    protected override void Start()
    {
        base.Start();
        hp = master_Hp[(int)Level.LV2];
    }
}
