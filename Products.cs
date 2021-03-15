using Microsoft.Data.SqlClient;
using ProductLibrary.business_rule;
using ProductLibrary.models.models;
using ProductLibrary.views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment3
{
    public partial class Form1 : Form, IProductsView
    {

        public Form1()
        {
            InitializeComponent();
            presenter = new ProductLibrary.presenters.ProductPresenter(this);
        }

        ProductLibrary.presenters.ProductPresenter presenter;
        BindingSource bs;

        public int Quantity { get => int.Parse(txtQuantity.Text); set => txtQuantity.Text = value.ToString(); }
        public float UnitPrice { get => float.Parse(txtUnitPrice.Text);  set => txtUnitPrice.Text = value.ToString(); }
        int IProductsView.ProductID { get => int.Parse(txtProductID.Text); }
        string IProductsView.ProductName { get => txtProductName.Text; }
        public List<Product> products { get => convertGridToList(); set => convertListToGrid(value); }
        public float SubTotal { set =>  txtSubTotal.Text = value.ToString();  }

        private List<Product> convertGridToList()
        {
            List<Product> list = new List<Product>();
            foreach(DataGridViewRow row in dgvProducts.Rows)
            {
                int id = (int) row.Cells[0].Value;
                string name = (string)row.Cells[1].Value;
                int quantity = (int)row.Cells[2].Value;
                float price = (float)row.Cells[3].Value;
                Product p = new Product(id, name, quantity, price);
                list.Add(p);
            }
            return list;
        }
        private void convertListToGrid(List<Product> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID",typeof(int));
            dt.Columns.Add("ProductName",typeof(string));
            dt.Columns.Add("Quantity",typeof(int));
            dt.Columns.Add("UnitPrice",typeof(float));
            dt.Columns.Add("SubTotal",typeof(float));

            foreach (var p in list)
            {
                dt.Rows.Add(new Object[] { p.ProductID, p.ProductName, p.Quantity, p.UnitPrice, p.SubTotal });
            }
            bs.DataSource = dt;
            dgvProducts.DataSource = bs;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProductValidation valid = new ProductValidation(txtProductID.Text,txtProductName.Text,txtQuantity.Text,txtUnitPrice.Text);
            List<string> errors = valid.Errors;
            if (errors.Count > 0)
            {
                string message = "";
                foreach (string mes in errors)
                {
                    message += mes + "\n";
                }
                MessageBox.Show(message);
            }
            else {
                int max = (int) ((DataTable)bs.DataSource).Compute("Max(ProductID)", string.Empty) + 1;
                bool result = presenter.Insert(max);
                if (result)
                {
                    MessageBox.Show("Insert success.");
                    loadData();
                }
                else
                {
                    MessageBox.Show("Insert fail");
                }
            }
                    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProductValidation valid = new ProductValidation(txtProductID.Text, txtProductName.Text, txtQuantity.Text, txtUnitPrice.Text);
            List<string> errors = valid.Errors;
            if (errors.Count > 0)
            {
                string message = "";
                foreach (string mes in errors)
                {
                    message += mes + "\n";
                }
                MessageBox.Show(message);
            }
            else
            {
                bool result = presenter.Update();
                if (result)
                {
                    MessageBox.Show("Update success.");
                    loadData();
                }
                else
                {
                    MessageBox.Show("Update fail");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtProductID.Text);
                bool result = presenter.Delete();
                if (result)
                {
                    MessageBox.Show("Delete success.");
                    loadData();
                }
                else
                {
                    MessageBox.Show("Delete fail");
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("ProductID isn't format of number.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Product p = presenter.Find();
            FormDetail detail = new FormDetail(p,this);
            detail.Visible = true;
            this.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadData();
        }
        private void loadData()
        {
            bs = new BindingSource();
            presenter.loadInit();

            txtProductID.DataBindings.Clear();
            txtProductName.DataBindings.Clear();
            txtQuantity.DataBindings.Clear();
            txtUnitPrice.DataBindings.Clear();
            txtSubTotal.DataBindings.Clear();

            txtProductID.DataBindings.Add("Text", bs, "ProductID");
            txtProductName.DataBindings.Add("Text", bs, "ProductName");
            txtQuantity.DataBindings.Add("Text", bs, "Quantity");
            txtUnitPrice.DataBindings.Add("Text", bs, "UnitPrice");
            txtSubTotal.DataBindings.Add("Text", bs, "SubTotal");

            bs.Sort = "ProductID DESC";
        }
    }
}
