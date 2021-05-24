using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App6.Model;
using App6.Services;

namespace App6.Service
{
  public class SessionManagement
  {
    ISharedPreferences sharedPreferences;
    ISharedPreferencesEditor editor;
    string SESSION_KEY = "session_user";
    Context newContext;

    public SessionManagement(Context context)
    {
      newContext = context;
      sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(newContext);
      editor = sharedPreferences.Edit();
    }

    public void saveSession(int loggedUser)
    {
      editor.PutInt(SESSION_KEY, loggedUser);
      editor.Commit();
    }

    public async Task<Registration> getSession()
    {
      Registration loggedInUser = new Registration();
      loggedInUser = await RegistrationService.GetRegistration(sharedPreferences.GetInt(SESSION_KEY, -1));


      if (loggedInUser == null)
      {
        loggedInUser = new Registration()
        {
            Id = -1,
            Email = "None",
            Password = "None",
            FirstLastName = "None",
            Username = "None",
            CreatedDate = "None"
        };

      }
      return loggedInUser;
    }

    public void removeSession()
    {
      editor.PutInt(SESSION_KEY, -1).Commit();
    }
  }
}