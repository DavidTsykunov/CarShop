using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Shop.Helpers;
using System;

namespace Shop.Views
{
    public partial class EditProfilePage : ContentPage
    {
        private FirebaseClient firebaseClient = new FirebaseClient("https://car-shop-fde53-default-rtdb.europe-west1.firebasedatabase.app/");
        private FirebaseStorage firebaseStorage = new FirebaseStorage("car-shop-fde53.appspot.com");
        public EditProfilePage()
        {
            InitializeComponent();
        }
        private async void OnGoBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            // ��������� ��������� ������������� ������ �� ����� �����
            string name = nameEntry.Text;
            string pass = passEntry.Text;
            string rPass = rPassEntry.Text;

            if (pass != rPass)
            {
                await DisplayAlert("Error", "������, ������ �� ���������.", "OK");
            }
            else {
                // ���������� ������� ������������ � ���� ������ Firebase
                var user = FirebaseHelper.AuthProvider.User;
                if (user != null)
                {
                    // ���������� ����� ������������               
                    await user.ChangeDisplayNameAsync(name);

                    await user.ChangePasswordAsync(pass);

                    // ����� ��������� �� �������� ����������
                    await DisplayAlert("Success", "������� ������.", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    // ��������� ������ ��� ��������� ������� ������������
                    await DisplayAlert("Error", "������, ������� �� ������.", "OK");
                }
            }
        }
        private async void UploadAvatar(object sender, EventArgs e)
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
                        var imageUrl = await firebaseStorage.Child("avatars").Child(fileName).PutAsync(stream);
                        var photoUrl = await firebaseStorage.Child("avatars").Child(fileName).GetDownloadUrlAsync();

                        // Update the user's profile with the new photo URL
                        var userInfo = FirebaseHelper.AuthProvider.User;
                        userInfo.Info.PhotoUrl = photoUrl;

                        // Save the updated user profile in the database
                        await firebaseClient.Child("users").Child(userInfo.Uid).PutAsync(userInfo);

                        // Update the UI with the new photo URL
                        Resources["PhotoUrl"] = userInfo.Info.PhotoUrl;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                Console.WriteLine($"Error uploading avatar: {ex.Message}");
            }
        }
    }
}