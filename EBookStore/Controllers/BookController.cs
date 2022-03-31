using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBookStore.Models;
using System.Data.SqlClient;

namespace EBookStore.Controllers
{
    public class BookController : Controller
    {
        DBConfig dbConfig;

        public BookController()
        {
            this.dbConfig = new DBConfig();
        }

        // GET: Books
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateBook()
        {
            List<SelectListItem> genres = new List<SelectListItem>();
            string connectionString = dbConfig.getConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand sql = new SqlCommand("SELECT * FROM genre", conn);
            SqlDataReader reader = sql.ExecuteReader();
            genres.Clear();
            while (reader.Read())
            {
                genres.Add(new SelectListItem()
                {
                    Value = reader["id"].ToString(),
                    Text = reader["name"].ToString(),
                    Selected = false
                });
            }
            conn.Close();
            ViewBag.Genres = genres;


            List<SelectListItem> publishers = new List<SelectListItem>();
            conn.Open();
            sql = new SqlCommand("SELECT * FROM publishers", conn);
            reader = sql.ExecuteReader();
            publishers.Clear();
            while (reader.Read())
            {
                publishers.Add(new SelectListItem()
                {
                    Value = reader["id"].ToString(),
                    Text = reader["name"].ToString()
                });
            }
            conn.Close();
            ViewBag.Publishers = publishers;

            List<SelectListItem> authors = new List<SelectListItem>();
            conn.Open();
            sql = new SqlCommand("SELECT * FROM authors", conn);
            reader = sql.ExecuteReader();
            authors.Clear();
            while (reader.Read())
            {
                authors.Add(new SelectListItem()
                {
                    Value = reader["id"].ToString(),
                    Text = reader["name"].ToString()
                });
            }
            conn.Close();
            ViewBag.Authors = authors;

            return View();
        }

        [HttpPost]
        public ActionResult CreateBook(BookModel bm)
        {
            if (Session["utype"] != null)
            {
                if (Session["utype"].ToString() == "ADMIN")
                {
                    string connectionString = dbConfig.getConnectionString();
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand sql = new SqlCommand("INSERT INTO books(name,isbn,authorId,genreId,publisherId,price,stock) " +
                        "values(@name,@isbn,@authorId,@genreId,@publisherId,@price,@stock)", conn);
                    sql.Parameters.AddWithValue("@name", bm.name);
                    sql.Parameters.AddWithValue("@isbn", bm.isbn);
                    sql.Parameters.AddWithValue("@authorId", bm.authorId);
                    sql.Parameters.AddWithValue("@genreId", bm.genreId);
                    sql.Parameters.AddWithValue("@publisherId", bm.publisherId);
                    sql.Parameters.AddWithValue("@price", bm.price);
                    sql.Parameters.AddWithValue("@stock", bm.stock);

                    sql.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Only admin can add book!");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Not logged in");
            }
            return RedirectToAction("CreateBook");
        }

        [HttpGet]
        public ActionResult CreateGenre()
        {
            List<GenreModel> genres = new List<GenreModel>();

            string connectionString = dbConfig.getConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand sql = new SqlCommand("SELECT * FROM genre", conn);
            SqlDataReader reader = sql.ExecuteReader();

            genres.Clear();
            while (reader.Read())
            {
                genres.Add(new GenreModel()
                {
                    id = (int)reader["id"],
                    name = reader["name"].ToString()
                });
            }

            conn.Close();
            return View(genres);
        }

        [HttpPost]
        public ActionResult CreateGenre(String genre)
        {
            if (Session["utype"] != null)
            {
                if (Session["utype"].ToString() == "ADMIN")
                {
                    string connectionString = dbConfig.getConnectionString();
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand sql = new SqlCommand("INSERT INTO genre(name) values(@name)", conn);
                    sql.Parameters.AddWithValue("@name", genre);
                    sql.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Only admin can add genre!");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Not logged in");
            }
            return RedirectToAction("CreateGenre");
        }

        [HttpGet]
        public ActionResult DeleteGenre(string genreId)
        {
            System.Diagnostics.Debug.WriteLine("Going to delete genre with id: " + genreId);
            string connectionString = dbConfig.getConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand sql = new SqlCommand("SELECT * FROM genre", conn);
            SqlDataReader reader = sql.ExecuteReader();
            if (reader.Read())
            {
                GenreModel gm = new GenreModel()
                {
                    id = (int)reader["id"],
                    name = reader["name"].ToString()
                };
                conn.Close();
                return View(gm);

            }
            else
            {
                conn.Close();
                return View();
            }
        }

        [HttpPost]
        public ActionResult DeleteGenre(int genreId, GenreModel gm)
        {
            if (Session["utype"] != null)
            {
                if (Session["utype"].ToString() == "ADMIN")
                {
                    string connectionString = dbConfig.getConnectionString();
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand sql = new SqlCommand("DELETE FROM genre WHERE id = @id", conn);
                    sql.Parameters.AddWithValue("@id", genreId);
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
                System.Diagnostics.Debug.WriteLine("Not logged in");
            }
            return RedirectToAction("CreateGenre");
        }

        [HttpGet]
        public ActionResult CreatePublisher()
        {
            List<PublisherModel> publishers = new List<PublisherModel>();

            string connectionString = dbConfig.getConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand sql = new SqlCommand("SELECT * FROM publishers", conn);
            SqlDataReader reader = sql.ExecuteReader();

            publishers.Clear();
            while (reader.Read())
            {
                publishers.Add(new PublisherModel()
                {
                    id = (int)reader["id"],
                    name = reader["name"].ToString()
                });
            }

            conn.Close();
            return View(publishers);
        }

        [HttpPost]
        public ActionResult CreatePublisher(String publisher)
        {
            if (Session["utype"] != null)
            {
                if (Session["utype"].ToString() == "ADMIN")
                {
                    string connectionString = dbConfig.getConnectionString();
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand sql = new SqlCommand("INSERT INTO publishers(name) values(@name)", conn);
                    sql.Parameters.AddWithValue("@name", publisher);
                    sql.ExecuteNonQuery();
                    conn.Close();
                    return RedirectToAction("CreatePublisher");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Only admin can add genre!");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Not logged in");
            }
            return RedirectToAction("CreatePublisher");
        }

        [HttpPost]
        public ActionResult DeletePublisher(int publisherId, PublisherModel pm)
        {
            if (Session["utype"] != null)
            {
                if (Session["utype"].ToString() == "ADMIN")
                {
                    string connectionString = dbConfig.getConnectionString();
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand sql = new SqlCommand("DELETE FROM publishers WHERE id = @id", conn);
                    sql.Parameters.AddWithValue("@id", publisherId);
                    sql.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Only admin can delete publishers!");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Not logged in");
            }
            return RedirectToAction("CreatePublisher");
        }

        [HttpGet]
        public ActionResult CreateAuthor()
        {
            List<AuthorModel> authors = new List<AuthorModel>();

            string connectionString = dbConfig.getConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand sql = new SqlCommand("SELECT * FROM authors", conn);
            SqlDataReader reader = sql.ExecuteReader();

            authors.Clear();
            while (reader.Read())
            {
                authors.Add(new AuthorModel()
                {
                    id = (int)reader["id"],
                    name = reader["name"].ToString()
                });
            }

            conn.Close();
            return View(authors);
        }

        [HttpPost]
        public ActionResult CreateAuthor(String author)
        {
            if (Session["utype"] != null)
            {
                if (Session["utype"].ToString() == "ADMIN")
                {
                    string connectionString = dbConfig.getConnectionString();
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand sql = new SqlCommand("INSERT INTO authors(name) values(@name)", conn);
                    sql.Parameters.AddWithValue("@name", author);
                    sql.ExecuteNonQuery();
                    conn.Close();
                    return RedirectToAction("CreateAuthor");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Only admin can add genre!");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Not logged in");
            }
            return RedirectToAction("CreateAuthor");
        }

        [HttpPost]
        public ActionResult DeleteAuthor(int authorId, AuthorModel am)
        {
            if (Session["utype"] != null)
            {
                if (Session["utype"].ToString() == "ADMIN")
                {
                    string connectionString = dbConfig.getConnectionString();
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand sql = new SqlCommand("DELETE FROM authors WHERE id = @id", conn);
                    sql.Parameters.AddWithValue("@id", authorId);
                    sql.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Only admin can delete authors!");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Not logged in");
            }
            return RedirectToAction("CreateAuthor");
        }

        [HttpGet]
        public ActionResult SearchBooks()
        {
            List<BookDataModel> books = new List<BookDataModel>();

            string connectionString = dbConfig.getConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand sql = new SqlCommand("select b.id, b.name, b.isbn as isbn, b.price as price, b.stock as stock, p.name as publisher, a.name as author, g.name as genre from books b " +
                                            "left join authors a on b.authorId = a.id " +
                                            "left join genre g on b.genreId = g.id " +
                                            "left join publishers p on b.publisherId = p.id; ", conn);
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

    }
}