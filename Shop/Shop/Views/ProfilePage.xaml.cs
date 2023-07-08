using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Shop.Helpers;
using Shop.Model;
using System.Collections.ObjectModel;

namespace Shop.Views;

public partial class ProfilePage : ContentPage
{
    public static bool IsAuth = false;


    FirebaseClient firebaseClient = new FirebaseClient("https://car-shop-fde53-default-rtdb.europe-west1.firebasedatabase.app/");
    public ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();
    public ProfilePage()
	{
		InitializeComponent();
        BindingContext = this;
        CheckAuth();

        var collection = firebaseClient
        .Child("Todo")
        .AsObservable<TodoItem>()
        .Subscribe((item) =>
        {
            if (item.Object != null)
            {
                TodoItems.Add(item.Object);
            }
        });
    }
    private void OnBtnClciked(object sender, EventArgs e)
    {
        firebaseClient.Child("Todo").PostAsync(new TodoItem
        {
            Title = TitleEntry.Text,
        });
    }
    private async void OnGoToReg(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new RegisterPage());
    }
    private async void OnGoToJoin(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new JoinPage());
    }
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        FirebaseHelper.Logout();
        CheckAuth();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        CheckAuth();
    }

    public void CheckAuth()
    {
        if (FirebaseHelper.IsUserLoggedIn() == false)
        {
            IsAuth = false;
            Resources["authInfo"] = "Вы не вошли в аккаунт!";
        }
        else
        {
            IsAuth = true;
            var userInfo = FirebaseHelper.GetUser();
            Resources["authInfo"] = userInfo.User.Email;
        }
    }
}