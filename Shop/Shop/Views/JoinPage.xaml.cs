using Shop.ViewModels;

namespace Shop.Views;

public partial class JoinPage : ContentPage
{
	public JoinPage()
	{
		InitializeComponent();
        BindingContext = new LoginViewModel(Navigation);
    }
    private async void OnGoToReg(object sender, EventArgs e)
    {      
        await Navigation.PushModalAsync(new RegisterPage());
    }

}