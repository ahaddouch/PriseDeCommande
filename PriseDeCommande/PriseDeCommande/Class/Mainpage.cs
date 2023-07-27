using PriseDeCommande.Pages;
using Xamarin.Forms;

namespace PriseDeCommande
{
    public class Mainage : TabbedPage
    {
        public Mainage()
        {
            // Create instances of the ClientPage and ListCommandPage
            var clientPage = new ClientPage();
            var listCommandPage = new ListCommandPage();

            // Set the title for each tab
            clientPage.Title = "Clients";
            listCommandPage.Title = "List Commands";

            // Add the pages to the TabbedPage
            Children.Add(clientPage);
            Children.Add(listCommandPage);
        }
    }
}
