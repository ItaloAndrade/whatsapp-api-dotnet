
using System.Windows.Forms;

namespace WhatsAppDotNet
{
    partial class Demo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Demo));
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtMensagem = new System.Windows.Forms.TextBox();
            this.btnSend = new  System.Windows.Forms.Button();
            this.qrCodeImage = new System.Windows.Forms.PictureBox();
            this.txtMessages = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.qrCodeImage)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPhone
            // 
 
            this.txtPhone.PlaceholderText = "Phone";
            this.txtPhone.Location = new System.Drawing.Point(13, 90); 
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.PasswordChar = '\0';
            this.txtPhone.SelectedText = "";
            this.txtPhone.SelectionLength = 0;
            this.txtPhone.SelectionStart = 0;
            this.txtPhone.Size = new System.Drawing.Size(446, 23);
            this.txtPhone.TabIndex = 0;
            this.txtPhone.UseSystemPasswordChar = false;
            this.txtPhone.Visible = false;
            // 
            // txtMensagem
            // 
       
            this.txtMensagem.PlaceholderText = "Message";
            this.txtMensagem.Location = new System.Drawing.Point(13, 139); 
            this.txtMensagem.Name = "txtMensagem";
            this.txtMensagem.PasswordChar = '\0';
            this.txtMensagem.SelectedText = "";
            this.txtMensagem.SelectionLength = 0;
            this.txtMensagem.SelectionStart = 0;
            this.txtMensagem.Size = new System.Drawing.Size(446, 23);
            this.txtMensagem.TabIndex = 1;
            this.txtMensagem.UseSystemPasswordChar = false;
            this.txtMensagem.Visible = false;
            // 
            // btnSend
            // 
            this.btnSend.AutoSize = true;
            this.btnSend.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink; 
            this.btnSend.Location = new System.Drawing.Point(346, 188);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6); 
            this.btnSend.Name = "btnSend"; 
            this.btnSend.Size = new System.Drawing.Size(113, 36);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send Message";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Visible = false;
            this.btnSend.Click += new System.EventHandler(this.sendMessage);
            // 
            // qrCodeImage
            // 
            this.qrCodeImage.Image = ((System.Drawing.Image)(resources.GetObject("qrCodeImage.Image")));
            this.qrCodeImage.Location = new System.Drawing.Point(134, 168);
            this.qrCodeImage.Name = "qrCodeImage";
            this.qrCodeImage.Size = new System.Drawing.Size(205, 203);
            this.qrCodeImage.TabIndex = 3;
            this.qrCodeImage.TabStop = false;
            // 
            // txtMessages
            // 
            this.txtMessages.Location = new System.Drawing.Point(13, 233);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessages.Size = new System.Drawing.Size(446, 205);
            this.txtMessages.TabIndex = 4;
            this.txtMessages.Visible = false;
            // 
            // Demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 450);
            this.Controls.Add(this.qrCodeImage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMensagem);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtMessages);
            this.MaximizeBox = false;
            this.Name = "Demo";
            this.Text = "Api  WhatsApp";
            ((System.ComponentModel.ISupportInitialize)(this.qrCodeImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox  txtPhone;
        private System.Windows.Forms.TextBox txtMensagem;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.PictureBox qrCodeImage;
        private System.Windows.Forms.TextBox txtMessages;
    }
}