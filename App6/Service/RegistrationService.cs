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
  public static class RegistrationService
  {
    static SQLiteAsyncConnection db;
    public static SQLiteConnection db_root;
    static List<Registration> users;
    static SQLiteConnection GetSQLConnection()
    {
      var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "XamarinProject.db");

      db_root = new SQLiteConnection(databasePath);
      return db_root;
    }
    static async Task Init()
    {
      if (db != null)
        return;

      // Get an absolute path to the database file
      var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "XamarinProject.db");

      db = new SQLiteAsyncConnection(databasePath);

      await db.CreateTableAsync<Registration>();
    }

    public static async Task AddRegistration(string firstLast, string username, string email, string password, string createdDate)
    {
      await Init();
      var registration = new Registration
      {
        FirstLastName = firstLast,
        Username = username,
        Email = email,
        Password = password,
        CreatedDate = createdDate
      };

      var id = await db.InsertAsync(registration);
    }

    public static void NewAddRegistration(SQLiteConnection db,string firstLast, string username, string email, string password, string createdDate)
    {
      var registration = new Registration
      {
        FirstLastName = firstLast,
        Username = username,
        Email = email,
        Password = password,
        CreatedDate = createdDate
      };

      var id = db.Insert(registration);
    }

    public static Boolean CheckUsername(SQLiteConnection db, string firstLast, string username, string email, string password, string createdDate)
    {
      var query = db.Query<Registration>("select * from Registration where Email = ?", email);
      Boolean valid = (query == null)? false: true; ;
      

      return valid;
    }

   
    public static async Task RemoveRegistration(int id)
    {

      await Init();

      await db.DeleteAsync<Registration>(id);
    }


    public static async Task<Boolean> CheckEmailPassword(string email, string password, Context context)
    {
      await Init();
      var myTask = Task.Run(() => GetRegistration());

      List<Registration> result = await myTask;

      Boolean isValid = false;
      foreach (var item in result)
      {
        if (item.Email.Equals(email) && item.Password.Equals(password))
        {
          isValid = true;
          SessionManagement sessionManagement = new SessionManagement(context);
          sessionManagement.saveSession(item.Id);
        }
      }

      return isValid;
    }


    public static async Task<Registration> CheckEmailPassword2(string email, string password, Context context)
    {
      await Init();
      var myTask = Task.Run(() => GetRegistration());

      List<Registration> result = await myTask;
      Registration finalResult = new Registration();
      Boolean isValid = false;
      foreach (var item in result)
      {
        if (item.Email.Equals(email) && item.Password.Equals(password))
        {
          isValid = true;
          SessionManagement sessionManagement = new SessionManagement(context);
          sessionManagement.saveSession(item.Id);

          finalResult = item;
        }
      }

      return finalResult;
    }


    public static async Task<List<Registration>> GetRegistration()
    {
      await Init();

      var registration = await db.Table<Registration>().ToListAsync();
      return registration;
    }
    public static async Task<Registration> GetRegistration(int id)
    {
      await Init();

      var registration = await db.Table<Registration>()
          .FirstOrDefaultAsync(c => c.Id == id);

      return registration;
    }

  }
}