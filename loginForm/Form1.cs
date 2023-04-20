using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace loginForm
{
    public partial class Form1 : Form
    {
#pragma warning disable CS0169 // The field 'Form1.cmd' is never used
        SqlCommand cmd;
#pragma warning restore CS0169 // The field 'Form1.cmd' is never used
        SqlConnection cn;
        public Form1()
        {
            InitializeComponent();
        }


        private void Login_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True"))
            {
                cn.Open();
                string password = txtPassword.Text;
                string username = txtUsername.Text;

                SqlCommand cmd = new SqlCommand("SELECT username, password, userType FROM LoginTable WHERE username = @username", cn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
          
                
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    try
                    {
                        
                        string encryptedPassword = reader.GetString(1);
                        string decryptedPassword = DecryptPassword(encryptedPassword);

                        if (decryptedPassword == password)
                        {
                            string userType = reader.GetString(2);
                            if(userType == "Admin")
                            {
                                MessageBox.Show("Login successful!");
                                this.Hide();
                                Menu adminForm = new Menu();
                                adminForm.ShowDialog();
                            }
                            else if(userType == "Borrower")
                            {
                                MessageBox.Show("Login successful!");
                                this.Hide();
                                HomeBorrower borrowerForm = new HomeBorrower();
                                borrowerForm.ShowDialog();
                            }
                                                      

                        }
                        else
                        {
                            MessageBox.Show("Invalid password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);         
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string DecryptPassword(string encryptedPassword)
        {
            string decryptedPassword = "";
            foreach (char c in encryptedPassword)
            {
                // Shift each character by 2 to the left in the ASCII table
                decryptedPassword += (char)(c - 2);
            }
            return decryptedPassword;
        }

   

        private void Form1_Load(object sender, EventArgs e)
        {
            cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\loginForm\loginForm\Database1.mdf;Integrated Security=True");

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registration registration = new Registration();
            registration.ShowDialog();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            changePassword change = new changePassword();
            change.Show();
        }
    }
}
