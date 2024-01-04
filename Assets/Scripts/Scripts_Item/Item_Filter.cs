using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Filter : ItemBlock
{
    public override void Item_Use()
    {
        PlayerBlock.Instance.obj_Filter.transform.position = PlayerBlock.Instance.transform.position;
        PlayerBlock.Instance.obj_Filter.SetActive(true);
        PlayerBlock.Instance.SetItemTime(10.0f);
        Destroy(this.gameObject);
    }
}
