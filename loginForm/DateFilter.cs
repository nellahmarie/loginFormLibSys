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
    public partial class DateFilter : Form
    {
        public DateFilter()
        {
            InitializeComponent();
        }

        private void DateFilter_Load(object sender, EventArgs e)
        {
            loadBorrowedBooks();
        }
        public void loadBorrowedBooks()
        {
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True");
            cn.Open();
            using (cn)
            {
                /* SqlCommand cmd = new SqlCommand("SELECT Borrowed.BorrowedId, Book.AccessionNumber, Title, Author, Borrower.BorrowerId, Borrower.Firstname, Borrower.Lastname, Borrowed.Quantity, Borrowed.BorrowDate, COALESCE(ReturnItems.ReturnDate, Borrowed.ReturnDate) AS ReturnDate" +
                     "FROM Borrowed" +
                     "INNER JOIN Book ON Borrowed.AccessionNumber = Book.AccessionNumber" + 
                     "INNER JOIN Borrower ON Borrowed.BorrowerId = Borrower.BorrowerId" + 
                     "LEFT JOIN ReturnItems ON Borrowed.BorrowedId = ReturnItems.BorrowedId" +
                     "ORDER BY BorrowDate ASC", cn);*/

                //Borrowed Date and Due Date
                SqlCommand cmd = new SqlCommand("SELECT BorrowedId, Book.AccessionNumber, Title, Author, Borrower.BorrowerId, Borrower.Firstname, Borrower.Lastname, Borrowed.Quantity, BorrowDate, ReturnDate " +
                                                 "FROM Borrowed " +
                                                 "INNER JOIN Book ON Borrowed.AccessionNumber = Book.AccessionNumber " +
                                                 "INNER JOIN Borrower ON Borrowed.BorrowerId = Borrower.BorrowerId " +
                                                 "ORDER BY BorrowDate ASC", cn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            loadBorrowedBooks(); 
        }
        //Filter Date for Borrowing Date and Due date
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True");
            cn.Open();
            using (cn)
            {
                DateTime startDate = StartDate.Value.Date;
                DateTime endDate = EndDate.Value.Date.AddDays(1).AddSeconds(-1);

                SqlCommand cmd = new SqlCommand("SELECT BorrowedId, Book.AccessionNumber, Title, Author, Borrower.BorrowerId, Borrower.Firstname, Borrower.Lastname, Borrowed.Quantity, BorrowDate, ReturnDate " +
                                 "FROM Borrowed " +
                                 "INNER JOIN Book ON Borrowed.AccessionNumber = Book.AccessionNumber " +
                                 "INNER JOIN Borrower ON Borrowed.BorrowerId = Borrower.BorrowerId " +
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
