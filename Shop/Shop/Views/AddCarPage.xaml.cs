using Firebase.Database;
using Firebase.Database.Query;
using Shop.Model;
using Shop.Helpers;
using System.Collections.ObjectModel;
using Firebase.Storage;
using Shop.ViewModels;
using FirebaseAdmin.Auth;

namespace Shop.Views;
public partial class AddCarPage : ContentPage
{
    private List<string> carsImagesName = new List<string>();
    private List<string> carsImagesUrl = new List<string>();

    public static bool IsAuth = false;
    public AddCarPage()
    {
        InitializeComponent();
        BindingContext = this;
        CheckAuth();
    }

    public void CheckAuth()
    {
        if (!AppSettings.IsLoggedIn)
        {
            IsAuth = false;
            Resources["authInfo"] = "Вы не вошли в аккаунт!";
        }
        else
        {
            IsAuth = true;
            var userInfo = FirebaseHelper.FindUserByUid(AppSettings.UserId);
            Resources["authInfo"] = userInfo.Result.Email;
        }
        Resources["IsAuth"] = IsAuth;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Resources["carImagesLabel"] = string.Empty;
        CheckAuth();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (carsImagesName.Count != 0)
        {
            int count = carsImagesName.Count;
            int i = count - 1;
            while (i >= 0)
            {
                FirebaseHelper.Storage.Child("cars").Child(carsImagesName[i]).DeleteAsync();
                carsImagesName.RemoveAt(i);
                carsImagesUrl.RemoveAt(i);
                i--;
            }

        }
    }

    private async void OnSelectPhotosClicked(object sender, EventArgs e)
    {
        try
        {
            var file = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select an image"
            });

            if (file != null)
            {
                using (var stream = await file.OpenReadAsync())
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var imageUrl = await FirebaseHelper.Storage.Child("cars").Child(fileName).PutAsync(stream);
                    var photoUrl = await FirebaseHelper.Storage.Child("cars").Child(fileName).GetDownloadUrlAsync();

                    // Update the user's profile with the new photo URL                    
                    carsImagesName.Add(fileName);
                    carsImagesUrl.Add(photoUrl);
                    Resources["carImagesLabel"] += fileName + "; ";
                }
            }
            if(carsImagesName.Count > 5)            
                BtnPhotoSelector.IsEnabled = false;
            
        }
        catch (Exception ex)
        {
            // Handle any errors
            Console.WriteLine($"Error uploading image: {ex.Message}");
        }
    }

    private async void OnCreateListingClicked(object sender, EventArgs e)
    {
        // Обработчик события для кнопки "Создать объявление"
        // Здесь ты можешь получить значения из полей ввода и выполнить соответствующие действия для создания объявления о продаже автомобиля
        if (VinNumberEntry.Text == null || StsNumberEntry == null ||
            LicensePlateEntry.Text == null || BrandModelEntry.Text == null ||
            SteeringPositionEntry.Text == null || YearEntry.Text == null ||
            BodyTypeEntry.Text == null || MileageEntry.Text == null ||
            ExteriorColorEntry.Text == null || DescriptionEditor.Text == null ||
            PriceEntry.Text == null || CityEntry.Text == null || PhoneEntry.Text == null)
        {
            await DisplayAlert("Ошибка!", "Не все поля заполненны.", "OK");
            return;
        }

        Car car = new Car {
             VinNumber = VinNumberEntry.Text,
             StsNumber = StsNumberEntry.Text,
             LicensePlate = LicensePlateEntry.Text,
             BrandModel = BrandModelEntry.Text,
             SteeringPosition = SteeringPositionEntry.Text,
             Year = Convert.ToInt32(YearEntry.Text),
             BodyType = BodyTypeEntry.Text,
             Mileage = Convert.ToInt32(MileageEntry.Text),
             ExteriorColor = ExteriorColorEntry.Text,
             IsGboInstalled = GboSwitch.IsToggled,
             IsRepairNeeded = RepairNeededSwitch.IsToggled,
             Description = DescriptionEditor.Text,
             Price = Convert.ToDecimal(PriceEntry.Text),
             IsTradePossible = TradePossibleSwitch.IsToggled,
             City = CityEntry.Text,
             Phone = PhoneEntry.Text,
        };

        if (carsImagesName.Count != 0)
            for (int i = 0; i < carsImagesUrl.Count; i++)
            {
                car.PhotosUrl.Add(carsImagesUrl[i]);
            }

        var carListings = FirebaseHelper.Database.Child("users").Child(FirebaseHelper.AuthProvider.User.Uid).Child("cars");
        // Теперь ты можешь использовать полученные значения для создания объявления о продаже автомобиля

        // Загрузка данных об автомобиле в базу данных Firebase
        await carListings.PostAsync<Car>(car);

        await DisplayAlert("Ура!", "Ваше объявление выставленно!", "OK");
        await Navigation.PopAsync(true);
    }
}
