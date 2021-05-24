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
using App6.Model;

namespace App6
{
  public class ViewHolder : Java.Lang.Object
  {
    public TextView txtName { get; set; }
    public TextView txtAge { get; set; }
    public TextView txtEmail { get; set; }
    public TextView txtUsername { get; set; }
  }
  public class ListViewAdapter : BaseAdapter
  {
    private Activity activity;
    private List<Login> lstPerson;
    public ListViewAdapter(Activity activity, List<Login> lstPerson)
    {
      this.activity = activity;
      this.lstPerson = lstPerson;
    }

    public override int Count
    {
      get
      {
        return lstPerson.Count;
      }
    }

    public override Java.Lang.Object GetItem(int position)
    {
      return null;
    }

    public override long GetItemId(int position)
    {
      return lstPerson[position].Id;
    }

    public override View GetView(int position, View convertView, ViewGroup parent)
    {
      var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_view_dataTemplate, parent, false);

      var txtUsername = view.FindViewById<TextView>(Resource.Id.txtUsername);
      var txtName = view.FindViewById<TextView>(Resource.Id.txtName);
      var txtEmail = view.FindViewById<TextView>(Resource.Id.txtEmail);
      var txtPassword = view.FindViewById<TextView>(Resource.Id.txtPassword);
      //var txtCreatedDate = view.FindViewById<TextView>(Resource.Id.txtCreateDate);


      txtUsername.Text = lstPerson[position].Username;
      txtName.Text = lstPerson[position].FirstLastName;
      txtEmail.Text = lstPerson[position].Email;
      txtPassword.Text = "" + lstPerson[position].Password;
      //txtCreatedDate.Text = lstPerson[position].CreatedDate;

      return view;
    }
  }
}