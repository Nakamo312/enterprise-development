# Лабораторная работа 2: «Сервер» - Реализация серверного приложения с использованием REST API и .NET Aspire

## Уровни приложения

### 1. **Clinic.Domain - Доменный слой**
- **Сущности бизнес-логики:** `Patient`, `Doctor`, `Specialization`, `Appointment`
- **Базовый класс:** `Model` - общая основа для всех сущностей
- **Перечисления:** `BloodGroup`, `Gender`, `RhFactor` - типизированные значения
- **Данные:** `DataSeed` - начальные данные для инициализации

### 2. **Clinic.Application - Слой приложения**
- **DTO (Data Transfer Objects):**
  - `{Entity}CreateDto` – для создания данных с валидацией
  - `{Entity}UpdateDto` – для обновления с частичными изменениями  
  - `{Entity}ResponseDto` – для чтения данных
- **Сервисы:**
  - `ICrudService<TDto, TCreateDto, TUpdateDto>` - generic интерфейс CRUD операций
  - `BaseCrudService<TModel, TDto, TCreateDto, TUpdateDto>` - базовая реализация сервиса
- **Маппинг:** `MappingProfile` - AutoMapper для преобразования
- **Валидация:** `EnumRangeAttribute` - кастомные валидаторы

### 3. **Clinic.Infrastructure - Слой инфраструктуры**
- **Реализации репозиториев:**
  - `IRepository<T>` - generic интерфейс репозитория
  - `Repository<T>` - EF Core реализация для продакшена
- **База данных:**
  - `AppDbContext` - конфигурация Entity Framework Core с PostgreSQL
  - **Миграции:** Автоматическое создание и применение миграций через EF Core
  - **Конфигурации:** `{Entity}Configuration` - настройка моделей и seed данных через `HasData()`
- **Data Seeding:** `EfDataSeeder` - заполнение БД начальными данными

### 4. **Clinic.API - Презентационный слой**
- **Базовый контроллер:** `CrudControllerBase<TDto, TCreateDto, TUpdateDto>` - общая CRUD логика и маршрутизация
- **Middleware:** Централизованная обработка запросов и исключений
- **Интеграция с Aspire:** Использование `AddNpgsqlDbContext` для конфигурации БД

### 5. **Clinic.ServiceDefaults - Общие сервисы Aspire**
- **Конфигурация по умолчанию:** Стандартные настройки для микросервисов
- **Service Discovery:** Автоматическое обнаружение сервисов
- **OpenTelemetry:** Инструментирование и мониторинг

### 6. **Clinic.AppHost - Хостинг приложения Aspire**
- **Оркестрация:** Управление всеми сервисами приложения
- **Конфигурация окружения:** Настройки разработки и продакшена
- **Зависимости:** Управление PostgreSQL контейнером и другими ресурсами
- **Развертывание:** Единая точка входа для запуска всего приложения
