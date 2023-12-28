using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] public int Number;//�� ��Ϻ� ����
    [SerializeField] public float speed;//�̼�
    [SerializeField] TextMeshPro textMeshPro; //�� ��Ϻ� ���� ǥ��
    float NumberSize;
    public void NumberRender()
    {
        if (Number > 999)
        {
            NumberSize = 3.5f;
        }else if (Number>99)
        {
            NumberSize = 4.5f;
        }else if (Number > 9)
        {
            NumberSize = 6.5f;
        }
        else
        {
            NumberSize = 7f;
        }
        textMeshPro.fontSize = NumberSize;
        textMeshPro.text = Number.ToString();
    }
    public void NumberAlpha(float alpha)
    {
        textMeshPro.alpha = alpha;
    }
}
