using Diplom.Krasnov__WindowsForms.DB;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplom.Krasnov__WindowsForms.Ui
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.PassField.AutoSize = false; //Отключили авторазмер
            this.PassField.Size = new Size(this.PassField.Size.Width, 64); //Установили ширину и высоту поля пароль (64 взяли с поля user)
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit(); //метод закрытия приложения авторизации
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
        private void MainPanel_MouseMove(object sender, MouseEventArgs e)
        {
            //Делаем проверку, если нажали на кнопку - двигаем наше окно
            if (e.Button == MouseButtons.Left)
            {
                //обратились к координате X, сможем определиться, где находится наш курсор
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void MainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            //Устанавливаем новые координаты
            lastPoint = new Point(e.X, e.Y);
        }

        // Функция Обработки кнопки "Войти", будет срабатывать каждый раз, когда мы будем нажимать на кнопку
        private async void ButtonLogin_Click(object sender, EventArgs e)
        {
            // первое соединение с сервером может быть медленным
            // поэтому отключаем кнопку на время работы
            ButtonLogin.Enabled = false;
            /*Эта функция, при нажатии на кнопку должна брать данные, которые вводит пользователь их обрабатывать и проверять с базой данных в SQL и вытаскивать их оттуда 
             Если пользователь найдет - авторизация успешна*/

            var login = LoginField.Text;
            var password = PassField.Text;
            // пробуем найти пользователя
            var user = await Task.Run(() => Db.Instance.FindUser(login, password));
            if (user != null)
            {
                this.Hide(); //Скрываем окно авторизации
                MainForm mainForm = new MainForm(user); //создаем оюъект главная форма, выделяем память под нее
                mainForm.Show(); //Показываем это окно Главное окно
            }
            else
            {
                MessageBox.Show("Не удалось авторизоваться, проверьте правильно ли написаны логин и пароль.");
            }
            ButtonLogin.Enabled = true;
        }

        private void RegisterLabel_Click(object sender, EventArgs e)
        {
            this.Hide(); //Прячем окно, при нажатии на кнопку зарегистрироваться в панели Авторизация
            RegisterForm registerForm = new RegisterForm(); //Создали объект и выделели под него память
            registerForm.Show(); //Открываем это окно

        }

        private void RegisterLabel_MouseEnter(object sender, EventArgs e)
        {
            RegisterLabel.ForeColor = Color.Orange; //При наведении на поле Зарегистрироваться, она меняет цвет
        }

        private void RegisterLabel_MouseLeave(object sender, EventArgs e)
        {
            RegisterLabel.ForeColor = Color.White; //При отведени мыши от надписи зарегистрироваться 
        }
    }
}
