using System;
using System.Collections.Generic;
using System.Text;

namespace PriseDeCommande.Class
{
    public class CategorieClient
    {
        public int IDCategorie_client { get; set; }
        public string Libelle { get; set; }
        public bool FixerPrix { get; set; }
        public bool HT_TTC { get; set; }
    }
}
