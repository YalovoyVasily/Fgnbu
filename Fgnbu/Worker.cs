using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fgnbu
{
    public partial class Worker : Form
    {
        public Worker()
        {
            InitializeComponent();
        }
        
        private void Worker_Load(object sender, EventArgs e)
        {
            Table_Class table = new Table_Class(DBConnection.qrWorker);
            dgvWorker.DataSource = table.table;
            dgvConfiguration(dgvWorker);
            cbFill();
        }

        private void dgvConfiguration(DataGridView dgv)
        {
            //Запрет на редактирование
            dgv.ReadOnly = true;
            //Запрет на добавление новых строк
            dgv.AllowUserToAddRows = false;
            //Запрет на изменение ширины и длины
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            //Отключение заголовка строк
            dgv.RowHeadersVisible = false;
            //Отключение выбора нескольких строк
            dgv.MultiSelect = false;
            //Прячем ID в таблице
            dgv.Columns[0].Visible = false;
        }

        private void cbFill()
        {
            Table_Class table = new Table_Class("Select [ID_Dolzh], [Dolzh_Title] from [dbo].[Dolzh]");
            cbDolzh.DataSource = table.table;
            cbDolzh.DisplayMember = "Dolzh_Title";
            cbDolzh.ValueMember = "ID_Dolzh";
        }

        //Добавление
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand command = new SqlCommand("", DBConnection.connection);
                
                 var s = "insert into [dbo].[Worker] ( [Worker_Name], [Worker_Surname], " +
                 "[Worker_MiddleName], [Seriers_Number_Password], [Worker_Login], [Worker_Password], [Dolzh_ID] ) values" +
                 " ('" + tbName.Text + "','" + tbSurname.Text + "','" + tbMiddleName.Text + "','" + tbNumSer.Text + "','"
                 + tbLogin.Text + "','" + tbPassword.Text + "'," + cbDolzh.SelectedValue.ToString() + ")";
                command.CommandText = s;
                DBConnection.connection.Open();
                command.ExecuteNonQuery();
                DBConnection.connection.Close();
                Worker_Load(sender, e);
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
            
        }

        //Изменение
        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("", DBConnection.connection);
            command.CommandText = "update [dbo].[Worker] set " +
             "[Worker_Surname] ='" + tbSurname.Text + "', " +
             "[Worker_Name] ='" + tbName.Text + "', " +
             "[Worker_MiddleName] ='" + tbMiddleName.Text + "', " +
             "[Seriers_Number_Password] ='" + tbNumSer.Text + "', " +
             "[Worker_Login] ='" + tbLogin.Text + "', " +
             "[Worker_Password] ='" + tbPassword.Text + "', " +
             "[Dolzh_ID] ='" + cbDolzh.SelectedValue.ToString() + "'" +
             " where ID_Worker = " + DBConnection.IDrecord + "";
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
            Worker_Load(sender, e);
        }

        //Удаление
        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("", DBConnection.connection);
            command.CommandText = "delete from [dbo].[Worker]" +
                "where ID_Worker =" + DBConnection.IDrecord + "";
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
            Worker_Load(sender, e);
        }

        private void dgvWorker_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Чтобы программа не крашилась при сортировке в таблице используется try-catch
            try
            {
                //Получение ID записи
                DBConnection.IDrecord = Convert.ToInt32(dgvWorker.Rows[e.RowIndex].Cells[0].Value);
                //Запись  в тексбоксы
                tbName.Text = dgvWorker.Rows[e.RowIndex].Cells[1].Value.ToString();
                tbSurname.Text = dgvWorker.Rows[e.RowIndex].Cells[2].Value.ToString();
                tbMiddleName.Text = dgvWorker.Rows[e.RowIndex].Cells[3].Value.ToString();
                tbNumSer.Text = dgvWorker.Rows[e.RowIndex].Cells[4].Value.ToString();
                tbLogin.Text = dgvWorker.Rows[e.RowIndex].Cells[5].Value.ToString();
                tbPassword.Text = dgvWorker.Rows[e.RowIndex].Cells[6].Value.ToString();
                //Запись ID из таблицы в комбобокс
                cbDolzh.SelectedIndex = Convert.ToInt32(dgvWorker.Rows[e.RowIndex].Cells[7].Value) - 1;
            }
            catch { };
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            //Поиск по всем ячейчам таблицы, подойдёт для каждой
            for (Int32 Column = 0; Column < dgvWorker.ColumnCount; Column++)
            {
                for (Int32 Row = 0; Row < dgvWorker.RowCount; Row++)
                {
                    if (dgvWorker.Rows[Row].Cells[Column].Value.ToString().IndexOf(tbSearch.Text) > -1)
                    {
                        dgvWorker.Rows[Row].Cells[Column].Selected = true;
                        return;
                    }

                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Фильтрация, суть которой в поиске совпадений в таблице в бд
            switch (chkFilter.Checked)
            {
                case (true):
                    Table_Class table1 = new Table_Class(DBConnection.qrWorker + string.Format("where [Worker_Surname] like '%{0}%' " +
                                "or" + "[Worker_Name] like '%{0}%' " +
                                "or" + "[Worker_MiddleName] like '%{0}%' " +
                                "or [Seriers_Number_Password] like '%{0}%'" +
                                "or [Worker_Login] like '%{0}%'" +
                                "or [Worker_Password] like '%{0}%'", tbSearch.Text));
                    dgvWorker.DataSource = table1.table;
                    break;
                case (false):
                    Table_Class table2 = new Table_Class(DBConnection.qrWorker);
                    dgvWorker.DataSource = table2.table;
                    break;
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (chkFilter.Checked)
                checkBox1_CheckedChanged(sender, e);
        }

        private void Worker_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            Vaktsiny vaktsiny = new Vaktsiny();
            vaktsiny.Show(this); ;
            Hide();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            Dolzh vaktsiny = new Dolzh();
            vaktsiny.Show(this); ;
            Hide();
        }
    }
}
