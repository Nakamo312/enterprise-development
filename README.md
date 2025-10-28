## Лабораторная работа 2: «Сервер» - Реализация серверного приложения с использованием REST API

### Уровни приложения

#### 1. **Clinic.Domain - Доменный слой**
- **Сущности бизнес-логики:** `Patient`, `Doctor`, `Specialization`, `Appointment`
- **Базовый класс:** `Model` - общая основа для всех сущностей
- **Перечисления:** `BloodGroup`, `Gender`, `RhFactor` - типизированные значения
- **Данные:** `DataSeed` - начальные данные для инициализации

#### 2. **Clinic.Application - Слой приложения**
- **DTO (Data Transfer Objects):**
  - `{Entity}CreateDto` – для создания данных с валидацией
  - `{Entity}UpdateDto` – для обновления с частичными изменениями  
  - `{Entity}ResponseDto` – для чтения данных
- **Сервисы:**
  - `ICrudService<TDto, TCreateDto, TUpdateDto>` - generic интерфейс CRUD операций
  - `BaseCrudService<TModel, TDto, TCreateDto, TUpdateDto>` - базовая реализация сервиса
- **Маппинг:** `MappingProfile` - AutoMapper для преобразования
- **Валидация:** `EnumRangeAttribute` - кастомные валидаторы

#### 3. **Clinic.Infrastructure - Слой инфраструктуры**
- **Реализации репозиториев:**
  - `IRepository<T>` - generic интерфейс репозитория
  - `InMemoryRepository<T>` - in-memory реализация 
  - `Repository<T>` - EF Core реализация для продакшена
- **База данных:**
  - `AppDbContext` - конфигурация Entity Framework Core

#### 4. **Clinic.API - Презентационный слой**
- **REST API контроллеры** для всех сущностей
- **Базовый контроллер:** `CrudControllerBase<TDto, TCreateDto, TUpdateDto>` - общая CRUD логика
- **Обработка HTTP запросов** и маршрутизация
- **Middleware:** Централизованная обработка запросов и исключений