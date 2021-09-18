using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace OGaAI
{
	public partial class Form1 : Form

	{
		private bool isinprogress = false;
		private bool dragging = false;
		private Point startPoint = new Point(0, 0);
		[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

		private static extern IntPtr CreateRoundRectRgn
		 (
			  int nLeftRect,
			  int nTopRect,
			  int nRightRect,
			  int nBottomRect,
			  int nWidthEllipse,
				 int nHeightEllipse

		  );
		public Form1()
		{
			InitializeComponent();
			Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
			//Program.Main();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
            if (!Program.isAdbHere())
            {
				Program.downandinstadb();
            }
		}

		private void panel2_Paint(object sender, PaintEventArgs e)
		{

		}


		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{

			}
		}



		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			dragging = true;
			startPoint = new Point(e.X, e.Y);
		}

		private void panel1_MouseUp(object sender, MouseEventArgs e)
		{
			dragging = false;
		}

		private void panel1_MouseMove(object sender, MouseEventArgs e)
		{
			if (dragging)
			{
				Point p = PointToScreen(e.Location);
				Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);
			}
		}

        private void guna2Button1_Click(object sender, EventArgs e)
        {
			backcolor();
            if (isinprogress)
            {
				MessageBox.Show("Process is already in progress...");
            }
            else
            {
				isinprogress = true;
				OpenFileDialog ofd = new OpenFileDialog();
				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					if (Program.isFileValit(ofd.FileName))
					{
                        try
                        {
							Process proc = new Process();
							proc.StartInfo.FileName = "adb.exe";
							proc.StartInfo.UseShellExecute = false;
							proc.StartInfo.Arguments = "install " + "\"" + ofd.FileName + "\"";
							proc.StartInfo.RedirectStandardOutput = true;
							proc.StartInfo.RedirectStandardError = true;
							proc.Start();
							String output = proc.StandardOutput.ReadToEnd();
							String error = proc.StandardError.ReadToEnd();
							addline(output);
							addline(error);
							//guna2TextBox1.Text = "Installation...";
							//guna2TextBox1.Text = "If you have obb folder, dont worket to move ";
							//guna2TextBox1.Text = "it into Oculus/Android/obb/ in your headset...";
							guna2TextBox1.ForeColor = Color.Green;
							proc.WaitForExit();
							isinprogress = false;
							if(error.Contains("error: no devices/emulators found"))
                            {
								MessageBox.Show("No headset is connected!");
								guna2TextBox1.ForeColor = Color.Red;
								addline("Please connect your Oculus Quest / Quest 2 and retry");
                            }
                            if (error.Contains("Success"))
                            {
								MessageBox.Show("Your Game/Application was sucessfully installed!");
								addline("If you have obb folder, dont forge to upload it via explorer!");
                            }
						}catch(Exception er)
                        {
							guna2TextBox1.Text = er.Message;
						}
					}
					else
					{
						MessageBox.Show("Your file is invalid, please choose correct one!");
						isinprogress = false;
					}
				}
			}
		}

        private void guna2Button2_Click(object sender, EventArgs e)
        {
			this.Close();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
			this.WindowState = FormWindowState.Minimized;
		}

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
			dragging = true;
			startPoint = new Point(e.X, e.Y);
		}

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
			dragging = false;
		}

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
			if (dragging)
			{
				Point p = PointToScreen(e.Location);
				Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);
			}
		}
		public void addline(String text)
        {
			String oldtext = guna2TextBox1.Text;
			String newstring = oldtext + Environment.NewLine + text;
			guna2TextBox1.Text = newstring;

		}
		public void addline2(String text)
		{
			packagelisttextbox.Text = text;

		}
		public void backcolor()
        {
			Color def = new Color();
			def = Color.FromArgb(51, 225, 194);
			guna2TextBox1.ForeColor = def;

		}

        private void tabControl1_DrawItem(object sender,
		System.Windows.Forms.DrawItemEventArgs e)
		{

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
			backcolor();
			if (isinprogress)
			{
				MessageBox.Show("Process is already in progress...");
			}
			else
			{
				isinprogress = true;				
						try
						{
							String nameofapp = nameofappbox.Text;
							Process proc = new Process();
							proc.StartInfo.FileName = "adb.exe";
							proc.StartInfo.UseShellExecute = false;
							proc.StartInfo.Arguments = "uninstall " + nameofapp;
							proc.StartInfo.RedirectStandardOutput = true;
							proc.StartInfo.RedirectStandardError = true;
							proc.Start();
							String output = proc.StandardOutput.ReadToEnd();
							String error = proc.StandardError.ReadToEnd();
							packagelisttextbox.Text = error;
							packagelisttextbox.ForeColor = Color.Green;
							proc.WaitForExit();
							isinprogress = false;
							if (error.Contains("error: no devices/emulators found"))
							{
								MessageBox.Show("No headset is connected!");
								packagelisttextbox.ForeColor = Color.Red;
							}
							if (error.Contains("Success"))
							{
								MessageBox.Show("Your Game/Application was sucessfully uninstalled!");
							}
						}
						catch (Exception er)
						{
							guna2TextBox1.Text = er.Message;
						}
					}
			}

        private void nameofappbox_TextChanged(object sender, EventArgs e)
        {

        }

		public void changepackagelistcolortored()
        {
			packagelisttextbox.ForeColor = Color.Red;
        }

        private void seepackagesbutton_Click(object sender, EventArgs e)
        {
			Form1 f = new Form1();

			Process proc = new Process();
			proc.StartInfo.FileName = "adb.exe";
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.Arguments = "shell pm list packages -3";
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.RedirectStandardError = true;
			proc.Start();
			proc.WaitForExit();

			String error = proc.StandardOutput.ReadToEnd();

			String error2 = error.Replace("package:", "");

			packagelisttextbox.Text = error2;
			proc.WaitForExit();
		}

        private void nameofappbox_Enter(object sender, EventArgs e)
        {
			if(nameofappbox.Text == "Package name of application")
            {
				nameofappbox.Text = "";
			
            }
        }

        private void nameofappbox_Leave(object sender, EventArgs e)
        {
			if(nameofappbox.Text == "")
            {
				nameofappbox.Text = "Package name of application";

			}
        }
    }
    }

