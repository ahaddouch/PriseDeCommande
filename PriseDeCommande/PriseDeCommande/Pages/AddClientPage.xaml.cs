using PriseDeCommande.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriseDeCommande
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddClientPage : ContentPage
    {
        private readonly DAL dal = new DAL();

        public ObservableCollection<DelaiPaiement> DelaiPaiements { get; set; } = new ObservableCollection<DelaiPaiement>();
        public ObservableCollection<CategorieClient> Categories { get; set; } = new ObservableCollection<CategorieClient>();

        public DelaiPaiement SelectedDelaiPaiement { get; set; }
        public CategorieClient SelectedCategorieClient { get; set; }

        public AddClientPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (DelaiPaiements.Count == 0)
            {
                List<DelaiPaiement> delaiPaiements = dal.GetDelaiPaiements();
                foreach (var delaiPaiement in delaiPaiements)
                {
                    DelaiPaiements.Add(delaiPaiement);
                }
            }

            if (Categories.Count == 0)
            {
                List<CategorieClient> categories = dal.GetCategorieClients();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
            }
        }

        private async void OnAddClientClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NumeroEntry.Text) || string.IsNullOrWhiteSpace(RaisonSocialeEntry.Text) || string.IsNullOrWhiteSpace(TelFixeEntry.Text) || string.IsNullOrWhiteSpace(ICEEntry.Text) ||
        SelectedDelaiPaiement == null || 
        SelectedCategorieClient == null) 
            {
                DisplayAlert("Validation Error", "Please fill all required fields.", "OK");
                return;
                
            }
            bool clientExists = App.ClientPageInstance.Clients.Any(client => client.Numero == NumeroEntry.Text);

            if (clientExists)
            {
                
                DisplayAlert("Error", "Client with the same Numero already exists.", "OK");
                
            }
            else
            {
                string numero = NumeroEntry.Text;
                string raisonSociale = RaisonSocialeEntry.Text;
                string telFixe = TelFixeEntry.Text;
                string adresse = AdresseEntry.Text;
                string abrRcNom = AbrRcNomEntry.Text;
                string ice = ICEEntry.Text;
                int idCategorieClient = SelectedCategorieClient?.IDCategorie_client ?? 0; 
                double htTtc = double.TryParse(HT_TTCEntry.Text, out double parsedHtTtc) ? parsedHtTtc : 0;
                string ville = VilleEntry.Text;
                string secteur = SecteurEntry.Text;
                int idDelaiPaiement = SelectedDelaiPaiement?.IDDelaiPaiement ?? 0; 

                bool isInserted = dal.InsertClientData(numero, raisonSociale, telFixe, adresse, abrRcNom, ice, idCategorieClient, htTtc, ville, secteur, idDelaiPaiement);

                if (isInserted)
                {
                    
                    DisplayAlert("Success", "Client added successfully!", "OK");

                    await Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Error", "Failed to add client. Please try again.", "OK");

                }
            }
        }
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}