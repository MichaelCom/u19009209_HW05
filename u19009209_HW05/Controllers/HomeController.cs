using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using u19009209_HW05.Models;
using u19009209_HW05.Models.ViewModels;

namespace u19009209_HW05.Controllers
{
    public class HomeController : Controller
    {

        //SqlConnection SqlConnection = new SqlConnection("Data Source = DESKTOP - 3ML24ME\\SQLEXPRESS; Initial Catalog = Library; Integrated Security = True");


        public DataService db = new DataService();
        public static List<BookHybrid> Books = new List<BookHybrid>();
        public static List<Student> Students = new List<Student>();
        public static List<Borrow> Borrows = new List<Borrow>();
        public static List<BookHybrid> Searched = null; 
        public static int CheckingBook = 0;

        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            List<BookHybrid> returnBooks = null;
            try
            {
                Books.Clear();
                Borrows.Clear();
                if (Searched != null)
                {
                    returnBooks = Searched;
                }
                else
                {
                    Books = db.getAllBooks();
                    Students = db.getAllStudents();
                    Borrows = db.getAllBorrows();
                    for (int i = 0; i < Borrows.Count; i++)
                    {
                        if (Borrows[i].broughtDate == null)
                        {
                            BookHybrid book = Books.Where(x => x.bookId == Borrows[i].bookId).FirstOrDefault();
                            book.status = false;
                            book.studentId = (int)Borrows[i].studentId;
                        }
                    }
                    returnBooks = Books;
                }


                ViewBag.Types = db.GetTypes();

                ViewBag.Authors = db.GetAuthors();

            }
            catch (Exception message)
            {
                ViewBag.Message = message.Message;
            }
            finally
            {
                db.closeConnection();
            }

            return View(returnBooks);
        }

        [HttpGet]
        public ActionResult Clear()
        {
            Searched.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]

        public ActionResult Search(string name, int? typeId, int? authorId)
        {
            try
            {
                if (Searched != null)
                {
                    Searched.Clear();
                }
                else if (name != "" && typeId != null && authorId != null)
                {
                    //search has to be on all 3 parameters
                    db.getAllBooks();
                    Searched = Books.Where(x => x.name == name && x.typeId == typeId && x.authorId == authorId).ToList();
                }
                else if (name != "" && typeId != null && authorId == null)
                {
                    // search on name and type 
                    db.getAllBooks();
                    Searched = Books.Where(x => x.name == name && x.typeId == typeId).ToList();
                }
                else if (name != "" && typeId == null && authorId != null)
                {
                    // search on name and author
                    db.getAllBooks();
                    Searched = Books.Where(x => x.name == name && x.authorId == authorId).ToList();
                }
                else if (name == "" && typeId != null && authorId != null)
                {
                    //search on type and author
                    db.getAllBooks();
                    Searched = Books.Where(x => x.typeId == typeId && x.typeId == typeId).ToList();
                }
                else if (name == "" && typeId == null && authorId != null)
                {
                    //search on author
                    db.getAllBooks();
                    Searched = Books.Where(x => x.authorId == authorId).ToList();
                }
                else if (name == "" && typeId != null && authorId == null)
                {
                    // search on type
                    db.getAllBooks();
                    Searched = Books.Where(x => x.typeId == typeId).ToList();
                }
                else if (name != "" && typeId == null && authorId == null)
                {
                    // search on name 
                    db.getAllBooks();
                    Searched = Books.Where(x => x.name == name).ToList();
                }
                else
                {
                    TempData["Message"] = "Search field empty. Please enter some values.";
                }
            }
            catch (Exception message)
            {
                TempData["Message"] = message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int bookId)
        {
            BookHybrid bookInList = Books.Where(x => x.bookId == bookId).FirstOrDefault();
            if (bookInList != null)
            {
                var AllRecordsOfBookInBorrows = Borrows.Where(x => x.bookId == bookId).ToList();
                bookInList.totalBorrows = AllRecordsOfBookInBorrows.Count();
                List<BorrowsHybrid> RecordOfBorrowed = new List<BorrowsHybrid>();
                for (int i = 0; i < AllRecordsOfBookInBorrows.Count(); i++)
                {
                    BorrowsHybrid record = new BorrowsHybrid();
                    record.borrowId = AllRecordsOfBookInBorrows[i].borrowId;
                    record.studentName = Students.Where(x => x.studentId == AllRecordsOfBookInBorrows[i].studentId).FirstOrDefault().name;
                    record.takenDate = AllRecordsOfBookInBorrows[i].takenDate;
                    record.broughtDate = AllRecordsOfBookInBorrows[i].broughtDate;
                    RecordOfBorrowed.Add(record);
                }
                bookInList.borrowedRecords = RecordOfBorrowed;
            }
            else
            {
                ViewBag.Message = "Book Not Found";
            }
            return View(bookInList);
        }

        [HttpGet]
        public ActionResult ViewStudents(int bookId)
        {
            BookHybrid book = Books.Where(x => x.bookId == bookId).FirstOrDefault();
            ViewBag.Status = book.status;
            if (book.studentId != 0)
            {
                ViewBag.studentId = book.studentId;
            }
            else
            {
                ViewBag.studentId = 0;
            }
            CheckingBook = 0;
            CheckingBook = bookId;
            return View(Students);
        }

        
    }
}