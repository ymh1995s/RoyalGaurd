using UnityEngine;

public class LV3Monster : BaseMonster
{
    protected override void Start()
    {
        base.Start();
        hp = master_Hp[(int)Level.LV3];
    }
}
