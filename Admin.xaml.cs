using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppDev1_assignment1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
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

        private void insert_button_Click(object sender, RoutedEventArgs e)
        {
            if (con != null)
            {
                try
                {
                    con.Open();
                    string query = "insert into market values(@prodName, @prodID, @amount, @price)";
                    cmd = new SqlCommand(query, con);

                    try
                    {
                        cmd.Parameters.AddWithValue("@prodName", product_name_box.Text);
                        cmd.Parameters.AddWithValue("@prodID", int.Parse(product_id_box.Text));
                        cmd.Parameters.AddWithValue("@amount", int.Parse(amount_box.Text));
                        cmd.Parameters.AddWithValue("@price", Math.Round(float.Parse(price_box.Text), 2));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please ensure all boxes are filled properly");
                    }

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Insertion is successful");
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
                    dt.Columns["amount"].ColumnName = "Amount(kg)";
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

        private void select_button_Click(object sender, RoutedEventArgs e)
        {
            if (con != null)
            {
                try
                {
                    con.Open();
                    string query = "select * from market where product_id=@prodID";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@prodID", product_id_box.Text);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    try
                    {
                        if (dt.Rows != null)
                        {
                            product_name_box.Text = dt.Rows[0]["product_name"].ToString();
                            amount_box.Text = dt.Rows[0]["amount"].ToString();
                            price_box.Text = dt.Rows[0]["price"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Product with the current ID does not exist");
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

        private void update_button_Click(object sender, RoutedEventArgs e)
        {
            if (con != null)
            {
                try
                {
                    con.Open();
                    string query = "update market set product_name=@prodName, amount=@amount, "
                                    + "price=@price where product_id=@prodID";
                    cmd = new SqlCommand(query, con);
                    try
                    {
                        cmd.Parameters.AddWithValue("@prodName", product_name_box.Text);
                        cmd.Parameters.AddWithValue("@prodID", int.Parse(product_id_box.Text));
                        cmd.Parameters.AddWithValue("@amount", int.Parse(amount_box.Text));
                        cmd.Parameters.AddWithValue("@price", Math.Round(float.Parse(price_box.Text), 2));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please ensure all boxes all filled properly");
                    }

                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        MessageBox.Show("Information updated");
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

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            if (con != null)
            {
                try
                {
                    con.Open();
                    string query = "delete from market where product_id=@prodID";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@prodID", product_id_box.Text);

                    int i = cmd.ExecuteNonQuery();

                    if (i == 1)
                    {
                        MessageBox.Show("Entry deleted");
                    }
                    else
                    {
                        MessageBox.Show("Please check if you have entered the proper ID");
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

        //What can be improved:
        //Exception message that is thrown when an improper value is
        //entered into the box (make it more user-friendly)
        //Not allow negative values to be entered for price and amount
    }
}
