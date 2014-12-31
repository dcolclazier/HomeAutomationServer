namespace HomeAutomationServer
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
            this.richText_Status = new System.Windows.Forms.RichTextBox();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.btn_SendCommand = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_RefreshDevices = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richText_Status
            // 
            this.richText_Status.Location = new System.Drawing.Point(910, 451);
            this.richText_Status.Name = "richText_Status";
            this.richText_Status.Size = new System.Drawing.Size(348, 284);
            this.richText_Status.TabIndex = 0;
            this.richText_Status.Text = "";
            this.richText_Status.TextChanged += new System.EventHandler(this.richText_Status_TextChanged);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(910, 412);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(75, 23);
            this.btn_Clear.TabIndex = 3;
            this.btn_Clear.Text = "Clear";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // btn_SendCommand
            // 
            this.btn_SendCommand.Location = new System.Drawing.Point(1170, 412);
            this.btn_SendCommand.Name = "btn_SendCommand";
            this.btn_SendCommand.Size = new System.Drawing.Size(42, 21);
            this.btn_SendCommand.TabIndex = 4;
            this.btn_SendCommand.Text = "Send";
            this.btn_SendCommand.UseVisualStyleBackColor = true;
            this.btn_SendCommand.Click += new System.EventHandler(this.btn_SendCommand_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(992, 412);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(172, 21);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(992, 369);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(172, 21);
            this.comboBox2.TabIndex = 6;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(989, 353);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Pick Device";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(989, 393);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Pick Command";
            // 
            // btn_RefreshDevices
            // 
            this.btn_RefreshDevices.Location = new System.Drawing.Point(910, 367);
            this.btn_RefreshDevices.Name = "btn_RefreshDevices";
            this.btn_RefreshDevices.Size = new System.Drawing.Size(75, 23);
            this.btn_RefreshDevices.TabIndex = 9;
            this.btn_RefreshDevices.Text = "Refresh";
            this.btn_RefreshDevices.UseVisualStyleBackColor = true;
            this.btn_RefreshDevices.Click += new System.EventHandler(this.btn_RefreshDevices_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1266, 744);
            this.Controls.Add(this.btn_RefreshDevices);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btn_SendCommand);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.richText_Status);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richText_Status;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Button btn_SendCommand;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_RefreshDevices;
    }
}

