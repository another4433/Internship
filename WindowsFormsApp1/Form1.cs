using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string theText = "Click Me!";
        int theNumber = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (theNumber != 0)
            {
                listView1.Items.Add(new ListViewItem(new[] { theText, theNumber.ToString(CultureInfo.InvariantCulture) }));
                listBox1.Items.Add(theText + " - " + theNumber.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                MessageBox.Show("Failed to receive data from user! Contact Admin.");
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Select();
            listBox1.Select();
            MessageBox.Show("You selected: " + listBox1.Items[listView1.SelectedIndices[0]].ToString());
        }
        private void name_Click(object sender, EventArgs e)
        {
            TheName = textBox1.Text;
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString(CultureInfo.InvariantCulture);
            TheAge = trackBar1.Value;
        }
        private int TheAge
        {
            get
            {
                return theNumber;
            }
            set
            {
                theNumber = value;
            }
        }
        private string TheName
        {
            get
            {
                return theText;
            }
            set
            {
                theText = value;
            }
        }
    }
}
