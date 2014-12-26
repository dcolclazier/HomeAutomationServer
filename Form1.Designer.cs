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
            this.label1 = new System.Windows.Forms.Label();
            this.tempBox1 = new System.Windows.Forms.TextBox();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ObjectTemp 1 - Celcius";
            // 
            // tempBox1
            // 
            this.tempBox1.Location = new System.Drawing.Point(146, 12);
            this.tempBox1.Name = "tempBox1";
            this.tempBox1.Size = new System.Drawing.Size(152, 20);
            this.tempBox1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1266, 744);
            this.Controls.Add(this.tempBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richText_Status);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richText_Status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tempBox1;
    }
}

