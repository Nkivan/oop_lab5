# Схеми до лабораторної роботи 3.5 - варіант 2

## 1. Архітектура та місце тестів

```mermaid
---
config:
  layout: elk
---
flowchart TD
    U(["Користувач"]) -->|"команди в консолі"| PL

    subgraph PL["Lab5.PL - представлення"]
        M["Menu, EntityMenu, ConsoleIO"]
        CR["Program: складання залежностей"]
    end

    subgraph BLL["Lab5.BLL - бізнес-логіка"]
        S["PersonService, StudentService"]
        MD["Моделі, Validator, винятки"]
    end

    subgraph DAL["Lab5.DAL - доступ до даних"]
        IR["IRepository"]
        JR["JsonFileRepository"]
    end

    F[("JSON-файли")]

    subgraph TESTS["Lab5.Tests - xUnit"]
        T["Модульні тести"]
        D["Двійники: Stub, Mock"]
        IT["Інтеграційні тести"]
    end

    M -->|"викликає"| S
    S --> MD
    S -->|"залежить від інтерфейсу"| IR
    JR -->|"реалізує"| IR
    JR -->|"читає і пише"| F
    CR -->|"передає JsonFileRepository"| S
    T -->|"перевіряє"| S
    T -->|"підставляє замість репозиторію"| D
    D -->|"реалізує"| IR
    IT -->|"перевіряє зі справжнім файлом"| JR
```

## 2. Діаграми класів

### 2.1. Моделі та винятки (BLL)

```mermaid
---
config:
  layout: elk
---
classDiagram
    direction TB

    class Person {
        <<abstract>>
        +string FirstName
        +string LastName
        +int Dances
        +string DriverLicense
        +string Key
        +Dance()
        +ObtainDriverLicense(results, licenseNumber)
    }

    class Student {
        +int Course
        +string StudentId
        +string ArrivalCity
        +string Passport
        +string DormitoryRoom
        +bool LivesInDormitory
        +MoveIntoDormitory(universityCity, dormitoryNumber, roomNumber)
    }

    class TaxiDriver {
        +int Trips
        +CompleteTrip()
    }

    class Acrobat {
        +string CertificateId
        +int ExperienceYears
        +AddExperienceYear()
    }

    class DrivingExamResults {
        +int Age
        +bool MedicalCertificate
        +bool TheoryExamPassed
        +bool PracticeExamPassed
    }

    class Validator {
        <<static>>
        +IsValid(pattern, value) bool
        +Require(pattern, value, field)
        +RequireRange(value, min, max, field)
    }

    class BllException
    class InvalidEntityDataException
    class DormitoryException
    class DriverLicenseException
    class EntityNotFoundException
    class DuplicateEntityException

    Person <|-- Student : успадковує
    Person <|-- TaxiDriver : успадковує
    Person <|-- Acrobat : успадковує
    Person ..> DrivingExamResults : перевіряє умови отримання посвідчення
    Person ..> Validator : контроль даних у конструкторі
    Person ..> DriverLicenseException : умови не виконано
    Student ..> DormitoryException : поселення неможливе
    Validator ..> InvalidEntityDataException : значення некоректне
    BllException <|-- InvalidEntityDataException : успадковує
    BllException <|-- DormitoryException : успадковує
    BllException <|-- DriverLicenseException : успадковує
    BllException <|-- EntityNotFoundException : успадковує
    BllException <|-- DuplicateEntityException : успадковує
```

### 2.2. Сервіси та ін'єкція залежностей

```mermaid
---
config:
  layout: elk
---
classDiagram
    direction TB

    class PersonService~TModel~ {
        -IRepository repository
        -IEntityMapper mapper
        +GetAll() TModel[]
        +Add(model)
        +Remove(key)
        +GetByKey(key) TModel
        +FindByLastName(lastName) TModel[]
        +Dance(key)
        +ObtainDriverLicense(key, results, licenseNumber)
        #Modify(key, change)
    }

    class StudentService {
        +GetFirstYearFromOtherCities(universityCity) FirstYearReport
        +MoveIntoDormitory(key, universityCity, dormitoryNumber, roomNumber)
    }

    class TaxiDriverService {
        +CompleteTrip(key)
    }

    class AcrobatService {
        +AddExperienceYear(key)
    }

    class FirstYearReport {
        +string UniversityCity
        +int FirstYearCount
        +int FromOtherCitiesCount
        +double Percent
        +Students
    }

    class IEntityMapper~TModel~ {
        <<interface>>
        +ToModel(entity) TModel
        +ToEntity(model) Entity
    }

    class IRepository~TEntity~ {
        <<interface>>
        +LoadAll() TEntity[]
        +SaveAll(entities)
    }

    class JsonFileRepository~TEntity~ {
        +LoadAll() TEntity[]
        +SaveAll(entities)
    }

    class DataAccessException

    PersonService~TModel~ <|-- StudentService : успадковує
    PersonService~TModel~ <|-- TaxiDriverService : успадковує
    PersonService~TModel~ <|-- AcrobatService : успадковує
    PersonService~TModel~ ..> IRepository~TEntity~ : отримує через конструктор (DI)
    PersonService~TModel~ ..> IEntityMapper~TModel~ : отримує через конструктор (DI)
    IRepository~TEntity~ <|.. JsonFileRepository~TEntity~ : реалізує
    JsonFileRepository~TEntity~ ..> DataAccessException : помилка файлу
    StudentService ..> FirstYearReport : створює результат
```

### 2.3. Тести та двійники

```mermaid
---
config:
  layout: elk
---
classDiagram
    direction TB

    class IRepository~TEntity~ {
        <<interface>>
        +LoadAll() TEntity[]
        +SaveAll(entities)
    }

    class StubRepository~TEntity~ {
        +LoadAll() TEntity[]
        +SaveAll(entities)
    }

    class MockRepository~TEntity~ {
        +int LoadAllCallCount
        +int SaveAllCallCount
        +LastSaved
        +LoadAll() TEntity[]
        +SaveAll(entities)
    }

    class FailingRepositoryStub~TEntity~ {
        +LoadAll() TEntity[]
        +SaveAll(entities)
    }

    class StudentServiceTests
    class StudentTests
    class DriverLicenseTests
    class ValidatorTests
    class JsonFileRepositoryIntegrationTests

    class StudentService
    class Student
    class JsonFileRepository~TEntity~

    IRepository~TEntity~ <|.. StubRepository~TEntity~ : реалізує
    IRepository~TEntity~ <|.. MockRepository~TEntity~ : реалізує
    IRepository~TEntity~ <|.. FailingRepositoryStub~TEntity~ : реалізує

    StudentServiceTests ..> StudentService : тестує
    StudentServiceTests ..> StubRepository~TEntity~ : підставляє заготовлені дані
    StudentServiceTests ..> MockRepository~TEntity~ : перевіряє виклики
    StudentServiceTests ..> FailingRepositoryStub~TEntity~ : імітує збій
    StudentTests ..> Student : тестує
    DriverLicenseTests ..> Student : тестує
    JsonFileRepositoryIntegrationTests ..> JsonFileRepository~TEntity~ : тестує зі справжнім файлом
```

## 3. Алгоритм Person.ObtainDriverLicense

```mermaid
---
config:
  layout: elk
---
flowchart TD
    A(["ObtainDriverLicense(results, licenseNumber)"]) --> B{"results = null?"}
    B -->|"так"| X1["ArgumentNullException"]
    B -->|"ні"| C{"Посвідчення вже є?"}
    C -->|"так"| X2["DriverLicenseException: вже отримано"]
    C -->|"ні"| D{"Вік менший за 18?"}
    D -->|"так"| X3["DriverLicenseException: мінімальний вік"]
    D -->|"ні"| E{"Немає медичної довідки?"}
    E -->|"так"| X4["DriverLicenseException: потрібна довідка"]
    E -->|"ні"| F{"Теоретичний іспит не складено?"}
    F -->|"так"| X5["DriverLicenseException: теорія"]
    F -->|"ні"| G{"Практичний іспит не складено?"}
    G -->|"так"| X6["DriverLicenseException: практика"]
    G -->|"ні"| H{"Номер має коректний формат?"}
    H -->|"ні"| X7["InvalidEntityDataException"]
    H -->|"так"| I["DriverLicense = licenseNumber"]
    I --> R(["Кінець: посвідчення видано"])
```

## 4. Алгоритм Student.MoveIntoDormitory

```mermaid
---
config:
  layout: elk
---
flowchart TD
    A(["MoveIntoDormitory(universityCity, dormitoryNumber, roomNumber)"]) --> B{"Місто університету коректне?"}
    B -->|"ні"| X1["InvalidEntityDataException"]
    B -->|"так"| C{"Студент уже в гуртожитку?"}
    C -->|"так"| X2["DormitoryException: уже проживає"]
    C -->|"ні"| D{"Місто прибуття збігається з містом університету?"}
    D -->|"так"| X3["DormitoryException: лише для приїжджих"]
    D -->|"ні"| E["room = номер гуртожитку, крапка, номер кімнати"]
    E --> F{"room відповідає формату?"}
    F -->|"ні"| X4["InvalidEntityDataException"]
    F -->|"так"| G["DormitoryRoom = room"]
    G --> R(["Кінець: студента поселено"])
```

## 5. Операція варіанта: відсоток студентів 1-го курсу з інших міст

```mermaid
---
config:
  layout: elk
---
flowchart TD
    A(["GetFirstYearFromOtherCities(universityCity)"]) --> B{"Місто університету коректне?"}
    B -->|"ні"| X["InvalidEntityDataException"]
    B -->|"так"| C["Прочитати всіх студентів з файлу через репозиторій"]
    C --> D{"Є ще студент?"}
    D -->|"так"| E{"Курс = 1?"}
    E -->|"ні"| D
    E -->|"так"| F["Додати до першокурсників"]
    F --> G{"Місто прибуття не збігається з містом університету?"}
    G -->|"так"| H["Додати до приїжджих"]
    G -->|"ні"| D
    H --> D
    D -->|"ні"| I{"Є першокурсники?"}
    I -->|"ні"| J["Відсоток = 0"]
    I -->|"так"| K["Відсоток = приїжджі / першокурсники * 100"]
    J --> R(["Повернути звіт"])
    K --> R
```

## 6. Взаємодія об'єктів у модульному тесті з мок-об'єктом

```mermaid
sequenceDiagram
    participant Test as Тест (xUnit)
    participant Mock as MockRepository
    participant Svc as StudentService
    participant Model as Student

    Note over Test: Arrange
    Test->>Mock: new MockRepository(дані)
    Test->>Svc: new StudentService(mock)

    Note over Test: Act
    Test->>Svc: Dance("KB123456")
    Svc->>Mock: LoadAll()
    Mock-->>Svc: сутності
    Svc->>Model: Dance()
    Svc->>Mock: SaveAll(оновлені сутності)

    Note over Test: Assert
    Test->>Mock: SaveAllCallCount = 1?
    Test->>Mock: LastSaved[0].Dances = 5?
    Mock-->>Test: так
```
