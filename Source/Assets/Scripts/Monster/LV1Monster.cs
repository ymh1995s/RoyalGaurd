using UnityEngine;

public class LV1Monster : BaseMonster
{
    protected override void Start()
    {
        base.Start();
        hp = master_Hp[(int)Level.LV1];
    }
}
