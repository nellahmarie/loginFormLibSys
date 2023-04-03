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
    public partial class listBorrowers : Form
    {
        public listBorrowers()
        {
            InitializeComponent();
        }

        private void listBorrowers_Load(object sender, EventArgs e)
        {
            LoadBorrowers();
        }
        private void LoadBorrowers()
        {
            using (SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True"))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT BorrowerId, FirstName, LastName, Address, Age, Phone, status FROM Borrower ORDER BY BorrowerId ASC", cn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataTable tab = new DataTable();
                adap.Fill(tab);
                gridBorrowers.DataSource = tab;
            }
        }

        private void gridBorrowers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadBorrowers();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            /* PrintDocument printDoc = new PrintDocument();
             printDoc.DocumentName = "Borrower Report";

             printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

             PrintPreviewDialog previewDlg = new PrintPreviewDialog();
             previewDlg.Document = printDoc;
             previewDlg.ShowDialog();*/

            DGVPrinter printer = new DGVPrinter();

            printer.Title = "List of Borrowers Report";
            printer.SubTitle = "An Easy to Use DataGridView Printing Object";
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "LibSys";
            printer.FooterSpacing = 15;

            if (gridBorrowers.Rows.Count > 0)
            {
                printer.PrintDataGridView(gridBorrowers);
            }
            else
            {

            }
        }
            private void PrintPage(object sender, PrintPageEventArgs e)
            {
                // Create a bitmap with the same dimensions as the DataGridView
                Bitmap bitmap = new Bitmap(gridBorrowers.Width, gridBorrowers.Height);

                // Draw the DataGridView onto the bitmap
                gridBorrowers.DrawToBitmap(bitmap, new Rectangle(0, 0, gridBorrowers.Width, gridBorrowers.Height));

                // Draw the bitmap onto the page
                e.Graphics.DrawImage(bitmap, new Point(50, 50));
            }
    }
}
