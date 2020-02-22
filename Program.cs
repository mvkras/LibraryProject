using Diplom.Krasnov__WindowsForms.Ui;
using System;
using System.Windows.Forms;

namespace Diplom.Krasnov__WindowsForms
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
         //   Application.Run(new LoginForm()); // запуск Окна авторизации
            Application.Run(new RegisterForm());  //Запуск окна регистрации
        }
    }
}
