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
using System.Windows.Forms.DataVisualization.Charting;

namespace loginForm
{
    public partial class Chart : Form
    {
        public Chart()
        {
            InitializeComponent();
        }

       private void Chart_Load(object sender, EventArgs e)
        {
           

            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True");
            cn.Open();
            // Set up the chart area and series
            ChartArea chartArea = new ChartArea();
            chart1.Size = new Size(800, 600);
            chart1.Dock = DockStyle.None; // unset dock style
            chart1.Anchor = AnchorStyles.None; // unset anchor style
            chart1.Left = (this.ClientSize.Width - chart1.Width) / 2;
            chart1.Top = (this.ClientSize.Height - chart1.Height) / 2;

            Series borrowedSeries = new Series("Borrowed Books");
            Series returnedSeries = new Series("Returned Books");

            chart1.ChartAreas.Add(chartArea);
            chart1.Series.Add(borrowedSeries);
            chart1.Series.Add(returnedSeries);
            chart1.BackColor = Color.White;
            chart1.Legends[0].BackColor = Color.White;
            chart1.BorderlineColor = this.BackColor;


            Title chartTitle = chart1.Titles.Add("borrowed and returned books");
            chartTitle.Font = new Font("Arial", 18, FontStyle.Bold);

            // Set the X-axis label
            chart1.ChartAreas[0].AxisX.Title = "Date";
            chart1.ChartAreas[0].AxisY.Title = "The no. of Borrowed and returned books";
          
            // Connect to the database and retrieve the data
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT BorrowDate, COUNT(*) as BorrowedBooks FROM Borrowed GROUP BY BorrowDate", connection);
            SqlDataReader reader;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    borrowedSeries.Points.AddXY(reader.GetDateTime(0).ToString("d"), reader.GetInt32(1));
                }

                reader.Close();

                command.CommandText = "SELECT ReturnDate, COUNT(*) as ReturnedBooks FROM ReturnItems GROUP BY ReturnDate";
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    returnedSeries.Points.AddXY(reader.GetDateTime(0).ToString("d"), reader.GetInt32(1));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            chart1.BackColor = Color.White;
            chart1.ChartAreas[0].BackColor = Color.Transparent;

            chart1.Legends[0].BackColor = Color.FromArgb(255, 224, 192);
            chart1.BorderlineColor = this.BackColor;
        }
    }
}

