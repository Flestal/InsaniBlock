using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Invincible : ItemBlock
{
    public override void Item_Use()
    {
        PlayerBlock.Instance.obj_InvincibleEffect.SetActive(true);
        PlayerBlock.Instance.SetItemTime(8.0f);
        Destroy(this.gameObject);
    }
}
