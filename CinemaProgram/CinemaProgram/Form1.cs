using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CinemaProgram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Подключение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            con = new SQLiteConnection("Data Source=" + Application.StartupPath + "\\BDCinema.db");
            con.Open();
            sda = new SQLiteDataAdapter("SELECT * FROM [Пользователи] Where [Логин]='"+login.Text+"' AND [Пароль]='"+parol.Text+"';", con);
            dt = new DataTable();

            if (sda.Fill(dt) == 0)
            {
                bron.Visible = false;
                dgv.DataSource = null;
                MessageBox.Show("Неверный логин или пароль!", "Произошла ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                codeUser = dt.Rows[0][0].ToString();
                LoadSessions();
            }
        }

        static string codeUser = "";

        /// <summary>
        /// Обновить все таблицы
        /// </summary>
        void LoadSessions()
        {
            UpdateDGV("not exists", dgv);
            UpdateDGV("exists",dgv2);
            tabControl1_SelectedIndexChanged(tabControl1,null);
            Debug.WriteLine("Обновление таблиц успешно пройдено");
        }

        /// <summary>
        /// Обновляет таблицы
        /// </summary>
        /// <param name="flag">true - exist / false - not exists</param>
        void UpdateDGV(string command,DataGridView table)
        {
            sda = new SQLiteDataAdapter("SELECT * FROM Сеансы " +
               "WHERE  "+command+"(SELECT * FROM [Брони] " +
               "WHERE [Брони].[Код пользователя]=" + codeUser + " AND [Сеансы].[Код]=[Брони].[Код сеанса])", con);

           
            dt = new DataTable();

            sda.Fill(dt);
            table.DataSource = dt;
            bron.Visible = true;
            table.Columns[0].Visible = false;
            table.ClearSelection();
        }

        SQLiteDataAdapter sda;
        SQLiteConnection con;
        DataTable dt;
        SQLiteCommand com;

        /// <summary>
        /// Поставить \ снять бронь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bron_Click(object sender, EventArgs e)
        {
            if(indexTab==0)
            {
                com = new SQLiteCommand("INSERT INTO [Брони] VALUES("+codeUser+","+name1+");", con);
                com.ExecuteNonQuery();

                LoadSessions();
            }
            else
            {
                com = new SQLiteCommand("DELETE FROM [Брони] WHERE [Брони].[Код пользователя]="+codeUser+ " AND [Брони].[Код сеанса]="+name2+";", con);
                com.ExecuteNonQuery();

                LoadSessions();
            }
        }

        /// <summary>
        /// Событие закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(con!=null)
             con.Close();
        }

        /// <summary>
        /// Событие загрузки формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.RowHeadersVisible = false;
            dgv.MultiSelect = false;

            dgv2.AllowUserToResizeColumns = false;
            dgv2.AllowUserToResizeRows = false;
            dgv2.AllowUserToAddRows = false;
            dgv2.AllowUserToDeleteRows = false;
            dgv2.RowHeadersVisible = false;
            dgv2.MultiSelect = false;
        }

        static string name1 = "";
        /// <summary>
        /// Событие выбора ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView buffer = (DataGridView)sender;

            if(buffer.SelectedCells.Count!=0)
            {
                dgv.Rows[buffer.SelectedCells[0].RowIndex].Selected = true;
                name1 = dgv[0, buffer.SelectedCells[0].RowIndex].Value.ToString();
            }
        }
        static string name2 = "";

        /// <summary>
        /// Событие выбора ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgw2_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView buffer = (DataGridView)sender;

            if (buffer.SelectedCells.Count != 0)
            {
                dgv2.Rows[buffer.SelectedCells[0].RowIndex].Selected = true;
                name2 = dgv2[0, buffer.SelectedCells[0].RowIndex].Value.ToString();
            }
        }
        static int indexTab  = 0;

        /// <summary>
        /// Изменение названия кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl buffer = (TabControl)sender;

            if(buffer.SelectedIndex==1)
            {
                bron.Text = "Убрать бронь";
            }
            else
            {
                bron.Text = "Добавить бронь";
            }

            indexTab = buffer.SelectedIndex;
        }
    }
}
