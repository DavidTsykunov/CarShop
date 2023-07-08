using Firebase.Database;
using Shop.Helpers;
using Shop.Model;
using System;

namespace Shop.Views
{
    public partial class ProfilePage : ContentPage
    {
        public static bool IsAuth = false;

        FirebaseClient firebaseClient = new FirebaseClient("https://car-shop-fde53-default-rtdb.europe-west1.firebasedatabase.app/");

        public UserProfile UserProfile { get; set; }

        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = this;
            CheckAuth();
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
            LoadUserProfile();
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
            Resources["IsAuth"] = IsAuth;
        }

        private void LoadUserProfile()
        {
            if (IsAuth)
            {
                var userInfo = FirebaseHelper.GetUser();

                Resources["DisplayName"] = userInfo.User.DisplayName;
                Resources["Email"] = userInfo.User.Email;
                Resources["PhotoUrl"] = userInfo.User.PhotoUrl;
            }
            else
            {
                Resources["DisplayName"] = string.Empty;
                Resources["Email"] = string.Empty;
                Resources["PhotoUrl"] = string.Empty;
            }
        }
    }
}