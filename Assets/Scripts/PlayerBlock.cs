using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerBlock : Block
{
    public static PlayerBlock Instance;
    public bool isLive;
    [SerializeField] float HP;//ü��
    [SerializeField] float DR;//������ ����
    [SerializeField] int MaxHP;//�ִ�ü��, �⺻ 3
    [SerializeField] Image HPBar;
    [SerializeField] SpriteRenderer spriteRenderer;
    public int Score;//���� �ִ� ���
    bool Invincible;//����
    int InvincibleGraphic;//���� ������
    [SerializeField] float InvincibleTimeCheck;//�����ð�
    float invincibleTime;
    [SerializeField] GameObject GameOver;
    [SerializeField] TMP_Text GameOverText;
    [SerializeField] float ResultMag;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        List<int> upgrades = UpgradeScript.Instance.ReturnUpgrades();
        isLive = true;
        //MaxHP = 3;
        MaxHP = upgrades[0]+3;//ü�� ���׷��̵� ���ô� �ִ�ü�� 1 ����
        HP = MaxHP;
        Number = 10;
        speed = 2.0f*Mathf.Pow(1.1f,upgrades[1]);//�̼� ���׷��̵� ���ô� 1.1�� ������
        DR = 1 * Mathf.Pow(0.9f, upgrades[2]);//���� ���׷��̵� ���ô� 0.9�� ������
        InvincibleTimeCheck = 3.0f * Mathf.Pow(1.15f, upgrades[3]);//�����ð� ���׷��̵�, �ִ� 3����, ���ô� 1.15�� ������
        ResultMag = 1*Mathf.Pow(1.15f,upgrades[4]);//���� ���� ����, ���ô� 1.15�� ������
        Invincible = true;
        invincibleTime = 0;
        InvincibleGraphic = 1;
        NumberRender();
        GameOver.SetActive(false);
    }

    // Update is called once per frame
    public void Move(Vector2 direction, float alpha)
    {
        Vector2 dir_refined = direction.normalized;
        float move_spd = alpha * speed;
        this.gameObject.transform.Translate(dir_refined*move_spd*Time.deltaTime);
    }
    void Update()
    {
        
        //MoveCheck();
        InvincibleCheck();
        InvincibleRender();
    }
    void MoveCheck()
    {
        Vector2 dir = Vector2.zero;
        float angle = Mathf.Deg2Rad;
        if (Input.GetKey(KeyCode.Q))//������ �� 45��
        {
            angle *= 45;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.W))//�� 90��
        {
            angle *= 90;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.E))//���� �� 135��
        {
            angle *= 135;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.R))//���� 180��
        {
            angle *= 180;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.A))//���� �Ʒ� 225��
        {
            angle *= 225;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.S))//�Ʒ� 270��
        {
            angle *= 270;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.D))//���� �Ʒ� 315��
        {
            angle *= 315;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.F))//������ 0��
        {
            angle *= 0;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            this.gameObject.transform.position = Vector2.zero;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Break();
        }
        else
        {
            return;
        }
        Debug.Log("angle : " + angle + ", vector2 angle : [" + dir.x + ", " + dir.y + "]");
        Move(dir, 2);
    }

    IEnumerator blink(float time)
    {
        HPBar.enabled = true;
        while (Invincible)
        {
            yield return new WaitForSeconds(time);
            InvincibleGraphic++;
            if(InvincibleGraphic%2==0)
            {
                InvincibleGraphic = 0;
            }
        }
        HPBar.enabled = false;
    }
    void InvincibleCheck()
    {
        if (Invincible)
        {
            invincibleTime += Time.deltaTime;
            if(invincibleTime > InvincibleTimeCheck)
            {
                Invincible = false;
                invincibleTime = 0;
            }
        }
        
    }
    void InvincibleRender()
    {
        if (Invincible)
        {
            spriteRenderer.color = new Color(1,1,1,0.5f+(InvincibleGraphic*0.5f));
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision event");
        if (collision.gameObject.CompareTag("NPCBlock"))
        {
            NPCBlock otherInfo = collision.gameObject.GetComponent<NPCBlock>();
            if (otherInfo != null&&this.isLive)
            {
                if (otherInfo.Number < this.Number)//���� ��ƸԴ� ���
                {
                    this.Number++;
                    NumberRender();
                    Timeline.instance.sizeRender();
                    otherInfo.SelfDestroy();
                }
                else
                {
                    if (Invincible)
                    {

                    }
                    else
                    {
                        HPCalc(-1*DR);
                        Invincible = true;
                        StartCoroutine(blink(0.5f));
                    }
                    otherInfo.FadeOutDestroy();
                }
            }
            else
            {
                otherInfo.SelfDestroy();
            }
        }
    }
    void HPCalc(float value)
    {
        this.HP += value;
        HPBar.fillAmount = (float)this.HP/(float)this.MaxHP;
        if(this.HP <= 0)
        {
            //Debug.Log("Game Over");
            isLive = false;
            int ResultScore = Mathf.FloorToInt(this.Number * ResultMag);
            ScoreWriter.Instance.Score = ResultScore;
            ScoreWriter.Instance.AppendHighScore();
            UpgradeScript.Instance.AppendResult(ResultScore);
            GameOverRender(GameOverText);
            GameOver.SetActive(true);
            //this.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }
    void GameOverRender(TMP_Text text)
    {
        string txt = "x ";
        txt += ResultMag.ToString("F3");
        txt += Environment.NewLine + "= ";
        txt += Mathf.FloorToInt(this.Number * ResultMag).ToString();
        text.text = txt;
    }
}
