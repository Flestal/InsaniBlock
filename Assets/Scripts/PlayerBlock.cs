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
    [SerializeField] float HP;//체력
    [SerializeField] float DR;//데미지 감소
    [SerializeField] int MaxHP;//최대체력, 기본 3
    [SerializeField] Image HPBar;
    [SerializeField] SpriteRenderer spriteRenderer;
    public int Score;//점수 최대 기록
    bool Invincible;//무적
    int InvincibleGraphic;//무적 깜빡임
    [SerializeField] float InvincibleTimeCheck_template;//무적시간
    [SerializeField]float InvincibleTimeCheck;
    [SerializeField]float invincibleTime;
    public GameObject obj_Shield, obj_InvincibleEffect, obj_Filter, GameOver;
    [SerializeField] TMP_Text GameOverText, CoinResultText;
    [SerializeField] float ResultMag;
    public bool Mirrored;
    float ItemTime, ItemTimeLimit;
    public int ThisRoundBalance;

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
        MaxHP = upgrades[0]+3;//체력 업그레이드 스택당 최대체력 1 증가
        HP = MaxHP;
        Number = 10;
        speed = 2.0f*Mathf.Pow(1.1f,upgrades[1]);//이속 업그레이드 스택당 1.1배 곱연산
        DR = 1 * Mathf.Pow(0.9f, upgrades[2]);//뎀감 업그레이드 스택당 0.9배 곱연산
        InvincibleTimeCheck_template = 3.0f * Mathf.Pow(1.15f, upgrades[3]);//무적시간 업그레이드, 최대 3스택, 스택당 1.15배 곱연산
        ResultMag = 1*Mathf.Pow(1.15f,upgrades[4]);//최종 점수 배율, 스택당 1.15배 곱연산
        Invincible = true;
        InvincibleTimeCheck = 3f;
        invincibleTime = 0;
        InvincibleGraphic = 1;
        ItemTime = -1;
        Mirrored = false;
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
        ItemTimeCheck();
    }
    void MoveCheck()
    {
        Vector2 dir = Vector2.zero;
        float angle = Mathf.Deg2Rad;
        if (Input.GetKey(KeyCode.Q))//오른쪽 위 45도
        {
            angle *= 45;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.W))//위 90도
        {
            angle *= 90;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.E))//왼쪽 위 135도
        {
            angle *= 135;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.R))//왼쪽 180도
        {
            angle *= 180;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.A))//왼쪽 아래 225도
        {
            angle *= 225;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.S))//아래 270도
        {
            angle *= 270;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.D))//왼쪽 아래 315도
        {
            angle *= 315;
            dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        else if (Input.GetKey(KeyCode.F))//오른쪽 0도
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

    //IEnumerator blink(float time)
    //{
    //    HPBar.enabled = true;
    //    while (Invincible)
    //    {
    //        yield return new WaitForSeconds(time);
    //        InvincibleGraphic++;
    //        if(InvincibleGraphic%2==0)
    //        {
    //            InvincibleGraphic = 0;
    //        }
    //    }
    //    HPBar.enabled = false;
    //}
    void InvincibleCheck()
    {
        if (Invincible)
        {
            invincibleTime += Time.deltaTime;
            if(invincibleTime >= InvincibleTimeCheck)
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
            HPBar.enabled = true;
            InvincibleGraphic = Mathf.FloorToInt(invincibleTime*2)%2;
            spriteRenderer.color = new Color(1,1,1,0.5f+(InvincibleGraphic*0.5f));
            
        }
        else
        {
            HPBar.enabled = false;
            spriteRenderer.color = Color.white;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision event");
        if (collision.gameObject.CompareTag("NPCBlock"))
        {
            NPCBlock otherInfo = collision.gameObject.GetComponent<NPCBlock>();
            CollisionNPC(otherInfo);
            return;
        }
        if (collision.gameObject.CompareTag("Item"))
        {
            ItemBlock otherInfo = collision.gameObject.GetComponent<ItemBlock>();
            CollisionItem(otherInfo);
            return;
        }

    }

    void CollisionNPC(NPCBlock otherInfo)
    {
        if (otherInfo == null || !this.isLive)//내가 살아있지 않은 경우 삭제하고 끝
        {
            otherInfo.SelfDestroy();
            return;
        }
        //if (otherInfo.Number < this.Number)//내가 잡아먹는 경우
        if(otherInfo.Eatable_)
        {
            this.Number++;
            NumberRender();
            Timeline.instance.sizeRender(Mirrored);
            otherInfo.SelfDestroy();
            return;
        }
        otherInfo.FadeOutDestroy();//내가 맞는경우
        if (Invincible||obj_InvincibleEffect.activeSelf)//무적시간인 경우 데미지 판정 없음
        {
            return;
        }
        HPCalc(-1 * DR);//데미지 판정
        InvincibleTimeCheck = InvincibleTimeCheck_template;//기본 무적시간
        Invincible = true;
        //StartCoroutine(blink(0.5f));
        
    }

    void CollisionItem(ItemBlock otherInfo)
    {
        //Debug.Log(otherInfo.ItemCode);
        otherInfo.Item_Use();
    }
    public void UpBalance(int amount)
    {
        ThisRoundBalance += amount;
    }
    public void SetItemTime(float time)
    {
        ItemTimeLimit = time;
        ItemTime = 0;
    }
    void ItemTimeCheck()
    {
        if (ItemTime == -1)
        {
            return;
        }
        if (ItemTime < ItemTimeLimit)
        {
            ItemTime += Time.deltaTime;
            return;
        }
        obj_Shield.SetActive(false);
        obj_InvincibleEffect.SetActive(false);
        obj_Filter.SetActive(false);
        Mirrored = false;
        Timeline.instance.sizeRender(Mirrored);
        ItemTime = -1;
        InvincibleTimeCheck = InvincibleTimeCheck_template;//기본 무적시간
        Invincible = true;
    }

    public void HPCalc(float value)
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
            UpgradeScript.Instance.AppendResult(ThisRoundBalance);
            GameOverRender(GameOverText,CoinResultText);
            GameOver.SetActive(true);
        }
    }
    void GameOverRender(TMP_Text Scoretext, TMP_Text CoinText)
    {
        string txt = "x ";
        txt += ResultMag.ToString("F3");
        txt += Environment.NewLine + "= ";
        txt += Mathf.FloorToInt(this.Number * ResultMag).ToString();
        Scoretext.text = txt;
        txt = "x " + ThisRoundBalance.ToString();
        CoinText.text = txt;
    }
}
