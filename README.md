# Identity-Server
Authrization server using OAuth 2.0 and IdentityServer4 with test api, test mvc-client and test api-client
    
![alt text](https://user-images.githubusercontent.com/36980493/118080135-3f2eef80-b3c2-11eb-9eb5-1c20abd5aacf.png)  
  
During the work on the project:  
• The Authorization Server was designed using IdentyServer4 and OAuth 2.0, which supports various types of authorization (authorization code, implicit, client credentials, hybrid)  
• A database (PostgeSQL) has been created to store user models and configuration of the authorization server (information about client applications, settings for access tokens, api resources, orders and volumes)  
• Possibility of authorization using both access_token and cookies (the ability to authorize both specific users and external services)  
• Developed and configured a test API with a closed authorization method  
• Developed and configured a test MVC client and an API client for testing various authorization options through the server  
• The architecture and methods of interaction of the authorization server with closed APIs and client applications have been developed  
• An API has been added to the authorization server for managing users, their roles and items  
• User data management and corresponding API methods are implemented using SQRC and Mediator templates (using MediatR library), as well as EntityFramework Core as ORM
• Policy and claims used to control user and service access  
• Implemented support for refresh_token and offline_access  
• Added additional settings for SameSiteCookiePolicy for correct operation in newer versions of browsers  
  
------------------------------------------------------------------------------------------  
  
В ходе работы над проектом:   
• Разработан сервер авторизации с использованием IdentyServer4 и протокола OAuth 2.0, поддерживающий различные типы авторизации (Authorization code, Implicit, Client credentials, Hybrid)  
• Создана база данных (PostgeSQL) для хранения моделей пользователей и конфигурации сервера авторизации (информация о приложениях-клиентах, настройки для access tokens, api resources, claims и scopes)  
• Возможность авторизации как с помощью access_token, так и с помощью cookies (возможность авторизации как конкретных пользователей, так и внешних сервисов)  
• Разработано и настроено тестовое API с закрытым авторизацией методом   
• Разработаны и настроены тестовые MVC-клиент и API-клиент для проверки различных вариантов авторизации через сервер  
• Проработана архитектура и способы взаимодействия для сервера авторизации с закрытыми API и приложениями-клиентами   
• К серверу авторизации добавлено API для управления пользователями, их ролями и клаймами  
• Управление данными пользователей и соответсвующие API-методы реализованы с помощью паттернов SQRC и Mediator (использована библиотека MediatR), а также EntityFramework Core в качестве ORM  
• Использованы policy и claims для управления доступом пользователей и сервисов  
• Реализована поддержка refresh_token и offline_access  
• Добавлены дополнительные настройки для SameSiteCookiePolicy для корректной работы в новых версиях браузеров  
  
