using UnityEngine;
using YG;
using TMPro;

public class CheckConnectYG : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += CheckSDK;
    private void OnDisable() => YandexGame.GetDataEvent -= CheckSDK;

    private TextMeshProUGUI scoreBest;

    void Start()
    {
        if (YandexGame.SDKEnabled)
        {
            CheckSDK();
        }
    }

    public void CheckSDK()
    {
        if (YandexGame.auth)
        {
            Debug.Log("User authorization ok");
        }
        else
        {
            Debug.Log("User not authorization");
            YandexGame.AuthDialog();
        }

        GameObject scoreBO = GameObject.Find("BestScore");
        scoreBest = scoreBO.GetComponent<TextMeshProUGUI>();
        scoreBest.text = "Best score: " + YandexGame.savesData.bestScore.ToString();

        if (YandexGame.savesData.achiveMent[0] == null && GameObject.Find("ListAchive")) 
        {

        }
        else
        {
            foreach(string value in YandexGame.savesData.achiveMent)
            {
                GameObject.Find("ListAchive").GetComponent<TextMeshProUGUI>().text += value + "\n";
            }
        }
    }
}