using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Bunifu;
using System.Threading;
using System.Data.OleDb;

namespace KeyGen
{
    public partial class KeyGenForm : DevExpress.XtraEditors.XtraForm
    {
        
        public Thread trd;
        public int x = 0;
        public static string ConnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Activator_Cods.mdb;Jet OLEDB:Engine Type=5;Jet OLEDB:Database Password=Baranchik";
        const String KEY_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public OleDbConnection baza;
        public OleDbDataAdapter dA;
        public DataSet ds;
        public KeyGenForm()
        {
            InitializeComponent();
            Application.DoEvents();
            //Создание Access файла
            ADOX.Catalog BD = new ADOX.Catalog();
            try
            {
                BD.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Activator_Cods.mdb;Jet OLEDB:Engine Type=5;Jet OLEDB:Database Password=Baranchik");
                BD = null;
            }
            catch
            {
                System.IO.File.Delete("Activator_Cods.mdb");
                BD.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Activator_Cods.mdb;Jet OLEDB:Engine Type=5;Jet OLEDB:Database Password=Baranchik");
                BD = null;
            }
            //Открытие Файла
            baza = new OleDbConnection(ConnString);
            baza.Open();
            //Создание таблицы в БД
            OleDbCommand com = new OleDbCommand("CREATE TABLE Actvators (Cod int IDENTITY(1,1),Code1 STRING, Code2 string)", baza);
            com.ExecuteNonQuery();
            MessageBox.Show("База созданна");
            
            //button3.Visible = false;
        }
        Random rnd = new Random();
        string GenerateKey()
        { 
            string results = "";
            for (int i = 0; i < 7; i++)
            {
                results += KEY_CHARS[rnd.Next(0, KEY_CHARS.Length)];
            }
            return results;
        }
        string GenerateKey2()
        {
            string results = "";
            for (int i = 0; i < 35; i++)
            {
                results += KEY_CHARS[rnd.Next(0, KEY_CHARS.Length)];
            }
            return results;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            bunifuProgressBar1.Value = 0;
            bunifuProgressBar1.MaximumValue = x;
            string query = "";
            for (int i = 0; i < x; i++)
            {
                query = "INSERT INTO Actvators (Code1, Code2) VALUES ('" + dataGridView1.Rows[i].Cells[0].Value+"', '"+ dataGridView1.Rows[i].Cells[1].Value + "')";
                // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                OleDbCommand command = new OleDbCommand(query, baza);
                // выполняем запрос к MS Access
                command.ExecuteNonQuery();
                bunifuProgressBar1.Value++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           try
           {
               x = Convert.ToInt32(ColTextBox.Text);
           }
           catch
           {
               MessageBox.Show("Не верный ввод");
           }
           bunifuProgressBar1.Value = 0;
           bunifuProgressBar1.MaximumValue = x;
            
            for (int i = 0; i < x; i++)
           {
                listBox1.Items.Insert(i, GenerateKey()+"-"+ GenerateKey() + "-" + GenerateKey());
                listBox2.Items.Insert(i, GenerateKey2());
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = listBox2.Items[i];
               dataGridView1.Rows[i].Cells[1].Value = listBox1.Items[i];
               bunifuProgressBar1.Value++;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            dA.Dispose();
            ds.Dispose();
            baza.Dispose();
            baza = null;
            ds = null;
            dA = null;
            Application.Exit();
            MessageBox.Show("Отключено");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string CmdText = "SELECT * FROM [Actvators]";
            dA = new OleDbDataAdapter(CmdText, ConnString);
            ds = new DataSet();
            dA.Fill(ds, "[Actvators]");
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
