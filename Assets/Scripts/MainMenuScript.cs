using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject MainMenu, HighScore, Upgrade;
    [SerializeField] Button btn_StartGame, btn_HighScore, btn_GotoMain_Highscore, btn_ScoreReset, btn_PurchaseUpgrade, btn_GotoMain_Upgrade, btn_ResetUpgrade;
    [SerializeField] TMP_Text HighScoreText, balanceText;
    [SerializeField] List<GameObject> btn_Upgrades;
    // Start is called before the first frame update
    void Start()
    {
        btn_StartGame.onClick.AddListener(() =>
        {
            ClickStartGame();
        });
        btn_HighScore.onClick.AddListener(() =>
        {
            ClickHighScore();
        });
        btn_GotoMain_Highscore.onClick.AddListener(() =>
        {
            ClickMainMenu();
        });
        btn_ScoreReset.onClick.AddListener(() =>
        {
            ClickScoreReset();
        });
        btn_PurchaseUpgrade.onClick.AddListener(() =>
        {
            ClickUpgrade();
        });
        btn_GotoMain_Upgrade.onClick.AddListener(() =>
        {
            ClickMainMenu();
        });
        btn_ResetUpgrade.onClick.AddListener(() =>
        {
            ClickUpgradeReset();
        });
    }
    void ClickStartGame()
    {
        SceneManager.LoadScene(1);
    }
    void ViewHighScore()
    {
        string ScoreText = "";
        for (int i = 0; i < ScoreWriter.Instance.HighScore.Count; i++)
        {
            ScoreText += (i + 1).ToString() + ". " + ScoreWriter.Instance.HighScore[i].ToString() + '\n';
        }
        ScoreText = ScoreText.TrimEnd('\n');
        HighScoreText.text = ScoreText;
    }
    void ClickHighScore()
    {
        ViewHighScore();
        MainMenu.SetActive(false);
        HighScore.SetActive(true);
    }
    void ClickMainMenu()
    {
        HighScore.SetActive(false);
        Upgrade.SetActive(false);
        MainMenu.SetActive(true);
    }
    void ClickScoreReset()
    {
        ScoreWriter.Instance.ResetScore();
        ViewHighScore();
    }
    void ClickUpgrade()
    {
        UpgradeScript.Instance.RenderMenu(btn_Upgrades,balanceText);
        Upgrade.SetActive(true);
        MainMenu.SetActive(false);
    }
    public void ClickUpgradeReset()
    {
        UpgradeScript.Instance.ResetUpgrades();
        UpgradeScript.Instance.RenderMenu(btn_Upgrades, balanceText);
    }
    public void AppendBalance(int balance)
    {
        balanceText.text = "Balance : " + balance.ToString();
    }
}
