using System;
using System.Collections.Generic;
using System.Text;

namespace PriseDeCommande.Class
{
    public class Clients
    {
        public int IDCompte { get; set; }
        public string Numero { get; set; }
        public bool Actif { get; set; }
        public string RaisonSociale { get; set; }
        public string TelFixe { get; set; }
        public string Adresse { get; set; }
        public string TypeCompte { get; set; }
        public string AbrRcNom { get; set; }
        public string ICE { get; set; }
        public int IDCategorieClient { get; set; }
        public double HT_TTC { get; set; }
        public string Ville { get; set; }
        public string Secteur { get; set; }
        public int IDDelaiPaiement { get; set; }
        public bool NouveauClient { get; set; }
        public string DC { get; set; }
    }
}
