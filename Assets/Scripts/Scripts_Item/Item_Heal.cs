using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Heal : ItemBlock
{
    public override void Item_Use()
    {
        PlayerBlock.Instance.HPCalc(0.5);
        Destroy(this.gameObject);
    }
}
