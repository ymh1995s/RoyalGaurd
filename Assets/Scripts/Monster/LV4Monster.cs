using UnityEngine;

public class LV4Monster : BaseMonster
{
    protected override void Start()
    {
        base.Start();
        hp = master_Hp[(int)Level.LV4];
    }
}
