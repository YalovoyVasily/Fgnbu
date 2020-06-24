using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Fgnbu
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();
        }
        //Выход из окна
        private void BtLeave_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Fgnbu", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        //Глобальное событие на любую кнопку
        private void back_Color(object sender, EventArgs e)
        {
            if (sender is TextBox)
                (sender as TextBox).BackColor = Color.White;
        }
        bool hide = false;
        private void BtEnter_Click(object sender, EventArgs e)
        {
            //Проверка на заполнение полей ввода
            switch (TextBox_Login.Text == "")
            {
                case true:
                    TextBox_Login.BackColor = Color.Red;
                    break;
                case false:
                    switch (TextBox_Password.Text == "")
                    {
                        case true:
                            TextBox_Password.BackColor = Color.Red;
                            break;
                        case false:
                            Function_Class function = new Function_Class();
                            //Прохождение авторизации
                            function.Authorization(TextBox_Login.Text,
                                TextBox_Password.Text);
                            switch (Program.intID == "")
                            {
                                //Не верно введёные данные
                                case true:
                                    MessageBox.Show("Введённые данные не верны! " +
                                        "Повторите ввод!",
                                               "Fgnbu", MessageBoxButtons.OK,
                                               MessageBoxIcon.Error);
                                    TextBox_Login.Clear();
                                    TextBox_Password.Clear();
                                    TextBox_Login.Focus();
                                    break;
                                case false:
                                    
                                    Worker worker = new Worker();
                                    worker.Show(this);
                                    this.Visible = false;
                                    Hide();
                                    hide = true;
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }

        private void Authorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.GoodBye = false;
            
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Fgnbu", MessageBoxButtons.YesNo);
                if ((result == DialogResult.Yes) & (Program.GoodBye == false))
                {
                    System.Windows.Forms.Application.Exit();
                }
            }
            Program.GoodBye = true;
        }
        
        Loading loading = new Loading();
        private void Authorization_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            
            loading.Show(this);
            
            
            
            
        }

        /// <summary>
        ///Генератор повторения секретного ключа
        /// </summary>
        /// <param name="s">Секретный ключ</param>
        /// <param name="n">Длина текста</param>
        /// <returns></returns>
        private string GetRepeatKey(string s, int n)
        {
            var r = s;
            while (r.Length < n)
            {
                r += r;
            }

            return r.Substring(0, n);
        }

        /// <summary>
        /// Метод шифрования/дешифрования
        /// </summary>
        /// <param name="text">Текст, над которым надо произвести операцию</param>
        /// <param name="secretKey">Секретный ключ</param>
        /// <returns></returns>
        private string Cipher(string text, string secretKey)
        {
            var currentKey = GetRepeatKey(secretKey, text.Length);
            var res = string.Empty;
            for (var i = 0; i < text.Length; i++)
            {
                res += ((char)(text[i] ^ currentKey[i])).ToString();
            }

            return res;
        }
        
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (loading.Visible == true) this.Visible = false;
            else this.Visible = true;


            if (this.Visible == false) timer1.Enabled = true;
            if (hide == true)
            {
                this.Visible = false;
                timer1.Enabled = false;
            }
        }
    }
}