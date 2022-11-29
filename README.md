# Разработка игровых сервисов
Отчет по лабораторной работе #5 выполнил(а):
- Фалалеев Вадим Эдуардович
- РИ-300012

Отметка о выполнении заданий (заполняется студентом):

| Задание | Выполнение | Баллы |
| ------ | ------ | ------ |
| Задание 1 | * | 60 |
| Задание 2 | * | 20 |
| Задание 3 | * | 20 |

знак "*" - задание выполнено; знак "#" - задание не выполнено;

Работу проверили:
- к.т.н., доцент Денисов Д.В.
- к.э.н., доцент Панов М.А.
- ст. преп., Фадеев В.О.

Структура отчета

- Данные о работе: название работы, фио, группа, выполненные задания.
- Цель работы.
- Задание 1.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Задание 2.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Задание 3.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Выводы.

## Цель работы
создание интерактивного приложения с рейтинговой системой пользователя и интеграция игровых сервисов в готовое приложение.

## Задание 1
### Используя видео-материалы практических работ 1-5 повторить реализацию приведенного ниже функционала:

– 1 Практическая работа «Интеграции авторизации с помощью Яндекс SDK»

– 2 Практическая работа «Сохранение данных пользователя на платформе Яндекс Игры»

– 3 Практическая работа «Сбор данных об игроке и вывод их в интерфейсе»

– 4 Практическая работа «Интеграция таблицы лидеров»

– 5 Практическая работа «Интеграция системы достижений в проект»

Ход работы:

- Начнем с того, что добавим объект на сцену, позволяющий работать с YandexSDK.

![image](https://user-images.githubusercontent.com/54228342/203308526-97d2c5fb-9a5b-41b2-b43e-c739e8a66989.png)

- Создадим скрипт CheckConnectYG, который будет проверять, авторизован пользователь или нет. Если нет, то будет предлагать авторизироваться.

```c#

using UnityEngine;
using YG;

public class CheckConnectYG : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += CheckSDK;
    private void OnDisable() => YandexGame.GetDataEvent -= CheckSDK;
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
    }
}

```

- Создадим на сцене пустой объект YandexManager, в который добавим только что созданный скрипт. В этом объекте будут храниться все скрипты для работы с YandexSDK.

![image](https://user-images.githubusercontent.com/54228342/203309490-4665e1b4-5a0e-49d4-9602-002832946a2e.png)

- Если запустим проект в Unity, то увидим логи внизу.

![image](https://user-images.githubusercontent.com/54228342/203309695-e76e82dd-b8ed-4e1f-84d5-a6b813ca81bb.png)

- Выводит False, потому что SDK будет работать только на локальном сервере, либо на сервисе Яндекс.Игр. Поэтому нужно создать сборку, и загрузить ее на Яндекс.Игры, после чего можно будет проверить функционал скрипта.
- Создадим сборку тем же способам, каким создавали в прошлых работах и загрузим .zip файл на сервис. Так же нужно будет поставить галочку напротив поддержки авторизации.

![image](https://user-images.githubusercontent.com/54228342/203310614-a2f1eb6c-2e1e-41ef-9aac-1abdeddd9a5e.png)

- После загрузки выйдем из аккаунта Янедкс.Игр и попробуем зайти в игру.

![image](https://user-images.githubusercontent.com/54228342/203325468-f721471c-01ac-4338-adb3-2e3f407640dd.png)

- Теперь у неавторизованных пользователей будет вылезать окно авторизации. Далее посмотрим консоль в коде страницы, чтобы убедиться, что скрипт работает полностью.

![image](https://user-images.githubusercontent.com/54228342/203326883-81cb0164-53d5-4c92-93a7-0fdab8c93999.png)

- Как мы видим, в начале вышел наш лог из скрипта "User not authorization", но после авторизации в консоле вывелось "User authorization ok". Тем самым, мы убедились, что скрипт полностью работает.
- Теперь создадим сохранение очков пользователя на сервере. Для этого зайдем на игровую сцену и также добавим объект YandexGame. Далее изменим скрипт SaveYG в папке YandexGame > WorkingData.

```c#


namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        public bool isFirstSession = true;
        public string language = "ru";
        public bool feedbackDone;
        public bool promptDone;

        // Ваши сохранения
        public int score;
    }
}

```

- Сохраним скрипт и удалим скрипт SaverTest из папкм YandexGame > Example > ExampleScritps, чтобы избавиться от ошибок. Теперь можем зайти в скрипт DragonPicker, чтобы добавить строчки кода. Они позволят сохранять прогресс пользователя. В нашем случае пока что будут сохраняться лишь очки. Новые строчки и методы выделены комментарием "new".

```c#

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

```

- Сохраняем изменения, и теперь можно снова билдить проект и заливать его на Яндекс.Игры, где проверим работоспособность скрипта. Обязательно нужно поставить галочку напротив облачных сохранений.

![image](https://user-images.githubusercontent.com/54228342/203935175-b2e04cc8-d21c-4ea0-a48c-8d927c4bd486.png)

- теперь начнем играть и откроем консоль, в которой можно будет увидеть строку, в которой отображается количество изначальных очков.

![image](https://user-images.githubusercontent.com/54228342/203937530-a24b9fc4-87d3-48ea-a47c-8f7a9df57f2b.png)

- Если проиграть, то в консоли выведется общее количество очков, то есть сколько яиц поймал игрок за все сессии.

![image](https://user-images.githubusercontent.com/54228342/203937644-a233f298-1688-45e3-a2ab-85ef33a05997.png)

- Благодаря скрипту данные сохраняются, поэтому, если начать новую игру, тогда в консоли выведется количество сохраненных очков за прошлые игры(в моем случае 5).
- Далее будем сохранять лучший результат игрока за одну сессию и показывать ему в главном меню игры. Создадим на сцене новый текст, напишем "Best Score:" и поместим его в правый нижний угол.

![image](https://user-images.githubusercontent.com/54228342/204282871-f107ab80-8c14-4de3-b2dc-f5eff58eb9e4.png)

- Зайдем в скрипт "SavesYG", добавив туда новую строчку. Это будет переменная, в которой и будет сохраняться лучший результат.

```c#

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        public bool isFirstSession = true;
        public string language = "ru";
        public bool feedbackDone;
        public bool promptDone;

        // Ваши сохранения
        public int score;
        public int bestScore; // new
    }
}

```

- Теперь реализуем функционал сохранения лучшего результата. в скрипте "DragonPicker" добавим и редактируем строчки.

```c#

...

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
            GameObject scoreGO = GameObject.Find("Score");
            scoreGT = scoreGO.GetComponent<TextMeshProUGUI>();
            UserSave(int.Parse(scoreGT.text), YandexGame.savesData.bestScore); // new

            SceneManager.LoadScene("_0Scene");

            GetLoadSave();
        }
    }
    
...

    public void UserSave(int currentScore, int currentBestScore) // new
    {
        YandexGame.savesData.score = currentScore;

        if (currentScore > currentBestScore) YandexGame.savesData.bestScore = currentScore; // new

        YandexGame.SaveProgress();
    }

```

- Функционал готов, осталось лишь показывать игроку этот самый лучший результат. Реализуем это в скрипте "CheckConnectYG".

```c#

using UnityEngine;
using YG;
using TMPro; // new

public class CheckConnectYG : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += CheckSDK;
    private void OnDisable() => YandexGame.GetDataEvent -= CheckSDK;

    private TextMeshProUGUI scoreBest; // new

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

        GameObject scoreBO = GameObject.Find("BestScore"); // new
        scoreBest = scoreBO.GetComponent<TextMeshProUGUI>(); // new
        scoreBest.text = "Best score: " + YandexGame.savesData.bestScore.ToString(); // new
    }
}

```

- Создадим еще одну фишку - отображение имени игрока над персонажем. На сцене уровня создадим текст и назовем его PlayerName. Это же и напишем в самом тексте. Расположим текст над головой игрока, корректируя положение и размер текста.

![image](https://user-images.githubusercontent.com/54228342/204292209-ef9b73a6-0633-44a3-a22c-7f7cb151eb48.png)

- Зайдем в скрипт "DragonPicker" и добавим новые строчки для реализации отображения имени игрока.

```c#

...

public class DragonPicker : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += GetLoadSave;
    private void OnDisable() => YandexGame.GetDataEvent -= GetLoadSave;

    public GameObject energyShieldPrefab;
    public int numEnergyShield = 3;
    public float energyShieldBottomY = -6;
    public float energyShieldRadius = 1.5f;
    public List<GameObject> shieldList;
    public TextMeshProUGUI scoreGT;
    public TextMeshProUGUI playerName; // new
    
...

 public void GetLoadSave()
    {
        Debug.Log(YandexGame.savesData.score);

        GameObject playerNamePrefabGUI = GameObject.Find("PlayerName"); // new
        playerName = playerNamePrefabGUI.GetComponent<TextMeshProUGUI>(); // new
        playerName.text = YandexGame.playerName; // new
    }

...

```

- Теперь можно проверить работоспособость отображения лучшего результата в главном меню и имени игрока над персонажем прямо в Unity.

![Видео 28-11-2022 191302_Trim](https://user-images.githubusercontent.com/54228342/204299607-01da5cc2-acb4-4fb9-bba7-92d96a034a67.gif)

- Как мы видим, лучший результат обновляется и имя героя отображается.
- Реализуем еще один способ удержания игроков в игре - создадим таблицу лидеров. Зайдем в скрипт "DragonPicker" и добавим строчку.

```c#

...

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
            GameObject scoreGO = GameObject.Find("Score");
            scoreGT = scoreGO.GetComponent<TextMeshProUGUI>();
            UserSave(int.Parse(scoreGT.text), YandexGame.savesData.bestScore);

            YandexGame.NewLeaderboardScores("TOPPlayerScore", int.Parse(scoreGT.text)); // new

            SceneManager.LoadScene("_0Scene");

            GetLoadSave();
        }
    }
    
...

```

- Скопируем название таблицы лидеров, чтобы использовать в будущем. После этого создадим новую сборку и загрузим его на Яндекс.Игры. Дополнительно с этим нужно зайти в раздел Лидерборды и вставить название таблицы лидеров из скрипта.

![image](https://user-images.githubusercontent.com/54228342/204542354-50e2f9f7-535f-48f4-a0d1-40fe28089d70.png)

- В самом низу страницы нужно дать отображаемое название лидерборда и поставить определенные настройки. После этого нажать кнопку сохранить, тогда внизу появится название данного лидерборда.

![image](https://user-images.githubusercontent.com/54228342/204543093-fec9b80d-0f68-4323-b78d-7bda2f78d24b.png)

- Заходим в нашу игру в черновике и поиграем, собрав несколько очков. После этого зайдем в меню, где отображается лидерборд и посмотрим на результат.

![image](https://user-images.githubusercontent.com/54228342/204545611-688a4c6a-e3bd-41e3-a4c4-e05a43b81fd7.png)

- лидерборд работает и отображает количество очков. Вместо имени написано, что пользователь скрыт, так как игра запущена из черновика.


## Задание 2
### Описать не менее трех дополнительных функций Яндекс SDK, которые могут быть интегрированы в игру.

Ход работы:



## Задание 3
### Доработать стилистическое оформление списка лидеров и системы достижений, реализованных в задании 1

Ход работы:



## Выводы


