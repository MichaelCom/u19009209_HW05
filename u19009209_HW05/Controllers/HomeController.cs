using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using u19009209_HW05.Models;
using u19009209_HW05.Models.ViewModels;

namespace u19009209_HW05.Controllers
{
    public class HomeController : Controller
    {
        //Public Static list
        public DataService db = new DataService();

        public static List<BookHybrid> Books = new List<BookHybrid>();
        public static List<Student> Students = new List<Student>();
        public static List<Borrow> Borrows = new List<Borrow>();

        public static List<BookHybrid> SearchedBooks = new List<BookHybrid>();
        public static List<Student> SearchedStudents = new List<Student>();

        public static int CheckingBook = 0;

        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            List<BookHybrid> returnBooks = null;
            try
            {
                if (SearchedBooks.Count > 0)
                {
                    returnBooks = SearchedBooks;
                }
                else
                {
                    RefreshData();
                    returnBooks = Books;
                }

                ViewBag.Message = TempData["Message"];
                ViewBag.Types = db.GetTypes();
                ViewBag.Authors = db.GetAuthors();
            }
            catch (Exception message)
            {
                ViewBag.Message = message.Message;
            }
            return View(returnBooks);
        }

        [HttpGet]
        public ActionResult ClearBooks()
        {
            SearchedBooks.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SearchBooks(string name, int? typeId, int? authorId)
        {
            try
            {
                if (SearchedBooks.Count > 0 && name == "" && typeId == null && authorId == null)
                {
                    TempData["Message"] = "You Didnt Search For Anything Stop Wasting my Time !!!";
                }
                else
                {
                    SearchedBooks.Clear();
                    RefreshData();
                    if (name != "" && typeId != null && authorId != null)
                    {
                        //search has to be on all 3 parameters
                        SearchedBooks = Books.Where(x => x.name == name && x.typeId == typeId && x.authorId == authorId).ToList();
                    }
                    else if (name != "" && typeId != null && authorId == null)
                    {
                        // search on name and type 
                        SearchedBooks = Books.Where(x => x.name == name && x.typeId == typeId).ToList();
                    }
                    else if (name != "" && typeId == null && authorId != null)
                    {
                        // search on name and author
                        SearchedBooks = Books.Where(x => x.name == name && x.authorId == authorId).ToList();
                    }
                    else if (name == "" && typeId != null && authorId != null)
                    {
                        //search on type and author
                        SearchedBooks = Books.Where(x => x.typeId == typeId && x.authorId == authorId).ToList();
                    }
                    else if (name == "" && typeId == null && authorId != null)
                    {
                        //search on author
                        SearchedBooks = Books.Where(x => x.authorId == authorId).ToList();
                    }
                    else if (name == "" && typeId != null && authorId == null)
                    {
                        // search on type
                        SearchedBooks = Books.Where(x => x.typeId == typeId).ToList();
                    }
                    else if (name != "" && typeId == null && authorId == null)
                    {
                        // search on name 
                        SearchedBooks = Books.Where(x => x.name == name).ToList();
                    }
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
            RefreshData();
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
                ViewBag.Message = TempData["Details"];
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
            RefreshData();

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

            List<Student> returnStudents = null;
            if (SearchedStudents.Count > 0)
            {
                returnStudents = SearchedStudents;
            }
            else
            {
                RefreshData();
                returnStudents = Students;
            }

            ViewBag.Message = TempData["MessageStudent"];


            var Classes = Students.GroupBy(x => x.Class).Select(y => new { Class = y.Key }).OrderByDescending(z => z.Class).ToList();
            ViewBag.Classes = new SelectList(Classes, "Class", "Class");


            return View(returnStudents);
        }

        [HttpGet]
        public ActionResult ClearStudents()
        {
            SearchedStudents.Clear();
            return RedirectToAction("ViewStudents", new { bookId = CheckingBook });
        }

        [HttpPost]
        public ActionResult SearchStudents(string name, string Class)
        {
            try
            {
                if (SearchedStudents.Count > 0 && name == "" && Class == "")
                {
                    TempData["MessageStudent"] = "You Didnt Search For Anything Stop Wasting my Time !!!";
                }
                else
                {
                    SearchedStudents.Clear();
                    RefreshData();
                    if (name != "" && Class != "")
                    {
                        //search has to be on all 2 parameters
                        SearchedStudents = Students.Where(x => x.name == name && x.Class == Class).ToList();
                    }
                    else if (name != "" && Class == "")
                    {
                        // search on name 
                        SearchedStudents = Students.Where(x => x.name == name).ToList();
                    }
                    else if (name == "" && Class != "")
                    {
                        // search on name and author
                        SearchedStudents = Students.Where(x => x.Class == Class).ToList();
                    }
                }
            }
            catch (Exception message)
            {
                TempData["MessageStudent"] = message;
            }
            return RedirectToAction("ViewStudents", new { bookId = CheckingBook });
        }


        [HttpGet]
        public ActionResult BorrowBook(int studentId)
        {
            string res = db.BorrowBook(studentId, CheckingBook);
            if (res == "")
            {
                TempData["MessaeStudent"] = "Failed To Borrow Book for Student";
                return RedirectToAction("ViewStudents", new { bookId = CheckingBook });
            }
            return RedirectToAction("Details", new { bookId = CheckingBook });
        }


        [HttpGet]
        public ActionResult ReturnBook()
        {
            string res = db.ReturnBook(Books.Where(x => x.bookId == CheckingBook).FirstOrDefault().borrowId);
            if (res == "")
            {
                TempData["MessageStudent"] = "An Error Occourred While attempting to Return the book";
                return RedirectToAction("ViewStudents", new { bookId = CheckingBook });
            }
            TempData["Details"] = res;
            return RedirectToAction("Details", new { bookId = CheckingBook });
        }


        //Method to Refresh Lists 
        private void RefreshData()
        {
            Books.Clear();
            Students.Clear();
            Borrows.Clear();

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
                    book.borrowId = Borrows[i].borrowId;
                }
            }
        }


    }
}