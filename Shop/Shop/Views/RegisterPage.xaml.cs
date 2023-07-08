using Shop.Helpers;
using Shop.ViewModels;

namespace Shop.Views
{
    public partial class RegisterPage : ContentPage
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel();
        }

        private async void OnGoBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void OnRegisterUserClicked(object sender, EventArgs e)
        {
            var viewModel = (RegisterViewModel)BindingContext;

            if (!string.IsNullOrWhiteSpace(viewModel.Email) && !string.IsNullOrWhiteSpace(viewModel.Password))
            {
                bool success = await viewModel.Register();

                if (success)
                {
                    await DisplayAlert("�����", "������������ ���������������", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("������", "�� ������� ���������������� ������������", "OK");
                }
            }
            else
            {
                await DisplayAlert("������", "����������, ��������� ��� ����", "OK");
            }
        }
    }
}
