using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;  
using System.Threading.Tasks;
using DGVPrinterHelper;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection;

namespace loginForm
{
    public partial class listBooks : Form
    {

        public listBooks()
        {
            InitializeComponent();
        }

        private void listBooks_Load(object sender, EventArgs e)
        {
            LoadBooks();
        }
        private void LoadBooks()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True";
            string query = "SELECT * FROM Book"; // SELECT query to retrieve all books

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                gridBooks.DataSource = dataTable; // bind DataTable to DataGridView
            }
        }

        private void gridBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadBooks();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            /* PrintDocument printDoc = new PrintDocument();
             printDoc.DocumentName = "Books Report";

             printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

             PrintPreviewDialog previewDlg = new PrintPreviewDialog();
             previewDlg.Document = printDoc;
             previewDlg.ShowDialog();*/

            DGVPrinter printer = new DGVPrinter();

            printer.Title = "List of Books Report";
            printer.SubTitle = "A report for Books inside Datagrid view";
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "LibSys";
            printer.FooterSpacing = 15;

            if (gridBooks.Rows.Count > 0)
            {
                printer.PrintDataGridView(gridBooks);
            }
            else
            {
                MessageBox.Show("There is no data to print", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


         private void PrintPage(object sender, PrintPageEventArgs e)
         {
             // Create a bitmap with the same dimensions as the DataGridView
             Bitmap bitmap = new Bitmap(gridBooks.Width, gridBooks.Height);

             // Draw the DataGridView onto the bitmap
             gridBooks.DrawToBitmap(bitmap, new Rectangle(0, 0, gridBooks.Width, gridBooks.Height));

             // Draw the bitmap onto the page
             e.Graphics.DrawImage(bitmap, new Point(50, 50));
         }

    }
}
