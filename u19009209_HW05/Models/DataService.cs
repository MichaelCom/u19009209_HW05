using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.Common;
using u19009209_HW05.Models.ViewModels;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace u19009209_HW05.Models
{
    public class DataService
    {

        private static DataService instance;
        public static DataService GetDataService()
        {
            if (instance == null)
            {
                instance = new DataService();
            }
            return instance;
        }
        private SqlConnection buildConnection()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder["Data Source"] = "DESKTOP-3ML24ME\\SQLEXPRESS";
            sqlConnectionStringBuilder["Initial Catalog"] = "Library";
            sqlConnectionStringBuilder["IntergratedSecurity"] = ".";
            return new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
        }

        public bool openConnection()
        {
            using (SqlConnection conn = buildConnection())
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool closeConnection()
        {
            using (SqlConnection conn = buildConnection())
            {
                try
                {
                    conn.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public List<BookHybrid> getAllBooks()
        {
            List<BookHybrid> bookList = new List<BookHybrid>();
            String command = "SELECT book.[bookId] as bookId ,book.[name] as name ,book.[pagecount] as pagecount ,book.[point] as point, auth.[surname] as authorSurname ,type.[name] typeName,  book.[authorId],book.[typeId] " +
                            "FROM [Library].[dbo].[books] book " +
                            "JOIN [Library].[dbo].[authors] auth on book.authorId = auth.authorId " +
                            "JOIN [Library].[dbo].[types] type on book.typeId = type.typeId";
            using (SqlConnection conn = buildConnection())
            {
                try
                {
                    openConnection();
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        SqlDataReader readBooks = cmd.ExecuteReader();
                        while (readBooks.Read())
                        {
                            BookHybrid book = new BookHybrid();
                            book.bookId = (int)readBooks["bookId"];
                            book.name = (string)readBooks["name"];
                            book.pagecount = (int)readBooks["pagecount"];
                            book.point = (int)readBooks["point"];
                            book.authorId = (int)readBooks["authorId"];
                            book.typeId = (int)readBooks["typeId"];
                            book.authorSurname = (string)readBooks["authorSurname"];
                            book.typeName = (string)readBooks["typeName"];
                            book.status = true;
                            bookList.Add(book);
                        }
                    }
                    closeConnection();
                }
                catch
                {

                }
            }
            return bookList;
        }

        public List<Student> getAllStudents()
        {
            List<Student> studentList = new List<Student>();
            String command = "SELECT * FROM [Library].[dbo].[students]";
            using (SqlConnection conn = buildConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    SqlDataReader readStudents = cmd.ExecuteReader();
                    while (readStudents.Read())
                    {
                        Student student = new Student();
                        student.studentId = (int)readStudents["studentId"];
                        student.name = (string)readStudents["name"];
                        student.surname = (string)readStudents["surname"];
                        student.birthdate = (DateTime)readStudents["birthdate"];
                        student.gender = (string)readStudents["gender"];
                        student.Class = (string)readStudents["class"];
                        student.point = (int)readStudents["point"];
                        studentList.Add(student);
                    }
                }
            }
            return studentList;
        }

        public List<Borrow> getAllBorrows()
        {
            List<Borrow> borrowList = new List<Borrow>();
            String command = "SELECT * FROM [Library].[dbo].[borrows]";
            using (SqlConnection conn = buildConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    SqlDataReader readBorrows = cmd.ExecuteReader();
                    while (readBorrows.Read())
                    {
                        Borrow borrow = new Borrow();
                        borrow.borrowId = (int)readBorrows["borrowId"];
                        borrow.studentId = (int)readBorrows["studentId"];
                        borrow.bookId = (int)readBorrows["bookId"];
                        borrow.takenDate = Convert.ToDateTime(readBorrows["takenDate"]);
                        var broughtDate = readBorrows["broughtDate"].ToString();
                        if (broughtDate != "")
                        {
                            borrow.broughtDate = Convert.ToDateTime(readBorrows["broughtDate"]);
                        }
                        else
                        {
                            borrow.broughtDate = null;
                        }

                        borrowList.Add(borrow);
                    }
                }
            }
            return borrowList;
        }

        public SelectList GetTypes()
        {
            List<type> typesList = new List<type>();
            String command = "SELECT * FROM [Library].[dbo].[types]";
            using (SqlConnection conn = buildConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    SqlDataReader readTypes = cmd.ExecuteReader();
                    while (readTypes.Read())
                    {
                        type type = new type();
                        type.typeId = (int)readTypes["typeId"];
                        type.name = (string)readTypes["name"];
                        typesList.Add(type);
                    }
                }
            }
            return new SelectList(typesList, "typeId", "name");
        }

        public SelectList GetAuthors()
        {
            List<Author> authorsList = new List<Author>();
            String command = "SELECT * FROM [Library].[dbo].[authors]";
            using (SqlConnection conn = buildConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    SqlDataReader readAuthors = cmd.ExecuteReader();
                    while (readAuthors.Read())
                    {
                        Author author = new Author();
                        author.authorId = (int)readAuthors["authorId"];
                        author.name = (string)readAuthors["name"];
                        authorsList.Add(author);
                    }
                }
            }
            return new SelectList(authorsList, "typeId", "name");
        }
    }
}
