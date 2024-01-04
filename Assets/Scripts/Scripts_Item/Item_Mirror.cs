using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Mirror : ItemBlock
{
    public override void Item_Use()
    {
        PlayerBlock.Instance.Mirrored = true;
        Timeline.instance.sizeRender(PlayerBlock.Instance.Mirrored);
        PlayerBlock.Instance.SetItemTime(10.0f);
        Destroy(this.gameObject);
    }
}
