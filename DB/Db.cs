using Diplom.Krasnov__WindowsForms.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Diplom.Krasnov__WindowsForms.DB
{
    class Db  // Класс быза данных
    {
        private EFContext context;

        private Db()
        {
            context = new EFContext(GetConnectionString());
        }
        private string GetConnectionString()
        {
            // строка подключения
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = "(localdb)\\MSSQLLocalDB",
                UserID = "",
                Password = "",
                InitialCatalog = "MyLibraryDb"
            };
            return builder.ConnectionString;
        }
        // сохраняем изменения
        public void SaveChanges() =>
            context.SaveChanges();

        // метод для получения всех доступных (не взятых) книг
        public IEnumerable<Book> GetAvailavleBooks() =>
            context.Books.Where(b => b.IsAvailable).ToList();

        // методя для получения всех книг взятым пользователем
        public IEnumerable<TakenBook> GetAllTakenBookForUser(int userId)
        {
            return context.TakenBooks.Where(tb => tb.UserId == userId).ToArray();
        }
        // метод для поиска пользователя по логину и паролю
        public User FindUser(string login, string password)
        {
            return context.Users.ToList().FirstOrDefault(user =>
                 user.Login == login &&
                 Utils.Password.Verify(password, user.Password));
        }
        // метод для проверки что такой логин уже не занят
        public bool LoginIsAvailable(string login) =>
            context.Users.All(u => u.Login != login);
        // метод добавления нового пользователя
        public User AddUser(User user)
        {
            var u = context.Users.Add(user);
            context.SaveChanges();
            return u;
        }
        // метод добавления новой книги
        public Book AddBook(Book book)
        {
            var b = context.Books.Add(book);
            context.SaveChanges();
            return b;
        }
        // метод удаления книги по id
        public void RemoveBook(int id)
        {
            var book = context.Books.Find(id);
            context.Books.Remove(book);
            context.SaveChanges();
        }
        // метод для выдачи книги
        public TakenBook TakeBook(int bookId, int userId)
        {
            var book = context.Books.Find(bookId);
            var user = context.Users.Find(userId);
            book.IsAvailable = false;
            var takenBook = new TakenBook
            {
                Book = book,
                BookId = bookId,
                User = user,
                UserId = userId,
                Date = DateTime.UtcNow
            };
            var tb = context.TakenBooks.Add(takenBook);
            context.SaveChanges();
            return tb;
        }
        // метод для возвращения книги по Id
        public bool ReturnBook(int takenId)
        {
            var takenBook = context.TakenBooks.Find(takenId);
            context.TakenBooks.Remove(takenBook);
            var book = context.Books.Find(takenBook.BookId);
            book.IsAvailable = true;

            return context.SaveChanges() != 0;
        }

        public static Db Instance { get; } = new Db();
    }
}
