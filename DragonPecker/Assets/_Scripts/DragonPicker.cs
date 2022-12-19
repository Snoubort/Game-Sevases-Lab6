using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using TMPro;

public class DragonPicker : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += GetLoadSave;
    private void OnDisable() => YandexGame.GetDataEvent -= GetLoadSave;
    public GameObject energyShieldPrefab;
    public int numEnergyShield = 3;
    public float energyShieldBottomY = -6f;
    public float energyShieldRadius = 1.5f;
    public TextMeshProUGUI scoreGT;
    private TextMeshProUGUI playerName;

    public List<GameObject> shieldList;

    public AudioSource backgroundMusic;
    
    void Start()
    {
        if(YandexGame.SDKEnabled){
            GetLoadSave();
        }
        backgroundMusic.volume = Dataholder.soundLevel;

        shieldList = new List<GameObject>();
        for (int i = 1; i<= numEnergyShield; i++){
            GameObject tShieldGo = Instantiate<GameObject>(energyShieldPrefab);
            tShieldGo.transform.position = new Vector3(0, energyShieldBottomY, 0);
            tShieldGo.transform.localScale = new Vector3(1*i, 1*i, 1*i);
            shieldList.Add(tShieldGo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DragonEggDestroyd(){
        GameObject[] tDragonEggArray = GameObject.FindGameObjectsWithTag("Dragon Egg");
        foreach (GameObject tGO in tDragonEggArray){
            Destroy(tGO);
        }
        
        int shieldIndex = shieldList.Count -1;
        GameObject tShieldGo = shieldList[shieldIndex];
        shieldList.RemoveAt(shieldIndex);
        Destroy(tShieldGo);

        if (shieldList.Count == 0){
            GameObject scoreGO = GameObject.Find("Score");
            scoreGT = scoreGO.GetComponent<TextMeshProUGUI>();
            bool[] achivList = new bool[10];
            achivList[0] = true;
            UserSave(int.Parse(scoreGT.text), YandexGame.savesData.bestScore, achivList);
            YandexGame.NewLeaderboardScores("TOPPlayerScore", int.Parse(scoreGT.text));
            YandexGame.RewVideoShow(0);
            SceneManager.LoadScene("_0Scene");
            GetLoadSave();
        }
    }

    public void GetLoadSave(){
        Debug.Log(YandexGame.savesData.score);
        GameObject playerNamePrefabGUI = GameObject.Find("PlayerName");
        playerName = playerNamePrefabGUI.GetComponent<TextMeshProUGUI>();
        if(YandexGame.auth){
            playerName.text = "Online \n" + YandexGame.playerName;
        }
        else{
            playerName.text = "Offline \n" + YandexGame.playerName;
        }
        
        Debug.Log(YandexGame.playerName + " YANAME");
    }

    public void UserSave(int currentScore, int currentBestScore, bool[] currentAchiv){
        YandexGame.savesData.score = currentScore;
        if(currentScore > currentBestScore){
            YandexGame.savesData.bestScore = currentScore;
        }
        YandexGame.savesData.achiv0 = currentAchiv[0];
        YandexGame.SaveProgress();
    }

}
