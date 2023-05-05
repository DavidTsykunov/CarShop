namespace Shop.Views;

public partial class AddCarPage : ContentPage
{
    int count = 0;
    public AddCarPage()
	{
		InitializeComponent();
	}
    private void Car(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            NewCar.Text = $"Clicked {count} time";
        else
            NewCar.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(NewCar.Text);
    }
}
