using System;
using System.Collections.Generic;
using System.Linq;
using Felipecsl.GifImageViewLibrary;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Content.Res;
using App6.Model;

namespace App6.Service
{
  public class Helpers
  {
    ProgressDialog progress;
    public void showLoader(Context appContext)
    {
      progress = new ProgressDialog(appContext);
      progress.SetCancelable(false);
      progress.SetMessage("Loading");
      progress.SetProgressStyle(ProgressDialogStyle.Spinner);
      progress.Show();

    }
    public void hideLoader()
    {
      progress.Hide();

    }
    public bool isValidEmail(string email)
    {
      return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
    }

    public Login CreateDummy()
    {
      Login dummy = new Login()
      {
        Id = -1,
        UserId = -1,
        Email = "None",
        Password = "None",
        FirstLastName = "None",
        Username = "None",
        CreatedDate = "None"
      };

      return dummy;
    }
  }

}
