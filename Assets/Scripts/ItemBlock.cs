using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBlock : MonoBehaviour
{
    //public int ItemCode = -1;//������ �ڵ�, -1�� null, 0�� ����(�뷱��), 1�� ȸ��, 2�� ����, 3�� ������, 4�� ����, 5�� �����ð�
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
