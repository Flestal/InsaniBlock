using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBlock : MonoBehaviour
{
    //public int ItemCode = -1;//아이템 코드, -1은 null, 0은 코인(밸런스), 1은 회복, 2는 방패, 3은 색반전, 4는 필터, 5는 무적시간
    float lifetime = 0;
    public float deadline = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        lifetime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyCheck();
    }
    void DestroyCheck()
    {
        if (lifetime < deadline)
        {
            lifetime += Time.deltaTime;
            return;
        }
        if (lifetime < deadline*2.0f)
        {
            lifetime = deadline*2.0f+1;
            Destroy(this.gameObject);
            return;
        }
    }

    public abstract void Item_Use();
}
