# todo-bot

## Цель бота - дать пользователю возможность вести список дел (ToDo List). Бот должен обладать следующими функциями: 

* создание нового дела

* показ списка дел на сегодня

* напоминание о деле

* подведение итогов дня

* список дел по дате

  # Быстрый старт🚀
Написать в Telegram в сервис создания ботов - [BotFather](https://t.me/BotFather). Регистрируем бота по инструкции которую он даст. BotFather вышлет токен, который понадобится далее.
## Запуск через Rider
1. Создаешь .NET User Secrets (В райдере правой кнопкой по проекту Tools -> .NET User Secrets)
2. Откроется локальный файл конфигов, куда нужно вставить следущее:
```json
{
  "TelegramOptions":{
    "Token":"твой токен"
  },
  "ConnectionStrings": {
    "Database" : "User ID=;Password=;Host=;Port=;Database=BotRps;"
  }
}
```

## Запуск через Docker
Для запуска понадобится установленный на машине докер и база данных
```bash
docker run -d --restart=always --name botrps -e ConnectionStrings__Database='User ID=;Password=;Host=;Port=;Database=BotRps;' -e TelegramOptions__Token='твой токен' wxhami/todo-bot:latest
```



