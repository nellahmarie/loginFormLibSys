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
    public partial class HomeBorrower : Form
    {
        public HomeBorrower()
        {
            InitializeComponent();

        }

        private void borrowingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Borrow borrowBooks = new Borrow();
            borrowBooks.Show();
        }

        private void returningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Return returnBooks = new Return();
            returnBooks.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void transactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
