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

namespace loginForm
{
    public partial class ReturnedDate : Form
    {
        public ReturnedDate()
        {
            InitializeComponent();
        }

        private void ReturnedDate_Load(object sender, EventArgs e)
        {
            loadReturnedItems();
        }
        private void loadReturnedItems()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True";
            string query = "SELECT ReturnId, Book.AccessionNumber, Title, Author, Borrower.BorrowerId, Borrower.Firstname, Borrower.Lastname, ReturnItems.Quantity, BorrowDate, ReturnDate " +
                                                 "FROM ReturnItems " +
                                                 "INNER JOIN Book ON ReturnItems.AccessionNumber = Book.AccessionNumber " +
                                                 "INNER JOIN Borrower ON ReturnItems.BorrowerId = Borrower.BorrowerId " +
                                                 "ORDER BY BorrowDate ASC";  // SELECT query to retrieve all returned books

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable; // bind DataTable to DataGridView
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            loadReturnedItems();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True");
            cn.Open();
            using (cn)
            {
                DateTime startDate = StartDate.Value.Date;
                DateTime endDate = EndDate.Value.Date.AddDays(1).AddSeconds(-1);

                SqlCommand cmd = new SqlCommand("SELECT ReturnId, Book.AccessionNumber, Title, Author, Borrower.BorrowerId, Borrower.Firstname, Borrower.Lastname, ReturnItems.Quantity, BorrowDate, ReturnDate " +
                                 "FROM ReturnItems " +
                                 "INNER JOIN Book ON ReturnItems.AccessionNumber = Book.AccessionNumber " +
                                 "INNER JOIN Borrower ON ReturnItems.BorrowerId = Borrower.BorrowerId " +
                                 "WHERE BorrowDate >= @StartDate AND BorrowDate <= @EndDate " +
                                 "ORDER BY BorrowDate ASC", cn);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }
    }
}
