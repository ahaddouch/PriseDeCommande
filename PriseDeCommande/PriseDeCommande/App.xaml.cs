using PriseDeCommande.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PriseDeCommande
{
    public partial class App : Application
    {
        private DAL dal = new DAL();

        public static ClientPage ClientPageInstance { get; set; } = new ClientPage();
        public static ListCommandPage ListCommandPageInstance { get; set; } = new ListCommandPage();

        public App()
        {
            InitializeComponent();

            // Call the OnStart method to check if the user is already logged in
            OnStart();
        }

        protected override void OnStart()
        {
            



            if (SecureStorage.GetAsync("Username").Result != null && SecureStorage.GetAsync("Password").Result != null)
            {
                string username = SecureStorage.GetAsync("Username").Result;
                string password = SecureStorage.GetAsync("Password").Result;
                bool isAuthenticated = dal.ValidateUserCredentials(username, password);
                NavigateToTabbedPage();
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        public void OnLoginSuccessful()
        {
            // Call the method to navigate to the TabbedPage
            NavigateToTabbedPage();
        }

        //private void NavigateToTabbedPage()
        //{
        //    // Create the TabbedPage and set its Children property with the ClientPage and ListCommandPage
        //    var tabbedPage = new TabbedPage
        //    {
        //        Children =
        //        {
        //            new NavigationPage(ClientPageInstance)
        //            {
        //                Title = "Clients",
        //                IconImageSource = "assets/client.png" // Add your own icon image here
        //            },
        //            new NavigationPage(ListCommandPageInstance)
        //            {
        //                Title = "Commands",
        //                IconImageSource = "assets/command.png" // Add your own icon image here
        //            }
        //        }
        //    };


        //    tabbedPage.ToolbarItems.Add(new ToolbarItem
        //    {
        //        Text = "Logout",
        //        Command = new Command(() =>
        //        {
        //            // Call the OnLogout method in the App class
        //            OnLogout();
        //        })
        //    });


        //    // Set the MainPage as the TabbedPage
        //    MainPage = tabbedPage;
        //}

        private void NavigateToTabbedPage()
        {
            // Create the TabbedPage and set its Children property with the ClientPage and ListCommandPage
            var tabbedPage = new TabbedPage
            {
                Children =
        {
            new NavigationPage(ClientPageInstance)
            {
                Title = "Clients",
                IconImageSource = "assets/client.png" // Add your own icon image here
            },
            new NavigationPage(ListCommandPageInstance)
            {
                Title = "Commands",
                IconImageSource = "assets/command.png" // Add your own icon image here
            }
        }
            };

            // Add the Logout button to the left side of the navigation bar
            var logoutToolbarItem = new ToolbarItem
            {
                Text = "Logout",
                Command = new Command(() =>
                {
                    // Call the OnLogout method in the App class
                    OnLogout();
                }),
                Priority = 0, // Set a lower priority to make it appear on the left
            };
            tabbedPage.ToolbarItems.Add(logoutToolbarItem);

            // Set the MainPage as the TabbedPage
            MainPage = tabbedPage;
        }

        public void OnLogout()
        {
            SecureStorage.Remove("Username");
            SecureStorage.Remove("Password");
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
