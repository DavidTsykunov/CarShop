using Firebase.Database;
using Firebase.Database.Query;
using Shop.Model;
using System.Collections.ObjectModel;

namespace Shop.Views;
public partial class AddCarPage : ContentPage
{
    public AddCarPage()
    {
        InitializeComponent();
        BindingContext = this;      
    }

}