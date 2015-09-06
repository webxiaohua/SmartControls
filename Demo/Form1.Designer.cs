namespace Demo
{
    partial class Form1
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
            this.btnBeginTest = new SmartWinControls.SmartControls.Button.SmartButton();
            this.smartCheckBox1 = new SmartWinControls.SmartControls.CheckBox.SmartCheckBox();
            this.smartRadioButton1 = new SmartWinControls.SmartControls.RadioButton.SmartRadioButton();
            this.smartRadioButton2 = new SmartWinControls.SmartControls.RadioButton.SmartRadioButton();
            this.smartRadioButton3 = new SmartWinControls.SmartControls.RadioButton.SmartRadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBeginTest
            // 
            this.btnBeginTest.ForeImage = null;
            this.btnBeginTest.ForeImageSize = new System.Drawing.Size(0, 0);
            this.btnBeginTest.ForePathAlign = SmartWinControls.Common.ButtonImageAlignment.Left;
            this.btnBeginTest.ForePathGetter = null;
            this.btnBeginTest.ForePathSize = new System.Drawing.Size(0, 0);
            this.btnBeginTest.Image = null;
            this.btnBeginTest.Location = new System.Drawing.Point(93, 172);
            this.btnBeginTest.Name = "btnBeginTest";
            this.btnBeginTest.Size = new System.Drawing.Size(88, 29);
            this.btnBeginTest.SpaceBetweenPathAndText = 0;
            this.btnBeginTest.TabIndex = 0;
            this.btnBeginTest.Text = "开始测试";
            this.btnBeginTest.UseVisualStyleBackColor = true;
            // 
            // smartCheckBox1
            // 
            this.smartCheckBox1.BackColor = System.Drawing.Color.Transparent;
            this.smartCheckBox1.Image = null;
            this.smartCheckBox1.Location = new System.Drawing.Point(67, 80);
            this.smartCheckBox1.Name = "smartCheckBox1";
            this.smartCheckBox1.Size = new System.Drawing.Size(65, 21);
            this.smartCheckBox1.TabIndex = 1;
            this.smartCheckBox1.Text = "羽毛球";
            this.smartCheckBox1.UseVisualStyleBackColor = false;
            // 
            // smartRadioButton1
            // 
            this.smartRadioButton1.Image = null;
            this.smartRadioButton1.Location = new System.Drawing.Point(81, 124);
            this.smartRadioButton1.Name = "smartRadioButton1";
            this.smartRadioButton1.Size = new System.Drawing.Size(40, 21);
            this.smartRadioButton1.TabIndex = 2;
            this.smartRadioButton1.Text = "男";
            this.smartRadioButton1.UseVisualStyleBackColor = true;
            // 
            // smartRadioButton2
            // 
            this.smartRadioButton2.Image = null;
            this.smartRadioButton2.Location = new System.Drawing.Point(127, 124);
            this.smartRadioButton2.Name = "smartRadioButton2";
            this.smartRadioButton2.Size = new System.Drawing.Size(40, 21);
            this.smartRadioButton2.TabIndex = 3;
            this.smartRadioButton2.TabStop = true;
            this.smartRadioButton2.Text = "女";
            this.smartRadioButton2.UseVisualStyleBackColor = true;
            // 
            // smartRadioButton3
            // 
            this.smartRadioButton3.Checked = true;
            this.smartRadioButton3.Image = null;
            this.smartRadioButton3.Location = new System.Drawing.Point(173, 124);
            this.smartRadioButton3.Name = "smartRadioButton3";
            this.smartRadioButton3.Size = new System.Drawing.Size(52, 21);
            this.smartRadioButton3.TabIndex = 4;
            this.smartRadioButton3.TabStop = true;
            this.smartRadioButton3.Text = "保密";
            this.smartRadioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(25, 17);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(29, 16);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "A";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(60, 17);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(29, 16);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.Text = "B";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Location = new System.Drawing.Point(127, 225);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 53);
            this.panel1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 369);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.smartRadioButton3);
            this.Controls.Add(this.smartRadioButton2);
            this.Controls.Add(this.smartRadioButton1);
            this.Controls.Add(this.smartCheckBox1);
            this.Controls.Add(this.btnBeginTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SmartWinControls.SmartControls.Button.SmartButton btnBeginTest;
        private SmartWinControls.SmartControls.CheckBox.SmartCheckBox smartCheckBox1;
        private SmartWinControls.SmartControls.RadioButton.SmartRadioButton smartRadioButton1;
        private SmartWinControls.SmartControls.RadioButton.SmartRadioButton smartRadioButton2;
        private SmartWinControls.SmartControls.RadioButton.SmartRadioButton smartRadioButton3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Panel panel1;
    }
}

