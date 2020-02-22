using Diplom.Krasnov__WindowsForms.DB;
using Diplom.Krasnov__WindowsForms.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Diplom.Krasnov__WindowsForms.Ui
{
    public partial class EditBookForm : Form  //Форма редактирования книги
    {
        private readonly Book book;
        public EditBookForm(Book book)
        {
            InitializeComponent();

            this.book = book;

            titleField.Text = book.Title;
            authorField.Text = book.Authour;
            genreField.Text = book.Genre;
            yearField.Text = book.Year.ToString();
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CloseButton_MouseEnter(object sender, EventArgs e) //Эффекты при наведении на X (изменение цвета)
        {
            CloseButton.ForeColor = Color.MediumVioletRed;
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e) //Эффекты при отведении мыши от Х (меняется цвет на другой) 
        {
            CloseButton.ForeColor = Color.Red;
        }
        private void EditBookBtn_Click(object sender, EventArgs e) //Метод удалении пробелов с обеих сторон текста, при заполнении
        {
            var title = titleField.Text.Trim();
            var author = authorField.Text.Trim();
            var genre = genreField.Text.Trim();
            var yearStr = yearField.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))  //проверка, если поле пустое, выводится сообщение
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

            if (!int.TryParse(yearStr, out int year) || year < 0) //проверка заполнения формы (должно быть введены только числовые значения)
            {
                MessageBox.Show("Укажите корректный год выпуска");
                return;
            }

            book.Genre = genre;
            book.Year = year;
            book.Authour = author;
            book.Title = title;

            try   //Сохранение изменений и закрытие окна
            {
                Db.Instance.SaveChanges();
                DialogResult = DialogResult.OK;
                this.Close();
            }

            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }
    }
}
