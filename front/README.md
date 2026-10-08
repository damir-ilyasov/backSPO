# SPO Frontend — Аренда складских ячеек

SPA для просмотра и бронирования складских ячеек.
Чистый HTML / CSS / JavaScript, без фреймворков и сборщиков.

Связанный проект: [backSPO](https://github.com/damir-ilyasov/backSPO) — бэкенд на ASP.NET Core.

## Статус


Реализовано:

- вход и регистрация;
- JWT-авторизация;
- роли Client и Administrator;
- вкладки: ячейки, склады, мои аренды;
- бронирование ячейки;
- удаление ячейки (только администратор);
- просмотр складов;
- просмотр и отмена аренд.

Не реализовано:

- создание ячеек и складов через интерфейс;
- переключение на реальный API;
- фильтрация аренд по пользователю.

## Запуск

1. Открыть папку в VS Code.
2. Установить расширение Live Server.
3. ПКМ по `index.html` → Open with Live Server.
4. Открыть `http://127.0.0.1:5500`.

Бэкенд и база данных не нужны.

## Проверка

Администратор: первый зарегистрированный пользователь получает роль Administrator. Видит кнопки «Забронировать» и «Удалить».

Клиент: второй и последующие пользователи. Видят только кнопку «Забронировать».

После бронирования ячейка получает статус «занята», состояние сохраняется после F5.

## Структура

```
spo-frontend/
├── index.html   — разметка экранов
├── styles.css   — стили
├── mock.js      — заглушка API на localStorage
├── app.js       — логика приложения
└── README.md
```

## Переключение на реальный API

В `app.js`:

```js
const USE_MOCK = true;                   // false — использовать реальный API
const API = 'https://localhost:7001';    // адрес бэкенда
```

При `USE_MOCK = false` запросы идут на бэкенд. Порт взять из `launchSettings.json`.

## Эндпоинты

Auth:

```
POST /Auth/register   { email, password, fullName }   -> { token }
POST /Auth/login      { email, password }             -> { token }
```

StorageCell:

```
GET    /StorageCell                 -> список ячеек
GET    /StorageCell/{id}            -> одна ячейка
POST   /StorageCell/{id}/reserve    -> 204
DELETE /StorageCell/{id}            -> 204
```

WareHouse:

```
GET /WareHouse        -> список складов
GET /WareHouse/{id}   -> один склад
```

Rentalagreement:

```
GET    /api/Rentalagreement                        -> список аренд
POST   /api/Rentalagreement?currentUserId=<guid>   -> guid
DELETE /api/Rentalagreement/{id}                   -> 204
```

## CORS

Бэкенд должен разрешить origin `http://127.0.0.1:5500`.
В `Program.cs` до `builder.Build()`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
              .AllowAnyHeader()
              .AllowAnyMethod());
});
```

После `var app = builder.Build();` до `app.UseAuthentication();`:

```csharp
app.UseCors("Frontend");
```

На время разработки `app.UseHttpsRedirection()` можно закомментировать.

## Роли

Регистрация в бэкенде создаёт пользователя с ролью Client.
В mock-режиме первый зарегистрированный получает роль Administrator — это нужно
только для демонстрации интерфейса.

Роль читается из JWT:

```js
const payload = decodeJwt(token.get());
const isAdmin = payload?.role === 'Administrator';
```

