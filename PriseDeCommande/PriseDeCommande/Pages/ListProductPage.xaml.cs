using PriseDeCommande.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;


namespace PriseDeCommande.Pages
{
    public partial class ListProductPage : ContentPage
    {
        private readonly DAL dal = new DAL();
        private readonly CategorieArticle selectedCategory;
        private List<Product> products;
        private List<Product> selectedProducts;

        // Property to store the list of selected products
        public List<Product> SelectedProducts
        {
            get { return selectedProducts; }
            set
            {
                selectedProducts = value;
                OnPropertyChanged(nameof(SelectedProducts));
            }
        }

        public ListProductPage(CategorieArticle category)
        {
            InitializeComponent();
            this.selectedCategory = category;
            this.selectedProducts = new List<Product>();
            LoadProducts();
        }

        private void LoadProducts()
        {
            // Fetch products from the database based on the selected category ID
            products = dal.GetProductsByCategory(selectedCategory.IdCategorie);

            // Bind the list of products to the ListView
            productCollection.ItemsSource = products;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                productCollection.ItemsSource = products; // Show all products if search text is empty
            }
            else
            {
                // Filter products by name containing the search text
                productCollection.ItemsSource = products.FindAll(product =>
                    product.ProductName.ToLower().Contains(e.NewTextValue.ToLower()));
            }
        }

        private void OnProductSelected(object sender, SelectionChangedEventArgs e)
        {
            // Handle product selection here
            // The selected products will be stored in 'SelectedProducts' list

            // Convert the collection of 'object' to a list of 'Product'
            SelectedProducts = e.CurrentSelection.Select(item => (Product)item).ToList();
        }


        //private async void OnValidateClicked(object sender, EventArgs e)
        //{
        //    // Handle the validate button click
        //    // For example, return to the category page and pass the list of selected products
        //    await Navigation.PopAsync();
        //}
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Handle the cancel button click
            // For example, return to the category page without saving selected products
            await Navigation.PopAsync();
        }

        private async void OnValidateClicked(object sender, EventArgs e)
        {
            // Handle the validate button click
            // For example, return to the category page and pass the list of selected products
            CategorieListePage.SelectedProducts.AddRange(SelectedProducts);
            await Navigation.PopAsync();

            // Access the previous page (CategorieListePage) and pass the list of selected products
            
            
        }
    }
}
