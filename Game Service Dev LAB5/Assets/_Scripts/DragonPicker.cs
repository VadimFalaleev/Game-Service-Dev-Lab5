using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG; // new
using TMPro; // new

public class DragonPicker : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += GetLoadSave; // new
    private void OnDisable() => YandexGame.GetDataEvent -= GetLoadSave; // new

    public GameObject energyShieldPrefab;
    public int numEnergyShield = 3;
    public float energyShieldBottomY = -6;
    public float energyShieldRadius = 1.5f;
    public List<GameObject> shieldList;
    public TextMeshProUGUI scoreGT; // new

    private void Start()
    {
        if (YandexGame.SDKEnabled) GetLoadSave(); // new

        shieldList = new();

        for (int i = 1; i <= numEnergyShield; i++)
        {
            GameObject tShieldGo = Instantiate(energyShieldPrefab);
            tShieldGo.transform.position = new(0, energyShieldBottomY, 0);
            tShieldGo.transform.localScale = new(1 * i, 1 * i, 1 * i);
            shieldList.Add(tShieldGo);
        }
    }

    public void DragonEggDestroyed()
    {
        GameObject[] tDragonEggArray = GameObject.FindGameObjectsWithTag("Dragon Egg");
        foreach (GameObject tGO in tDragonEggArray)
            Destroy(tGO);

        int shieldIndex = shieldList.Count - 1;
        GameObject tShieldGo = shieldList[shieldIndex];
        shieldList.RemoveAt(shieldIndex);
        Destroy(tShieldGo);

        if (shieldList.Count == 0)
        {
            GameObject scoreGO = GameObject.Find("Score"); // new
            scoreGT = scoreGO.GetComponent<TextMeshProUGUI>(); // new
            UserSave(int.Parse(scoreGT.text)); // new

            SceneManager.LoadScene("_0Scene");

            GetLoadSave(); // new
        }
    }

    public void GetLoadSave() // new
    {
        Debug.Log(YandexGame.savesData.score);
    }

    public void UserSave(int currentScore) // new
    {
        YandexGame.savesData.score = currentScore;
        YandexGame.SaveProgress();
    }
}