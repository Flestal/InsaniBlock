using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBlock : Block
{
    public bool Eatable_;
    public bool Hostile;
    public Vector2 direction;
    [SerializeField] Sprite Eatable, Unable;
    [SerializeField] SpriteRenderer spriteRenderer;
    float lifetime = 0;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(DestroyTimer());
        NumberRender();
        SizeRender();
    }
    public void OnSpawn()
    {
        NumberRender();
        SizeRender(PlayerBlock.Instance.Mirrored);
        spriteRenderer.color = Color.white;
        lifetime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //NumberRender();
        //SizeRender();
        DestroyCheck();
        Move();
    }
    public void SizeRender(bool Mirrored=false)
    {
        int sizeGap = (this.Number - PlayerBlock.Instance.Number);
        this.transform.localScale = Vector3.one * (1+sizeGap*0.03f);//0.55배 크기~1.45배 크기, 이론상 더 클 수 있음
        if (Mirrored)
        {
            if (sizeGap < 0)
            {
                spriteRenderer.sprite = Unable;
                Eatable_ = false;
            }
            else
            {
                spriteRenderer.sprite = Eatable;
                Eatable_ = true;
            }
            return;
        }
        if (sizeGap < 0)
        {
            spriteRenderer.sprite = Eatable;
            Eatable_ = true;
        }
        else
        {
            spriteRenderer.sprite = Unable;
            Eatable_ = false;
        }
    }
    void Move()
    {
        this.gameObject.transform.Translate(direction * speed * Time.deltaTime);
    }
    //WaitForSeconds wait10s = new WaitForSeconds(10);
    WaitForFixedUpdate waitF = new WaitForFixedUpdate();
    //IEnumerator DestroyTimer()
    //{
    //    yield return wait10s;
    //    yield return FadeOut();
    //}
    void DestroyCheck()
    {
        if (lifetime < 10.0f)
        {
            lifetime += Time.deltaTime;
        }
        else if (lifetime >= 10.0f && lifetime < 100.0f)
        {
            lifetime = 101.0f;
            StartCoroutine(FadeOut());
        }
    }
    IEnumerator FadeOut()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            spriteRenderer.color = new Color(1, 1, 1, alpha);
            NumberAlpha(alpha);
            yield return waitF;
        }
        //Destroy(gameObject);
        this.gameObject.SetActive(false);
    }
    public void FadeOutDestroy()
    {
        StartCoroutine(FadeOut());
    }
    public void SelfDestroy()
    {
        StopAllCoroutines();
        //Destroy(gameObject);
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            SelfDestroy();
            return;
        }
        if (collision.CompareTag("Filter"))
        {
            if (!Eatable_)
            {
                SelfDestroy();
                return;
            }
        }
    }
}
