# Uczelniana Wypożyczalnia Sprzętu (APBD)

Projekt zrealizowany w ramach przedmiotu Aplikacje Baz Danych (APBD). 
Jest to konsolowa aplikacja w języku C# służąca do zarządzania uczelnianą wypożyczalnią sprzętu. 
Głównym celem projektu było poprawne zastosowanie zasad programowania obiektowego, 
SOLID oraz wyraźny podział odpowiedzialności klas.

## Instrukcja uruchomienia

Aplikacja jest standardowym projektem konsolowym platformy .NET. Można ją uruchomić na dwa sposoby:

1. Przez terminal:
    - Sklonuj repozytorium na swój komputer.
    - Otwórz terminal w głównym folderze projektu (tam, gdzie znajduje się plik .csproj).
    - Wpisz komendę: dotnet run

2. Przez środowisko programistyczne:
    - Otwórz plik APBD-Cw1-s30790.sln w programie Visual Studio lub JetBrains Rider.
    - Uruchom projekt domyślnym przyciskiem "Start".

Po uruchomieniu aplikacja automatycznie wykona scenariusz demonstracyjny zdefiniowany w pliku Program.cs, pokazując działanie wszystkich reguł biznesowych.

## Decyzje projektowe i architektura

Kod został podzielony na warstwy, aby zapewnić wysoką kohezję (spójność) i zminimalizować coupling (sprzężenie) między klasami. Cała logika biznesowa została wyciągnięta z pliku Program.cs.

1. Modele (folder Models)
   Odpowiadają wyłącznie za reprezentację danych i podstawowych stanów. Zastosowano tu dziedziczenie (klasy abstrakcyjne Sprzet i Uzytkownik), ale tylko dlatego, że wynikało to wprost z domeny biznesowej. Wspólne cechy (jak Id, Status) znajdują się w klasie bazowej, a specyficzne właściwości w klasach pochodnych (np. Student, Pracownik, Laptop).

2. Warstwa logiki (folder Services)
   Wszelkie operacje biznesowe, takie jak wypożyczanie, zwroty i sprawdzanie limitów, zostały scentralizowane w klasie WypozyczalniaService. Dzięki temu klasa ta ma jedną, wyraźną odpowiedzialność (Single Responsibility Principle).

Aby uniknąć silnego sprzężenia (couplingu), logika wyliczania kar finansowych została wyodrębniona do interfejsu IKalkulatorKar. Serwis wypożyczalni przyjmuje ten interfejs w konstruktorze (Dependency Injection). Jeśli w przyszłości zmienią się zasady naliczania kar, wystarczy napisać nową klasę implementującą ten interfejs, bez konieczności modyfikowania kodu samej wypożyczalni (Open/Closed Principle).

3. Obsługa błędów (folder Exceptions)
   Zamiast rzucać ogólne wyjątki systemu lub wypisywać błędy bezpośrednio do konsoli z poziomu serwisu, stworzono dedykowane klasy wyjątków (np. RegulaBiznesowaException). Serwis jawnie sygnalizuje problem (np. sprzęt jest niedostępny lub limit został przekroczony), a ostateczną decyzję o tym, jak ten błąd zaprezentować użytkownikowi, podejmuje warstwa prezentacji (Program.cs).

4. Warstwa prezentacji (Program.cs)
   Odpowiada wyłącznie za spięcie wszystkich elementów (wstrzyknięcie zależności), dodanie danych testowych i wyświetlenie wyników w konsoli. Nie ma tu żadnej logiki domenowej.