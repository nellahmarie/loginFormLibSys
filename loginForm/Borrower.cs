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
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

    namespace loginForm
    {
        public partial class Borrower : Form
        {
        
            public Borrower()
            {
                InitializeComponent();
                loadDatagrid();
                loadDataUsers();


            }
        private void USERS()
        {
            if (dataGridViewUSERS.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridViewUSERS.SelectedRows[0];
                string username = row.Cells["Username"].Value.ToString();
                string userType = row.Cells["UserType"].Value.ToString();
                
                txtusername.Text = $"{username}";
                comboBox2.Text = $"{userType}";
                
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
            {
            

                if (txtFirstname.Text == string.Empty || txtLastname.Text == string.Empty || txtAddress.Text == string.Empty || txtAge.Text == string.Empty)
                {
                    MessageBox.Show("Fill some text Field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
                    cn.Open();

                    // Check if user already exists
                    SqlCommand selectCmd = new SqlCommand("SELECT COUNT(*) FROM Borrower WHERE Firstname = @Firstname AND Lastname = @Lastname AND Address = @Address AND Age = @Age", cn);
                    selectCmd.Parameters.AddWithValue("@Firstname", txtFirstname.Text);
                    selectCmd.Parameters.AddWithValue("@Lastname", txtLastname.Text);
                    selectCmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    selectCmd.Parameters.AddWithValue("@Age", txtAge.Text);

                    int count = (int)selectCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Borrower already exists in the database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cn.Close();
                        return;
                    }

                // Insert new record
                SqlCommand cmd = new SqlCommand("INSERT INTO Borrower (username, Firstname, Lastname, Address, Age, Phone, userType, status) VALUES (@username, @Firstname, @Lastname, @Address, @Age, @Phone, @userType, @status)", cn);

                cmd.Parameters.AddWithValue("@BorrowerId", txtBorrowerId.Text);
                cmd.Parameters.AddWithValue("@Firstname", txtFirstname.Text);
                cmd.Parameters.AddWithValue("@Lastname", txtLastname.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@status", comboBox1.Text);
                cmd.Parameters.AddWithValue("@username", txtusername.Text);
                cmd.Parameters.AddWithValue("@userType", comboBox2.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully Added!", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cn.Close();
                loadDatagrid();
                //GetLoginUser();
                loadDataUsers();

            }
        }
           



            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
                
                if(e.RowIndex >= 0)
                {
                    DataGridViewRow row = grid2.Rows[e.RowIndex];
                    txtBorrowerId.Text = row.Cells["BorrowerId"].Value.ToString();
                    txtFirstname.Text = row.Cells["Firstname"].Value.ToString();
                    txtLastname.Text = row.Cells["Lastname"].Value.ToString();
                    txtAddress.Text = row.Cells["Address"].Value.ToString();
                    txtAge.Text = row.Cells["Age"].Value.ToString();
                    txtPhone.Text = row.Cells["Phone"].Value.ToString();
                    txtusername.Text = row.Cells["username"].Value.ToString();
                    comboBox2.Text = row.Cells["userType"].Value.ToString();    

                }
            
            }
             private void loadDatagrid()
             {
                 SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
                 cn.Open();

            // SqlCommand cmd = new SqlCommand("SELECT Borrower.BorrowerId, Borrower.Firstname, Borrower.Lastname, Borrower.Address, Borrower.Age, Borrower.Phone, Borrower.status, LoginTable.username, LoginTable.userType FROM Borrower INNER JOIN LoginTable ON Borrower.username = LoginTable.username WHERE LoginTable.userType = 'Borrower' ORDER BY BorrowerId ASC", cn);
            // SqlCommand cmd = new SqlCommand("SELECT LoginTable.username, LoginTable.userType, Borrower.BorrowerId, Borrower.Firstname, Borrower.Lastname, Borrower.Address, Borrower.Age, Borrower.Phone, Borrower.status FROM LoginTable INNER JOIN Borrower ON LoginTable.username = Borrower.username WHERE LoginTable.userType = 'Borrower' ORDER BY BorrowerId ASC", cn);
            /* SqlCommand cmd = new SqlCommand("SELECT Borrower.BorrowerId, Borrower.Firstname, Borrower.Lastname, Borrower.Address, Borrower.Age, Borrower.Phone, Borrower.status, LoginTable.username, LoginTable.userType " + 
                 "FROM Borrower INNER JOIN LoginTable ON Borrower.username = LoginTable.username " + 
                 "WHERE LoginTable.userType = 'Borrower' " +
                 "ORDER BY BorrowerId ASC", cn);*/
                SqlCommand cmd = new SqlCommand("SELECT * FROM Borrower", cn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                 DataTable tab = new DataTable();
                 adap.Fill(tab);

                 grid2.DataSource = tab;

                 cn.Close();
             }
            private void loadDataUsers()
            {
                SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT username, userType FROM LoginTable", cn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataTable tab = new DataTable();
                adap.Fill(tab);

                dataGridViewUSERS.DataSource = tab;

                cn.Close();
                USERS();
        }

            private void btnUpdate_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(txtBorrowerId.Text))
                {
                    MessageBox.Show("Select a record to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtFirstname.Text == string.Empty || txtLastname.Text == string.Empty || txtAddress.Text == string.Empty || txtAge.Text == string.Empty || txtPhone.Text == string.Empty)
                {
                    MessageBox.Show("Fill all text fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Borrower SET Firstname = @Firstname, Lastname = @Lastname, Address = @Address, Age = @Age, status = @status, username = @username, userType=@userType WHERE BorrowerId = @BorrowerId", cn))
                {
                    cmd.Parameters.AddWithValue("@BorrowerId", txtBorrowerId.Text);
                    cmd.Parameters.AddWithValue("@Firstname", txtFirstname.Text);
                    cmd.Parameters.AddWithValue("@Lastname", txtLastname.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@status", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@username", txtusername.Text);
                    cmd.Parameters.AddWithValue("@userType", comboBox2.Text);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadDatagrid();
                        }
                        else
                        {
                            MessageBox.Show("No record updated!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error updating record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

              
            }

            private void btnDelete_Click(object sender, EventArgs e)
            {
                SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
                cn.Open();
                string num = txtBorrowerId.Text;

                if (string.IsNullOrEmpty(txtBorrowerId.Text))
                {
                    MessageBox.Show("Select a record to Delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                /*// Check if there are any borrowed items for the book to be deleted
                    SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM Borrower WHERE BorrowerId = @BorrowerId", cn);
                    cmdCheck.Parameters.AddWithValue("@BorrowerId", txtBorrowerId.Text);

                    int count = (int)cmdCheck.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Cannot delete this borrower because there are borrowed items associated with it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }*/


            
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("Delete from Borrower where BorrowerId='" + num + "'", cn);
                    cmd.Parameters.AddWithValue("@BorrowerId", txtBorrowerId.Text);
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

            private void txtSearch_TextChanged(object sender, EventArgs e)
            {
                SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");

                using (cn)
                {
                    cn.Open();

                    SqlCommand cmd = new SqlCommand("Select * from Borrower where Firstname like @Firstname", cn);
                    cmd.Parameters.AddWithValue("@Firstname", "%" + txtSearch.Text + "%");

                    SqlDataAdapter adap = new SqlDataAdapter(cmd);
                    DataTable tab = new DataTable();

                    adap.Fill(tab);
                    grid2.DataSource = tab;
                }
            }

            private void button1_Click(object sender, EventArgs e)
            {
                SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\LoginForm\loginForm\Database1.mdf;Integrated Security=True");
                cn.Open();

                txtBorrowerId.Clear();
                txtFirstname.Clear();
                txtLastname.Clear();
                txtAddress.Clear();
                txtAge.Clear();
                txtPhone.Clear();
                txtSearch.Clear();
                comboBox1.Text = "";

                cn.Close();
                loadDatagrid();
            }

            private void Borrower_Load(object sender, EventArgs e)
            {
            // TODO: This line of code loads data into the 'database1DataSet6.LoginTable' table. You can move, or remove it, as needed.
            this.loginTableTableAdapter.Fill(this.database1DataSet6.LoginTable);
            loadDatagrid();
            //loadDataUsers();
            }

        private void dataGridViewUSERS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                USERS();
            }
        }
    }
    }
