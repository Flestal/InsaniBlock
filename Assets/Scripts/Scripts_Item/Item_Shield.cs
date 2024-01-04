using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shield : ItemBlock
{
    public override void Item_Use()
    {
        float angle = UnityEngine.Random.Range(0f, 360f);
        //float angle = 45f;
        Vector2 shieldPos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        PlayerBlock.Instance.obj_Shield.transform.localPosition = shieldPos;
        PlayerBlock.Instance.obj_Shield.transform.rotation = Quaternion.Euler(0, 0, angle);
        PlayerBlock.Instance.obj_Shield.SetActive(true);
        //Debug.Log("angle : " + angle + ", pos : [" + shieldPos.x + ", " + shieldPos.y + "]");
        PlayerBlock.Instance.SetItemTime(3.0f);
        Destroy(this.gameObject);
    }
}
