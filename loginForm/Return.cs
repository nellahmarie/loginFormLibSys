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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;

namespace loginForm
{
    public partial class Return : Form
    {
        public Return()
        {
            InitializeComponent();
        }

        private void Return_Load(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True");
            LoadBorrowedItems();
            LoadBooks();
            gridBorrowedItems.ClearSelection();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True");

            cn.Open();

            if (gridBorrowedItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a borrowed item to return!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int borrowedItemId = (int)gridBorrowedItems.SelectedRows[0].Cells["BorrowedId"].Value;
            int bookId = (int)gridBorrowedItems.SelectedRows[0].Cells["AccessionNumber"].Value;
            string title = gridBorrowedItems.SelectedRows[0].Cells["Title"].Value.ToString();
            string author = gridBorrowedItems.SelectedRows[0].Cells["Author"].Value.ToString();
            int borrowerId = (int)gridBorrowedItems.SelectedRows[0].Cells["BorrowerId"].Value;
            //string Firstname = gridBorrowedItems.SelectedRows[0].Cells["First name"].Value.ToString();
            //string Lastname = gridBorrowedItems.SelectedRows[0].Cells["Last name"].Value.ToString();
            int quantity = (int)gridBorrowedItems.SelectedRows[0].Cells["Quantity"].Value;
            DateTime borrowDate = (DateTime)gridBorrowedItems.SelectedRows[0].Cells["BorrowDate"].Value;
            DateTime returnDate = (DateTime)gridBorrowedItems.SelectedRows[0].Cells["ReturnDate"].Value;

            if (quantity == 0)
            {
                MessageBox.Show("This item is already returned", "OK", MessageBoxButtons.OK);
            }
            else if (quantity > 0)
            {

                TimeSpan duration = DateTime.Now.Subtract(returnDate);
                int daysLate = duration.Days;
                if (daysLate < 0) daysLate = 0;

                decimal lateFee = daysLate * 5; //assuming P5 per day late fee

                 if (MessageBox.Show($"Do you want to return the book {title}?\n\n" +
                                    // $"Days Late: {daysLate}\n" +
                                    // $"Late Fee: ${lateFee}\n\n" +
                                     "Click Yes to proceed.", "Confirm Return", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                 {
                    using (cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True"))
                    {
                        cn.Open();

                        SqlTransaction transaction = cn.BeginTransaction();

                        try
                        {
                            // mark the borrowed item as returned
                           // SqlCommand cmd = new SqlCommand("UPDATE Borrowed SET Quantity = 0 WHERE BorrowedId = @BorrowedId", cn);
                            SqlCommand cmd = new SqlCommand("UPDATE Borrowed SET Quantity = Quantity - 1 WHERE BorrowedId = @BorrowedId", cn);
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@BorrowedId", borrowedItemId);
                            cmd.Parameters.AddWithValue("@Quantity", 1);
                            cmd.ExecuteNonQuery();

                            // insert record in ReturnItems table
                            cmd = new SqlCommand("INSERT INTO ReturnItems (BorrowedId, AccessionNumber, BorrowerId, Quantity, BorrowDate, ReturnDate, LateFee) " +
                                                    "VALUES (@BorrowedId, @AccessionNumber, @BorrowerId, @Quantity, @BorrowDate, @ReturnDate, @LateFee)", cn);
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@BorrowedId", borrowedItemId);
                            cmd.Parameters.AddWithValue("@AccessionNumber", bookId);
                            cmd.Parameters.AddWithValue("@BorrowerId", borrowerId);
                            cmd.Parameters.AddWithValue("@Quantity", quantity);
                            cmd.Parameters.AddWithValue("@BorrowDate", borrowDate);
                            cmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@LateFee", lateFee);
                            cmd.ExecuteNonQuery();

                            // update quantity in Book table
                            cmd = new SqlCommand("UPDATE Book SET Quantity = Quantity + 1 WHERE AccessionNumber = @AccessionNumber AND (Quantity + @Quantity) >= Quantity", cn);
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@AccessionNumber", bookId);
                            cmd.Parameters.AddWithValue("@Quantity", 1);
                            cmd.ExecuteNonQuery();
                            transaction.Commit();

                            MessageBox.Show("Book has been returned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadBorrowedItems();
                            LoadBooks();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("An error occurred while returning the book. Please try again later." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            cn.Close();
                        }
                    }
                }
            }
        }

        private void LoadBorrowedItems()
        {
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True");

            try
            {
                cn.Open();

                /*SqlCommand cmd = new SqlCommand("SELECT BorrowedId, Book.AccessionNumber, Title, Author, Borrower.BorrowerId, Borrower.Firstname, Borrower.Lastname, Borrowed.Quantity, BorrowDate, ReturnDate " +
                                                 "FROM Borrowed " +
                                                 "INNER JOIN Book ON Borrowed.AccessionNumber = Book.AccessionNumber " +
                                                 "INNER JOIN Borrower ON Borrowed.BorrowerId = Borrower.BorrowerId " +
                                                 "ORDER BY BorrowDate ASC", cn);*/
                SqlCommand cmd = new SqlCommand("SELECT BorrowedId, Book.AccessionNumber, Title, Author, Borrower.BorrowerId, Borrower.Firstname, Borrower.Lastname, Borrowed.Quantity, BorrowDate, ReturnDate " +
                                 "FROM Borrowed " +
                                 "INNER JOIN Book ON Borrowed.AccessionNumber = Book.AccessionNumber " +
                                 "INNER JOIN Borrower ON Borrowed.BorrowerId = Borrower.BorrowerId " +
                                 "WHERE Borrowed.Quantity != 0 " +
                                 "ORDER BY BorrowDate ASC", cn);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gridBorrowedItems.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading borrowed items. Please try again later." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
            }
        }

        private void gridBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadBooks();
        }
        private void LoadBooks()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True";
            string query = "SELECT * FROM Book WHERE status = 'Available'"; // SELECT query to retrieve all books

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                gridBooks.DataSource = dataTable; // bind DataTable to DataGridView
            }
        }

     
    }
}

