using Firebase.Auth;
using System.ComponentModel;

namespace Shop.ViewModels
{
    internal class RegisterViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyCXtTb_BIGOuCIZiBqqEZTzGZ8WtmqxEr4 ";

        private INavigation _navigation;
        private string email;
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string Password
        {
            get => password; set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        public Command RegisterUser { get; }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public RegisterViewModel(INavigation navigation)
        {
            this._navigation = navigation;

            RegisterUser = new Command(RegisterUserTappedAsync);
        }

        private async void RegisterUserTappedAsync(object obj)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);
                string token = auth.FirebaseToken;
                if (token != null)
                    await App.Current.MainPage.DisplayAlert("Alert", "User Registered successfully", "OK");

                if (_navigation.ModalStack.Count > 0)
                {
                    var thisPage = _navigation.ModalStack[_navigation.ModalStack.Count - 1];
                    _navigation.InsertPageBefore(thisPage, _navigation.ModalStack[0]);

                    for (int i = 0; i < _navigation.ModalStack.Count; i++)
                    {
                        await _navigation.PopModalAsync();
                    }
                }
                else await _navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }   
}