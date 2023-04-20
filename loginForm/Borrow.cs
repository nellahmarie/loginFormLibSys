using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net;

namespace loginForm
{
    public partial class Borrow : Form
    {
        public Borrow()
        {
            InitializeComponent();
        }

        private void gridBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                UpdateBookQuantityLabel();
            }
            //LoadBookList();
        }
        private void UpdateBookQuantityLabel()
        {
            if (gridBooks.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gridBooks.SelectedRows[0];
                int bookQuantity = (int)row.Cells["Quantity"].Value;
                string title = row.Cells["Title"].Value.ToString();
                string author = row.Cells["Author"].Value.ToString();
                string status = row.Cells["status"].Value.ToString();
                lblBookQuantity.Text = $"{bookQuantity}";
                lblTitle.Text = $"{title}";
                lblAuthor.Text = $"{author}";
                StatusLabel.Text = $"{status}";
            }
        }
        private void UpdateBorrowerNameLabel()
        {
            if (gridBorrowers.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gridBorrowers.SelectedRows[0];
                string borrowerName = row.Cells["FirstName"].Value.ToString();
                string lastname = row.Cells["Lastname"].Value.ToString();
                string phone = row.Cells["Phone"].Value.ToString();
                string Address = row.Cells["Address"].Value.ToString();
                string age = row.Cells["Age"].Value.ToString();
                string status = row.Cells["status"].Value.ToString();
                lblBorrowerName.Text = $"{borrowerName}";
                lblLastname.Text = $"{lastname}";
                lblPhone.Text = $"{phone}";
                lblAddress.Text = $"{Address}";
                lblAge.Text = $"{age}";
                lblStatus.Text = $"{status}";

            }
        }
        private void LoadData()
        {
            LoadBookList();
            LoadBorrowers();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                UpdateBorrowerNameLabel();

            }
           // LoadBorrowers();
        }
        private void LoadBookList()
        {
            using (SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True"))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT AccessionNumber, Title, Author, Quantity, status FROM Book WHERE status = 'Available' ORDER BY AccessionNumber ASC", cn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataTable tab = new DataTable();
                adap.Fill(tab);
                gridBooks.DataSource = tab;
            }
        }

        private void LoadBorrowers()
        {
            using (SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True"))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT BorrowerId, FirstName, LastName, Address, Age, Phone, status FROM Borrower WHERE status = 'Active' ORDER BY BorrowerId ASC", cn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataTable tab = new DataTable();
                adap.Fill(tab);
                gridBorrowers.DataSource = tab;
            }
        }
        private void Borrow_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Borrower' table. You can move, or remove it, as needed.
            this.borrowerTableAdapter.Fill(this.database1DataSet.Borrower);
            LoadBookList();
            LoadBorrowers();
            gridBooks.ClearSelection();
            gridBorrowers.ClearSelection();
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True");

            cn.Open();
            if (gridBooks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (gridBorrowers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a borrower!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
          

            int bookId = (int)gridBooks.SelectedRows[0].Cells["AccessionNumber"].Value;
            string title = gridBooks.SelectedRows[0].Cells["Title"].Value.ToString();
            string author = gridBooks.SelectedRows[0].Cells["Author"].Value.ToString();
            int bookQuantity = (int)gridBooks.SelectedRows[0].Cells["Quantity"].Value;

            if (quantity > bookQuantity)
            {
                MessageBox.Show("Not enough quantity of book!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (gridBorrowers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a borrower!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int borrowerId = (int)gridBorrowers.SelectedRows[0].Cells["BorrowerId"].Value;

            using (cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True"))
            {
                cn.Open();

                SqlTransaction transaction = cn.BeginTransaction();

                try
                {

                   /* SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Borrowed WHERE AccessionNumber = @AccessionNumber AND BorrowerId = @BorrowerId", cn);
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@AccessionNumber", bookId);
                    cmd.Parameters.AddWithValue("@BorrowerId", borrowerId);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("You already borrowed this book!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }*/
                    // insert record in Borrowed table
                    DateTime currentDate = DateTime.Now;
                    DateTime returnDate = dateTimePicker1.Value;
                    DateTime borrowDate = dateTimePicker2.Value;
                    int daysLeft = CalculateDaysLeft(returnDate, borrowDate);

                    ShowDateTimePicker(dateTimePicker1);

                    if (daysLeft < 1)
                    {
                        ShowErrorMessage("Please select a valid due date.");
                        return;
                    }


                    if (daysLeft < 0)
                    {
                        MessageBox.Show("Due date should not be in the past of the current date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (daysLeft == 7)
                    {
                        // Allow book to be borrowed
                        SqlCommand cmd = new SqlCommand("INSERT INTO Borrowed (AccessionNumber, BorrowerId, Quantity, ReturnDate, BorrowDate) SELECT @AccessionNumber, @BorrowerId, @Quantity, @ReturnDate, @BorrowDate", cn);
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@AccessionNumber", bookId);
                        cmd.Parameters.AddWithValue("@BorrowerId", borrowerId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@ReturnDate", dateTimePicker1.Text);
                        cmd.Parameters.AddWithValue("@BorrowDate", DateTime.Today);
                        cmd.ExecuteNonQuery();
                        // update quantity in Books table
                        cmd = new SqlCommand("UPDATE Book SET Quantity = Quantity - @Quantity WHERE AccessionNumber = @AccessionNumber", cn);
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@AccessionNumber", bookId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        MessageBox.Show("The book is due for return in 7 days.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (daysLeft > 7)
                    {
                        MessageBox.Show("Return date should be within 7 days from the date of borrowing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                      SqlCommand  cmd = new SqlCommand("INSERT INTO Borrowed (AccessionNumber, BorrowerId, Quantity, ReturnDate, BorrowDate) SELECT @AccessionNumber, @BorrowerId, @Quantity, @ReturnDate, @BorrowDate", cn);
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@AccessionNumber", bookId);
                        cmd.Parameters.AddWithValue("@BorrowerId", borrowerId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                        cmd.Parameters.AddWithValue("@BorrowDate", borrowDate);
                        cmd.ExecuteNonQuery();

                        // update quantity in Books table
                        cmd = new SqlCommand("UPDATE Book SET Quantity = Quantity - @Quantity WHERE AccessionNumber = @AccessionNumber", cn);
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@AccessionNumber", bookId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                        MessageBox.Show("The book has been borrowed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    cn.Close();
                }
            }
        }
        private int CalculateDaysLeft(DateTime returnDate, DateTime borrowDate)
        {
            TimeSpan timeSpan = returnDate - borrowDate;
            return (int)timeSpan.TotalDays;
        }
        private void ShowDateTimePicker(DateTimePicker dateTimePicker)
        {
            dateTimePicker.CustomFormat = "yyyy-MM-dd hh:mm:ss";
            dateTimePicker.Format = DateTimePickerFormat.Custom;
        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        private void btnClear_Click(object sender, EventArgs e)
        {
            txtQuantity.Clear();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            gridBooks.ClearSelection();
            gridBorrowers.ClearSelection();
            lblBookQuantity.Text = "";
            lblBorrowerName.Text = "";
            lblLastname.Text = "";
            lblTitle.Text = "";
            lblAuthor.Text = "";
            lblPhone.Text = "";
            lblAddress.Text = "";
            lblAge.Text = "";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            
                SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
                cn.Open();

                SqlCommand cmd = new SqlCommand("Select * from Book where Title like @Title", cn);
                cmd.Parameters.AddWithValue("@Title", "%" + txtSearch.Text + "%");

                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataTable tab = new DataTable();

                adap.Fill(tab);
                gridBooks.DataSource = tab;
            
        }

        private void txtSearchBorrower_TextChanged(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True");
            cn.Open();

            SqlCommand cmd = new SqlCommand("Select * from Borrower where Firstname like @Firstname", cn);
            cmd.Parameters.AddWithValue("@Firstname", "%" + txtSearchBorrower.Text + "%");

            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            gridBorrowers.DataSource = tab;
        }
    }
}

