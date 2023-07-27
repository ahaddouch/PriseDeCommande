using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlX.XDevAPI;
using PriseDeCommande.Class;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriseDeCommande.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientPage : ContentPage
    {

        private readonly DAL dal = new DAL();

        public List<Clients> Clients { get; set; }


        public ClientPage()
        {
            InitializeComponent();
            BindingContext = this;
        }


        protected async override  void OnAppearing()
        {
            base.OnAppearing();
           


            RefreshClients();
            FilteredClients = Clients;
        }

        private  void RefreshClients()
        {
            Clients =  dal.GetClientsForRComercial();

            ClientsListView.ItemsSource = null;
            ClientsListView.ItemsSource = Clients;
        }
        private List<Clients> filteredClients;
        public List<Clients> FilteredClients
        {
            get => filteredClients;
            set
            {
                filteredClients = value;
                OnPropertyChanged(); 
            }
        }


        private  void OnAddClientClicked(object sender, System.EventArgs e)
        {

             Navigation.PushAsync(new AddClientPage());
        }
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredClients = Clients; // Show all clients when search text is empty
            }
            else
            {
                FilteredClients = Clients.Where(c => c.RaisonSociale.ToLower().Contains(searchText.ToLower())).ToList();
            }

            ClientsListView.ItemsSource = FilteredClients; // Update the ListView with filtered data
        }



        private async void OnClientSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null)
                    return;

                Clients selectedClient = (Clients)e.SelectedItem;
                await Navigation.PushAsync(new ClientDetailsPage(selectedClient));

                // Deselect the selected item to avoid multiple taps
                ClientsListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
       



    }
}
