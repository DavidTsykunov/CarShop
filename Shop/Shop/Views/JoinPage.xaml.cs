using Shop.ViewModels;

namespace Shop.Views;

public partial class JoinPage : ContentPage
{
	public JoinPage()
	{
		InitializeComponent();
        BindingContext = new LoginViewModel(Navigation);
    }
    private async void OnGoBack(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}