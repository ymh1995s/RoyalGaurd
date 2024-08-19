using UnityEngine;

public class LV5Monster : BaseMonster
{
    protected override void Start()
    {
        base.Start();
        hp = master_Hp[(int)Level.LV5];
    }
}
