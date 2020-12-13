using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Подключени к БД по Логину и паролю
        /// </summary>
        [TestMethod]
        public void Connection()
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=" + Application.StartupPath + "\\BDCinema.db");
            con.Open();

            //Запрос
            SQLiteDataAdapter sda = new SQLiteDataAdapter("SELECT * FROM [Пользователи] Where [Логин]='user' AND [Пароль]='123';", con);
           
            //Таблица данных
            DataTable dt = new DataTable();

            //Проверка успеха
            if (sda.Fill(dt) == 0)
            {
                Assert.IsTrue(false);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        /// <summary>
        /// Таблица сеансов
        /// </summary>
        [TestMethod]
        public void UpdateTable()
        {
            //Строка подключения
            SQLiteConnection con = new SQLiteConnection("Data Source=" + Application.StartupPath + "\\BDCinema.db");
            con.Open();
            //Запрос
            SQLiteDataAdapter sda = new SQLiteDataAdapter("SELECT * FROM Сеансы " +
              "WHERE  not exists(SELECT * FROM [Брони] " +
              "WHERE [Брони].[Код пользователя]=2 AND [Сеансы].[Код]=[Брони].[Код сеанса])", con);
           
            //Таблица для данных
            DataTable dt = new DataTable();

            //Проверка успеха
            if (sda.Fill(dt) == 0)
            {
                Assert.IsTrue(false);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
    }
}
