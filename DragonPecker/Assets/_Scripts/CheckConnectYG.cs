using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using TMPro;

public class CheckConnectYG : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += CheckSDK;
    private void OnDisable() => YandexGame.GetDataEvent -= CheckSDK;
    private TextMeshProUGUI scoreBest;
    public GameObject Achv0;
    // Start is called before the first frame update
    void Start(){   
        Debug.Log(YandexGame.savesData.achiv0);
        Debug.Log("Test");
        Debug.Log(YandexGame.SDKEnabled);
        if (YandexGame.SDKEnabled){
            CheckSDK();
        }
        Debug.Log(YandexGame.savesData.achiv0);
        Debug.Log("Test3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckSDK(){
        if(YandexGame.auth){
            Debug.Log("User authorization OK");
            Debug.Log(YandexGame.playerName + " YANAME");
        }
        else{
            Debug.Log("User NOT authorization");
            YandexGame.AuthDialog();
        }
        YandexGame.RewVideoShow(0);
        GameObject scoreBO = GameObject.Find("BestScore");
        scoreBest = scoreBO.GetComponent<TextMeshProUGUI>();
        scoreBest.text = "Best Score: " + YandexGame.savesData.bestScore.ToString();
        Debug.Log(YandexGame.savesData.achiv0);
        Debug.Log("Test2");
        if(YandexGame.savesData.achiv0){
            Achv0.SetActive(true);
        }
        

    }
}
