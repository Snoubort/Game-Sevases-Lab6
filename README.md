Создание индивидуальной системы достижения пользователя и ее интеграция в пользовательский интерфейс
Отчет по лабораторной работе #5 выполнил(а):
- Кулаков Иван Александрович
- РИ300003

| Задание | Выполнение | Баллы |
| ------ | ------ | ------ |
| Задание 1 | * |   60 |
| Задание 2 | * |   20 |
| Задание 3 | * |   20 |

## Цель работы
создание интерактивного приложения с рейтинговой системой пользователя и интеграция игровых сервисов в готовое приложение.
## Задание 1
### Используя видео-материалы практических работ 1-5 повторить реализацию приведенного ниже функционала:
### - 1 Практическая работа «Интеграция баннерной рекламы».
### – 2 Практическая работа «Интеграция видеорекламы».
### - 3 Практическая работа «Показ видеорекламы пользователю за вознаграждение».
### - 4 Практическая работа «Создание внутриигрового магазина».
### - 5 Практическая работа «Система антиблокировки рекламы».
Ход работы:
- Заполняем форму на подключение монетизации в консоли яндекс игр.
- Создаём свой шаблон рекламы, копируем его ID, добавляем его в раздел статик баннера настройки сборки.
![image](https://github.com/Snoubort/Game-Sevases-Lab6/blob/main/MatForReadMe/SettingBannerAd.PNG)
- Рекламный баннер без AdBlock
![image](https://github.com/Snoubort/Game-Sevases-Lab6/blob/main/MatForReadMe/AdBlock.PNG)
- Добавляем видео-рекламу на страрте игры(скрипт CheckConnectYG) и при поражении(DragonPicker)
![image](https://github.com/Snoubort/Game-Sevases-Lab6/blob/main/MatForReadMe/ADOnStartGame.PNG)
![image](https://github.com/Snoubort/Game-Sevases-Lab6/blob/main/MatForReadMe/ADInMenu.PNG)



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

        
      
- Добавляем универсальный способ запуска показа видео-рекламы
- Добавляем кнопку для вызова рекламы за вознаграждение
![image](https://github.com/Snoubort/Game-Sevases-Lab6/blob/main/MatForReadMe/AdButton.PNG)




        public class ADReward : MonoBehaviour
        {
            private void OnEnable() => YandexGame.CloseVideoEvent += Rewarded;
            private void OnDisable() => YandexGame.CloseVideoEvent -= Rewarded;
            void Rewarded(int id){
                if(id ==1){
                    Debug.Log("Пользователь получил награду");
                }
                else{
                    Debug.Log("Пользователь остался без награды");
                }
            }
            public void OpenAd(){
                YandexGame.RewVideoShow(Random.Range(0, 2));
            }
        }
        
        
        
## Задание 2
### Добавить в приложение интерфейс для вывода статуса наличия игрока в сети (онлайн или офлайн).


  

## Задание 3
### Предложить наиболее подходящий на ваш взгляд способ монетизации игры D.Picker. Дать развернутый ответ с комментариями.
- Создаём в меню настроек слайдер, привязываем к его изменению изменение параметра Volume в AudioSource
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/Music.PNG)
