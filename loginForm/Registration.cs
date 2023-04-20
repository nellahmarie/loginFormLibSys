using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace loginForm
{
    public partial class Registration : Form
    {
#pragma warning disable CS0169 // The field 'Registration.cmd' is never used
        SqlCommand cmd;
#pragma warning restore CS0169 // The field 'Registration.cmd' is never used
        public string encryp { get; set; }
        public string type { get; set; }
        public Registration()
        {
            InitializeComponent();
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\loginForm\loginForm\Database1.mdf;Integrated Security=True");
            cn.Open();
        }

        public SqlConnection cn { get; private set; }
        private void login_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.ShowDialog();
        }

        private void Register_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textuser.Text) && !string.IsNullOrEmpty(textpass.Text) && !string.IsNullOrEmpty(textConfirmpass.Text))
            {
                using (cn)
                {
                    if (textpass.Text != textConfirmpass.Text)
                    {
                        MessageBox.Show("Looks like confirm password and password didn't match, match it.");
                    }
                    else
                    {
                        cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\loginForm\loginForm\Database1.mdf;Integrated Security=True");
                        cn.Open();

                        // Check if the user already exists in the database
                        SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM LoginTable WHERE username=@username", cn);
                        checkCmd.Parameters.AddWithValue("@username", textuser.Text);
                        int userCount = (int)checkCmd.ExecuteScalar();


                        if (userCount > 0)
                        {
                            MessageBox.Show("This user is already exist");
                        }
                        else
                        {
                            type = comboBox1.SelectedItem.ToString();
                            encryp = "";

                            foreach (char c in textpass.Text)
                            {
                                int asciValue = (int)c;
                                asciValue += 2;
                                encryp += (char)asciValue;
                            }

                            SqlCommand cmd = new SqlCommand("INSERT INTO LoginTable (username, password, userType) VALUES (@username, @userpassword, @userType)", cn);
                            cmd.Parameters.AddWithValue("@username", textuser.Text);
                            cmd.Parameters.AddWithValue("@userpassword", encryp);
                            cmd.Parameters.AddWithValue("@userType", type);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Registration successful! Now click login");
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("Please enter a value in both fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void Registration_Load(object sender, EventArgs e)
        {
           
           
        }

        private void textpass_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
