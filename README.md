# tets-pi-i

## Импорт и экспорт CSV

Приложение поддерживает экспорт текущего списка IP-адресов и обратный импорт из CSV-файла.

### Экспорт

1. Настройте фильтры в верхней части окна, чтобы оставить только нужные записи.
2. Нажмите кнопку **«Экспорт CSV»** и сохраните файл.

### Импорт

1. Подготовьте CSV-файл в кодировке UTF-8 со следующими колонками:
   - `SystemName`
   - `Environment` (`Test`, `Production` или `Both`)
   - `IPAddress`
   - `IsRegisteredInNamen`
   - `NamenRequestNumber`
   - `RegistrationDate`
   - `Description`
   - `CuratorName`
   - `CuratorEmail`
   - `OwnerName`
   - `OwnerEmail`
   - `TechnicalSpecialistName`
   - `TechnicalSpecialistEmail`

   Необязательные поля можно оставить пустыми. Для примера структуры файла используйте
   [`Samples/import_example.csv`](Samples/import_example.csv).

2. Нажмите **«Импорт CSV»** и выберите подготовленный файл.
3. После завершения импорта появится сводка с количеством добавленных и обновленных записей.
