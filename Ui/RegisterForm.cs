using Diplom.Krasnov__WindowsForms.DB;
using Diplom.Krasnov__WindowsForms.Models;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplom.Krasnov__WindowsForms.Ui
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            UserNameField.Text = "Введите имя";//Говорим, что работаем с текстом внутри него
            UserNameField.ForeColor = Color.Gray; //Меняем цвет надписи
            UserSurnameField.Text = "Введите фамилию";
            UserSurnameField.ForeColor = Color.Gray;
            LoginField.Text = "Введите логин";
            LoginField.ForeColor = Color.Gray;
            PassField.Text = "Введите пароль";
            PassField.ForeColor = Color.Gray;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit(); //Закрываем приложение
        }


        Point lastPoint; //Специальный класс для задания координат
        private void Register_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //обратились к координате X, сможем определиться, где находится наш курсор
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void Register_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void MainPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //обратились к координате X, сможем определиться, где находится наш курсор
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void MainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            CloseButton.ForeColor = Color.MediumVioletRed;
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            CloseButton.ForeColor = Color.Red;
        }

        private void UserNameField_Enter(object sender, EventArgs e)
        {
            //Вводим проверку, чтобы поле можно было отчистить
            if (UserNameField.Text == "Введите имя")
            {
                UserNameField.Text = ""; //Текст будет пустым, очистим текст
                UserNameField.ForeColor = Color.Black; //текст будет черным, что вводит пользователь
            }
        }

        private void UserNameField_Leave(object sender, EventArgs e) //Когда будем выходить из формочки
        {
            if (UserNameField.Text == "") //Если пользователь ничего не ввел, тогда появится текст введите имя
            {
                UserNameField.Text = "Введите имя"; //Текст будет пустым, очистим текст
                UserNameField.ForeColor = Color.Gray; //Текст будет становиться другого цвета
            }
        }

        private void UserSurnameField_Enter(object sender, EventArgs e)
        {
            if (UserSurnameField.Text == "Введите фамилию")
            {
                UserSurnameField.Text = "";
                UserSurnameField.ForeColor = Color.Black;
            }
        }

        private void UserSurnameField_Leave(object sender, EventArgs e)
        {
            if (UserSurnameField.Text == "")
            {
                UserSurnameField.Text = "Введите фамилию";
                UserSurnameField.ForeColor = Color.Gray;
            }
        }

        private void LoginField_Enter(object sender, EventArgs e)
        {
            if (LoginField.Text == "Введите логин")
            {
                LoginField.Text = "";
                LoginField.ForeColor = Color.Black;
            }
        }

        private void LoginField_Leave(object sender, EventArgs e)
        {
            if (LoginField.Text == "")
            {
                LoginField.Text = "Введите логин";
                LoginField.ForeColor = Color.Gray;
            }
        }

        private void PassField_Enter(object sender, EventArgs e)
        {
            if (PassField.Text == "Введите пароль")
            {
                PassField.Text = "";
                PassField.ForeColor = Color.Black;
            }
        }

        private void PassField_Leave(object sender, EventArgs e)
        {
            if (PassField.Text == "")
            {
                PassField.Text = "Введите пароль";
                PassField.ForeColor = Color.Gray;
            }
        }

        //ОБРАБОТКА ПОЛЯ РЕГИСТРАЦИИ
        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            // первое соединение с сервером может быть медленным
            // поэтому отключаем кнопку на время работы
            RegisterButton.Enabled = false;
            //Делаем проверку, если пользователь ничего не ввел
            var password = PassField.Text.Trim();
            var login = LoginField.Text.Trim();
            if (password == "Введите пароль" || string.IsNullOrWhiteSpace(password)) //означает, что пользователь ничего не ввел
            {
                MessageBox.Show("Укажите свой пароль");
                return; //выходим из функции
            }

            if (LoginField.Text == "Введите логин" || string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Напишите пожалуйста свой логин");
                return;
            }

            //Проверка, если пользователь существует выведется окно с сообщением
            var isAvailable = await Task.Run(() => Db.Instance.LoginIsAvailable(login));
            if (!isAvailable)
            {
                MessageBox.Show("Пользователь с таким логином уже существует. Выберите что-то другое");
                return;
            }
            // "наивная" проверка что пользователь админ
            var isAdmin = login == "admin" && password == "admin";
            var user = new User
            {
                // хешируем пароль чтобы не хранить его в БД в открытом виде
                Password = Utils.Password.GetHash(PassField.Text),
                Login = login,
                Name = UserNameField.Text,
                SurName = UserSurnameField.Text,
                Role = isAdmin ? Role.Admin : Role.User
            };
            user = Db.Instance.AddUser(user);
            if (user != null)
            {
                MessageBox.Show("Вы зарегистрировались!");
                this.Hide(); //Скрываем окно авторизации
                MainForm mainForm = new MainForm(user); //создаем оюъект главная форма, выделяем память под нее
                mainForm.Show();
            }
            RegisterButton.Enabled = true;
        }
        
        private void AutorizationLabel_Click(object sender, EventArgs e)
        {
            this.Hide(); //Прячем это окно
            LoginForm loginForm = new LoginForm(); //Создаем объект, освобождаем под него память
            loginForm.Show(); //открываем это окно
        }

        private void AutorizationLabel_MouseEnter(object sender, EventArgs e)
        {
            AutorizationLabel.ForeColor = Color.Orange; //При наведении на надпись Авторизоваться, меняется цвет
        }

        private void AutorizationLabel_MouseLeave(object sender, EventArgs e)
        {
            AutorizationLabel.ForeColor = Color.White; //При отведении от надписи Авторизоваться - цвет становится белым
        }
    }
}
