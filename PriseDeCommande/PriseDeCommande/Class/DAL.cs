using MySqlConnector;
using MySqlX.XDevAPI;
using PriseDeCommande.Class;
using PriseDeCommande.Pages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                    string query = "SELECT IDCompte FROM compte WHERE sous_nom = @username AND password = @password AND actif = 1 AND type_compte='RC'";
                    //string query = "SELECT COUNT(*) FROM user WHERE username = @username AND password = @password";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            isValidUser = true;
                            LoginPage.IdCom=reader.GetInt32(0);
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


        public List<Clients> GetClientsForRComercial(String rc)
        {
            List<Clients> clients = new List<Clients>();

            try
            {
                using (var connection = new MySqlConnection(connect()))
                {
                    connection.Open();

                    string query = "SELECT IDCompte,sous_nom,adresse,ville,  FROM Compte WHERE Registre_com = @Rcomercial and type_compte= 'CL' and actif=1 ";


                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Rcomercial", rc);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Clients client = new Clients
                                {
                                    ClientId = reader.GetInt32("IDCompte"),
                                    ClientName = reader.GetString("sous_nom"),
                                    ClientAdresse=reader.GetString("adresse"),
                                    ClientVille = reader.GetString("ville"),
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


        public bool InsertClient(string numero, string raison_sociale,  string tel_fixe, string adresse, string ville)
        {
            try
            {
                using (var connection = new MySqlConnection(connect()))
                {
                    connection.Open();

                    string query = "INSERT INTO Compte (numero,raison_sociale,Registre_com,tel_fixe,adresse,type_compte,ville, actif) VALUES (@numero,@raison_sociale,@Registre_com,@tel_fixe,@adresse,'CL',@ville,1)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@numero", numero);
                        command.Parameters.AddWithValue("@raison_sociale", raison_sociale);
                        command.Parameters.AddWithValue("@Registre_com", LoginPage.IdCom);
                        command.Parameters.AddWithValue("@tel_fixe", tel_fixe);
                        command.Parameters.AddWithValue("@adresse", adresse);
                        command.Parameters.AddWithValue("@ville", ville);


                        int rowsAffected =  command.ExecuteNonQuery();
                        connection.Close();
                        return rowsAffected > 0;
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

            return false;
        }


        //public List<Category> GetCategories()
        //{
        //    List<Category> categories = new List<Category>();

        //    try
        //    {
        //        using (var connection = new MySqlConnection(connect()))
        //        {
        //            connection.Open();

        //            string query = "SELECT CategoryId, CategoryName FROM categories";

        //            using (var command = new MySqlCommand(query, connection))
        //            {
        //                using (var reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        Category category = new Category
        //                        {
        //                            //CategoryId = reader.GetInt32("CategoryId"),
        //                            //CategoryName = reader.GetString("CategoryName")
        //                        };
        //                        categories.Add(category);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (MySqlException ex)
        //    {
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            await Application.Current.MainPage.DisplayAlert("MySQL Error", ex.Message, "OK");
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        //        });
        //    }

        //    return categories;
        //}


        //public List<Product> GetProductsByCategory(int categoryId)
        //{
        //    List<Product> products = new List<Product>();

        //    try
        //    {
        //        using (var connection = new MySqlConnection(connect()))
        //        {
        //            connection.Open();

        //            string query = "SELECT ProductId, ProductName, Price FROM products WHERE CategoryId = @categoryId";

        //            using (var command = new MySqlCommand(query, connection))
        //            {
        //                command.Parameters.AddWithValue("@categoryId", categoryId);
        //                using (var reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        Product product = new Product
        //                        {
        //                            //ProductId = reader.GetInt32("ProductId"),
        //                            //ProductName = reader.GetString("ProductName"),
        //                            //Price = reader.GetDecimal("Price")
        //                        };
        //                        products.Add(product);
        //                        return products;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (MySqlException ex)
        //    {
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            await Application.Current.MainPage.DisplayAlert("MySQL Error", ex.Message, "OK");
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        //        });
        //    }

        //    return products;
        //}
    }
}
