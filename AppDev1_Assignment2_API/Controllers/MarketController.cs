using Microsoft.AspNetCore.Mvc;
using AppDev1_Assignment2_API.Models;
using System.Data.SqlClient;
using System.Collections;

namespace AppDev1_Assignment2_API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class MarketController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MarketController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllProducts")]

        public Response GetAllProducts()
        {
            Response response = new Response();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("marketConnection"));

            DBApplication dba = new DBApplication();
            response = dba.GetAllProducts(con);

            return response;
        }

        [HttpGet]
        [Route("GetProductByID/{id}")]

        public Response GetProductByID(int id)
        {
            Response response = new Response();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("marketConnection"));

            DBApplication dba = new DBApplication();
            response = dba.GetProductByID(con, id);

            return response;
        }

        [HttpPost]
        [Route("AddProduct")]

        public Response AddProduct(Market product)
        {
            Response response = new Response();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("marketConnection"));

            DBApplication dba = new DBApplication();
            response = dba.AddProduct(con, product);

            return response;
        }

        [HttpPut]
        [Route("UpdateProduct/{id}")]

        public Response UpdateProduct(Market product, int id)
        {
            Response response = new Response();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("marketConnection"));

            DBApplication dba = new DBApplication();
            response = dba.UpdateProduct(con, product, id);

            return response;
        }

        [HttpPut]
        [Route("PurchaseProduct")]

        public Response PurchaseProduct(ArrayList purchase)
        {
            Response response = new Response();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("marketConnection"));

            DBApplication dba = new DBApplication();
            response = dba.PurchaseProduct(con, purchase);
            //if just working on API side, can pass through multiple inputs (ie name and amount)
            //however when passing it through wpf, can only be at most 1 variable and 1 object
            //so here we are passing an object of ArrayList purchase (defined in Response)

            return response;
        }

        [HttpDelete]
        [Route("DeleteProduct/{id}")]

        public Response DeleteProduct(int id)
        {
            Response response = new Response();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("marketConnection"));

            DBApplication dba = new DBApplication();
            response = dba.DeleteProduct(con, id);

            return response;
        }

    }
}
