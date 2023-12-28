using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
    public static Timeline instance;
    [SerializeField] GameObject NPCPrefab, CameraWrapper;
    //[SerializeField] PlayerBlock playerBlock;
    [SerializeField] Transform NPCHolder;
    [SerializeField] List<float> timeTriggers;
    [SerializeField] int BlockCntMax,BlockCur;
    [SerializeField] List<GameObject> Blocks;
    public float SpawnRange;
    public int difficulty_NumRangeMin, difficulty_NumRangeMax;
    public float difficulty_HostileFactor, difficulty_SpeedFactor;
    List<float> times;
    [SerializeField] Button btn_ReturnToMain;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        difficulty_NumRangeMin = -15;
        difficulty_NumRangeMax = 15;
        times = new List<float>();
        foreach(float trigger in timeTriggers)
        {
            times.Add(0);
        }
        BlockCntMax = (int)(1/timeTriggers[0])*12;
        CameraWrapper.transform.parent = PlayerBlock.Instance.transform;
        PreSpawnNPC();
        btn_ReturnToMain.onClick.AddListener(() =>
        {
            ReturnToMain();
        });
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<times.Count; i++)
        {
            times[i] += Time.deltaTime;
            //Debug.Log("times["+i+"] : "+times[i]);
            if (times[i] >= timeTriggers[i])//타임라인 달성 시
            {
                switch (i)//각 타임라인별 행동
                {
                    case 0:
                        //Debug.Log("case 0");
                        SpawnNPC();
                        break;
                    default:
                        break;
                }
                times[i] = 0;
            }
        }
    }
    void PreSpawnNPC()
    {
        Blocks = new List<GameObject>();
        for(int i=0;i<BlockCntMax;i++)
        {
            GameObject block = Instantiate(NPCPrefab, NPCHolder);
            block.SetActive(false);
            Blocks.Add(block);
        }
    }
    void SpawnNPC()
    {
        if (Blocks[BlockCur].activeSelf == true)
        {
            BlockCur++;
            if (BlockCur >= BlockCntMax)
            {
                BlockCur = 0;
            }
            return;
        }
        int SpawnDir = Random.Range(0, 4);
        float RandomAngle = Random.Range(-45f, 45f) + SpawnDir * 90;
        RandomAngle *= Mathf.Deg2Rad;
        Vector2 RandomDirection = new Vector2(Mathf.Cos(RandomAngle), Mathf.Sin(RandomAngle));
        float RandomPoint = Random.Range(-SpawnRange, SpawnRange);
        float SecondPoint = 0;
        switch (SpawnDir)
        {
            case 0:
            case 1:
                SecondPoint = -SpawnRange;
                break;
            case 2:
            case 3:
                SecondPoint = SpawnRange;
                break;
        }
        Vector2 SpawnPoint = Vector2.zero;
        if (SpawnDir % 2 == 0)
        {
            SpawnPoint = new Vector2(SecondPoint, RandomPoint);
        }
        else
        {
            SpawnPoint = new Vector2(RandomPoint, SecondPoint);
        }
        Vector2 playerPos = PlayerBlock.Instance.transform.position;
        int SpawnNumber = Random.Range(difficulty_NumRangeMin, difficulty_NumRangeMax + 1) + PlayerBlock.Instance.Number;
        //Debug.Log("Number " + SpawnNumber+ " SpawnDir : "+ SpawnDir+ ", randomAngle : "+ RandomAngle+ ", randomPoint : ["+ SpawnPoint.x + "," + SpawnPoint.y+ "]") ;
        //GameObject block = Instantiate(NPCPrefab,SpawnPoint+playerPos,Quaternion.Euler(0,0,0),NPCHolder);
        GameObject block = Blocks[BlockCur];
        block.transform.position = SpawnPoint + playerPos;
        block.GetComponent<SpriteRenderer>().color = Color.white;
        NPCBlock blockInfo = block.GetComponent<NPCBlock>();
        if (blockInfo != null)
        {
            blockInfo.Number = SpawnNumber;
            blockInfo.Hostile = (Random.value <= difficulty_HostileFactor)&&(SpawnNumber>(PlayerBlock.Instance.Number+3));
            blockInfo.speed = 
                (blockInfo.Number - PlayerBlock.Instance.Number) * -0.02f
                + difficulty_SpeedFactor
                + (Random.Range(0.2f,1.0f)*System.Convert.ToInt32(blockInfo.Hostile))
                + (PlayerBlock.Instance.Number*0.005f)
                ;//-15~15 범위에서 0.7~1.3 속도, 작을수록 빠름, 1점마다 +0.005씩 속도 증가
            if (blockInfo.Hostile)
            {
                //float angle = Vector2.Angle(SpawnPoint,playerBlock.gameObject.transform.position)*Mathf.Deg2Rad;
                blockInfo.direction = (PlayerBlock.Instance.transform.position - block.transform.position).normalized;
                blockInfo.Number += 2;
            }
            else
            {
                blockInfo.direction = RandomDirection;
            }
            blockInfo.NumberAlpha(1.0f);
            blockInfo.OnSpawn();
            blockInfo.enabled = true;
        }
        block.SetActive(true);
        BlockCur++;
        if (BlockCur >= BlockCntMax)
        {
            BlockCur = 0;
        }
        //Debug.Log("spawned");
    }
    public void sizeRender()
    {
        foreach(GameObject obj in Blocks)
        {
            NPCBlock blockInfo;
            if(obj != null && obj.TryGetComponent<NPCBlock>(out blockInfo))
            {
                blockInfo.SizeRender();
            }
        }
    }
    void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }
}
