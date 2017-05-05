namespace TP1
{
    partial class Window
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
            this.descriptionBox = new System.Windows.Forms.RichTextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.pushButton = new System.Windows.Forms.Button();
            this.pullButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.keyText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.valueText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // descriptionBox
            // 
            this.descriptionBox.Location = new System.Drawing.Point(12, 261);
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.Size = new System.Drawing.Size(1141, 239);
            this.descriptionBox.TabIndex = 0;
            this.descriptionBox.Text = "";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(931, 13);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(222, 72);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect To Ring";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // pushButton
            // 
            this.pushButton.Location = new System.Drawing.Point(12, 169);
            this.pushButton.Name = "pushButton";
            this.pushButton.Size = new System.Drawing.Size(222, 72);
            this.pushButton.TabIndex = 2;
            this.pushButton.Text = "Push Values";
            this.pushButton.UseVisualStyleBackColor = true;
            this.pushButton.Click += new System.EventHandler(this.pushButton_Click);
            // 
            // pullButton
            // 
            this.pullButton.Location = new System.Drawing.Point(252, 169);
            this.pullButton.Name = "pullButton";
            this.pullButton.Size = new System.Drawing.Size(222, 72);
            this.pullButton.TabIndex = 3;
            this.pullButton.Text = "Pull Values";
            this.pullButton.UseVisualStyleBackColor = true;
            this.pullButton.Click += new System.EventHandler(this.pullButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(490, 169);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(222, 72);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "Delete Value";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // keyText
            // 
            this.keyText.Location = new System.Drawing.Point(12, 34);
            this.keyText.Name = "keyText";
            this.keyText.Size = new System.Drawing.Size(700, 31);
            this.keyText.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "Value";
            // 
            // valueText
            // 
            this.valueText.Location = new System.Drawing.Point(12, 112);
            this.valueText.Name = "valueText";
            this.valueText.Size = new System.Drawing.Size(700, 31);
            this.valueText.TabIndex = 7;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 514);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.valueText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.keyText);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.pullButton);
            this.Controls.Add(this.pushButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.descriptionBox);
            this.Name = "Window";
            this.Text = "Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox descriptionBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button pushButton;
        private System.Windows.Forms.Button pullButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.TextBox keyText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox valueText;
    }
}