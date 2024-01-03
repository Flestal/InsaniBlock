using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtnScript : MonoBehaviour
{
    [SerializeField] int WhatIsThisButton, MaxUpgrade, reqBalance;
    [SerializeField] List<Image> Images = new List<Image>();
    [SerializeField] TMP_Text ReqBalanceText;
    [SerializeField]List<Color> Colors = new List<Color>() { new Color(1,0,0),new Color(1,1,0),new Color(0,1,0) };
    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnClick()
    {
        UpgradeScript.Instance.UpgradeRequest(WhatIsThisButton,this);
    }

    public void UpdateRender(int reqBalance=-1)
    {
        int thisUpgrades = UpgradeScript.Instance.ReturnUpgrades()[WhatIsThisButton];
        if (thisUpgrades > 0&&thisUpgrades<MaxUpgrade)
        {
            for(int i=0; i<MaxUpgrade; i++)
            {
                Images[i].color = Colors[0];
            }
            for(int i = 0; i < thisUpgrades; i++)
            {
                Images[i].color = Colors[2];
            }
            Images[thisUpgrades].color = Colors[1];
        }
        else if (thisUpgrades >= MaxUpgrade)
        {
            for(int i=0;i<MaxUpgrade; i++)
            {
                Images[i].color = Colors[2];
            }
        }
        else
        {
            for (int i = 0; i < MaxUpgrade; i++)
            {
                Images[i].color = Colors[0];
            }
            Images[0].color = Colors[1];
        }
        if (reqBalance > 0)
        {
            this.reqBalance = reqBalance;
            if(thisUpgrades >= MaxUpgrade)
            {
                ReqBalanceText.text = "Max";
            }
            else
            {
                ReqBalanceText.text = reqBalance.ToString();
            }
        }
    }
    public int ReturnMaxUp()
    {
        return MaxUpgrade;
    }
    public int getReqBalance()
    {
        return reqBalance;
    }
}
