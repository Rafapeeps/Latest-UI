﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace App6.Model
{
  public class Registration
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string FirstLastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string CreatedDate { get; set; }
  }
}