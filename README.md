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

- Лидерборд работает и отображает количество очков. Вместо имени написано, что пользователь скрыт, так как игра запущена из черновика.
- Последний инстурмент удержания игроков, который мы реализуем в этой лабораторной работе - достижения. Первым шагом добавим строку в скрипт "SavesYG".

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
        public int bestScore;
        public string[] achiveMent; // new
    }
}

```

- Теперь на сцене главного меню создадим новую кнопку AchiveMent и напишем на ней ACHIVEMENT. Настроим расположение кнопок на сцене, после чего дублируем окно SettingMenu, назовем его Achive. В настройках кнопки AchiveMent поменяем функционал в OnClick, чтобы при нажатии на кнопку вместо окна настроек открывалось окно достижений.

![image](https://user-images.githubusercontent.com/54228342/204718601-119d478f-643a-44e6-b283-badff9bb26a7.png)

- Следующие действия в видео являются не правильными, так как по итогу код не работает и игра не запускается. Систему достижений реализую самостоятельно в третьем задании.

## Задание 2
### Описать не менее трех дополнительных функций Яндекс SDK, которые могут быть интегрированы в игру.

Ход работы:

### 1. Полноэкранная реклама (Fullscreen Ad).
- В InfoYG есть настройка Fullscreen Ad Challenge.

![image](https://user-images.githubusercontent.com/54228342/204756670-fe41c0cb-cca5-4b2b-94de-34b4d2cfac74.png)

- По умолчанию выбрана опция At Startup End Switch Scene. Это означает, что полноэкранная реклама запустится еще когда игра только загружается, и будет запускаться при переключении сцен.
- Опция Only At Startup означает, что реклама запустится только при запуске игры.
- Для вызова полноэкранной рекламы через скрипт есть метод FullscreenShow(). Благодаря ему можно включать рекламу в любой момент игры(Во время запуска игры, после поражения, при открытии меню достижений и т.д.)
- Вы можете подписаться на событие открытия полноэкранной рекламы OpenFullAdEvent() и на событие закрытия CloseFullAdEvent().

### 2. Реклама за вознаграждение (Reward Ad).
- Для вызова видео-рекламы через скрипт есть метод RewVideoShow(int id).
- Вы можете подписаться на событие открытия видео-рекламы OpenVideoEvent и на событие закрытия CloseVideoEvent.
- Так же есть событие ErrorVideoEvent. Подпишитесь на него, если хотите уведомить игроков о неудачном воспроизведении рекламы за вознаграждение.
- Используйте событие RewardVideoEvent(int id) для вознаграждения игрока за просмотр рекламы.
- Метод вызова видео рекламы (RewVideoShow) принимает одно значение типа integer. Это ID рекламы. Он нужен для нескольких видов вознаграждения.
- Допустим, у Вас есть вознаграждение “+100 монет” и вознаграждение “+оружие”. При вызове видео-рекламы за “+100 монет” запишите ID как 1 RewVideoShow(1). А для вознаграждения  “+оружие” запишите ID как 2 RewVideoShow(2).
- В своём скрипте подпишите свой метод вознаграждения на событие RewardVideoEvent. Подписанный метод должен принимать одно значение типа integer. Это значение и будет ID, которое вернётся тем числом, которое мы записывали при вызове рекламы. И в подписанном методе сделайте проверку. Если ID = 1, то выдаём “+100 монет”. Если ID = 2, то выдаём “+оружие”.

### 3. Пауза игры при просмотре рекламы.
- По умолчанию на префабе YandexGame висит скрипт ViewingAdsYG. При просмотре полноэкранной или видео-рекламы данный скрипт ставит на паузу звук или временной масштаб в игре. Или и то и другое на Ваш выбор (за это отвечает опция Pause Type).

![image](https://user-images.githubusercontent.com/54228342/204755754-3fc12923-822e-40b0-a648-5a48f4dd2b70.png)

Pause Type
- Audio Pause — Ставить звук на паузу при просмотре рекламы.
- Time Scale Pause — Выставить временную шкалу на 0 (остановить время) при просмотре рекламы.
- All — Ставить на паузу и звук и время при просмотре рекламы.

Pause Method
- Remember Previous State — Ставить паузу при открытии рекламы. После закрытия рекламы звук и/или временная шкала придут в изначальное значение (до открытия рекламы).
- Custom State — Укажите свои значения, которые будут выставляться при открытии и закрытии рекламы.

### 4. Sticky-баннер.
- Чтобы включить показ sticky-баннера, откройте консоль разработчика и перейдите на вкладку Черновик.
- В блоке Sticky баннеры настройте отображение баннеров:
    - Для мобильных устройств — в поле Sticky-баннер в портретной ориентации выберите расположение Внизу или Вверху.
    - Для планшетов — в поле Sticky-баннер в альбомной ориентации выберите расположение Внизу или Вверху.
    - Для компьютеров — включите опцию Sticky-баннер на десктопе. Баннер будет показываться справа.

 ![image](https://user-images.githubusercontent.com/54228342/204760300-9b0354d0-03fb-4e7d-8fa9-2b7c3e8aeca0.png)

- По умолчанию sticky-баннер появляется при запуске игры и отображается всю сессию. Чтобы настроить момент показа баннера:
- В блоке Sticky баннеры включите опцию "Не показывать sticky-баннер на старте".
- Используйте API управления показом Sticky баннера.

Управление показом Sticky баннера
- Активируйте или деактивируйте Sticky баннер с помощью метода: YandexGame.StickyAdActivity(bool activity).
- Перегрузка activity задаёт активность Sticky баннера. StickyAdActivity(true) — включить Sticky баннер. StickyAdActivity(false) — выключить Sticky баннер.

### 5. Данные.
- Вы можете получить имя игрока, его аватар, девайс пользователя и т.д.
- Большинство данных берутся напрямую из класса YandexGame. Другие объекты и параметры SDK в отдельном классе YandexGame.EnvironmentData. Также есть класс для сохранений игры и класс для внутриигровых покупок.
- У данного плагина достаточно много методов для анализа данных, поэтому покажу по 2 метода из разных классов.

YandexGame.
- YandexGame.playerId: тип stringify. ID игрока.
- YandexGame.nowFullAd: тип boolean. true — Полноэкранная реклама открыта в данный момент. false — Полноэкранная реклама закрыта в данный момент.

YandexGame.EnvironmentData.
- YandexGame.EnvironmentData.isDesktop: тип boolean. true — компьютер. false — иное устройство.
- YandexGame.EnvironmentData.appID: тип stringify. ID игры.

YandexGame.savesData.
- YandexGame.savesData.isFirstSession: тип boolean. Техническое поле для плагина. Становится true после первой инициализации игры.
- YandexGame.savesData.promptDone: тип boolean. Становится true, когда ярлык игры установлен.

YandexGame.PaymentsData.
- YandexGame.PaymentsData.id[]: тип stringify. Идентификатор товара, который Вы записывали при создании товара в консоли разработчика.
- YandexGame.PaymentsData.priceValue[]: тип boolean. Стоимость товара.

## Задание 3
### Доработать стилистическое оформление списка лидеров и системы достижений, реализованных в задании 1

Ход работы:

- Для начала немного изменю главное меню игры. Поменяю кнопку выхода(она не нужна для веб-игр) на кнопку LEADERBOARD, где будет указана таблица лидеров в самой игре. Так же создам отдельное меню для достижений, как и для настроек с достижениями.

![image](https://user-images.githubusercontent.com/54228342/205901337-6f7c42c6-08d0-446a-9c5c-c43fff424b35.png)

- В самом меню для лидерборда нужно добавить префаб "Leaderboard EntriesText" и в опции Name LB компонента Leaderboard YG написать техническое имя нашего лидерборда. В моем случае это TOPPlayerScore.

![image](https://user-images.githubusercontent.com/54228342/205902192-4e953724-68cd-419a-bde6-007546bde59c.png)

- Загрузим игру на Яндекс.Игры и проверим, что лидерборд работает.

![image](https://user-images.githubusercontent.com/54228342/205902446-09d30ed1-49e4-45c3-b7b7-ce480f14609b.png)

- Все получилось. Отображается аноним, так как игра еще в черновике, и количество максимально полученных очков за сессию. Далее можно по разному работать с дизайном лидерборда - подобрать ему рамку и фон, размер и шрифт текста и так далее, но для минимума этого зватит.
- Далее перейдем к реализации системы достижений. Создадим в окне для достижений 3 текста - так будет отображаться прогресс в получении достижений. Если текст красный, то достижение не получено, а если зеленый - получено.

![image](https://user-images.githubusercontent.com/54228342/205917804-51f1d154-30f8-4774-b598-dd10b41d341b.png)

- Создадим новую переменную в скрипте SavesYG. Она будет считать количество смертей игроков.

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
        public int bestScore;
        public int deaths; // new
    }
}

```

- Теперь зайдем в скрипт DragonPicker и добавим в скрипт строчку, которая прибавляет к новой переменной 1 после того, когда игрок проиграл.

```c#

...

public void UserSave(int currentScore, int currentBestScore)
    {
        YandexGame.savesData.score = currentScore;
        if (currentScore > currentBestScore) YandexGame.savesData.bestScore = currentScore;
        
        YandexGame.savesData.deaths++; // new

        YandexGame.SaveProgress();
    }

```

- Создадим новый скрипт Achievements, который будет проверять, выполнил игрок определенные достижения, или нет.

```c#

using UnityEngine;
using TMPro;
using YG;

public class Achievements : MonoBehaviour
{
    private GameObject[] achievements;

    private void Awake()
    {
        achievements = GameObject.FindGameObjectsWithTag("Achievement");

        foreach (var a in achievements)
        {
            if (a.name == "10 Points" && YandexGame.savesData.bestScore >= 10) AchievementComplete(a);
            if (a.name == "20 Points" && YandexGame.savesData.bestScore >= 20) AchievementComplete(a);
            if (a.name == "First Death" && YandexGame.savesData.deaths >= 1) AchievementComplete(a);
        }
    }

    void AchievementComplete(GameObject text)
    {
        text.GetComponent<TextMeshProUGUI>().color = Color.green;
    }
}

```

- Теперь снова загрузим игру на платформу и проверим, что система достижений работает. Видео получилось слишком большим по памяти и GitHub не разрешает его загружать в Readme, поэтому оно будет в репозитории отдельным файлом под названием TestAchievement.

## Выводы

- После выполнения данной лабораторной работы я научился проверять, авторизован пользователь или нет, сохранять результаты игрока на сервере(например, количество очков или смертей), определять новый рекорд игрока и показывать его на экране, создавать таблицу лидеров и отображать ее как в самом сервисе, так и внутри игры, а так же создавать и отображать достижения, которые смог получить игрок.
