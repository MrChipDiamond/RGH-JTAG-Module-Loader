using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JRPC_Client;
using XDevkit;

namespace Module_Loader
{
    public partial class Form1 : Form
    {
        private IXboxConsole jtag;

        public Form1()
        {
            InitializeComponent();
            this.FormClosed += MainForm_FormClosed;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            try
            {
                jtag.CloseConnection(1U);
            }
            catch
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
            foreach (object obj in jtag.DebugTarget.Modules)
            {
                IXboxModule xboxModule = (IXboxModule)obj;
                string name = xboxModule.ModuleInfo.Name;
                string str = "0x";
                XBOX_MODULE_INFO moduleInfo = xboxModule.ModuleInfo;
                string text = str + moduleInfo.BaseAddress.ToString("X");
                string str2 = "0x";
                moduleInfo = xboxModule.ModuleInfo;
                string text2 = str2 + moduleInfo.Size.ToString("X");
                DataGridViewRowCollection rows = this.dataGridView1.Rows;
                object[] array = new object[4];
                array[0] = name;
                array[1] = text;
                array[2] = text2;
                rows.Add(array);
            }
            this.dataGridView1.Sort(this.dataGridView1.Columns[1], ListSortDirection.Ascending);
            this.toolStripStatusLabel5.Text = this.dataGridView1.Rows.Count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string text = "*" + Path.GetExtension(openFileDialog.FileName);
            openFileDialog.Filter = text + "|" + text;
            openFileDialog.FileName = Path.GetFileName(openFileDialog.FileName);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                jtag.SendFile(openFileDialog.FileName, "Hdd:\\" + Path.GetFileName(openFileDialog.FileName));
                try
                {
                    jtag.Call<uint>("xboxkrnl.exe", 409, new object[]
                    {
        "Hdd:\\" + Path.GetFileName(openFileDialog.FileName),
        8,
        0,
        0
                    });
                    this.button1_Click(sender, null);
                    toolStripStatusLabel2.ForeColor = Color.Green;
                    toolStripStatusLabel2.Text = "Module Injected!";
                }
                catch (COMException comEx)
                {
                    toolStripStatusLabel2.ForeColor = Color.Red;
                    toolStripStatusLabel2.Text = $"Failed to inject module. COM Error: {comEx.Message}, HResult: {comEx.HResult:X}";
                }
                catch (Exception ex)
                {
                    toolStripStatusLabel2.ForeColor = Color.Red;
                    toolStripStatusLabel2.Text = $"Failed to inject module. Error: {ex.Message}";
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (jtag.Connect(out jtag, textBox1.Text))
            {
                    jtag.DebugTarget.ConnectAsDebugger("jtag", XboxDebugConnectFlags.Force);
                    this.button1_Click(sender, null);
                    toolStripStatusLabel2.ForeColor = Color.Green;
                    toolStripStatusLabel2.Text = "Connected to " + textBox1.Text;
            }
            else
            {
                toolStripStatusLabel2.ForeColor = Color.Red;
                toolStripStatusLabel2.Text = ("No connection to your console was found! Try again.");
            }
        }
        private byte[] byte_0;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                DialogResult dialogResult = MessageBox.Show("This spoofs your current title id to dashboard to allow some modules to load in-game even though they would normally only load on dashboard. This might cause a crash for some modules. Are you sure you want to keep it enabled?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult != DialogResult.No)
                {
                    this.byte_0 = jtag.GetMemory(2171470776U, 12U);
                    byte[] array = new byte[12];
                    Buffer.BlockCopy(BitConverter.GetBytes(1012989950U).Reverse<byte>().ToArray<byte>(), 0, array, 0, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(1617102801U).Reverse<byte>().ToArray<byte>(), 0, array, 4, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(1317011488).Reverse<byte>().ToArray<byte>(), 0, array, 8, 4);
                    jtag.SetMemory(2171470776U, array);
                    toolStripStatusLabel2.ForeColor = Color.Green;
                    toolStripStatusLabel2.Text=("Spoofed As Dashboard!");
                }
                else
                {
                    this.checkBox1.Checked = false;
                }
            }
            else
            {
                jtag.SetMemory(2171470776U, this.byte_0);
                toolStripStatusLabel2.ForeColor = Color.Green;
                toolStripStatusLabel2.Text = ("Undid Spoofing! - Title ID : " + jtag.XamGetCurrentTitleId().ToString("X"));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                jtag.Call<uint>("xboxkrnl.exe", 409, new object[]
                {
                    this.textBox2.Text,
                    8,
                    0,
                    0
                });
                this.button1_Click(sender, null);
                toolStripStatusLabel2.ForeColor = Color.Green;
                toolStripStatusLabel2.Text = ("Succesfully injected module!");
            }
            catch (Exception)
            {
                toolStripStatusLabel2.ForeColor = Color.Red;
                toolStripStatusLabel2.Text = ("Failed to inject module, maybe the module you want to inject does not support it.");
            }
        }
        private void method_0(string string_0, bool bool_0)
        {
            uint num = this.method_1(string_0);
            if (num > 0U)
            {
                if (bool_0)
                {
                    jtag.WriteInt16(num + 64U, 1);
                }
                object[] arguments = new object[]
                {
                    num
                };
                jtag.CallVoid("xboxkrnl.exe", 417, arguments);
            }
        }
        private uint method_1(string string_0)
        {
            object[] arguments = new object[]
            {
                string_0
            };
            return jtag.Call<uint>("xam.xex", 1102, arguments);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                this.method_0(this.textBox3.Text, true);
                this.button1_Click(sender, null);
                toolStripStatusLabel2.ForeColor = Color.Green;
                toolStripStatusLabel2.Text = ("Succesfully unloaded module!");
            }
            catch (Exception)
            {
                toolStripStatusLabel2.ForeColor = Color.Red;
                toolStripStatusLabel2.Text = ("Failed to unload module, maybe the module you want to unload does not support it.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView1.Rows.Clear();
                jtag.Reboot(null, null, null, XboxRebootFlags.Cold);
                toolStripStatusLabel2.ForeColor = Color.OrangeRed;
                toolStripStatusLabel2.Text = ("Console Rebooted! Make sure to reconnect before attempting to load/ unload modules.");
                this.toolStripStatusLabel5.Text = "Unknown";
            }
            catch
            {
                this.dataGridView1.Rows.Clear();
                toolStripStatusLabel2.ForeColor = Color.Red;
                toolStripStatusLabel2.Text = ("Restart failed! Try manually rebooting...");
                this.toolStripStatusLabel5.Text = "Unknown";
            }
            
        }
    }
}
