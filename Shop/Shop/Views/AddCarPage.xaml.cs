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
    private void OnSelectPhotosClicked(object sender, EventArgs e)
    {
        // Обработчик события для кнопки "Выбрать фотографии"
        // Здесь ты можешь реализовать логику выбора и загрузки фотографий автомобиля
    }

    private async void OnCreateListingClicked(object sender, EventArgs e)
    {
        // Обработчик события для кнопки "Создать объявление"
        // Здесь ты можешь получить значения из полей ввода и выполнить соответствующие действия для создания объявления о продаже автомобиля

        string vinNumber = VinNumberEntry.Text;
        string stsNumber = StsNumberEntry.Text;
        string licensePlate = LicensePlateEntry.Text;
        string brandModel = BrandModelEntry.Text;
        string steeringPosition = SteeringPositionEntry.Text;
        int year = Convert.ToInt32(YearEntry.Text);
        string bodyType = BodyTypeEntry.Text;
        int mileage = Convert.ToInt32(MileageEntry.Text);
        string exteriorColor = ExteriorColorEntry.Text;
        bool gboInstalled = GboSwitch.IsToggled;
        bool repairNeeded = RepairNeededSwitch.IsToggled;
        string description = DescriptionEditor.Text;
        decimal price = Convert.ToDecimal(PriceEntry.Text);
        bool tradePossible = TradePossibleSwitch.IsToggled;
        string city = CityEntry.Text;
        string phone = PhoneEntry.Text;

        // Теперь ты можешь использовать полученные значения для создания объявления о продаже автомобиля
        var carListings = App.Firebase.Child("carListings");

        // Создание объекта с информацией об автомобиле
        var carListing = new
        {
            VinNumber = vinNumber,
            StsNumber = stsNumber,
            LicensePlate = licensePlate,
            BrandModel = brandModel,
            SteeringPosition = steeringPosition,
            Year = year,
            BodyType = bodyType,
            Mileage = mileage,
            ExteriorColor = exteriorColor,
            GboInstalled = gboInstalled,
            RepairNeeded = repairNeeded,
            Description = description,
            Price = price,
            TradePossible = tradePossible,
            City = city,
            Phone = phone
        };

        // Загрузка данных об автомобиле в базу данных Firebase
        await carListings.PostAsync(carListing);

    }
}
