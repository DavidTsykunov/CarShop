using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
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
        public static FileUserRepository FirebaseRepository = new FileUserRepository("Firebase\\Repository");

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
        public static async Task<UserRecord> FindUserByUid(string uid)
        {
            try
            {
                var auth = FirebaseAuth.DefaultInstance;
                if (auth != null)
                {
                    var user = await auth.GetUserAsync(uid);
                    Console.WriteLine("Найден пользователь: " + JsonConvert.SerializeObject(user));
                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при поиске пользователя: " + ex.Message);
            }

            return null;
        }
    }
}