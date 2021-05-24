using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using System.Data;
using App6.Services;
using Android.Content;
using App6.Model;
using System.Threading.Tasks;
using Felipecsl.GifImageViewLibrary;
using System.IO;
using App6.Service;
using Newtonsoft.Json;

namespace App6
{
    [Activity(Theme ="@style/MyTheme" ,Icon ="@drawable/appicon", Label = "DigiArts", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
    TextView userEmail, userPassword, signUpBtn;
    Button loginBtn;
    List<string> users;
    List<Login> lstSource = new List<Login>();
    ListView userList;
    Boolean isUserValid;
    Helpers helper;

    protected override void OnStart()
    {
      base.OnStart();
      CheckSession();
    }

    protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
             //Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            userEmail = FindViewById<TextView>(Resource.Id.userEmail);
            userPassword = FindViewById<TextView>(Resource.Id.userPassword);
            loginBtn = FindViewById<Button>(Resource.Id.btnLogins);
            signUpBtn = FindViewById<TextView>(Resource.Id.btnSignUp);
            helper = new Helpers();

          
            loginBtn.Click += async (o, e) => await ButtonClick();

            signUpBtn.Click += (o, e) => ButtonSignUp_Click();

               
        }

    private void ButtonSignUp_Click()
        {
          Intent intent = new Intent(this, typeof(UserRegistration));
          StartActivity(intent);
        }

    private async Task ButtonClick()
        {
            //await Refresh();

            var email = userEmail.Text;
            var password = userPassword.Text;
            

            helper.showLoader(this);
            await Task.Delay(2000);



            if (email.Trim() == "" || password.Trim() == "")
            { 
              Toast.MakeText(this, "Please input all required fields", ToastLength.Long).Show();
                  } else {
                  await CheckUserLogin();

                  //isUserValid = await RegistrationService.CheckEmailPassword(email, password, this);
                  Registration loginCheck = new Registration();
                  loginCheck = await RegistrationService.CheckEmailPassword2(email, password, this);


                  if(loginCheck.Id > 0){
                    Toast.MakeText(this, "Login successful!", ToastLength.Long).Show();
                    moveToMainActivity(loginCheck);
                  } else {
                    Toast.MakeText(this, "The Email Address or Password you entered is incorrect", ToastLength.Long).Show();
                  }
             }
            helper.hideLoader();
    }
    
    private async Task Refresh()
        {
            var allEmail = "";
            await Task.Delay(2000);
            userList = FindViewById<ListView>(Resource.Id.userList);
            users = new List<string>();
            var login = await LoginService.GetLogin();
            lstSource = await LoginService.GetLogin();
            foreach (var res in login) {
                   allEmail += res.Email;
                   users.Add(res.Email);
            }
            //ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, users);
            var adapter = new ListViewAdapter(this, lstSource);
            userList.Adapter = adapter;
        }
    private async Task CheckUserLogin()
    {
      var allEmail = "";
      users = new List<string>();
      var login = await LoginService.GetLogin();
      foreach (var res in login)
      {
        allEmail += res.Email;
      }
      if (allEmail.Equals(""))
      {
        isUserValid = false;
      }
      else
      {
        isUserValid = true;
      }
    }
    private async void CheckSession()
    {
      //check if user is logged in
      //if user is logged in --> move to mainActivity
      SessionManagement sessionManagement = new SessionManagement(this);
      Registration loggedInUser = new Registration();
      loggedInUser = await sessionManagement.getSession();

      if (loggedInUser.Id != -1)
      {

        //user id logged in and so move to mainActivity

        moveToMainActivity(loggedInUser);
      }
      else
      {
        //do nothing
      }
    }

    private void moveToMainActivity(Registration loggedInUSer)
    {
      Intent intent = new Intent(this, typeof(Dashboard_Home));
      intent.SetFlags(ActivityFlags.NewTask);
      intent.PutExtra("loggedInUser", JsonConvert.SerializeObject(loggedInUSer));
      StartActivity(intent);
      Finish();
    }

  }

}