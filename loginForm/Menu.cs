using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace loginForm
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void booksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Borrower borrower = new Borrower();
            borrower.Show();
        }

        private void booksToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
            Home home = new Home();
            home.ShowDialog();
        }

        private void borrowingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Borrow borrowed = new Borrow();
            borrowed.Show();
        }

        private void returningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Return returnitems = new Return();
            returnitems.Show();
        }

        private void inventoryOfBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBooks booklist = new listBooks();
                booklist.Show();
        }

        private void inventoryOfBorrowersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBorrowers borrowers = new listBorrowers();
            borrowers.Show();
        }

        private void returnedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBorrowed borrowed = new listBorrowed();
            borrowed.Show();
        }

        private void returnedToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listReturned returned = new listReturned();
            returned.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void borrowedDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateFilter borrowedDate = new DateFilter();
            borrowedDate.Show();
        }

        private void returnedDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReturnedDate returned = new ReturnedDate();
            returned.Show();
        }

        private void chartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Chart chart = new Chart();
            chart.Show();
        }
    }
}
