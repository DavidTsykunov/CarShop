using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Newtonsoft.Json;
using Shop.Model;

namespace Shop.Helpers
{
    public class FirebaseHelper
    {
        private static readonly string FirebaseAuthKey = "FreshFirebaseToken";
        public static FirebaseClient Database = new FirebaseClient(baseUrl: "https://car-shop-fde53-default-rtdb.europe-west1.firebasedatabase.app/");
        public static FirebaseStorage Storage = new FirebaseStorage("car-shop-fde53.appspot.com");
        public static FirebaseAuthClient AuthProvider = new FirebaseAuthClient(new FirebaseAuthConfig
        {
            ApiKey = "AIzaSyCXtTb_BIGOuCIZiBqqEZTzGZ8WtmqxEr4",
            AuthDomain = "car-shop-fde53.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            },
        });

        public static async Task<bool> Login(string email, string password)
        {
            try
            {
                var auth = await AuthProvider.SignInWithEmailAndPasswordAsync(email, password);
                var serializedAuth = JsonConvert.SerializeObject(auth);
                Preferences.Set(FirebaseAuthKey, serializedAuth);
                return true;
            }
            catch (Exception ex)
            {
                // Обработка ошибок аутентификации
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static void Logout()
        {
            if(AuthProvider.User != null)
                AuthProvider.SignOut();
        }

        public static Firebase.Auth.FirebaseAuthClient GetUser()
        {
            if (AuthProvider.User != null)
            {
                var serializedAuth = Preferences.Get(FirebaseAuthKey, "");
                var auth = JsonConvert.DeserializeObject<FirebaseAuthClient>(serializedAuth);
                return auth;
            }
            return null;
        }
        public static async Task<bool> Register(string email, string password, string displayName)
        {
            try
            {
                var auth = await AuthProvider.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
                auth.User.Info.PhotoUrl = "profile.png";

                // Сохранение токена аутентификации
                var serializedAuth = JsonConvert.SerializeObject(auth);
                Preferences.Set(FirebaseAuthKey, serializedAuth);
                await Database.Child("users").Child(auth.User.Uid).PutAsync(auth.User);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static async Task<UserProfile> GetUserProfile()
        {
            var auth = GetUser();
            if (auth != null)
            {
                var freshAuth = AuthProvider.User.Info;
                if (freshAuth != null)
                {
                    return new UserProfile
                    {
                        Email = freshAuth.Email,
                        DisplayName = freshAuth.DisplayName,
                        PhotoUrl = freshAuth.PhotoUrl
                    };
                }
            }
            return null;
        }

    }
}