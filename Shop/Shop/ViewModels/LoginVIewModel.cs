using Firebase.Auth;
using Newtonsoft.Json;
using Shop.Views;
using System.ComponentModel;

namespace Shop.ViewModels
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyCXtTb_BIGOuCIZiBqqEZTzGZ8WtmqxEr4";
        private INavigation _navigation;
        private string email;
        private string userPassword;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command LoginBtn { get; }

        public string Email
        {
            get => email; set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string Password
        {
            get => userPassword; set
            {
                userPassword = value;
                RaisePropertyChanged("Password");
            }
        }

        public LoginViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            LoginBtn = new Command(LoginBtnTappedAsync);
        }

        private async void LoginBtnTappedAsync(object obj)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("FreshFirebaseToken", serializedContent);
                await App.Current.MainPage.DisplayAlert("Alert", "User Login successfully", "OK");

                await _navigation.PopModalAsync();

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }


        }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}