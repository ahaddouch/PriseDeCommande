using K4os.Compression.LZ4.Streams.Adapters;
using PriseDeCommande.Class;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PriseDeCommande.Pages
{
    public partial class CategorieListePage : ContentPage
    {
        private readonly DAL dal = new DAL();
        private List<CategorieArticle> allCategories; 
        public static List<Product> SelectedProducts=new List<Product>();
        private Clients selectedClient;
        
        public CategorieArticle SelectedCategory { get; set; }

        public CategorieListePage(Clients selectedClient)
        {
            InitializeComponent();
            LoadCategories();
            SelectedCategory = null; 
            categoriesCollection.BindingContext = this; 
            this.selectedClient = selectedClient;
           
        }

        private void LoadCategories()
        {
            allCategories = dal.GetCategoriesArticle();

            categoriesCollection.ItemsSource = allCategories;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                categoriesCollection.ItemsSource = allCategories; 
            }
            else
            {
                categoriesCollection.ItemsSource = allCategories.FindAll(category =>
                    category.NameCategorie.ToLower().Contains(e.NewTextValue.ToLower()));
            }
        }

        private async void OnCategorySelected(object sender, SelectionChangedEventArgs e)
        {
            
            if (e.CurrentSelection.Count > 0)
            {
                SelectedCategory = (CategorieArticle)e.CurrentSelection[0];

                await Navigation.PushAsync(new ListProductPage(SelectedCategory));

                categoriesCollection.SelectedItem = null;
            }

        }

        private async void OnCheckoutClicked(object sender, EventArgs e)
        {
            if (SelectedProducts.Count > 0)
            {

                await Navigation.PushAsync(new CreateCommandPage(SelectedProducts,selectedClient));
            }
            else
            {
                
                await DisplayAlert("Checkout", "Please select a product first!", "OK");
            }
        }
    }
}
