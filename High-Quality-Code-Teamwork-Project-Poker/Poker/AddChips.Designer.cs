namespace Poker
{
    partial class AddChips
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addChipsLabel = new System.Windows.Forms.Label();
            this.addChipsButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.addChipsTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // addChipsLabel
            // 
            this.addChipsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addChipsLabel.Location = new System.Drawing.Point(48, 49);
            this.addChipsLabel.Name = "addChipsLabel";
            this.addChipsLabel.Size = new System.Drawing.Size(176, 23);
            this.addChipsLabel.TabIndex = 0;
            this.addChipsLabel.Text = "You ran out of chips !";
            this.addChipsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // addChipsButton
            // 
            this.addChipsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addChipsButton.Location = new System.Drawing.Point(12, 226);
            this.addChipsButton.Name = "addChipsButton";
            this.addChipsButton.Size = new System.Drawing.Size(75, 23);
            this.addChipsButton.TabIndex = 1;
            this.addChipsButton.Text = "Add Chips";
            this.addChipsButton.UseVisualStyleBackColor = true;
            this.addChipsButton.Click += new System.EventHandler(this.AddChipsButtonClick);
            // 
            // exitButton
            // 
            this.exitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.exitButton.Location = new System.Drawing.Point(197, 226);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButtonClick);
            // 
            // addChipsTextBox
            // 
            this.addChipsTextBox.Location = new System.Drawing.Point(91, 229);
            this.addChipsTextBox.Name = "addChipsTextBox";
            this.addChipsTextBox.Size = new System.Drawing.Size(100, 20);
            this.addChipsTextBox.TabIndex = 3;
            // 
            // AddChips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.addChipsTextBox);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.addChipsButton);
            this.Controls.Add(this.addChipsLabel);
            this.Name = "AddChips";
            this.Text = "You Ran Out Of Chips";
            this.Load += new System.EventHandler(this.AddChips_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label addChipsLabel;
        private System.Windows.Forms.Button addChipsButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.TextBox addChipsTextBox;
    }
}