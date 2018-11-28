namespace CSC430_Payroll
{
    partial class FormCreatePlan
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Create = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.benefitErrorLabel = new System.Windows.Forms.Label();
            this.planErrorLabel = new System.Windows.Forms.Label();
            this.payTypeErrorLabel = new System.Windows.Forms.Label();
            this.rateErrorLabel = new System.Windows.Forms.Label();
            this.checkBoxRate = new System.Windows.Forms.CheckBox();
            this.checkBoxFixed = new System.Windows.Forms.CheckBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.fixedAmtErrorLabel = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Benefit ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enter Plan Name";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(15, 74);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(149, 20);
            this.textBox2.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Payment Type";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(15, 189);
            this.textBox3.MaxLength = 2;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(96, 20);
            this.textBox3.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Rate (%)";
            // 
            // Create
            // 
            this.Create.Location = new System.Drawing.Point(173, 270);
            this.Create.Name = "Create";
            this.Create.Size = new System.Drawing.Size(75, 23);
            this.Create.TabIndex = 10;
            this.Create.Text = "Create";
            this.Create.UseVisualStyleBackColor = true;
            this.Create.Click += new System.EventHandler(this.Create_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(262, 270);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 11;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // benefitErrorLabel
            // 
            this.benefitErrorLabel.AutoSize = true;
            this.benefitErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.benefitErrorLabel.Location = new System.Drawing.Point(170, 28);
            this.benefitErrorLabel.Name = "benefitErrorLabel";
            this.benefitErrorLabel.Size = new System.Drawing.Size(25, 13);
            this.benefitErrorLabel.TabIndex = 12;
            this.benefitErrorLabel.Text = "      ";
            // 
            // planErrorLabel
            // 
            this.planErrorLabel.AutoSize = true;
            this.planErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.planErrorLabel.Location = new System.Drawing.Point(170, 77);
            this.planErrorLabel.Name = "planErrorLabel";
            this.planErrorLabel.Size = new System.Drawing.Size(31, 13);
            this.planErrorLabel.TabIndex = 13;
            this.planErrorLabel.Text = "        ";
            // 
            // payTypeErrorLabel
            // 
            this.payTypeErrorLabel.AutoSize = true;
            this.payTypeErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.payTypeErrorLabel.Location = new System.Drawing.Point(117, 131);
            this.payTypeErrorLabel.Name = "payTypeErrorLabel";
            this.payTypeErrorLabel.Size = new System.Drawing.Size(31, 13);
            this.payTypeErrorLabel.TabIndex = 14;
            this.payTypeErrorLabel.Text = "        ";
            // 
            // rateErrorLabel
            // 
            this.rateErrorLabel.AutoSize = true;
            this.rateErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.rateErrorLabel.Location = new System.Drawing.Point(118, 192);
            this.rateErrorLabel.Name = "rateErrorLabel";
            this.rateErrorLabel.Size = new System.Drawing.Size(31, 13);
            this.rateErrorLabel.TabIndex = 15;
            this.rateErrorLabel.Text = "        ";
            // 
            // checkBoxRate
            // 
            this.checkBoxRate.AutoSize = true;
            this.checkBoxRate.Location = new System.Drawing.Point(15, 127);
            this.checkBoxRate.Name = "checkBoxRate";
            this.checkBoxRate.Size = new System.Drawing.Size(49, 17);
            this.checkBoxRate.TabIndex = 16;
            this.checkBoxRate.Text = "Rate";
            this.checkBoxRate.UseVisualStyleBackColor = true;
            this.checkBoxRate.CheckedChanged += new System.EventHandler(this.checkBoxRate_CheckedChanged);
            // 
            // checkBoxFixed
            // 
            this.checkBoxFixed.AutoSize = true;
            this.checkBoxFixed.Location = new System.Drawing.Point(15, 150);
            this.checkBoxFixed.Name = "checkBoxFixed";
            this.checkBoxFixed.Size = new System.Drawing.Size(90, 17);
            this.checkBoxFixed.TabIndex = 17;
            this.checkBoxFixed.Text = "Fixed Amount";
            this.checkBoxFixed.UseVisualStyleBackColor = true;
            this.checkBoxFixed.CheckedChanged += new System.EventHandler(this.checkBoxFixed_CheckedChanged);
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(15, 231);
            this.textBox4.MaxLength = 15;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(96, 20);
            this.textBox4.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Fixed Amount ($)";
            // 
            // fixedAmtErrorLabel
            // 
            this.fixedAmtErrorLabel.AutoSize = true;
            this.fixedAmtErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.fixedAmtErrorLabel.Location = new System.Drawing.Point(118, 237);
            this.fixedAmtErrorLabel.Name = "fixedAmtErrorLabel";
            this.fixedAmtErrorLabel.Size = new System.Drawing.Size(31, 13);
            this.fixedAmtErrorLabel.TabIndex = 19;
            this.fixedAmtErrorLabel.Text = "        ";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(15, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(149, 21);
            this.comboBox1.TabIndex = 20;
            // 
            // FormCreatePlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 305);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.fixedAmtErrorLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.checkBoxFixed);
            this.Controls.Add(this.checkBoxRate);
            this.Controls.Add(this.rateErrorLabel);
            this.Controls.Add(this.payTypeErrorLabel);
            this.Controls.Add(this.planErrorLabel);
            this.Controls.Add(this.benefitErrorLabel);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Create);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(365, 344);
            this.MinimumSize = new System.Drawing.Size(365, 344);
            this.Name = "FormCreatePlan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create a Plan";
            this.Load += new System.EventHandler(this.FormCreateBenefit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Create;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label benefitErrorLabel;
        private System.Windows.Forms.Label planErrorLabel;
        private System.Windows.Forms.Label payTypeErrorLabel;
        private System.Windows.Forms.Label rateErrorLabel;
        private System.Windows.Forms.CheckBox checkBoxRate;
        private System.Windows.Forms.CheckBox checkBoxFixed;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label fixedAmtErrorLabel;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}