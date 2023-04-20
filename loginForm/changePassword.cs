using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace loginForm
{
    public partial class changePassword : Form
    {
        public changePassword()
        {
            InitializeComponent();
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True");
        }

       

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mariloucantilado\source\repos\Loginform\loginForm\Database1.mdf;Integrated Security=True");

            string encryp = txtpass.Text;
            if (!string.IsNullOrEmpty(txtuser.Text) && !string.IsNullOrEmpty(txtpass.Text) && !string.IsNullOrEmpty(txtConfirmpass.Text))
            {
                cn.Open();

                using (cn)
                {
                    try
                    {
                        if (txtpass.Text != txtConfirmpass.Text)
                        {
                            MessageBox.Show("Looks like confirm password and password didn't match, match it.");
                        }
                        else
                        {
                            SqlCommand cmd = new SqlCommand("SELECT username, password, userType FROM LoginTable WHERE username = @username", cn);
                            cmd.Parameters.AddWithValue("@username", txtuser.Text);
                            cmd.Parameters.AddWithValue("@password", txtpass.Text);
                            SqlDataReader reader = cmd.ExecuteReader();

                            if (reader.Read())
                            {
                                
                                encryp = "";
                                foreach (char c in txtpass.Text)
                                {
                                    int asciValue = (int)c;
                                    asciValue += 2;
                                    encryp += (char)asciValue;
                                }
                                string encryptedPassword = reader.GetString(1);
                                reader.Close();
                                cmd = new SqlCommand("UPDATE LoginTable SET password = @password WHERE username = @username", cn);
                                cmd.Parameters.AddWithValue("@username", txtuser.Text);
                                cmd.Parameters.AddWithValue("@password", encryp);

                                cmd.ExecuteNonQuery();
                                MessageBox.Show("password updated successful! Now click login");
                                
                            }
                            else
                            {
                                
                                MessageBox.Show("Error: ");
                            }
                            
                        }

                    }
                    catch (Exception ex)
                    {
                        
                        MessageBox.Show("Error: " + ex.Message);
                    }

                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields ");
            }
        }
    }

}

