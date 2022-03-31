using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBookStore.Models;
using System.Data.SqlClient;

namespace EBookStore.Controllers
{
    public class OrderController : Controller
    {
        DBConfig dbConfig;

        public OrderController()
        {
            this.dbConfig = new DBConfig();
        }

        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateOrder(int bookId)
        {
            if (Session["utype"] != null)
            {
                if (Session["utype"].ToString() == "CUSTOMER")
                {
                    string connectionString = dbConfig.getConnectionString();
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand sql = new SqlCommand("INSERT INTO orders(bookId,userId,isCheckedOut) values(@bookId, @userId, @isCheckedOut)", conn);
                    sql.Parameters.AddWithValue("@bookId", bookId);
                    sql.Parameters.AddWithValue("@userId", Session["uid"]);
                    sql.Parameters.AddWithValue("@isCheckedOut", "false");
                    sql.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Only admin can delete genre!");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Not logged in!!!!!!!!");
            }
            return RedirectToAction("SearchBooks","Book");
        }

        [HttpGet]
        public ActionResult Cart()
        {
            List<BookDataModel> books = new List<BookDataModel>();

            string connectionString = dbConfig.getConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand sql = new SqlCommand("select b.id, b.name, b.isbn as isbn, b.price as price, b.stock as stock, p.name as publisher, a.name as author, g.name as genre from orders o " +
                                            "left join books b on b.id = o.bookId " +
                                            "left join authors a on a.id = b.authorId " +
                                            "left join genre g on g.id = b.genreId " +
                                            "left join publishers p on p.id = b.publisherId " +
                                            "where o.isCheckedout = 'false'; ", conn);
            SqlDataReader reader = sql.ExecuteReader();

            books.Clear();
            while (reader.Read())
            {
                books.Add(new BookDataModel()
                {
                    id = (int)reader["id"],
                    isbn = reader["isbn"].ToString(),
                    price = Convert.ToInt32(reader["price"]),
                    stock = (int)reader["stock"],
                    name = reader["name"].ToString(),
                    publisher = reader["publisher"].ToString(),
                    author = reader["author"].ToString(),
                    genre = reader["genre"].ToString()
                });
            }

            conn.Close();
            return View(books);
        }

        [HttpGet]
        public ActionResult Checkout()
        {
            string connectionString = dbConfig.getConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand sql = new SqlCommand("UPDATE orders SET isCheckedOut = 'true' WHERE isCheckedOut = 'false'", conn);
            sql.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("SearchBooks", "Book");
        }
    }
}