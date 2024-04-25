namespace SocketServer6Test
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
            txtPort = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            listMsg = new ListBox();
            listUser = new ListBox();
            txtSendMsg = new TextBox();
            btnSend = new Button();
            listLog = new ListBox();
            SuspendLayout();
            // 
            // txtPort
            // 
            txtPort.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtPort.Location = new Point(6, 6);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(100, 23);
            txtPort.TabIndex = 0;
            txtPort.Text = "7000";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(112, 6);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 1;
            btnStart.Text = "서버 시작";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(193, 6);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 1;
            btnStop.Text = "서버 중지";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // listMsg
            // 
            listMsg.FormattingEnabled = true;
            listMsg.ItemHeight = 15;
            listMsg.Location = new Point(6, 33);
            listMsg.Name = "listMsg";
            listMsg.Size = new Size(427, 184);
            listMsg.TabIndex = 2;
            // 
            // listUser
            // 
            listUser.FormattingEnabled = true;
            listUser.ItemHeight = 15;
            listUser.Location = new Point(439, 33);
            listUser.Name = "listUser";
            listUser.Size = new Size(162, 184);
            listUser.TabIndex = 3;
            // 
            // txtSendMsg
            // 
            txtSendMsg.Location = new Point(6, 223);
            txtSendMsg.Name = "txtSendMsg";
            txtSendMsg.Size = new Size(427, 23);
            txtSendMsg.TabIndex = 4;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(439, 222);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(162, 23);
            btnSend.TabIndex = 1;
            btnSend.Text = "메시지 전송";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // listLog
            // 
            listLog.FormattingEnabled = true;
            listLog.ItemHeight = 15;
            listLog.Location = new Point(6, 252);
            listLog.Name = "listLog";
            listLog.Size = new Size(595, 304);
            listLog.TabIndex = 5;
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(607, 563);
            Controls.Add(listLog);
            Controls.Add(txtSendMsg);
            Controls.Add(listUser);
            Controls.Add(listMsg);
            Controls.Add(btnStop);
            Controls.Add(btnSend);
            Controls.Add(btnStart);
            Controls.Add(txtPort);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "ServerForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtPort;
        private Button btnStart;
        private Button btnStop;
        private ListBox listMsg;
        private ListBox listUser;
        private TextBox txtSendMsg;
        private Button btnSend;
        private ListBox listLog;
    }
}
