using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace AppDev1_Assignment2_API.Models
{
    public class DBApplication
    {
        public Response GetAllProducts(SqlConnection con)
        {
            Response response = new Response();

            string query = "select * from market";
            
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            List<Market> listOfProducts = new List<Market>();

            if (dt.Rows.Count > 0 )
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Market product = new Market();
                    product.product_name = (string)dt.Rows[i]["product_name"];
                    product.product_id = (int)dt.Rows[i]["product_id"];
                    product.amount = (int)dt.Rows[i]["amount"];
                    product.price = (double)dt.Rows[i]["price"];

                    listOfProducts.Add(product);
                }
            }

            if (listOfProducts.Count > 0 )
            {
                response.status_code = 200;
                response.status_message = "Successful, here are all the products";
                response.product = null;
                response.products = listOfProducts;
            }
            else
            {
                response.status_code = 100;
                response.status_message = "Query not success, please recheck";
                response.product = null;
                response.products = null;
            }

            return response;
        }

        public Response GetProductByID(SqlConnection con, int id)
        {
            Response response = new Response();

            string query = "select * from market where product_id = '" + id + "'";

            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                Market product = new Market();
                product.product_name = (string)dt.Rows[0]["product_name"];
                product.product_id = (int)dt.Rows[0]["product_id"];
                product.amount = (int)dt.Rows[0]["amount"];
                product.price = (double)dt.Rows[0]["price"];

                response.status_code = 200;
                response.status_message = "Success, product found";
                response.product = product;
                response.products = null;
            }
            else
            {
                response.status_code = 100;
                response.status_message = "Query not successful, please recheck";
                response.product = null;
                response.products = null;
            }

            return response;
        }

        public Response AddProduct(SqlConnection con, Market product)
        {
            Response response = new Response();

            string query = "insert into market values(@name, @id, @amount, @price)";

            SqlCommand command = new SqlCommand(query, con);

            command.Parameters.AddWithValue("@name", product.product_name);
            command.Parameters.AddWithValue("@id", product.product_id);
            command.Parameters.AddWithValue("@amount", product.amount);
            command.Parameters.AddWithValue("@price", product.price);

            con.Open();
            int i = command.ExecuteNonQuery();

            if (i == 1)
            {
                response.status_code = 200;
                response.status_message = "Success, product inserted";
                response.product = product;
                response.products = null;
            }
            else
            {
                response.status_code = 100;
                response.status_message = "Query not successful, please recheck";
                response.product = null;
                response.products = null;
            }

            con.Close();
            return response;
        }

        public Response UpdateProduct(SqlConnection con, Market product, int id)
        {
            Response response = new Response();

            string query = "update market set product_name=@name, amount=@amount, price=@price"
                            + " where product_id='" + id + "'";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@name", product.product_name);
            cmd.Parameters.AddWithValue("@amount", product.amount);
            cmd.Parameters.AddWithValue("@price", product.price);

            con.Open();
            int i = cmd.ExecuteNonQuery();

            if (i == 1)
            {
                response.status_code = 200;
                response.status_message = "Success, product updated";
                response.product = product;
            }
            else
            {
                response.status_code = 100;
                response.status_message = "Query not successful, please recheck";
            }

            con.Close();
            return response;
        }

        public Response PurchaseProduct(SqlConnection con, ArrayList purchase)
        {
            Response response = new Response();

            string name = purchase[0].ToString();
            int amount = int.Parse(purchase[1].ToString());

            string query1 = "select * from market where product_name='" + name + "'";

            SqlDataAdapter da = new SqlDataAdapter(query1, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                Market product = new Market();
                product.product_name = (string)dt.Rows[0]["product_name"];
                product.product_id = (int)dt.Rows[0]["product_id"];
                product.amount = (int)dt.Rows[0]["amount"];
                product.price = (double)dt.Rows[0]["price"];

                if (product.amount >= amount)
                {
                    product.amount -= amount;
                    double totalCost = product.price * amount;
                   
                    string query2 = "update market set amount=@amount where product_name='" + name + "'";

                    SqlCommand cmd = new SqlCommand(query2, con);
                    cmd.Parameters.AddWithValue("@amount", product.amount);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();

                    if (i == 1)
                    {
                        response.status_code = 200;
                        response.status_message = "Success, product purchased";
                        response.product = product;
                    }
                    else
                    {
                        response.status_code = 100;
                        response.status_message = "Query not successful, please recheck";
                    }
                }
                else
                {
                    response.status_code = 100;
                    response.status_message = "Purchase not successful, not enough items remaining";
                }
            }
            else
            {
                response.status_code = 100;
                response.status_message = "Query not successful, please recheck";
            }

            return response;
        }

        public Response DeleteProduct(SqlConnection con, int id)
        {
            Response response = new Response();

            string query = "delete from market where product_id='" + id + "'";

            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();
            int i = cmd.ExecuteNonQuery();

            if (i == 1)
            {
                response.status_code = 200;
                response.status_message = "Success, product deleted";
            }
            else
            {
                response.status_code = 100;
                response.status_message = "Query not successful, please recheck";
            }

            con.Close();
            return response;
        }

    }
}
