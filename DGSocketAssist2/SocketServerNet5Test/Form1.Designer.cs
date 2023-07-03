namespace SocketServerNet5Test
{
    partial class ServerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStart = new System.Windows.Forms.Button();
            txtPort = new System.Windows.Forms.TextBox();
            btnStop = new System.Windows.Forms.Button();
            listLog = new System.Windows.Forms.ListBox();
            listUser = new System.Windows.Forms.ListBox();
            pbDownImage = new System.Windows.Forms.PictureBox();
            txtSendMsg = new System.Windows.Forms.TextBox();
            btnSend = new System.Windows.Forms.Button();
            btnImageSend = new System.Windows.Forms.Button();
            btnDir = new System.Windows.Forms.Button();
            txtDir = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)pbDownImage).BeginInit();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new System.Drawing.Point(109, 5);
            btnStart.Name = "btnStart";
            btnStart.Size = new System.Drawing.Size(75, 23);
            btnStart.TabIndex = 0;
            btnStart.Text = "서버 시작";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // txtPort
            // 
            txtPort.Location = new System.Drawing.Point(3, 5);
            txtPort.Name = "txtPort";
            txtPort.Size = new System.Drawing.Size(100, 23);
            txtPort.TabIndex = 1;
            txtPort.Text = "7000";
            // 
            // btnStop
            // 
            btnStop.Location = new System.Drawing.Point(190, 5);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(75, 23);
            btnStop.TabIndex = 0;
            btnStop.Text = "서버 중지";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // listLog
            // 
            listLog.FormattingEnabled = true;
            listLog.ItemHeight = 15;
            listLog.Location = new System.Drawing.Point(3, 32);
            listLog.Name = "listLog";
            listLog.Size = new System.Drawing.Size(300, 334);
            listLog.TabIndex = 2;
            // 
            // listUser
            // 
            listUser.FormattingEnabled = true;
            listUser.ItemHeight = 15;
            listUser.Location = new System.Drawing.Point(309, 32);
            listUser.Name = "listUser";
            listUser.Size = new System.Drawing.Size(162, 184);
            listUser.TabIndex = 3;
            // 
            // pbDownImage
            // 
            pbDownImage.Location = new System.Drawing.Point(309, 222);
            pbDownImage.Name = "pbDownImage";
            pbDownImage.Size = new System.Drawing.Size(162, 144);
            pbDownImage.TabIndex = 4;
            pbDownImage.TabStop = false;
            // 
            // txtSendMsg
            // 
            txtSendMsg.Location = new System.Drawing.Point(3, 372);
            txtSendMsg.Name = "txtSendMsg";
            txtSendMsg.Size = new System.Drawing.Size(300, 23);
            txtSendMsg.TabIndex = 5;
            // 
            // btnSend
            // 
            btnSend.Location = new System.Drawing.Point(309, 372);
            btnSend.Name = "btnSend";
            btnSend.Size = new System.Drawing.Size(162, 23);
            btnSend.TabIndex = 6;
            btnSend.Text = "메시지 전송";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // btnImageSend
            // 
            btnImageSend.Location = new System.Drawing.Point(396, 400);
            btnImageSend.Name = "btnImageSend";
            btnImageSend.Size = new System.Drawing.Size(75, 23);
            btnImageSend.TabIndex = 7;
            btnImageSend.Text = "서버 시작";
            btnImageSend.UseVisualStyleBackColor = true;
            btnImageSend.Click += btnImageSend_Click;
            // 
            // btnDir
            // 
            btnDir.Location = new System.Drawing.Point(358, 401);
            btnDir.Name = "btnDir";
            btnDir.Size = new System.Drawing.Size(32, 23);
            btnDir.TabIndex = 7;
            btnDir.Text = "...";
            btnDir.UseVisualStyleBackColor = true;
            btnDir.Click += btnDir_Click;
            // 
            // txtDir
            // 
            txtDir.Location = new System.Drawing.Point(3, 401);
            txtDir.Name = "txtDir";
            txtDir.Size = new System.Drawing.Size(349, 23);
            txtDir.TabIndex = 8;
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(479, 436);
            Controls.Add(txtDir);
            Controls.Add(btnDir);
            Controls.Add(btnImageSend);
            Controls.Add(btnSend);
            Controls.Add(txtSendMsg);
            Controls.Add(pbDownImage);
            Controls.Add(listUser);
            Controls.Add(listLog);
            Controls.Add(txtPort);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            MaximizeBox = false;
            Name = "ServerForm";
            Text = "Socket Server Test";
            FormClosing += ServerForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pbDownImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ListBox listLog;
        private System.Windows.Forms.ListBox listUser;
        private System.Windows.Forms.PictureBox pbDownImage;
        private System.Windows.Forms.TextBox txtSendMsg;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnImageSend;
        private System.Windows.Forms.Button btnDir;
        private System.Windows.Forms.TextBox txtDir;
    }
}
