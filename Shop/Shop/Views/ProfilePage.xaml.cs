using Firebase.Database;
using Firebase.Database.Query;
using Shop.Model;
using System.Collections.ObjectModel;

namespace Shop.Views;

public partial class ProfilePage : ContentPage
{
    FirebaseClient firebaseClient = new FirebaseClient("https://car-shop-fde53-default-rtdb.europe-west1.firebasedatabase.app/");
    public ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();
    public ProfilePage()
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
    private void OnBtnClciked(object sender, EventArgs e)
    {
        firebaseClient.Child("Todo").PostAsync(new TodoItem
        {
            Title = TitleEntry.Text,
        });
    }
}