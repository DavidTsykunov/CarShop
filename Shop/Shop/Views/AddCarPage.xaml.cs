using Firebase.Database;
using Firebase.Database.Query;
using Shop.Model;
using System.Collections.ObjectModel;

namespace Shop.Views;
public partial class AddCarPage : ContentPage
{
    FirebaseClient firebaseClient = new FirebaseClient("https://car-shop-fde53-default-rtdb.europe-west1.firebasedatabase.app/");
    public ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();
    public AddCarPage()
    {
        InitializeComponent();
        BindingContext = this;

        var collection = firebaseClient
       .Child("Todo")
       .AsObservable<TodoItem>()
       .Subscribe((item) =>
       {
           if (item.Object != null)
           {
               TodoItems.Add(item.Object);
           }
       });
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

        AddItem();

        SemanticScreenReader.Announce(NewCar.Text);
    }

    private async void AddItem()
    {
        firebaseClient.Child("Todo").PostAsync(new TodoItem
        {
            Title = "123",
            Image = myImage.Source,
        });
        //await App.Current.MainPage.DisplayAlert("Alert", myImage.Source, "OK");
    }

}