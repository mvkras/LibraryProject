using Diplom.Krasnov__WindowsForms.DB;
using Diplom.Krasnov__WindowsForms.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Diplom.Krasnov__WindowsForms.Ui
{
    public partial class LibraryUserControl : UserControl
    {
        private readonly User user;
        private readonly bool isAdmin;
        public LibraryUserControl(User user)  //Проверка на администратора (кто залогинился)
        {
            InitializeComponent();
            isAdmin = user.Role == Role.Admin;
            actionBtn.Text = isAdmin ? "Добавить книгу" : "Вернуть книгу";
            this.user = user;
            ShowBooks();
        }

        private void ShowBooks() //список книг
        {
            dataGridView1.DataSource = Db.Instance.GetAvailavleBooks().ToArray();
            // меняем стандартные названия в заголовке на привычные
            dataGridView1.Columns[nameof(Book.Authour)].HeaderText = "Автор";
            dataGridView1.Columns[nameof(Book.Genre)].HeaderText = "Жанр";
            dataGridView1.Columns[nameof(Book.Title)].HeaderText = "Название";
            dataGridView1.Columns[nameof(Book.Year)].HeaderText = "Год выпуска";
            // скрываем столбец доступности книги так как здесь отображаются только те
            // которые есть в наличии
            dataGridView1.Columns[nameof(Book.IsAvailable)].Visible = false;

            if (dataGridView1.Columns["dataGridViewActionButton"] == null)
            {
                var actionButton = new DataGridViewButtonColumn
                {
                    Name = "dataGridViewActionButton",
                    HeaderText = "Действие",
                    Text = isAdmin ? "Удалить" : "Взять",
                    UseColumnTextForButtonValue = true
                };
                dataGridView1.Columns.Add(actionButton);
            }
            if (isAdmin && dataGridView1.Columns["editBookButton"] == null)
            {
                var editButton = new DataGridViewButtonColumn
                {
                    Name = "editBookButton",
                    HeaderText = "",
                    Text = "Редактировать",
                    UseColumnTextForButtonValue = true
                };
                dataGridView1.Columns.Add(editButton);
            }

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // если клик был на новой строке или на заголовке то ничего не делаем
            if (e.RowIndex == dataGridView1.NewRowIndex || e.RowIndex < 0)
                return;

            // проверяем был ли клик произведен по кнопке "действие"
            if (e.ColumnIndex == dataGridView1.Columns["dataGridViewActionButton"].Index)
            {
                // извлекаем данные в строке
                var data = (Book)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                if (isAdmin) // если пользователь админ
                {
                    // удаляем книгу
                    Db.Instance.RemoveBook(data.Id);
                }
                else
                {
                    // иначе забираем книгу
                    Db.Instance.TakeBook(data.Id, user.Id);
                }
                // обновляем отображаемую информацию
                ShowBooks();
                return;
            }
            // если нажали на кнопку редактирования
            if (isAdmin && e.ColumnIndex == dataGridView1.Columns["editBookButton"].Index)
            {
                // извлекаем данные в строке
                var data = (Book)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                // открываем форму редактирования
                var editForm = new EditBookForm(data);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Книга отредактирована!");
                    ShowBooks();
                }
            }
        }

        private void ActionBtn_Click(object sender, EventArgs e)
        {
            if (isAdmin)
            {
                var addFrm = new AddBookForm();
                if (addFrm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Книга добавлена!");
                    ShowBooks();
                }
            }
            else
            {
                var showFrm = new ShowTakenBooksForm(user);
                showFrm.ShowDialog();
                ShowBooks();
            }
        }
    }
}
