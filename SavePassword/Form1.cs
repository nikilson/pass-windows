using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SavePassword
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string password = GetRandomPassword(14);
            Clipboard.SetText(password);
            textBox2.ResetText();
            textBox3.ResetText();
            textBox2.AppendText(password);
            textBox3.AppendText(password);

        }
        public static string GetRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyz!@#$%^&*()ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string finalString = "";
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Please Provide a Title", "Invalid Title", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("Please Provide a Valid Password", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!String.Equals(textBox2.Text, textBox3.Text)){
                MessageBox.Show("Your Passwords doesn't match!!", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!File.Exists("C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe"))
            {
                MessageBox.Show("Please install gpg for windows in C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe", "GPG Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                finalString += textBox2.Text;
                if (textBox4.Text.Length != 0)
                {
                    finalString += "\n" + textBox4.Text ;
                }
                if (textBox5.Text.Length != 0)
                {
                    finalString += "\n" + textBox5.Text;
                }
                Clipboard.SetText(finalString);

                /// Getting the User Directory
                string home = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    home = Directory.GetParent(home).ToString();
                }
                string passwordDirectory = Path.Combine(home, ".password-store");
                string keyIdFileName = Path.Combine(passwordDirectory, Path.GetFileName(".gpg-id"));

                string keyId = File.ReadAllText(keyIdFileName).TrimEnd('\n', '\r');
                string passwordFile = Path.Combine(passwordDirectory, textBox1.Text);
                if (File.Exists(passwordFile + ".gpg"))
                {
                    MessageBox.Show("Please Change the title the following passwords exists!!", "Password Alread Exists!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    File.WriteAllText(passwordFile, finalString);
                    /// string command = "gpg -r " + keyId + " -e " + passwordFile;
                    Process process = new Process();
                    process.StartInfo.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.Arguments = "-r " + keyId + " --always-trust" + " -e " + passwordFile;
                    process.Start();
                    process.WaitForExit();
                    File.Delete(passwordFile);
                    MessageBox.Show("Your Password has been saved successfully!!", "Password Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!File.Exists("C:\\Program Files\\Git\\bin\\git.exe"))
            {
                MessageBox.Show("Please install Git for windows in C:\\Program Files\\Git\\bin\\git.exe", "Git Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string home = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    home = Directory.GetParent(home).ToString();
                }
                string passwordDirectory = Path.Combine(home, ".password-store");

                Process process_0 = new Process();
                process_0.StartInfo.FileName = "C:\\Program Files\\Git\\bin\\git.exe";
                process_0.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process_0.StartInfo.RedirectStandardOutput = true;
                process_0.StartInfo.RedirectStandardError = true;
                process_0.StartInfo.UseShellExecute = false;
                process_0.StartInfo.CreateNoWindow = true;
                // String time = DateTime.Now.ToString("HH:mm:ss tt");
                process_0.StartInfo.Arguments = " -C " + passwordDirectory + " add -A";
                process_0.Start();
                process_0.WaitForExit();

                Process process_1 = new Process();
                process_1.StartInfo.FileName = "C:\\Program Files\\Git\\bin\\git.exe";
                process_1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process_1.StartInfo.RedirectStandardOutput = true;
                process_1.StartInfo.RedirectStandardError = true;
                process_1.StartInfo.UseShellExecute = false;
                process_1.StartInfo.CreateNoWindow = true;
                // String time = DateTime.Now.ToString("HH:mm:ss tt");
                process_1.StartInfo.Arguments = " -C " + passwordDirectory + " commit -m \"Passwords updated\"";
                process_1.Start();
                process_1.WaitForExit();
                Process process_2 = new Process();
                process_2.StartInfo.FileName = "C:\\Program Files\\Git\\bin\\git.exe";
                process_2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process_2.StartInfo.RedirectStandardOutput = true;
                process_2.StartInfo.RedirectStandardError = true;
                process_2.StartInfo.UseShellExecute = false;
                process_2.StartInfo.CreateNoWindow = true;
                process_2.StartInfo.Arguments = " -C " + passwordDirectory + " push";
                process_2.Start();
                process_2.WaitForExit();
                MessageBox.Show("Passwords would be sent, If the repo is set, and authenticates properly", "Check the Remote", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
