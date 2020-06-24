using System;
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
    public partial class Tip : Form
    {
        public Tip()
        {
            InitializeComponent();
        }

        private void Bolezn_Load(object sender, EventArgs e)
        {
            Table_Class table = new Table_Class(DBConnection.qrTip);
            dgvWorker.DataSource = table.table;
            dgvConfiguration(dgvWorker);
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

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("", DBConnection.connection);
            command.CommandText = "insert into [dbo].[Tip] ( [Tip_Title]) values ('" + tbTitle.Text + "')";
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
            Bolezn_Load(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("", DBConnection.connection);
            command.CommandText = "update [dbo].[Tip] set " +
            "[Tip_Title] ='" + tbTitle.Text + "'" +
            " where ID_Tip = " + DBConnection.IDrecord + "";
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
            Bolezn_Load(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("", DBConnection.connection);
            command.CommandText = "delete from [dbo].[Tip]" +
                "where ID_Tip =" + DBConnection.IDrecord + "";
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
            Bolezn_Load(sender, e);
        }

        private void dgvWorker_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Чтобы программа не крашилась при сортировке в таблице используется try-catch
            try
            {
                //Получение ID записи
                DBConnection.IDrecord = Convert.ToInt32(dgvWorker.Rows[e.RowIndex].Cells[0].Value);
                //Запись  в тексбоксы
                tbTitle.Text = dgvWorker.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
            catch { };
        }

        private void Tip_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            SposobPrim sposobprim = new SposobPrim();
            sposobprim.Show(this);
            Hide();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            Bolezn form = new Bolezn();
            form.Show(this);
            Hide();
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

        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            //Фильтрация, суть которой в поиске совпадений в таблице в бд
            switch (chkFilter.Checked)
            {
                case (true):
                    Table_Class table1 = new Table_Class(DBConnection.qrTip + string.Format("where [Tip_Title] like '%{0}%' ", tbSearch.Text));
                    dgvWorker.DataSource = table1.table;
                    break;
                case (false):
                    Table_Class table2 = new Table_Class(DBConnection.qrTip);
                    dgvWorker.DataSource = table2.table;
                    break;
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (chkFilter.Checked)
                chkFilter_CheckedChanged(sender, e);
        }
    }
}
