using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OS_Lab7
{
    public partial class Form1 : Form
    {
        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowText(int hWnd, string text);


        public Form1()
        {
            InitializeComponent();
        }

        string GetWindowText(IntPtr hWnd)
        {
            int len = GetWindowTextLength(hWnd) + 1;
            StringBuilder sb = new StringBuilder(len);
            len = GetWindowText(hWnd, sb, len);
            return sb.ToString(0, len);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            EnumWindows((hWnd, lParam) =>
            {
                if (IsWindowVisible(hWnd) && GetWindowTextLength(hWnd) != 0)
                {
                    listBox1.Items.Add(hWnd + ", " + GetWindowText(hWnd));
                }
                return true;
            }, IntPtr.Zero);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                string Name = textBox1.Text;
                if (listBox1.SelectedIndex != -1)
                {
                    int hWnd = GetWindowHandle();
                    if (hWnd != 0)
                    {
                        SetWindowText(hWnd, Name);
                    }
                }
            }
            else
            {
                MessageBox.Show("Введите название");
            }
        }

        private int GetWindowHandle()
        {
            string selectedItem = listBox1.SelectedItem.ToString();
            string handleString = selectedItem.Substring(0, selectedItem.IndexOf(',')); ;
            if (int.TryParse(handleString, out int hWnd))
            {
                return hWnd;
            }
            return 0;
        }


    }
}