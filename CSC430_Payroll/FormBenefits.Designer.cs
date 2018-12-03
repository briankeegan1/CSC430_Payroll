namespace CSC430_Payroll
{
    partial class FormBenefits
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
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.CreateBenefit = new System.Windows.Forms.Button();
            this.DeleteBenefit = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.benefitTextBox = new System.Windows.Forms.TextBox();
            this.planTextBox = new System.Windows.Forms.TextBox();
            this.rateTextBox = new System.Windows.Forms.TextBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.ModifyInfo = new System.Windows.Forms.Button();
            this.DeletePlan = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fixedTextBox = new System.Windows.Forms.TextBox();
            this.modifierTextBox = new System.Windows.Forms.TextBox();
            this.modAmtTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CreatePlan = new System.Windows.Forms.Button();
            this.CreateModifier = new System.Windows.Forms.Button();
            this.DeleteModifier = new System.Windows.Forms.Button();
            this.benefitErrorLabel = new System.Windows.Forms.Label();
            this.planErrorLabel = new System.Windows.Forms.Label();
            this.rateErrorLabel = new System.Windows.Forms.Label();
            this.fixedErrorLabel = new System.Windows.Forms.Label();
            this.modifierErrorLabel = new System.Windows.Forms.Label();
            this.modAmtErrorLabel = new System.Windows.Forms.Label();
            this.radioRate = new System.Windows.Forms.RadioButton();
            this.radioFixed = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Benefits";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 27);
            this.listBox1.MaximumSize = new System.Drawing.Size(109, 199);
            this.listBox1.MinimumSize = new System.Drawing.Size(109, 199);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(109, 199);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // CreateBenefit
            // 
            this.CreateBenefit.Location = new System.Drawing.Point(12, 261);
            this.CreateBenefit.Name = "CreateBenefit";
            this.CreateBenefit.Size = new System.Drawing.Size(109, 23);
            this.CreateBenefit.TabIndex = 9;
            this.CreateBenefit.Text = "Create Benefit";
            this.CreateBenefit.UseVisualStyleBackColor = true;
            this.CreateBenefit.Click += new System.EventHandler(this.CreateBenefit_Click);
            // 
            // DeleteBenefit
            // 
            this.DeleteBenefit.Enabled = false;
            this.DeleteBenefit.Location = new System.Drawing.Point(12, 232);
            this.DeleteBenefit.Name = "DeleteBenefit";
            this.DeleteBenefit.Size = new System.Drawing.Size(109, 23);
            this.DeleteBenefit.TabIndex = 10;
            this.DeleteBenefit.Text = "Delete Benefit";
            this.DeleteBenefit.UseVisualStyleBackColor = true;
            this.DeleteBenefit.Click += new System.EventHandler(this.DeleteBenefit_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(136, 27);
            this.listBox2.MaximumSize = new System.Drawing.Size(109, 199);
            this.listBox2.MinimumSize = new System.Drawing.Size(109, 199);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(109, 199);
            this.listBox2.TabIndex = 16;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(133, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Benefit Plans";
            // 
            // benefitTextBox
            // 
            this.benefitTextBox.Enabled = false;
            this.benefitTextBox.Location = new System.Drawing.Point(265, 25);
            this.benefitTextBox.Name = "benefitTextBox";
            this.benefitTextBox.Size = new System.Drawing.Size(121, 20);
            this.benefitTextBox.TabIndex = 18;
            // 
            // planTextBox
            // 
            this.planTextBox.Enabled = false;
            this.planTextBox.Location = new System.Drawing.Point(265, 70);
            this.planTextBox.Name = "planTextBox";
            this.planTextBox.Size = new System.Drawing.Size(121, 20);
            this.planTextBox.TabIndex = 19;
            // 
            // rateTextBox
            // 
            this.rateTextBox.Enabled = false;
            this.rateTextBox.Location = new System.Drawing.Point(265, 152);
            this.rateTextBox.MaxLength = 2;
            this.rateTextBox.Name = "rateTextBox";
            this.rateTextBox.Size = new System.Drawing.Size(100, 20);
            this.rateTextBox.TabIndex = 20;
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.Enabled = false;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(265, 242);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 23;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // ModifyInfo
            // 
            this.ModifyInfo.Enabled = false;
            this.ModifyInfo.Location = new System.Drawing.Point(538, 305);
            this.ModifyInfo.Name = "ModifyInfo";
            this.ModifyInfo.Size = new System.Drawing.Size(102, 23);
            this.ModifyInfo.TabIndex = 25;
            this.ModifyInfo.Text = "Modify Info";
            this.ModifyInfo.UseVisualStyleBackColor = true;
            this.ModifyInfo.Click += new System.EventHandler(this.ModifyInfo_Click);
            // 
            // DeletePlan
            // 
            this.DeletePlan.Enabled = false;
            this.DeletePlan.Location = new System.Drawing.Point(136, 232);
            this.DeletePlan.Name = "DeletePlan";
            this.DeletePlan.Size = new System.Drawing.Size(109, 23);
            this.DeletePlan.TabIndex = 26;
            this.DeletePlan.Text = "Delete Plan";
            this.DeletePlan.UseVisualStyleBackColor = true;
            this.DeletePlan.Click += new System.EventHandler(this.DeletePlan_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(262, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Benefit Name";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(262, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Plan Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(262, 136);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Rate (%)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(262, 226);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(98, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "Credits/Deductions";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(262, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "Fixed Amount";
            // 
            // fixedTextBox
            // 
            this.fixedTextBox.Enabled = false;
            this.fixedTextBox.Location = new System.Drawing.Point(265, 198);
            this.fixedTextBox.Name = "fixedTextBox";
            this.fixedTextBox.Size = new System.Drawing.Size(100, 20);
            this.fixedTextBox.TabIndex = 34;
            // 
            // modifierTextBox
            // 
            this.modifierTextBox.Enabled = false;
            this.modifierTextBox.Location = new System.Drawing.Point(403, 242);
            this.modifierTextBox.Name = "modifierTextBox";
            this.modifierTextBox.Size = new System.Drawing.Size(100, 20);
            this.modifierTextBox.TabIndex = 35;
            // 
            // modAmtTextBox
            // 
            this.modAmtTextBox.Enabled = false;
            this.modAmtTextBox.Location = new System.Drawing.Point(403, 285);
            this.modAmtTextBox.Name = "modAmtTextBox";
            this.modAmtTextBox.Size = new System.Drawing.Size(100, 20);
            this.modAmtTextBox.TabIndex = 36;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(400, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "C/D Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(400, 269);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Amount";
            // 
            // CreatePlan
            // 
            this.CreatePlan.Location = new System.Drawing.Point(136, 261);
            this.CreatePlan.Name = "CreatePlan";
            this.CreatePlan.Size = new System.Drawing.Size(109, 23);
            this.CreatePlan.TabIndex = 39;
            this.CreatePlan.Text = "Create Plan";
            this.CreatePlan.UseVisualStyleBackColor = true;
            this.CreatePlan.Click += new System.EventHandler(this.CreatePlan_Click);
            // 
            // CreateModifier
            // 
            this.CreateModifier.Location = new System.Drawing.Point(265, 298);
            this.CreateModifier.Name = "CreateModifier";
            this.CreateModifier.Size = new System.Drawing.Size(109, 23);
            this.CreateModifier.TabIndex = 40;
            this.CreateModifier.Text = "Create C/D";
            this.CreateModifier.UseVisualStyleBackColor = true;
            this.CreateModifier.Click += new System.EventHandler(this.CreateModifier_Click);
            // 
            // DeleteModifier
            // 
            this.DeleteModifier.Enabled = false;
            this.DeleteModifier.Location = new System.Drawing.Point(265, 269);
            this.DeleteModifier.Name = "DeleteModifier";
            this.DeleteModifier.Size = new System.Drawing.Size(109, 23);
            this.DeleteModifier.TabIndex = 41;
            this.DeleteModifier.Text = "Delete C/D";
            this.DeleteModifier.UseVisualStyleBackColor = true;
            this.DeleteModifier.Click += new System.EventHandler(this.DeleteModifier_Click);
            // 
            // benefitErrorLabel
            // 
            this.benefitErrorLabel.AutoSize = true;
            this.benefitErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.benefitErrorLabel.Location = new System.Drawing.Point(392, 28);
            this.benefitErrorLabel.Name = "benefitErrorLabel";
            this.benefitErrorLabel.Size = new System.Drawing.Size(11, 13);
            this.benefitErrorLabel.TabIndex = 42;
            this.benefitErrorLabel.Text = "*";
            // 
            // planErrorLabel
            // 
            this.planErrorLabel.AutoSize = true;
            this.planErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.planErrorLabel.Location = new System.Drawing.Point(392, 73);
            this.planErrorLabel.Name = "planErrorLabel";
            this.planErrorLabel.Size = new System.Drawing.Size(11, 13);
            this.planErrorLabel.TabIndex = 43;
            this.planErrorLabel.Text = "*";
            // 
            // rateErrorLabel
            // 
            this.rateErrorLabel.AutoSize = true;
            this.rateErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.rateErrorLabel.Location = new System.Drawing.Point(371, 155);
            this.rateErrorLabel.Name = "rateErrorLabel";
            this.rateErrorLabel.Size = new System.Drawing.Size(11, 13);
            this.rateErrorLabel.TabIndex = 44;
            this.rateErrorLabel.Text = "*";
            // 
            // fixedErrorLabel
            // 
            this.fixedErrorLabel.AutoSize = true;
            this.fixedErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.fixedErrorLabel.Location = new System.Drawing.Point(371, 201);
            this.fixedErrorLabel.Name = "fixedErrorLabel";
            this.fixedErrorLabel.Size = new System.Drawing.Size(11, 13);
            this.fixedErrorLabel.TabIndex = 45;
            this.fixedErrorLabel.Text = "*";
            // 
            // modifierErrorLabel
            // 
            this.modifierErrorLabel.AutoSize = true;
            this.modifierErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.modifierErrorLabel.Location = new System.Drawing.Point(509, 245);
            this.modifierErrorLabel.Name = "modifierErrorLabel";
            this.modifierErrorLabel.Size = new System.Drawing.Size(11, 13);
            this.modifierErrorLabel.TabIndex = 46;
            this.modifierErrorLabel.Text = "*";
            // 
            // modAmtErrorLabel
            // 
            this.modAmtErrorLabel.AutoSize = true;
            this.modAmtErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.modAmtErrorLabel.Location = new System.Drawing.Point(509, 288);
            this.modAmtErrorLabel.Name = "modAmtErrorLabel";
            this.modAmtErrorLabel.Size = new System.Drawing.Size(11, 13);
            this.modAmtErrorLabel.TabIndex = 47;
            this.modAmtErrorLabel.Text = "*";
            // 
            // radioRate
            // 
            this.radioRate.AutoSize = true;
            this.radioRate.Enabled = false;
            this.radioRate.Location = new System.Drawing.Point(265, 113);
            this.radioRate.Name = "radioRate";
            this.radioRate.Size = new System.Drawing.Size(48, 17);
            this.radioRate.TabIndex = 48;
            this.radioRate.TabStop = true;
            this.radioRate.Text = "Rate";
            this.radioRate.UseVisualStyleBackColor = true;
            this.radioRate.CheckedChanged += new System.EventHandler(this.radioRate_CheckedChanged);
            // 
            // radioFixed
            // 
            this.radioFixed.AutoSize = true;
            this.radioFixed.Enabled = false;
            this.radioFixed.Location = new System.Drawing.Point(319, 113);
            this.radioFixed.Name = "radioFixed";
            this.radioFixed.Size = new System.Drawing.Size(86, 17);
            this.radioFixed.TabIndex = 49;
            this.radioFixed.TabStop = true;
            this.radioFixed.Text = "FixedAmount";
            this.radioFixed.UseVisualStyleBackColor = true;
            this.radioFixed.CheckedChanged += new System.EventHandler(this.radioFixed_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(262, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 50;
            this.label5.Text = "Payment Type";
            // 
            // FormBenefits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 340);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.radioFixed);
            this.Controls.Add(this.radioRate);
            this.Controls.Add(this.modAmtErrorLabel);
            this.Controls.Add(this.modifierErrorLabel);
            this.Controls.Add(this.fixedErrorLabel);
            this.Controls.Add(this.rateErrorLabel);
            this.Controls.Add(this.planErrorLabel);
            this.Controls.Add(this.benefitErrorLabel);
            this.Controls.Add(this.DeleteModifier);
            this.Controls.Add(this.CreateModifier);
            this.Controls.Add(this.CreatePlan);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.modAmtTextBox);
            this.Controls.Add(this.modifierTextBox);
            this.Controls.Add(this.fixedTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.DeletePlan);
            this.Controls.Add(this.ModifyInfo);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.rateTextBox);
            this.Controls.Add(this.planTextBox);
            this.Controls.Add(this.benefitTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.DeleteBenefit);
            this.Controls.Add(this.CreateBenefit);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(668, 379);
            this.MinimumSize = new System.Drawing.Size(668, 379);
            this.Name = "FormBenefits";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Benefits";
            this.Load += new System.EventHandler(this.FormBenefits_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button CreateBenefit;
        private System.Windows.Forms.Button DeleteBenefit;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox benefitTextBox;
        private System.Windows.Forms.TextBox planTextBox;
        private System.Windows.Forms.TextBox rateTextBox;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Button ModifyInfo;
        private System.Windows.Forms.Button DeletePlan;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox fixedTextBox;
        private System.Windows.Forms.TextBox modifierTextBox;
        private System.Windows.Forms.TextBox modAmtTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CreatePlan;
        private System.Windows.Forms.Button CreateModifier;
        private System.Windows.Forms.Button DeleteModifier;
        private System.Windows.Forms.Label benefitErrorLabel;
        private System.Windows.Forms.Label planErrorLabel;
        private System.Windows.Forms.Label rateErrorLabel;
        private System.Windows.Forms.Label fixedErrorLabel;
        private System.Windows.Forms.Label modifierErrorLabel;
        private System.Windows.Forms.Label modAmtErrorLabel;
        private System.Windows.Forms.RadioButton radioRate;
        private System.Windows.Forms.RadioButton radioFixed;
        private System.Windows.Forms.Label label5;
    }
}