using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Coin : ItemBlock
{
    public override void Item_Use()
    {
        PlayerBlock.Instance.UpBalance(1);
        Destroy(this.gameObject);
    }
}
