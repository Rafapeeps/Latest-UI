using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using App6.Model;
using App6.Service;
using App6.Services;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace App6
{
  [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
  public class Dashboard_Home : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
  {
    Helpers helper;
    Login checkLogin;
    TextView firstLast, email;
    ListView userList;
    List<string> users;
    View contentMain;
    List<Login> lstSource;

    protected override async void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.dashboard_main);
      Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
      SetSupportActionBar(toolbar);

      FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
      fab.Click += FabOnClick;

      DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
      ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
      drawer.AddDrawerListener(toggle);
      toggle.SyncState();

      NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
      navigationView.SetNavigationItemSelectedListener(this);

      var headerView = navigationView.GetHeaderView(0);

      Login loggedInUser = new Login();
      checkLogin = new Login();

      firstLast = headerView.FindViewById<TextView>(Resource.Id.navFirstName);
      email = headerView.FindViewById<TextView>(Resource.Id.navEmail);
      loggedInUser = JsonConvert.DeserializeObject<Login>(Intent.GetStringExtra("loggedInUser"));
      checkLogin = await LoginService.GetLogin(1);

      if (checkLogin == null)
      {
        checkLogin = helper.CreateDummy();
      }
      else
      {
       firstLast.Text = loggedInUser.FirstLastName;
       email.Text = loggedInUser.Email;
      }


      if (checkLogin.Id == -1)
      {
        await LoginService.AddLogin(1, loggedInUser.Id, loggedInUser.FirstLastName, loggedInUser.Username, loggedInUser.Email, loggedInUser.Password, loggedInUser.CreatedDate);

      }

      helper = new Helpers();


     View viewlist = LayoutInflater.Inflate(Resource.Layout.list_view_dataTemplate, null);

      Button editBtn = viewlist.FindViewById<Button>(Resource.Id.editBtn);


      editBtn.Click += delegate
      {
        Refresh();
      };
    }

    private void EditButtonClicked()
    {
      Toast.MakeText(this, "Edit Clicked", ToastLength.Long).Show();
    }

    protected override void OnSaveInstanceState(Bundle outState)
    {

      // always call the base implementation!
      base.OnSaveInstanceState(outState);
    }

    protected override void OnRestoreInstanceState(Bundle outState)
    {

      // always call the base implementation!
      base.OnRestoreInstanceState(outState);
    }

    public override void OnBackPressed()
    {
      DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
      if (drawer.IsDrawerOpen(GravityCompat.Start))
      {
        drawer.CloseDrawer(GravityCompat.Start);
      }
      else
      {
        base.OnBackPressed();
        Intent startMain = new Intent(Intent.ActionMain);
        startMain.AddCategory(Intent.CategoryHome);
        startMain.SetFlags(ActivityFlags.NewTask);
        StartActivity(startMain);
      }
    }

    public override bool OnCreateOptionsMenu(IMenu menu)
    {
      MenuInflater.Inflate(Resource.Menu.menu_main, menu);
      return true;
    }

    public override bool OnOptionsItemSelected(IMenuItem item)
    {
      int id = item.ItemId;
      if (id == Resource.Id.action_settings)
      {
        return true;
      }

      return base.OnOptionsItemSelected(item);
    }

    private void FabOnClick(object sender, EventArgs eventArgs)
    {
      View view = (View)sender;
      Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
          .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
    }

    public bool OnNavigationItemSelected(IMenuItem item)
    {
      int id = item.ItemId;

      if (id == Resource.Id.nav_camera)
      {
        List<Login> dummy = new List<Login>();
        var adapter = new ListViewAdapter(this, dummy);
        userList.Adapter = adapter;
      }
      else if (id == Resource.Id.nav_gallery)
      {
        List<View> viewlist = new List<View>();
        contentMain = LayoutInflater.Inflate(Resource.Layout.content_main, null, false);
        viewlist.Add(contentMain);

        userList = contentMain.FindViewById<ListView>(Resource.Id.userList);
        Refresh();
      }
      else if (id == Resource.Id.nav_slideshow)
      {

      }
      else if (id == Resource.Id.nav_manage)
      {

      }
      else if (id == Resource.Id.nav_share)
      {

      }
      else if (id == Resource.Id.nav_send)
      {
        helper.showLoader(this);
        SessionManagement sessionManagement = new SessionManagement(this);
        sessionManagement.removeSession();
      
        LoginService.RemoveLogin(1);
        MoveToLoginAsync();
      }

      DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
      drawer.CloseDrawer(GravityCompat.Start);
      return true;
    }

    public async void LoadAgentInfo(object sender, ElapsedEventArgs e)
    {
      userList = contentMain.FindViewById<ListView>(Resource.Id.userList);
      await Refresh();
    }

    private async System.Threading.Tasks.Task MoveToLoginAsync()
    {
      Intent intent = new Intent(this, typeof(MainActivity));
      await Task.Delay(2000);
      helper.hideLoader();
      StartActivity(intent);
    }
    private async Task Refresh()
    {
      var allEmail = "";
      helper.showLoader(this);
      await Task.Delay(2000);
      userList = FindViewById<ListView>(Resource.Id.userList);
      users = new List<string>();
      var login = await LoginService.GetLogin();
      lstSource = await LoginService.GetLogin();
      foreach (var res in login)
      {
        allEmail += res.Email;
        users.Add(res.Email);
      }
      //ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, users);
      var adapter = new ListViewAdapter(this, lstSource);
      userList.Adapter = adapter;
      helper.hideLoader();
    }
  }
}

