using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App6
{
    [Activity(Label = "layout")]
    public class UserDisplay : Activity
    {
    TextView userEmail;
    TextView userPassword;
    protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
          
            // Create your application here
            SetContentView(Resource.Layout.activity_main);

            userEmail = FindViewById<TextView>(Resource.Id.userEmail);
            userPassword = FindViewById<TextView>(Resource.Id.userPassword);

            //FindViewById<TextView>(Resource.Id.emailText).Text = userEmail.Text;
            //FindViewById<TextView>(Resource.Id.passText).Text = userPassword.Text;
      //TextView name = FindViewById<TextView>(Resource.Id.userEmail);
      // name.Text = Login.Email;
    }
    }
}