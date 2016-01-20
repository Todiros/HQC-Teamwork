namespace Poker
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class AddChips : Form
    {
        public int chipsQuantity;

        public AddChips()
        {
            FontFamily fontFamily = new FontFamily("Arial");
            this.InitializeComponent();
            this.ControlBox = false;
            this.addChipsLabel.BorderStyle = BorderStyle.FixedSingle;
        }

        public void AddChipsButtonClick(object sender, EventArgs e)
        {
            int parsedValue;
            if (int.Parse(this.addChipsTextBox.Text) > 100000000)
            {
                MessageBox.Show("The maximium chips you can add is 100000000");
                return;
            }

            if (!int.TryParse(this.addChipsTextBox.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
            }
            else if (int.TryParse(this.addChipsTextBox.Text, out parsedValue)
                     && int.Parse(this.addChipsTextBox.Text) <= 100000000)
            {
                this.chipsQuantity = int.Parse(this.addChipsTextBox.Text);
                this.Close();
            }
        }

        private void AddChips_Load(object sender, EventArgs e)
        {
        }

        private void ExitButtonClick(object sender, EventArgs e)
        {
            var message = "Are you sure?";
            var title = "Quit";
            var result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    Application.Exit();
                    break;
            }
        }
    }
}