using System;
using System.Collections.Generic;
using System.IO;
using App6.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using App6.Service;

namespace App6.Services
{
  public static class LoginService
  {
    static SQLiteAsyncConnection db;
    static async Task Init()
    {
      if (db != null)
        return;

      // Get an absolute path to the database file
      var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "XamarinProject.db");

      db = new SQLiteAsyncConnection(databasePath);

      await db.CreateTableAsync<Login>();
    }

    public static async Task AddLogin(int id, int userId, string firstLast, string username, string email, string password, string createdDate)
    {
      await Init();
      var login = new Login
      {
        Id = id,
        UserId = userId,
        FirstLastName = firstLast,
        Username = username,
        Email = email,
        Password = password,
        CreatedDate = createdDate
      };

      var checkId = await db.InsertAsync(login);
    }

    public static async Task RemoveLogin(int id)
    {

      await Init();

      await db.DeleteAsync<Login>(id);
    }

    //public static async Task<IEnumerable<Login>> GetLogin()
    public static async Task<List<Login>> GetLogin()
    {
      await Init();

      var login = await db.Table<Login>().ToListAsync();
      //var login = await db.Table<Login>().ToList();
      return login;
    }

    public static async Task<Login> GetLogin(int id)
    {
      await Init();

      var login = await db.Table<Login>()
          .FirstOrDefaultAsync(c => c.Id == id);

      Helpers helper = new Helpers();
      login = (login == null) ? helper.CreateDummy() : login;
      return login;
    }
  }
}