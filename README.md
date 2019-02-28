# WinColourLabels

![IconOverlay](./Docs/scr1.png)
![ContextMenu](./Docs/scr2.png)

## Инсталляция и деинсталляция
Для работы, компонент должен быть зарегистрирован в системе. Компонент можеть быть зарегистрирован двумя способами:

Установив компонент в глобальный кэш сборок, затем регистрируя его как COM-сервер.
Или разместить компонент свободно в файловой системе, а затем зарегистрировать с параметром /codebase.

Инсталляция и деинсталляция компонента в системе производиться утилитами [`regasm`](https://docs.microsoft.com/ru-ru/dotnet/framework/tools/regasm-exe-assembly-registration-tool) и [`gacutils`](https://docs.microsoft.com/ru-ru/dotnet/framework/app-domains/how-to-install-an-assembly-into-the-gac).

### Инсталляция (с использованием GAC):
```
gacutil -i WinColourLabels.dll
regasm WinColourLabels.dll
```

### Инсталляция (без использования GAC):
```
regasm /codebase WinColourLabels.dll
```

### Деинсталляция:
```
regasm /u WinColourLabels.dll
```
