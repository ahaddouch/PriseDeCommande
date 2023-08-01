using System;
using System.Collections.Generic;
using PriseDeCommande.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PriseDeCommande.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateCommandPage : ContentPage
    {

        List<Product> products;
        Clients client;
        public CreateCommandPage(List<Product> products,Clients selectedClient)
        {
            this.products = products;
            this.client = selectedClient;
           
        }

    }
}
