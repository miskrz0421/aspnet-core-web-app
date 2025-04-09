using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PiwkoMozna.Models;

namespace PiwkoMozna.Controllers{
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly string DatabaseName = "./PiwkoMozna.Data.db";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
   
    public IActionResult Dokumentacja()
    {
        return View();
    }
        [HttpPost]
    public IActionResult Login(UzytkownikModel model)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = DatabaseName };
        using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
        connection.Open();

        HttpContext.Session.SetString("IsLoggedIn", "false");
        HttpContext.Session.SetString("IsAdmin", "False");

        var verifiedUser = LoginVerify(connection, model.Email, model.Password);
        if (verifiedUser != null)
        {
            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("email", verifiedUser.Email);
            HttpContext.Session.SetString("IsAdmin", verifiedUser.IsAdmin.ToString());
            return RedirectToAction("Katalog", "PiwoModel", model);
        }
        else
        {
            ViewBag.ErrorMessage = "Nieprawidłowy adres e-mail lub hasło.";
            return View("Index");
        }
    }

    public UzytkownikModel ? LoginVerify(SqliteConnection connection, string email, string password)
    {
        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Email, IsAdmin, Token
            FROM UzytkownikModel
            WHERE Email = @Email AND Password = @Haslo;
        ";
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Haslo", PiwkoMozna.Commons.Hash.CalculateMD5Hash(password));
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new UzytkownikModel
            {
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Password = PiwkoMozna.Commons.Hash.CalculateMD5Hash(password),
                IsAdmin = reader.GetBoolean(reader.GetOrdinal("IsAdmin")),
                Token = reader.GetString(reader.GetOrdinal("Token"))
            };
        }
        return null;
    }
    
    

    public IActionResult Logout()
    {
        HttpContext.Session.SetString("IsLoggedIn", "false");
        HttpContext.Session.SetString("IsAdmin", "False");
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

}

