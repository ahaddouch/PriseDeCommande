//using MySql.Data.MySqlClient;
using MySqlConnector;
using PriseDeCommande.Class;
using PriseDeCommande.Converters;
using PriseDeCommande.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace PriseDeCommande
{
    public class DAL
    {


        public string connect()
        {

            MySqlConnectionStringBuilder Builder = new MySqlConnectionStringBuilder();
            Builder.Port = 3306;
            Builder.Server = "192.168.1.30";
            Builder.Database = "d3_mobile_app";
            Builder.UserID = "adam_ahad";
            Builder.Password = "123456";
            return Builder.ToString();
        }


        public bool ValidateUserCredentials(string username, string password)
        {

            bool isValidUser = false;

            using (MySqlConnection connection = new MySqlConnection(connect()))
            {
                try
                {

                    connection.Open();
                    string query = "SELECT IDCompte FROM compte WHERE raison_sociale = @username AND password = @password AND actif = 1 AND type_compte='RC'";
                    //string query = "SELECT COUNT(*) FROM user WHERE username = @username AND password = @password";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isValidUser = true;
                            LoginPage.IdCom = reader.GetInt32(0);
                        }
                    }


                    connection.Close();
                }
                catch (MySqlException ex)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert("MySQL Error", ex.Message, "OK");
                    });
                }
                catch (Exception ex)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                    });
                }
            }

            return isValidUser;
        }





        public List<Clients> GetClientsForRComercial()
        {
            List<Clients> clients = new List<Clients>();

            try
            {
                using (var connection = new MySqlConnection(connect()))
                {
                    connection.Open();

                    string query = "SELECT IDCompte,raison_sociale,adresse,ville,tel_fixe FROM Compte WHERE Registre_com = @Rcomercial AND type_compte = 'CL' AND actif = 1";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Rcomercial", LoginPage.IdCom);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Clients client = new Clients
                                {
                                    IDCompte = reader.GetInt32("IDCompte"),
                                    RaisonSociale = reader.GetString("raison_sociale"),
                                    Adresse = reader.GetString("adresse"),
                                    Ville = reader.GetString("ville"),
                                    TelFixe = reader.GetString("tel_fixe"),
                                };
                                clients.Add(client);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("MySQL Error", ex.Message, "OK");
                });
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                });
            }

            return clients;
        }

        public bool InsertClientData(string numero, string raison_sociale, string tel_fixe, string adresse, string abr_rc_nom, string ice, int IDCategorie_client, double ht_ttc, string ville, string secteur, int id_delai_paiement)
        {
            try
            {
                using (var connection = new MySqlConnection(connect()))
                {
                    connection.Open();

                    string query = "INSERT INTO Compte (numero, actif, raison_sociale, tel_fixe, adresse, type_compte, abr_rc_nom, ice, IDCategorie_client, ht_ttc, ville, secteur, id_delai_paiement, NouveauClient, D_C,Registre_com) VALUES (@numero, @actif, @raison_sociale, @tel_fixe, @adresse, @type_compte, @abr_rc_nom, @ice, @IDCategorie_client, @ht_ttc, @ville, @secteur, @id_delai_paiement, @NouveauClient, @D_C,@Registre_com)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@numero", numero);
                        command.Parameters.AddWithValue("@actif", 1);
                        command.Parameters.AddWithValue("@raison_sociale", raison_sociale);
                        command.Parameters.AddWithValue("@tel_fixe", tel_fixe);
                        command.Parameters.AddWithValue("@adresse", adresse);
                        command.Parameters.AddWithValue("@type_compte", "CL");
                        command.Parameters.AddWithValue("@abr_rc_nom", abr_rc_nom);
                        command.Parameters.AddWithValue("@ice", ice);
                        command.Parameters.AddWithValue("@IDCategorie_client", IDCategorie_client);
                        command.Parameters.AddWithValue("@ht_ttc", ht_ttc);
                        command.Parameters.AddWithValue("@ville", ville);
                        command.Parameters.AddWithValue("@secteur", secteur);
                        command.Parameters.AddWithValue("@id_delai_paiement", id_delai_paiement);
                        command.Parameters.AddWithValue("@NouveauClient", 1);
                        command.Parameters.AddWithValue("@D_C", "d");
                        command.Parameters.AddWithValue("@Registre_com", LoginPage.IdCom);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Client data inserted successfully
                            Debug.WriteLine("Client inserted successfully");
                            return true;
                        }
                        else
                        {
                            // No rows affected, failed to insert client data
                            Debug.WriteLine("Failed to insert client data");
                            return false;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL exceptions
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("MySQL Error", ex.Message, "OK");
                });
                Debug.WriteLine("MySQL Error: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                });
                Debug.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


        public List<DelaiPaiement> GetDelaiPaiements()
        {
            List<DelaiPaiement> delaiPaiements = new List<DelaiPaiement>();

            try
            {
                using (var connection = new MySqlConnection(connect()))
                {
                    connection.Open();

                    string query = "SELECT id_delai_paiement, libelle FROM delai_paiement";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DelaiPaiement delaiPaiement = new DelaiPaiement
                                {
                                    IDDelaiPaiement = reader.GetInt32("id_delai_paiement"),
                                    Libelle = reader.GetString("libelle")
                                };
                                delaiPaiements.Add(delaiPaiement);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("MySQL Error", ex.Message, "OK");
                });
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                });
            }

            return delaiPaiements;
        }

        public List<CategorieClient> GetCategorieClients()
        {
            List<CategorieClient> categories = new List<CategorieClient>();

            try
            {
                using (var connection = new MySqlConnection(connect()))
                {
                    connection.Open();

                    string query = "SELECT IDCategorie_client, Libelle FROM categorie_client";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategorieClient category = new CategorieClient
                                {
                                    IDCategorie_client = reader.GetInt32("IDCategorie_client"),
                                    Libelle = reader.GetString("Libelle")
                                };
                                categories.Add(category);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("MySQL Error", ex.Message, "OK");
                });
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                });
            }

            return categories;
        }



        public bool CheckClientExists(string numero)
        {
            using (var connection = new MySqlConnection(connect()))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Compte WHERE numero = @numero";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@numero", numero);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public List<CategorieArticle> GetCategoriesArticle()
        {
            List<CategorieArticle> categories = new List<CategorieArticle>();
            ByteArrayToImageConverter byteArrayToImageConverter= new ByteArrayToImageConverter();

            try
            {
                using (var connection = new MySqlConnection(connect()))
                {
                    connection.Open();

                    string query = "SELECT IDFamille_Art, Libelle ,image  FROM famille_art where actif=1";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategorieArticle category = new CategorieArticle
                                {
                                    IdCategorie = reader.GetInt32(0),
                                    NameCategorie = reader.GetString(1),
                                    ImageDataCategorie = reader.GetFieldValue<byte[]>(2)
                                    
                                };
                                categories.Add(category);
                            }
                        }
                    }
                }
               
            }
            
            catch (MySqlException ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("MySQL Error", ex.Message, "OK");
                });
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                });
            }

            return categories;
        }




        public List<Product> GetProductsByCategory(int categoryId)
        {
            List<Product> products = new List<Product>();

            try
            {
                using (var connection = new MySqlConnection(connect()))
                {
                    connection.Open();

                    string query = "SELECT IDARTICLE, reference, prix, image FROM article WHERE IDFamille_Art = @categoryId AND actif = 1";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@categoryId", categoryId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product
                                {
                                    ProductId = reader.GetInt32(0),
                                    ProductName = reader.GetString(1),
                                    Price = reader.GetFloat(2),
                                    ImageData = reader.GetFieldValue<byte[]>(3)
                                };
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("MySQL Error", ex.Message, "OK");
                });
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                });
            }

            return products; // Move the return statement outside the while loop
        }

    }
}
