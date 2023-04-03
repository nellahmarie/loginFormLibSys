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


            SqlCommand cmd = new SqlCommand("SELECT COUNT(DISTINCT BorrowedId) AS BorrowerCount FROM Borrowed", cn);
            int borrowerCount = 0;

            borrowerCount = (int)cmd.ExecuteScalar();
            // Create a new series
            Series borrowerCountSeries = new Series("BorrowerCountSeries");

            // Add the series to the chart control
            chart1.Series.Add(borrowerCountSeries);

            chart1.DataSource = new[] { borrowerCount };
            chart1.Series["BorrowerCountSeries"].XValueMember = "";
            chart1.Series["BorrowerCountSeries"].YValueMembers = "BorrowerCount";
            chart1.Series["BorrowerCountSeries"].ChartType = SeriesChartType.Column;
        }
    }
}

