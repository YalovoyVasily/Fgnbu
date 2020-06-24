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
    public partial class Vaktsiny : Form
    {
        public Vaktsiny()
        {
            InitializeComponent();
        }
        private Boolean message = true;
        private void Vaktsiny_Load(object sender, EventArgs e)
        {
            Table_Class table = new Table_Class(DBConnection.qrVaktsiny);
            dgvWorker.DataSource = table.table;
            dgvConfiguration(dgvWorker);
            cbFill();
            if (message == true)
                {
                MessageBox.Show("Уважаемый работник! Не забудьте, что вам " +
                    "необходимо произвести проверку вакцин на качество!");
                }
            if (message == true)
            {
                MessageBox.Show("Уважаемый работник! Не забудьте, что вам " +
                    "необходимо произвести создание плана приготовления вакцин!");
            }
            if (message == true)
            {
                MessageBox.Show("Уважаемый работник! Не забудьте, что вам " +
                    "необходимо произвести проверку ампул с веществами!");
            }
            if (message == true)
            {
                MessageBox.Show("Уважаемый работник! Не забудьте, что вам " +
                    "необходимо произвести ручной подсчёт поступаемых веществ!");
                message = !message;
            }
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
            dgv.Columns[2].Visible = false;
            dgv.Columns[4].Visible = false;
            dgv.Columns[6].Visible = false;
            dgv.Columns[8].Visible = false;
        }

        private void cbFill()
        {
            Table_Class Worker = new Table_Class("SELECT [ID_Worker], CONCAT_WS(\' \',[Worker_Name],  [Worker_Surname], [Worker_MiddleName])" +
            " FROM [dbo].[Worker]");
            cbWorker.DataSource = Worker.table;
            cbWorker.DisplayMember = "Column1";
            cbWorker.ValueMember = "ID_Worker";

            Table_Class Bolezn = new Table_Class("Select[ID_Bolezn], [Bolezn_Title] from [dbo].[Bolezn]");
            cbBolezn.DataSource = Bolezn.table;
            cbBolezn.DisplayMember = "Bolezn_Title";
            cbBolezn.ValueMember = "ID_Bolezn";

            Table_Class SposobPrim = new Table_Class("Select [ID_SposobPrim], [SposobPrim_Title] from [dbo].[SposobPrim]");
            cbSposobPrim.DataSource = SposobPrim.table;
            cbSposobPrim.DisplayMember = "SposobPrim_Title";
            cbSposobPrim.ValueMember = "ID_SposobPrim";

            Table_Class Tip = new Table_Class("Select [ID_Tip], [Tip_Title] from [dbo].[Tip]");
            cbTip.DataSource = Tip.table;
            cbTip.DisplayMember = "Tip_Title";
            cbTip.ValueMember = "ID_Tip";


        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            Bolezn bolezn = new Bolezn();
            bolezn.Show(this); ;
            Hide();
        }

        private void Vaktsiny_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            Worker form = new Worker();
            form.Show(this);
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("", DBConnection.connection);
            command.CommandText = "insert into [dbo].[Vaktsiny]" +
                " ([Vaktsiny_Title], [Vaktsiny_ID_Worker]," +
                " [Vaktsiny_ID_SposobPrim], [Vaktsiny_ID_Bolezn], [Vaktsiny_ID_Tip])" +
                " values ('" + tbTitle.Text + "'," + cbWorker.SelectedValue.ToString() + "," + cbSposobPrim.SelectedValue.ToString() + "," + cbBolezn.SelectedValue.ToString() + "," + cbTip.SelectedValue.ToString() +")";
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
            Vaktsiny_Load(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("", DBConnection.connection);
            command.CommandText = "update [dbo].[Vaktsiny] set " +
            "[Vaktsiny_Title] ='" + tbTitle.Text + "'," +
            "[Vaktsiny_ID_Worker] =" + cbWorker.SelectedValue.ToString() + "," +
            "[Vaktsiny_ID_SposobPrim] =" + cbSposobPrim.SelectedValue.ToString() + "," +
            "[Vaktsiny_ID_Bolezn] =" + cbBolezn.SelectedValue.ToString() + "," +
            "[Vaktsiny_ID_Tip] =" + cbTip.SelectedValue.ToString() + 
            " where ID_Vaktsiny = " + DBConnection.IDrecord + "";
            //var s = command.CommandText;
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
            Vaktsiny_Load(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("", DBConnection.connection);
            command.CommandText = "delete from [dbo].[Vaktsiny]" +
                "where ID_Vaktsiny =" + DBConnection.IDrecord + "";
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
            Vaktsiny_Load(sender, e);
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
                    Table_Class table1 = new Table_Class(DBConnection.qrTip + string.Format("where [Vaktsiny_Title] like '%{0}%' or" +
                        "[SposobPrim_Title] like '%{0}%' or [Bolezn_Title] like '%{0}%' or [Tip_Title] like '%{0}%' or CONCAT_WS(\' \',[Worker_Name],  [Worker_Surname], [Worker_MiddleName]) like '%{0}%' or ", tbSearch.Text));
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

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //Проверка saveFileDialog
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //Получаем название файла
            string filename = saveFileDialog.FileName;
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            //Объявление переменной с неявным типом (автоматическим выбором типа данных)
            var ExcelWorkBook = ExcelApp.Application.Workbooks.Add(Type.Missing);
            ExcelApp.Columns.ColumnWidth = 15;
            Table_Class table = new Table_Class("SELECT  [Vaktsiny_Title] as \"Название вакцины\", CONCAT_WS(\' \',[Worker_Name],  [Worker_Surname], [Worker_MiddleName]) as \"Руководящий сотрудник\"," +
            " [SposobPrim_Title] as \"Способ применения вакцины\",  [Bolezn_Title] as \"Болезнь\",  [Tip_Title] as \"Тип вакцины\" FROM [dbo].[Vaktsiny] " +
            "INNER JOIN [dbo].[Worker] ON [dbo].[Vaktsiny].[Vaktsiny_ID_Worker] = [dbo].[Worker].[ID_Worker]" +
            "INNER JOIN [dbo].[SposobPrim] ON [dbo].[Vaktsiny].[Vaktsiny_ID_SposobPrim] = [dbo].[SposobPrim].[ID_SposobPrim] " +
             "INNER JOIN [dbo].[Bolezn] ON [dbo].[Vaktsiny].[Vaktsiny_ID_Bolezn] = [dbo].[Bolezn].[ID_Bolezn] " +
             "INNER JOIN [dbo].[Tip] ON [dbo].[Vaktsiny].[Vaktsiny_ID_Tip] = [dbo].[Tip].[ID_Tip]");
            DataGridView dgv = new DataGridView();
            dgv.DataSource = table.table;
            dgvWorker.DataSource = table.table;
            
            //Заголовки для экселя
            ExcelApp.Cells[1, 1] = "Название вакцины";
            ExcelApp.Cells[1, 2] = "Руководящий сотрудник";
            ExcelApp.Cells[1, 3] = "Способ применения вакцины";
            ExcelApp.Cells[1, 4] = "Болезнь";
            ExcelApp.Cells[1, 5] = "Тип вакцины";
            //Цикл по выводу информации из DataGridView в Excel
            for (int i = 0; i < dgvWorker.ColumnCount; i++)
            {
                for (int j = 0; j < dgvWorker.RowCount; j++)
                {
                    ExcelApp.Cells[j + 2, i+1] = (dgvWorker[i, j].Value).ToString();
                }
            }
            //Сохранение
            ExcelWorkBook.SaveAs(filename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                Type.Missing, Type.Missing, false, false,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //Закрытие приложения
            ExcelApp.Quit();
            MessageBox.Show("Файл успешно сформирован");
            Vaktsiny_Load(sender, e);
        }
    }
}
