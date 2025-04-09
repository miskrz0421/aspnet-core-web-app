using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

namespace PiwkoMozna.Commons;
public static class Database
{

    public static void CreateDatabase(){
        var connectionStringBuilder = new SqliteConnectionStringBuilder{
            DataSource = "./PiwkoMozna.Data.db"
        };
        using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
        connection.Open();
        InitializeDatabase(connection);
    }

    static void InitializeDatabase(SqliteConnection connection)
    {
        // var command = connection.CreateCommand();
        // command.CommandText = @"
        //     DROP TABLE IF EXISTS Reviews;
        //     DROP TABLE IF EXISTS Beers;
        //     DROP TABLE IF EXISTS Users;
        //     DROP TABLE IF EXISTS Breweries;

        //     CREATE TABLE IF NOT EXISTS Users (
        //         UserID INTEGER PRIMARY KEY AUTOINCREMENT,
        //         Email TEXT NOT NULL UNIQUE,
        //         Password TEXT NOT NULL,
        //         IsAdmin TEXT NOT NULL
        //     );

        //     CREATE TABLE IF NOT EXISTS Breweries (
        //         BreweryName TEXT PRIMARY KEY,
        //         City TEXT NOT NULL,
        //         Country TEXT NOT NULL,
        //         Founded INTEGER,
        //         Description TEXT
        //     );

        //     CREATE TABLE IF NOT EXISTS Beers (
        //         BeerName TEXT PRIMARY KEY,
        //         BreweryName INTEGER NOT NULL,
        //         Style Text,
        //         ABV REAL,
        //         AverageRating REAL,
        //         FOREIGN KEY (BreweryName) REFERENCES Breweries(BreweryName)
        //     );

        //     CREATE TABLE IF NOT EXISTS Reviews (
        //         ReviewID INTEGER PRIMARY KEY AUTOINCREMENT,
        //         UserID INTEGER NOT NULL,
        //         BeerName INTEGER NOT NULL,
        //         Rating INTEGER NOT NULL,
        //         Comment TEXT,
        //         ReviewDate DATETIME DEFAULT CURRENT_TIMESTAMP,
        //         FOREIGN KEY (UserID) REFERENCES Users(UserID),
        //         FOREIGN KEY (BeerName) REFERENCES Beers(BeerName)
        //     );
        // ";

        // command.ExecuteNonQuery();
        var command = connection.CreateCommand();

        var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = @"
            SELECT COUNT(1)
            FROM UzytkownikModel
        ";
        var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
        if(!exists) {
            string adminEmail = "admin@piwkomozna.com";
            string adminPassword = "admin";

            var adminPasswordHash = PiwkoMozna.Commons.Hash.CalculateMD5Hash(adminPassword);
            command.CommandText = $"INSERT INTO UzytkownikModel (UserID, Email, Password,IsAdmin,Token) VALUES (1, '{adminEmail}', '{adminPasswordHash}', 1, '{Commons.Tokens.GenerateToken()}' )";
            command.ExecuteNonQuery();
        }

        checkCommand.CommandText = @"
            SELECT COUNT(1)
            FROM BrowarModel
        ";

        exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
        if(!exists) {
            command.CommandText = @"
            INSERT INTO BrowarModel (BreweryName, City, Country, Founded, Description) VALUES
            ('Browar Tyskie', 'Tychy', 'Polska', 1629, 'Największy producent piwa w Polsce.'),
            ('Browar Żywiec', 'Żywiec', 'Polska', 1856, 'Polski producent piwa o międzynarodowej renomie.'),
            ('Browar Lech', 'Poznań', 'Polska', 1975, 'Popularny producent piwa w Polsce i Europie.'),
            ('Browar Perła', 'Browary Lubelskie', 'Polska', 1846, 'Tradycyjny polski browar z bogatą historią.'),
            ('Browar Okocim', 'Brzesko', 'Polska', 1845, 'Jeden z najstarszych browarów w Polsce.'),
            ('Browar Warka', 'Warka', 'Polska', 1478, 'Jeden z najstarszych browarów w Polsce o bogatej tradycji.'),
            ('Browar Łomża', 'Łomża', 'Polska', 1968, 'Popularny producent piwa w Polsce.'),
            ('Browar EB', 'Leżajsk', 'Polska', 1995, 'Nowoczesny browar z bogatą ofertą piw.'),
            ('Browar Ciechan', 'Ciechanów', 'Polska', 1864, 'Mały browar specjalizujący się w piwach rzemieślniczych.'),
            ('Browar Śląskie', 'Katowice', 'Polska', 1846, 'Polski producent piwa o bogatej tradycji i historii.');
            ";
            command.ExecuteNonQuery();
        }        

        checkCommand.CommandText = @"
            SELECT COUNT(1)
            FROM PiwoModel
        ";

        exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
        if(!exists) {
            command.CommandText = @"
            INSERT INTO PiwoModel (BeerName, BreweryName, Style, ABV, AverageRating) VALUES
            ('Tyskie', 'Browar Tyskie', 'Jasne', 5.0, 0.0),
            ('Żywiec', 'Browar Żywiec', 'Jasne', 5.4, 0.0),
            ('Lech', 'Browar Lech', 'Jasne', 5.2, 0.0),
            ('Perła', 'Browar Perła', 'Jasne', 5.6, 0.0),
            ('Okocim', 'Browar Okocim', 'Jasne', 6.2, 0.0),
            ('Warka', 'Browar Warka', 'Jasne', 4.9, 0.0),
            ('Łomża', 'Browar Łomża', 'Jasne', 5.5, 0.0),
            ('EB', 'Browar EB', 'Jasne', 4.5, 0.0),
            ('Ciechan', 'Browar Ciechan', 'Jasne', 4.8, 0.0),
            ('Śląskie', 'Browar Śląskie', 'Ciemne', 5.8, 0.0)
            ";
            
            command.ExecuteNonQuery();
        }       
    }
}

