using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class UpgradeScript : MonoBehaviour
{
    public static UpgradeScript Instance;
    [SerializeField] int balance;
    [SerializeField] List<int> Upgrades = new List<int>();
    [SerializeField] List<int> reqBalances;
    // {
    //      �ִ�ü�� x3,
    //      �̵��ӵ� x5,
    //      ����(ü�°� �ִ�ü���� float�� ����) x5,
    //      �����ð� x3,
    //      ���� ��������(int ��, �Ҽ��� ����) x5
    // }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Upgrades"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Upgrades");
            ResetUpgrades();
        }
        else
        {
            Upgrades.Clear();
            string UpgradesTextRaw = File.ReadAllText(Application.persistentDataPath + "/Upgrades/Upgrades.txt");
            string[] strings = UpgradesTextRaw.Split(',');
            foreach (string str in strings)
            {
                Upgrades.Add(System.Convert.ToInt32(str));
            }
            balance = System.Convert.ToInt32(File.ReadAllText(Application.persistentDataPath + "/Upgrades/Balance.txt"));
        }
        reqBalances = new List<int>() { 30, 30, 40, 40, 50 };
    }
    public void ResetUpgrades()
    {
        Upgrades = new List<int>() { 0, 0, 0, 0, 0 };
        balance = 0;
        string UpgradesText = "0,0,0,0,0";
        File.WriteAllText(Application.persistentDataPath + "/Upgrades/Upgrades.txt", UpgradesText);
        File.WriteAllText(Application.persistentDataPath + "/Upgrades/Balance.txt", "0");
    }
    public void SaveUpgrades()
    {
        string UpgradesText = "";
        foreach (int upgrade in Upgrades)
        {
            UpgradesText += upgrade.ToString() + ",";
        }
        UpgradesText = UpgradesText.TrimEnd(',');
        File.WriteAllText(Application.persistentDataPath + "/Upgrades/Upgrades.txt", UpgradesText);
    }
    public void SaveBalance()
    {
        File.WriteAllText(Application.persistentDataPath + "/Upgrades/Balance.txt", balance.ToString());
    }
    public void AppendResult(int Score)
    {
        balance += Mathf.FloorToInt(Score/10f);
        SaveBalance();
    }
    public List<int> ReturnUpgrades()
    {
        return Upgrades;
    }
    public void UpgradeRequest(int type,UpgradeBtnScript whatBtn)
    {
        //if (balance >= reqBalances[type] && Upgrades[type]<whatBtn.ReturnMaxUp())
        if (balance >= whatBtn.getReqBalance() && Upgrades[type]<whatBtn.ReturnMaxUp())
        {
            balance -= whatBtn.getReqBalance();
            Upgrades[type] += 1;
            whatBtn.UpdateRender(Mathf.CeilToInt(reqBalances[type] * Mathf.Pow(1.5f, Upgrades[type])));
            GameObject.Find("MainMenuManager").GetComponent<MainMenuScript>().AppendBalance(balance);
            SaveUpgrades();
            SaveBalance();
        }
    }
    public void RenderMenu(List<GameObject> btn_Upgrades,TMP_Text balanceText)
    {
        balanceText.text = "Balance : " + balance.ToString();
        for(int i=0;i<btn_Upgrades.Count;i++)
        {
            //btn_Upgrades[i].GetComponent<UpgradeBtnScript>().UpdateRender(reqBalances[i]);
            btn_Upgrades[i].GetComponent<UpgradeBtnScript>().UpdateRender(Mathf.CeilToInt(reqBalances[i] * Mathf.Pow(1.5f, Upgrades[i])));
        }
    }
}
