namespace CSC430_Payroll
{
    partial class FormCreateModifier
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radioCredit = new System.Windows.Forms.RadioButton();
            this.radioDeduction = new System.Windows.Forms.RadioButton();
            this.Create = new System.Windows.Forms.Button();
            this.benefitErrorLabel = new System.Windows.Forms.Label();
            this.planErrorLabel = new System.Windows.Forms.Label();
            this.radioErrorLabel = new System.Windows.Forms.Label();
            this.nameErrorLabel = new System.Windows.Forms.Label();
            this.amountErrorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Enabled = false;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(152, 25);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Benefit";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(149, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Plan";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 107);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 156);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Amount";
            // 
            // radioCredit
            // 
            this.radioCredit.AutoSize = true;
            this.radioCredit.Location = new System.Drawing.Point(12, 61);
            this.radioCredit.Name = "radioCredit";
            this.radioCredit.Size = new System.Drawing.Size(52, 17);
            this.radioCredit.TabIndex = 8;
            this.radioCredit.Text = "Credit";
            this.radioCredit.UseVisualStyleBackColor = true;
            // 
            // radioDeduction
            // 
            this.radioDeduction.AutoSize = true;
            this.radioDeduction.Location = new System.Drawing.Point(70, 61);
            this.radioDeduction.Name = "radioDeduction";
            this.radioDeduction.Size = new System.Drawing.Size(74, 17);
            this.radioDeduction.TabIndex = 9;
            this.radioDeduction.Text = "Deduction";
            this.radioDeduction.UseVisualStyleBackColor = true;
            // 
            // Create
            // 
            this.Create.Location = new System.Drawing.Point(198, 184);
            this.Create.Name = "Create";
            this.Create.Size = new System.Drawing.Size(75, 23);
            this.Create.TabIndex = 10;
            this.Create.Text = "Create";
            this.Create.UseVisualStyleBackColor = true;
            this.Create.Click += new System.EventHandler(this.Create_Click);
            // 
            // benefitErrorLabel
            // 
            this.benefitErrorLabel.AutoSize = true;
            this.benefitErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.benefitErrorLabel.Location = new System.Drawing.Point(56, 9);
            this.benefitErrorLabel.Name = "benefitErrorLabel";
            this.benefitErrorLabel.Size = new System.Drawing.Size(10, 13);
            this.benefitErrorLabel.TabIndex = 12;
            this.benefitErrorLabel.Text = " ";
            // 
            // planErrorLabel
            // 
            this.planErrorLabel.AutoSize = true;
            this.planErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.planErrorLabel.Location = new System.Drawing.Point(183, 9);
            this.planErrorLabel.Name = "planErrorLabel";
            this.planErrorLabel.Size = new System.Drawing.Size(10, 13);
            this.planErrorLabel.TabIndex = 13;
            this.planErrorLabel.Text = " ";
            // 
            // radioErrorLabel
            // 
            this.radioErrorLabel.AutoSize = true;
            this.radioErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.radioErrorLabel.Location = new System.Drawing.Point(142, 65);
            this.radioErrorLabel.Name = "radioErrorLabel";
            this.radioErrorLabel.Size = new System.Drawing.Size(10, 13);
            this.radioErrorLabel.TabIndex = 14;
            this.radioErrorLabel.Text = " ";
            // 
            // nameErrorLabel
            // 
            this.nameErrorLabel.AutoSize = true;
            this.nameErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.nameErrorLabel.Location = new System.Drawing.Point(119, 113);
            this.nameErrorLabel.Name = "nameErrorLabel";
            this.nameErrorLabel.Size = new System.Drawing.Size(10, 13);
            this.nameErrorLabel.TabIndex = 15;
            this.nameErrorLabel.Text = " ";
            // 
            // amountErrorLabel
            // 
            this.amountErrorLabel.AutoSize = true;
            this.amountErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.amountErrorLabel.Location = new System.Drawing.Point(119, 162);
            this.amountErrorLabel.Name = "amountErrorLabel";
            this.amountErrorLabel.Size = new System.Drawing.Size(10, 13);
            this.amountErrorLabel.TabIndex = 16;
            this.amountErrorLabel.Text = " ";
            // 
            // CreateCreditOrDeduc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 219);
            this.Controls.Add(this.amountErrorLabel);
            this.Controls.Add(this.nameErrorLabel);
            this.Controls.Add(this.radioErrorLabel);
            this.Controls.Add(this.planErrorLabel);
            this.Controls.Add(this.benefitErrorLabel);
            this.Controls.Add(this.Create);
            this.Controls.Add(this.radioDeduction);
            this.Controls.Add(this.radioCredit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.MaximumSize = new System.Drawing.Size(301, 258);
            this.MinimumSize = new System.Drawing.Size(301, 258);
            this.Name = "CreateCreditOrDeduc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CreateCreditOrDeduc";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioCredit;
        private System.Windows.Forms.RadioButton radioDeduction;
        private System.Windows.Forms.Button Create;
        private System.Windows.Forms.Label benefitErrorLabel;
        private System.Windows.Forms.Label planErrorLabel;
        private System.Windows.Forms.Label radioErrorLabel;
        private System.Windows.Forms.Label nameErrorLabel;
        private System.Windows.Forms.Label amountErrorLabel;
    }
}