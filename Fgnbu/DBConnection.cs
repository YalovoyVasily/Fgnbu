using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Fgnbu
{
    public class DBConnection
    {
        public static SqlConnection connection = new SqlConnection( 
           Configuration_class.connection.ConnectionString);
        public DataTable dtDolzh = new DataTable("Dolzh");
        public DataTable dtWorker = new DataTable("Worker");
        public static Int32 ID_User = 0;
        public DataTable dtSposobPrim = new DataTable("SposobPrim");
        public DataTable dtBolezn = new DataTable("Bolezn");
        public DataTable dtTip = new DataTable("Tip");
        public DataTable dtVaktsiny = new DataTable("Vaktsiny");


        public static string qrDolzh = "SELECT [ID_Dolzh], [Dolzh_Title] as \"Должность\", " +
            "[Pay] as \"Оклад\" FROM [dbo].[Dolzh]",
             qrWorker = "SELECT [ID_Worker], [Worker_Name] as \"Имя сотрудника\", [Worker_Surname] as \"Фамилия сотрудника\", [Worker_MiddleName] as \"Отчество сотрудника\", [Seriers_Number_Password] as \"Серия и номер паспорта\", [Worker_Login] as \"Логин сотрудника\", [Worker_Password] as \"Пароль сотрудника\", [Dolzh_ID] as \"Должность\", [dbo].[Dolzh].[Dolzh_Title] FROM [dbo].[Worker] " +
            "INNER JOIN [dbo].[Dolzh] ON [dbo].[Worker].[Dolzh_ID] = [dbo].[Dolzh].[ID_Dolzh]",
            

            qrSposobPrim = "SELECT [ID_SposobPrim], [SposobPrim_Title] as \"Способ применения\" FROM [dbo].[SposobPrim]",

            qrBolezn = "SELECT [ID_Bolezn], [Bolezn_Title] as \"Название болезни\" FROM [dbo].[Bolezn]",

            qrTip = "SELECT [ID_Tip], [Tip_Title] as \"Тип вакцины\" FROM [dbo].[Tip] ",
            qrVaktsiny = "SELECT [ID_Vaktsiny], [Vaktsiny_Title] as \"Название вакцины\", [Vaktsiny_ID_Worker], CONCAT_WS(\' \',[Worker_Name],  [Worker_Surname], [Worker_MiddleName]) as \"Руководящий сотрудник\"," +
            " [Vaktsiny_ID_SposobPrim], [SposobPrim_Title] as \"Способ применения вакцины\", [Vaktsiny_ID_Bolezn], [Bolezn_Title] as \"Болезнь\", [Vaktsiny_ID_Tip], [Tip_Title] as \"Тип вакцины\" FROM [dbo].[Vaktsiny] " +
            "INNER JOIN [dbo].[Worker] ON [dbo].[Vaktsiny].[Vaktsiny_ID_Worker] = [dbo].[Worker].[ID_Worker]" +
            "INNER JOIN [dbo].[SposobPrim] ON [dbo].[Vaktsiny].[Vaktsiny_ID_SposobPrim] = [dbo].[SposobPrim].[ID_SposobPrim] " +
             "INNER JOIN [dbo].[Bolezn] ON [dbo].[Vaktsiny].[Vaktsiny_ID_Bolezn] = [dbo].[Bolezn].[ID_Bolezn] " +
             "INNER JOIN [dbo].[Tip] ON [dbo].[Vaktsiny].[Vaktsiny_ID_Tip] = [dbo].[Tip].[ID_Tip]";




        private SqlCommand command = new SqlCommand("", connection);
        public static Int32 IDrecord, IDuser;
        public void dbEnter(string login, string password)
        {
            command.CommandText = "SELECT count(*) FROM [dbo].[Worker] " +
                "where [Worker_Login] = '" + login + "' and [Worker_Password] = '" +
                password + "'";
            connection.Open();
            IDuser = Convert.ToInt32(command.ExecuteScalar().ToString());
            connection.Close();
        }

        private void dtFill(DataTable table, string query)
        {
            command.CommandText = query;
            connection.Open();
            table.Load(command.ExecuteReader());
            connection.Close();
        }

        public void WorkerFill()
        {
            dtFill(dtWorker, qrWorker);
        }

        

        public void SposobPrimFill()
        {
            dtFill(dtSposobPrim, qrSposobPrim);
        }
        public void BoleznFill()
        {
            dtFill(dtBolezn, qrBolezn);

        }

        public void TipFill()
        {
            dtFill(dtTip, qrTip);
        }
        public void DolzhFill()
        {
            dtFill(dtDolzh, qrDolzh);
        }
        
        public void VaktsinyFill()
        {
            dtFill(dtVaktsiny, qrVaktsiny);
        }

        public string userInfo(Int32 ID_record)
        {
            string user = "";
            command.CommandText = "select '. Сотрудник '" +
                "+[Worker_Surname]+' '+substring([Worker_Name],1,1)" +
                "+'. '+substring([Worker_MiddleName],1,1)+'.' " +
                "from [dbo].[Worker] where [ID_Worker]=" + IDuser;
            connection.Open();
            user = command.ExecuteScalar().ToString();
            connection.Close();
            return user;
        }
    }
}