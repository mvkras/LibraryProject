using Diplom.Krasnov__WindowsForms.DB;
using Diplom.Krasnov__WindowsForms.Models;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Diplom.Krasnov__WindowsForms.Ui
{
    public partial class ShowTakenBooksForm : Form  //Форма: Список книг, которые пользователь взял
    {
        private readonly User user;
        public ShowTakenBooksForm(User user)
        {
            InitializeComponent();

            this.user = user;
            ShowData();
            
        }

        public void ShowData()
        {
            booksCmb.DataSource = Db.Instance.GetAllTakenBookForUser(user.Id);
        }

        private void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            // если выбранный элемент "взятая" книга
            if (booksCmb.SelectedItem is TakenBook book)
            {
                // пробуем вернуть книгу
                if (Db.Instance.ReturnBook(book.Id))
                {
                    // очищаем информацию
                    bookInfoTxb.Clear();
                    // выводим сообщение
                    MessageBox.Show("Книга возвращена!");
                    ShowData();
                }
            }
        }
        // обработчик выбора элемента в выпадающем списке
        private void BooksCmb_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // если выбранный элемент "взятая" книга
            if (booksCmb.SelectedItem is TakenBook tbook)
            {
                var sb = new StringBuilder();

                var format = "{0, -15}{1}";
                var book = tbook.Book;
                // собираем информацию
                sb.AppendFormat(format, "Название:", book.Title);
                sb.AppendLine();
                sb.AppendFormat(format, "Автор:", book.Authour);
                sb.AppendLine();
                sb.AppendFormat(format, "Год выпуска:", book.Year);
                sb.AppendLine();
                sb.AppendFormat(format, "Жанр:", book.Genre);
                sb.AppendLine();
                sb.AppendFormat(format, "Выдача:", tbook.Date.ToString());
                sb.AppendLine();
                // отображаем в поле
                bookInfoTxb.Text = sb.ToString();
            }
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

        private void ShowTakenBooksForm_MouseMove(object sender, MouseEventArgs e)
        {
            //Делаем проверку, если нажали на кнопку - двигаем наше окно (Для верхней части, где авторизация)
            if (e.Button == MouseButtons.Left)
            {
                //обратились к координате X, сможем определиться, где находится наш курсор
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void ShowTakenBooksForm_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
    }
}
