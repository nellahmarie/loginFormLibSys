using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace loginForm
{
    public partial class Home : Form
    {
        SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
        

        public DataGridViewRow DataGridView { get; private set; }
        public string Author { get; internal set; }

        public Home()
        {
            InitializeComponent();
        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridView = this.grid1.Rows[e.RowIndex];
                txtno.Text = this.grid1.Rows[e.RowIndex].Cells["AccessionNumber"].Value.ToString();
                txtTitle.Text = this.grid1.Rows[e.RowIndex].Cells["Title"].Value.ToString();
                txtAuthor.Text = this.grid1.Rows[e.RowIndex].Cells["Author"].Value.ToString();
                txtQuantity.Text = this.grid1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
            }


        }

      private void btnAdd_Click(object sender, EventArgs e)
      {
            if (txtTitle.Text == string.Empty || txtAuthor.Text == string.Empty || txtQuantity.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Fill all required fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True"))
                {
                    cn.Open();
                    // Check if the book already exists in the database
                    SqlCommand checkCmd = new SqlCommand("SELECT AccessionNumber FROM Book WHERE Title = @Title AND Author = @Author", cn);
                    checkCmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                    checkCmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
                    object result = checkCmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Book already exists in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Book (Title, Author, Quantity, status) VALUES (@Title, @Author, @Quantity, @status)", cn))
                    {
                        cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                        cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
                        cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                        cmd.Parameters.AddWithValue("@status", comboBox1.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully Added!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    cn.Close();
                    loadDatagrid();
                }
            }

        }
        private void loadDatagrid()
        {
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
            cn.Open();
            SqlCommand cmd = new SqlCommand("Select * from Book where status != 'Lost' order by AccessionNumber asc", cn);
            cmd.ExecuteNonQuery();

            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            cn.Close();

        }

        private void Home_Load(object sender, EventArgs e)
        {
            loadDatagrid();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtno.Text))
            {
                MessageBox.Show("Select a record to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtno.Text == string.Empty || txtTitle.Text == string.Empty || txtAuthor.Text == string.Empty || txtQuantity.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Select Book id");
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True"))
                {
                    cn.Open();

                    // Create a SQL command to update the book's details
                    SqlCommand cmd = new SqlCommand("UPDATE Book SET Title = @Title, Author = @Author, Quantity = @Quantity, status = @status WHERE AccessionNumber = @AccessionNumber", cn);
                    cmd.Parameters.AddWithValue("@AccessionNumber", txtno.Text);
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
                    cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@status", comboBox1.Text);

                    // Execute the SQL command
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadDatagrid(); // Reload the data grid view to show the updated book details
                    }
                    else
                    {
                        MessageBox.Show("Unable to update book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
           
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
            cn.Open();

            string num = txtno.Text;

            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                MessageBox.Show("Select a record to Delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

           // Check if there are any borrowed items for the book to be deleted
            SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM Borrowed WHERE AccessionNumber = @AccessionNumber", cn);
            cmdCheck.Parameters.AddWithValue("@AccessionNumber", txtno.Text);

            int count = (int)cmdCheck.ExecuteScalar();

            if (count > 0)
            {
                MessageBox.Show("Cannot delete the book because there are borrowed items associated with it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(dr == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand("Delete from Book where AccessionNumber='"+ num + "'", cn);
                cmd.Parameters.AddWithValue("@AccessionNumber", txtno.Text);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully Deleted!", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Cancelled!", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            cn.Close();
            loadDatagrid();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
            cn.Open();

            txtno.Clear();
            txtTitle.Clear();
            txtAuthor.Clear();
            txtQuantity.Clear();
            comboBox1.Text = "";

            cn.Close();
            loadDatagrid();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //Search bar
            using (cn)
            {
                SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
                cn.Open();

                SqlCommand cmd = new SqlCommand("Select * from Book where Title like @Title", cn);
                cmd.Parameters.AddWithValue("@Title", "%" + textBox4.Text + "%");

                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataTable tab = new DataTable();

                adap.Fill(tab);
                grid1.DataSource = tab;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }

}
