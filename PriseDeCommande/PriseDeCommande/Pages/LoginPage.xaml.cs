using System;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriseDeCommande.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static int IdCom { get; set; }
        private DAL dal = new DAL();
        public LoginPage()
        {
            InitializeComponent();
        }
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the password string to a byte array
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash value of the password bytes
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hash bytes to a string representation
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // Convert each byte to a two-digit hexadecimal representation
                }

                return sb.ToString();
            }
        }
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {

            string username =UsernameEntry.Text;
            string password = PasswordEntry.Text;

            bool isAuthenticated = dal.ValidateUserCredentials(username, password);

            if (isAuthenticated)
            {
                //  Application.Current.MainPage = new ClientPage(username);
                //await Navigation.PushAsync(new ClientPage());
                //Application.Current.MainPage = new MainPage();
                SecureStorage.SetAsync("Username", username);
                SecureStorage.SetAsync("Password", password);
                (App.Current as App)?.OnLoginSuccessful();
                //App.Current.OnLoginSuccessful();
            }
            else
            {
                await DisplayAlert("Login Failed", "Invalid username or password.", "OK");
            }

        }


    }
}