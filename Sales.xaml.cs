using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppDev1_assignment1
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : Window
    {
        public Sales()
        {
            InitializeComponent();
        }

        static SqlConnection con;
        static SqlCommand cmd;

        private void connect_button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=EricWlaptop;Initial Catalog=master;Integrated Security=True";
            con = new SqlConnection(connectionString);
            con.Open();
            MessageBox.Show("Connection to database established");
            con.Close();
        }

        private void datagrid_button_Click(object sender, RoutedEventArgs e)
        {
            if (con != null)
            {
                try
                {
                    con.Open();
                    string query = "select * from market";

                    cmd = new SqlCommand(query, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dt.Columns["product_name"].ColumnName = "Product Name";
                    dt.Columns["product_id"].ColumnName = "Product ID";
                    dt.Columns["amount"].ColumnName = "Amount in inventory(kg)";
                    dt.Columns["price"].ColumnName = "Price(CA$/kg)";
                    market_datagrid.ItemsSource = dt.AsDataView();

                    DataContext = da;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Please connect to database first");
            }
        }

        private void purchase_button_Click(object sender, RoutedEventArgs e)
        {
            if (con != null)
            {
                try
                {
                    con.Open();

                    string query1 = "select * from market";

                    SqlDataAdapter da = new SqlDataAdapter(query1, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    List<Market> marketProducts = new List<Market>();

                    if (dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            Market market = new Market();
                            market.product_name = (string)dt.Rows[j]["product_name"];
                            market.product_id = (int)dt.Rows[j]["product_id"];
                            market.amount = (int)dt.Rows[j]["amount"];
                            market.price = (double)dt.Rows[j]["price"];

                            marketProducts.Add(market);
                        }
                    }

                    string userProductName = product_name_box.Text;
                    int userAmount = int.Parse(amount_box.Text);
                    int databaseAmount = 0;
                    double databasePrice = 0;

                    for (int k=0;k<marketProducts.Count;k++)
                    {
                        if (marketProducts[k].product_name.Equals(userProductName))
                        {
                            databaseAmount = marketProducts[k].amount;
                            databasePrice = marketProducts[k].price;
                            break;  
                        }
                    }

                    string query2 = "update market set amount=@amount where product_name=@prodName";
                    cmd = new SqlCommand(query2, con);

                    try
                    {
                        cmd.Parameters.AddWithValue("@prodName", userProductName);
                        cmd.Parameters.AddWithValue("@amount", (databaseAmount-userAmount));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please ensure all boxes all filled properly" +
                                        "\nProduct name should include proper capitilization" +
                                        "\nAmount box should not include letters");
                    }

                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        MessageBox.Show("Purchase confirmed for " + userAmount + " kg of "+
                                        userProductName + " for the price of " + 
                                        (databasePrice*userAmount) + "$");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Please connect to database first");
            }
        }

    }

    //Things to improve: fix up the purchase button to have no potential errors
}
