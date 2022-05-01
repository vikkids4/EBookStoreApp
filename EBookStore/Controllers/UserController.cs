using System;
using System.Web.Mvc;
using EBookStore.Models;
using System.Data.SqlClient;

namespace EBookStore.Controllers
{
    public class UserController : Controller
    {
        DBConfig dbConfig;

        public UserController()
        {
            this.dbConfig = new DBConfig();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserModel um)
        {
            Console.WriteLine("Received value: " + um.firstName);
            string connectionString = dbConfig.getConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand sql = new SqlCommand("insert into users(firstName, lastName, dob, email, phoneNumber, userType, username, password)" +
                " values(@fname, @lname, @dob, @email, @pnum, @utype, @uname, @pass)", conn);

            sql.Parameters.AddWithValue("@fname", um.firstName);
            sql.Parameters.AddWithValue("@lname", um.lastName);
            sql.Parameters.AddWithValue("@dob", DateTime.Now);
            sql.Parameters.AddWithValue("@email", um.email);
            sql.Parameters.AddWithValue("@pnum", um.phone_number);
            sql.Parameters.AddWithValue("@utype", um.userType);
            sql.Parameters.AddWithValue("@uname", um.username);
            sql.Parameters.AddWithValue("@pass", um.password);

            sql.ExecuteNonQuery();
            conn.Close();
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            Console.WriteLine("Received Login Form Details: " + username + ":" + password);
            if (Session["uname"] != null)
            {
                Console.WriteLine("Alreadyy loggedin!");
                System.Diagnostics.Debug.WriteLine("Alreadyy loggedin!");
                return View();
            } else {
                string connectionString = dbConfig.getConnectionString();
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand sql = new SqlCommand("SELECT * FROM users WHERE username = @uname AND password = @pass", conn);
                sql.Parameters.AddWithValue("@uname", username);
                sql.Parameters.AddWithValue("@pass", password);
                SqlDataReader reader = sql.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine("Login success!");
                    Response.Write("<script>alert('login successful');</script>");
                    System.Diagnostics.Debug.WriteLine("Login Success");
                    Session["uid"] = reader["id"];
                    Session["uname"] = username;
                    Session["utype"] = reader["userType"];
                }
                else
                {
                    Console.WriteLine("Login Failed!");
                    Response.Write("<script>alert('login failed');</script>");
                }
                conn.Close();
                return View();
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            if (Session["uname"] != null)
            {
                Session["uname"] = null;
                return View();
            }
            else {
                return View();
            }
            }
        }
}