# Aplikacja Webowa ASP.NET Core Web App

## Autorzy
- **[Michał Krzempek](https://github.com/miskrz0421)**
- **[Kacper Machnik](https://github.com/KacperMachnik)**

## Opis
Aplikacja ASP.NET Core Web App to aplikacja internetowa napisana w technologii ASP.NET Core MVC, która umożliwia użytkownikom przeglądanie, recenzowanie i ocenianie różnych piw. Aplikacja korzysta z mechanizmu sesji do zapamiętywania zalogowania użytkowników oraz przechowuje dane w bazie danych SQLite. Hasła użytkowników są przechowywane w postaci skrótu (hashu) dla zwiększenia bezpieczeństwa. Dodatkowo aplikacja oferuje interfejs REST API do zarządzania danymi.

## Funkcjonalności
- **Rejestracja i logowanie użytkowników**:  
  Domyślnie tylko administrator może dodawać nowych użytkowników do bazy. Użytkownicy mogą logować się do aplikacji, a ich hasła są przechowywane w postaci skrótu (hashu).  
- **Przeglądanie piw**:  
  Zalogowani użytkownicy mogą przeglądać listę dostępnych piw wraz ze szczegółowymi informacjami, takimi jak nazwa, procent alkoholu, styl i producent.  
- **Recenzje i oceny**:  
  Zalogowani użytkownicy mogą dodawać recenzje i oceny do piw. Każda recenzja zawiera komentarz oraz ocenę w skali od 0 do 10.  
- **Przeglądanie browarów**:  
  Dostępne są informacje o browarach, takie jak nazwa, miasto i kraj. Piwa są powiązane z ich producentami.  
- **REST API**:  
  Użytkownicy z uprawnieniami administratora mogą zarządzać browarami za pomocą REST API. Autoryzacja żądań odbywa się za pomocą tokenów.  
- **Uprawnienia**:  
  Administratorzy mają pełny dostęp do bazy danych oraz operacji CRUD (Create, Read, Update, Delete) na wszystkich modelach.  

## Tabele w bazie danych
### Schemat bazy danych
#### Users
- `UserID` (int) - klucz główny  
- `Email` (string) - adres e-mail użytkownika  
- `Password` (string) - hasło użytkownika (przechowywane w postaci skrótu)  
- `IsAdmin` (bool) - czy użytkownik jest administratorem  
- `Token` (string) - token do autoryzacji REST API  

#### Beers
- `BeerName` (string) - nazwa piwa (klucz główny)  
- `BreweryName` (string) - klucz obcy odnoszący się do tabeli `Breweries`  
- `Style` (string) - styl piwa (np. jasne, ciemne)  
- `ABV` (float) - procent alkoholu  
- `AverageRating` - średnia ocen  

#### Reviews
- `ReviewID` (int) - klucz główny  
- `UserID` (int) - klucz obcy odnoszący się do tabeli `Users`  
- `BeerName` (string) - klucz obcy odnoszący się do tabeli `Beers`  
- `Rating` (int) - ocena (1-5)  
- `Comment` (string) - komentarz  
- `ReviewDate` (DateTime) - data wprowadzenia recenzji  

#### Breweries
- `BreweryName` (string) - nazwa browaru (klucz główny)  
- `City` (string) - miasto  
- `Country` (string) - kraj  
- `Founded` (int) - rok założenia  
- `Description` (string) - opis browaru  

## Sposób użycia
### Początek
1. Zaloguj się jako administrator:  
   - **E-mail**: `admin@mail.com`  
   - **Hasło**: `admin`  
2. Po zalogowaniu masz pełny dostęp do aplikacji.  
3. Utwórz nowego użytkownika, a następnie wyloguj się.  

### Logowanie
1. Przejdź do strony logowania.  
2. Wprowadź swój adres e-mail oraz hasło.  
3. Po zalogowaniu możesz przeglądać piwa, browary oraz dodawać recenzje.  

### Dodawanie recenzji
1. Na stronie katalogu piw wybierz piwo.  
2. Wprowadź ocenę (w skali 0-10) oraz komentarz.  
3. Zatwierdź, aby dodać recenzję.  
