using Diplom.Krasnov__WindowsForms.DB;
using Diplom.Krasnov__WindowsForms.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Diplom.Krasnov__WindowsForms.Ui
{
    public partial class AddBookForm : Form
    {
        public AddBookForm()  //форма добавления книг
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();  
        }
        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            CloseButton.ForeColor = Color.MediumVioletRed;
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            CloseButton.ForeColor = Color.Red;
        }
        private void AddBookBtn_Click(object sender, EventArgs e)  //Метод убирает пробелы с обеих сторон при добавлении книг 
        {
            var title = titleField.Text.Trim();
            var author = authorField.Text.Trim();
            var genre = genreField.Text.Trim();
            var yearStr = yearField.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))  
            {
                MessageBox.Show("Название книги не может быть пустым!");
                return;
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                MessageBox.Show("Имя автора не может быть пустым!");
                return;
            }

            if (string.IsNullOrWhiteSpace(genre))
            {
                MessageBox.Show("Жанр не может быть пустым");
                return;
            }

            if (string.IsNullOrWhiteSpace(yearStr))
            {
                MessageBox.Show("Год не может быть пустым");
                return;
            }

            if (!int.TryParse(yearStr, out int year) || year < 0) //Проверка заполнения формы (должно быть введены только числовые значения)
            {
                MessageBox.Show("Укажите корректный год выпуска");
                return;
            }

            var book = new Book
            {
                Genre = genre,
                Year = year,
                Authour = author,
                Title = title,
                IsAvailable = true
            };
            try
            {
                Db.Instance.AddBook(book);
                DialogResult = DialogResult.OK;
                this.Close();
            }

            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }
        Point lastPoint; //Специальный класс для задания координат
        private void Logo_MouseMove(object sender, MouseEventArgs e)
        {
            //Делаем проверку, если нажали на кнопку - двигаем наше окно (Для верхней части, где авторизация)
            if (e.Button == MouseButtons.Left)
            {
                //обратились к координате X, сможем определиться, где находится наш курсор
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void Logo_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void AddBookForm_MouseMove(object sender, MouseEventArgs e)
        {
            //Делаем проверку, если нажали на кнопку - двигаем наше окно
            if (e.Button == MouseButtons.Left)
            {
                //обратились к координате X, сможем определиться, где находится наш курсор
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void AddBookForm_MouseDown(object sender, MouseEventArgs e)
        {
            //Устанавливаем новые координаты
            lastPoint = new Point(e.X, e.Y);
        }
    }
}
