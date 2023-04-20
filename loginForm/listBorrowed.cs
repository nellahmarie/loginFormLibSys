using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace loginForm
{
    public partial class listBorrowed : Form
    {
        public listBorrowed()
        {
            InitializeComponent();
        }

        private void listBorrowed_Load(object sender, EventArgs e)
        {
            LoadBooks();
        }
        private void LoadBooks()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True";
            string query = "SELECT BorrowedId, Book.AccessionNumber, Title, Author, Borrower.BorrowerId, Borrower.Firstname, Borrower.Lastname, Borrowed.Quantity, BorrowDate, ReturnDate " +
                                 "FROM Borrowed " +
                                 "INNER JOIN Book ON Borrowed.AccessionNumber = Book.AccessionNumber " +
                                 "INNER JOIN Borrower ON Borrowed.BorrowerId = Borrower.BorrowerId " +
                                 "ORDER BY BorrowDate ASC"; // SELECT query to retrieve all borrowed books

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                gridBorrowed.DataSource = dataTable; // bind DataTable to DataGridView
            }
        }

        private void gridBorrowed_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadBooks();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            // Create a bitmap with the same dimensions as the DataGridView
            Bitmap bitmap = new Bitmap(gridBorrowed.Width, gridBorrowed.Height);

            // Draw the DataGridView onto the bitmap
            gridBorrowed.DrawToBitmap(bitmap, new Rectangle(0, 0, gridBorrowed.Width, gridBorrowed.Height));

            // Draw the bitmap onto the page
            e.Graphics.DrawImage(bitmap, new Point(50, 50));
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            /* PrintDocument printDoc = new PrintDocument();
             printDoc.DocumentName = "Borrowed Report";

             printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

             PrintPreviewDialog previewDlg = new PrintPreviewDialog();
             previewDlg.Document = printDoc;
             previewDlg.ShowDialog();*/

            DGVPrinter printer = new DGVPrinter();

            printer.Title = "List of Borrowed Books Report";
           // printer.SubTitle = "An Easy to Use DataGridView Printing Object";
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "LibSys";
            printer.FooterSpacing = 15;

            if (gridBorrowed.Rows.Count > 0)
            {
                printer.PrintDataGridView(gridBorrowed);
            }
            else
            {
                MessageBox.Show("There is no data to print", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
