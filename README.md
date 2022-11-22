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

- Выводит False, потому что SDK будет работать только на локальном сервире, либо на сервисе Яндекс.Игр. Поэтому нужно создать сборку, и загрузить ее на Яндекс.Игры, после чего можно будет проверить функционал скрипта.
- Создадим сборку тем же способам, каким создавали в прошлых работах и загрузим .zip файл на сервис. Так же нужно будет поставить галочку напротив поддержки авторизации.

![image](https://user-images.githubusercontent.com/54228342/203310614-a2f1eb6c-2e1e-41ef-9aac-1abdeddd9a5e.png)

- После загрузки выйдем из аккаунта Янедкс.Игр и попробуем зайти в игру.

## Задание 2
### Описать не менее трех дополнительных функций Яндекс SDK, которые могут быть интегрированы в игру.

Ход работы:



## Задание 3
### Доработать стилистическое оформление списка лидеров и системы достижений, реализованных в задании 1

Ход работы:



## Выводы


