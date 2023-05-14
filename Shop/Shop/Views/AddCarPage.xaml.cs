namespace Shop.Views;
using Microsoft.Maui.Storage;
public partial class AddCarPage : ContentPage
{
    
    int count = 0;
    public AddCarPage()
	{
		InitializeComponent();
	}
    private async void Car(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Добавьте фото машины",
            FileTypes = FilePickerFileType.Images

        });
        if (result == null)
            return;
        var stream = await result.OpenReadAsync();
        myImage.Source = ImageSource.FromStream(() => stream);

        SemanticScreenReader.Announce(NewCar.Text);
    }
}

