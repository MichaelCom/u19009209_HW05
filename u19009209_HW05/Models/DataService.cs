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
            sqlConnectionStringBuilder["Integrated Security"] = "true";
            return new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
        }

        public List<BookHybrid> getAllBooks()
        {
            List<BookHybrid> bookList = new List<BookHybrid>();
            String command = "SELECT book.[bookId] as bookId ,book.[name] as name ,book.[pagecount] as pagecount ,book.[point] as point,auth.[name] as authorName,auth.[surname] as authorSurname ,type.[name] typeName,  book.[authorId],book.[typeId] " +
                            "FROM [Library].[dbo].[books] book " +
                            "JOIN [Library].[dbo].[authors] auth on book.authorId = auth.authorId " +
                            "JOIN [Library].[dbo].[types] type on book.typeId = type.typeId";
            using (SqlConnection conn = buildConnection())
            {
                try
                {
                    conn.Open();
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
                            book.authorName = (string)readBooks["authorName"];
                            book.authorSurname = (string)readBooks["authorSurname"];
                            book.typeName = (string)readBooks["typeName"];
                            book.status = true;
                            bookList.Add(book);
                        }
                    }
                    conn.Close();
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
            try
            {
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
                    conn.Close();
                }
            }
            catch
            {

            }
            return studentList;
        }

        public List<Borrow> getAllBorrows()
        {
            List<Borrow> borrowList = new List<Borrow>();
            try
            {
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
                    conn.Close();
                }
            }
            catch
            {

            }
            return borrowList;
        }

        public SelectList GetTypes()
        {
            List<type> typesList = new List<type>();
            try
            {
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
                    conn.Close();
                }
            }
            catch
            {

            }
            return new SelectList(typesList, "typeId", "name");
        }

        public SelectList GetAuthors()
        {
            List<Author> authorsList = new List<Author>();
            try
            {
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
                            author.name = (string)readAuthors["name"] + " " + (string)readAuthors["surname"];
                            authorsList.Add(author);
                        }
                    }
                    conn.Close();
                }
            }
            catch
            {

            }
            return new SelectList(authorsList, "authorId", "name");
        }

        public string BorrowBook(int studentId, int bookId)
        {
            string res = "";
            try
            {
                String command = "Insert Into [Library].[dbo].[borrows] (studentId,bookId,takenDate) values ('" + studentId + "','" + bookId + "',GETDATE())";
                using (SqlConnection conn = buildConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                        res = "Successfully Borrowed Book";
                    }
                    conn.Close();
                }

            }
            catch
            {

            }
            return res;
        }

        public string ReturnBook(int borrowId)
        {
            string res = "";
            try
            {
                String command = "Update [Library].[dbo].[borrows] set broughtDate = GETDATE() where borrowId =" + borrowId;
                using (SqlConnection conn = buildConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                        res = "Successfully Returned Book";
                    }
                    conn.Close();
                }
            }
            catch
            {

            }
            return res;
        }


    }
}
