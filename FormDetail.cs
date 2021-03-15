using ProductLibrary.models.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Assignment3
{
    public partial class FormDetail : Form
    {
        Product p;
        Form form;
        public FormDetail()
        {
            InitializeComponent();
        }
        public FormDetail(Product p,Form form)
        {
            InitializeComponent();
            this.form = form;
            this.p = p;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string s = "";
            this.Close();
        }

        private void FormDetail_Load(object sender, EventArgs e)
        {
            txtProductID.Text = p.ProductID.ToString();
            txtProductName.Text = p.ProductName;
            txtQuantity.Text = p.Quantity.ToString();
            txtUnitPrice.Text = p.UnitPrice.ToString();
            txtSubTotal.Text = p.SubTotal.ToString();
        }

        private void FormDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.Visible = true;
        }
    }
}
