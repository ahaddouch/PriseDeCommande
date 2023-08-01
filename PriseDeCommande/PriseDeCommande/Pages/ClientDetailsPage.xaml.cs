using System;
using PriseDeCommande.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriseDeCommande.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientDetailsPage : ContentPage
    {
        private Clients selectedClient;

        public ClientDetailsPage(Clients selectedClient)
        {
            InitializeComponent();
            BindingContext = this.selectedClient = selectedClient;
        }

        private async void OnCreateCommandClicked(object sender, EventArgs e)
        {
            // Navigate to the page for creating a new command for the selected client
            await Navigation.PushAsync(new CategorieListePage(selectedClient));
        }
    }
}
