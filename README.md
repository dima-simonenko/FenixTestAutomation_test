# FenixTestAutomation_test
Проект для автоматизации тестирования функций создания и редактирования 3D-моделей топологии в приложении Fenix.  

## Основные возможности:
- Активация инструментов через горячие клавиши.
- Проверка корректности активации инструментов.
- Автоматизация рисования стены с заданной длиной.
- Сохранение скриншотов на каждом этапе теста.
- Генерация отчётов в формате Allure (ручная генерация JSON).
- Поддержка .NET Framework.

## Структура тестов:
- **FenixHotkeyTests.cs** — Проверяет активацию всех инструментов в различных типах проектов.
- **FenixWallDrawingTest.cs** — Тест рисования стены с сохранением скриншотов.

## Как запускать:
1. Открыть проект в Visual Studio.
2. Запустить тесты через Test Explorer.
3. Для формирования Allure отчёта:
   ```powershell
   allure generate C:\Users\dimas\Documents\AllureResults -o C:\Users\dimas\Documents\AllureReport --clean
   allure open C:\Users\dimas\Documents\AllureReport

   ## Примечание:
   Тесты не стабильные. Проблемы, которые нужно решить: в Allure не передаётся JSON c параметрами теста на размещение стены в редакторе топологии.
   
<img width="1244" alt="image" src="https://github.com/user-attachments/assets/66f812d5-e3b8-4e37-a8f1-26682147fda6" />
