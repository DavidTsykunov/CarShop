using Shop.Helpers;
using Shop.ViewModels;

namespace Shop.Views
{
    public partial class JoinPage : ContentPage
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public JoinPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void OnGoBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password))
            {
                bool success = await FirebaseHelper.Login(Email, Password);
                if (success)
                {
                    // ������������ ������� ����� � �������, �������������� �� �������� �������
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("������", "�������� ������� ������", "OK");
                }
            }
            else
            {
                await DisplayAlert("������", "����������, ��������� ��� ����", "OK");
            }
        }
    }
}   