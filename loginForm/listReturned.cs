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
    public partial class listReturned : Form
    {
        public listReturned()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            /* PrintDocument printDoc = new PrintDocument();
             printDoc.DocumentName = "Borrowed Report";

             printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

             PrintPreviewDialog previewDlg = new PrintPreviewDialog();
             previewDlg.Document = printDoc;
             previewDlg.ShowDialog();*/
            DGVPrinter printer = new DGVPrinter();

            printer.Title = "List of Returned Books Report";
            printer.SubTitle = "An Easy to Use DataGridView Printing Object";
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = false;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "LibSys";
            printer.FooterSpacing = 15;

            if (gridReturned.Rows.Count > 0)
            {
                printer.PrintDataGridView(gridReturned);
            }
            else
            {

            }
        }

            private void PrintPage(object sender, PrintPageEventArgs e)
            {
                // Create a bitmap with the same dimensions as the DataGridView
                Bitmap bitmap = new Bitmap(gridReturned.Width, gridReturned.Height);

                // Draw the DataGridView onto the bitmap
                gridReturned.DrawToBitmap(bitmap, new Rectangle(0, 0, gridReturned.Width, gridReturned.Height));

                // Draw the bitmap onto the page
                e.Graphics.DrawImage(bitmap, new Point(50, 50));
            }

        private void listReturned_Load(object sender, EventArgs e)
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
                                                 "ORDER BY BorrowDate ASC";  // SELECT query to retrieve all books

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                gridReturned.DataSource = dataTable; // bind DataTable to DataGridView
            }
        }

        private void gridReturned_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            loadReturnedItems();
        }
    }
}
