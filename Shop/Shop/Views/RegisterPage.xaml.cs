using Shop.ViewModels;

namespace Shop.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior()
        {
            IsEnabled = false
        });

        BindingContext = new RegisterViewModel(Navigation);
    }
    private async void OnGoToJoin(object sender, EventArgs e)
    {     
        await Navigation.PushModalAsync(new JoinPage());
    }

}