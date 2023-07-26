using System;
using System.Collections.Generic;
using PriseDeCommande.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriseDeCommande.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandPage : ContentPage
    {
        private readonly DAL dal = new DAL();
        private readonly int clientId;
        private List<Category> categories;
        private List<Product> selectedProducts;

        public CommandPage(int clientId)
        {
            InitializeComponent();
            this.clientId = clientId;
            //categories = dal.GetCategories(); // Fetch all the categories from the database
            categoryPicker.ItemsSource = categories;
        }

        private void OnCategoryPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            //int selectedIndex = categoryPicker.SelectedIndex;
            //if (selectedIndex >= 0)
            //{
            //    int categoryId = categories[selectedIndex].CategoryId;
            //    selectedProducts = dal.GetProductsByCategory(categoryId); // Fetch products for the selected category
            //    productPicker.ItemsSource = selectedProducts;
            //}
        }

        private async void OnSubmitCommandClicked(object sender, EventArgs e)
        {
            if (categoryPicker.SelectedIndex < 0 || productPicker.SelectedIndex < 0 || string.IsNullOrWhiteSpace(quantityEntry.Text) || string.IsNullOrWhiteSpace(priceEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all the fields.", "OK");
                return;
            }

           // int productId = selectedProducts[productPicker.SelectedIndex].ProductId;
            int quantity = int.Parse(quantityEntry.Text);
            decimal price = decimal.Parse(priceEntry.Text);

            //bool isInserted = await dal.InsertCommand(clientId, productId, quantity, price);

            //if (isInserted)
            //{
            //    await DisplayAlert("Success", "Command added successfully.", "OK");
            //    await Navigation.PopAsync(); // Go back to the previous page (ClientPage)
            //}
            //else
            //{
            //    await DisplayAlert("Error", "Failed to add the command.", "OK");
            //}
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Go back to the previous page (ClientPage)
        }
    }
}
