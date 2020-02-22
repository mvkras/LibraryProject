using Diplom.Krasnov__WindowsForms.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Diplom.Krasnov__WindowsForms.Ui
{
    public partial class MainForm : Form  //главная форма
    {
        public MainForm(User user)
        {
            InitializeComponent();
            MainWindow.Text = user.Login;  //отображение логина пользователя, который вошел с главную форму
            MainPanel.Controls.Add(new LibraryUserControl(user));
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e) 
        {
           CloseButton.ForeColor = Color.MediumVioletRed;  //При наведении на закрыть, меняется цвет Х
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
           CloseButton.ForeColor = Color.Red; //Убираем мышь со значка закрыть, меняем на другой цает
        }
    }
}
