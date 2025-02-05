#Первоначальная настройка
1. Создать папку FinShark
2. Открыть её в vsCode
3. Открыть терминал и запустить команду `dotnet new webapi -o api`
4. Скопировать файлы .gitignore и README.md (НЕ в папку api, а в корневую)
5. Открыть терминал и запустить команду `git init`
6. Запустить команду в терминале `git add`
7. Запустить команду в терминале `git commin -m 'init commit'`
8. Авторизоваться в https://github.com и создать public репозиторий с именем c--finshark
9. Скопировать нижние команды (их 3 строчки) и выполнить в терминале
10. Скопировать файл docker-compose.yml в папку api

#Инструкции по запуску
1. После открытия терминала перейти в папку api командой `cd api`
2. Запустить dotnet командой `dotnet watch run`
3. Открыть файл api/api.csproj и посмотреть версию зависимости <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.1" />. Значит остальные пакеты нужно искать под версию 9.0.1