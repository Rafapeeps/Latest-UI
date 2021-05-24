using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App6.Service;
using App6.Services;

namespace App6
{
  [Activity(Label = "Sign Up")]
  public class UserRegistration : Activity
  {
    TextView userEmailtxt;
    TextView userPasswordtxt;
    TextView firstLastNametxt;
    TextView userNametxt;
    TextView confirmPasswordtxt;
    Button signUpBtn;
    Helpers helper;
    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.register);

      firstLastNametxt = FindViewById<TextView>(Resource.Id.firstLastName);
      userNametxt = FindViewById<TextView>(Resource.Id.userName);
      userEmailtxt = FindViewById<TextView>(Resource.Id.userEmail);
      userPasswordtxt = FindViewById<TextView>(Resource.Id.userPass);
      confirmPasswordtxt = FindViewById<TextView>(Resource.Id.confirmPass);
      signUpBtn = FindViewById<Button>(Resource.Id.btnSignUp);

      helper = new Helpers();
      signUpBtn.Click += async (o, e) => await SignUpClickAsync();
      // Create your application here
    }

    private async Task SignUpClickAsync()
    {
      //FindViewById<TextView>(Resource.Id.emailText).Text = userEmail.Text;
      //FindViewById<TextView>(Resource.Id.passText).Text = userPassword.Text;

      var firstLast = firstLastNametxt.Text;
      var userName = userNametxt.Text;
      var email = userEmailtxt.Text;
      var password = userPasswordtxt.Text;
      var confirmPass = confirmPasswordtxt.Text;
      var createdDate = DateTime.Now.ToString();



      if (firstLast.Trim() == "" || userName.Trim() == "" || email.Trim() == "" || password.Trim() == "" || email.Trim() == "" || confirmPass.Trim() == "")
      {
        Toast.MakeText(this, "Please input all required fields", ToastLength.Long).Show();
      }
      else if (helper.isValidEmail(email) == false)
      {
        Toast.MakeText(this, "Invalid Email Address", ToastLength.Long).Show();
      }
      else if (password.Trim().Length < 6)
      {
        Toast.MakeText(this, "Password should be at least 6 characters long", ToastLength.Long).Show();
      }
      else if (password.Equals(confirmPass))
      {
        helper.showLoader(this);
        await RegistrationService.AddRegistration(firstLast, userName, email, password, createdDate);
        await Task.Delay(2000);
        Intent intent = new Intent(this, typeof(MainActivity));
        StartActivity(intent);
        helper.hideLoader();
      }
      else
      {
        Toast.MakeText(this, "Passwords do not match", ToastLength.Long).Show();
      }
    }
  }
}