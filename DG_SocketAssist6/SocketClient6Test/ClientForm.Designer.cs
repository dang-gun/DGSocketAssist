namespace SocketClient6Test
{
    partial class ClientForm
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
            menuStrip1 = new MenuStrip();
            listMsg = new ListBox();
            label1 = new Label();
            txtIp = new TextBox();
            label2 = new Label();
            txtPort = new TextBox();
            button1 = new Button();
            listUser = new ListBox();
            txtAutoMsg = new TextBox();
            labID = new Label();
            btnSend = new Button();
            txtMsg = new TextBox();
            listLog = new ListBox();
            tsmiConnect = new ToolStripMenuItem();
            tsmiConnectReady = new ToolStripMenuItem();
            tsmiSendMessage = new ToolStripMenuItem();
            tsmiUserListCall = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { tsmiConnect });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(530, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // listMsg
            // 
            listMsg.FormattingEnabled = true;
            listMsg.ItemHeight = 15;
            listMsg.Location = new Point(4, 53);
            listMsg.Name = "listMsg";
            listMsg.Size = new Size(350, 184);
            listMsg.TabIndex = 1;
            // 
            // label1
            // 
            label1.Location = new Point(0, 24);
            label1.Name = "label1";
            label1.Size = new Size(83, 23);
            label1.TabIndex = 2;
            label1.Text = "Server IP : ";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtIp
            // 
            txtIp.Location = new Point(89, 24);
            txtIp.Name = "txtIp";
            txtIp.Size = new Size(85, 23);
            txtIp.TabIndex = 3;
            txtIp.Text = "127.0.0.1";
            // 
            // label2
            // 
            label2.Location = new Point(190, 24);
            label2.Name = "label2";
            label2.Size = new Size(83, 23);
            label2.TabIndex = 4;
            label2.Text = "Port : ";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(279, 24);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(75, 23);
            txtPort.TabIndex = 3;
            txtPort.Text = "7000";
            // 
            // button1
            // 
            button1.Location = new Point(360, 214);
            button1.Name = "button1";
            button1.Size = new Size(86, 23);
            button1.TabIndex = 5;
            button1.Text = "자동 메시지";
            button1.UseVisualStyleBackColor = true;
            // 
            // listUser
            // 
            listUser.FormattingEnabled = true;
            listUser.ItemHeight = 15;
            listUser.Location = new Point(360, 53);
            listUser.Name = "listUser";
            listUser.Size = new Size(162, 154);
            listUser.TabIndex = 1;
            // 
            // txtAutoMsg
            // 
            txtAutoMsg.Location = new Point(452, 214);
            txtAutoMsg.Name = "txtAutoMsg";
            txtAutoMsg.Size = new Size(70, 23);
            txtAutoMsg.TabIndex = 3;
            txtAutoMsg.Text = "자동 메시지";
            // 
            // labID
            // 
            labID.Location = new Point(4, 243);
            labID.Name = "labID";
            labID.Size = new Size(83, 23);
            labID.TabIndex = 2;
            labID.Text = "ID 입력";
            labID.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(452, 243);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(70, 23);
            btnSend.TabIndex = 5;
            btnSend.Text = "사인인";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txtMsg
            // 
            txtMsg.Location = new Point(89, 243);
            txtMsg.Name = "txtMsg";
            txtMsg.Size = new Size(357, 23);
            txtMsg.TabIndex = 3;
            // 
            // listLog
            // 
            listLog.FormattingEnabled = true;
            listLog.ItemHeight = 15;
            listLog.Location = new Point(4, 272);
            listLog.Name = "listLog";
            listLog.Size = new Size(518, 229);
            listLog.TabIndex = 1;
            // 
            // tsmiConnect
            // 
            tsmiConnect.DropDownItems.AddRange(new ToolStripItem[] { tsmiConnectReady, tsmiSendMessage, tsmiUserListCall });
            tsmiConnect.Name = "tsmiConnect";
            tsmiConnect.Size = new Size(77, 20);
            tsmiConnect.Text = "Server Test";
            // 
            // tsmiConnectReady
            // 
            tsmiConnectReady.Name = "tsmiConnectReady";
            tsmiConnectReady.Size = new Size(194, 22);
            tsmiConnectReady.Text = "접속 준비";
            // 
            // tsmiSendMessage
            // 
            tsmiSendMessage.Name = "tsmiSendMessage";
            tsmiSendMessage.Size = new Size(194, 22);
            tsmiSendMessage.Text = "메시지 보내기";
            tsmiSendMessage.Click += tsmiSendMessage_Click;
            // 
            // tsmiUserListCall
            // 
            tsmiUserListCall.Name = "tsmiUserListCall";
            tsmiUserListCall.Size = new Size(194, 22);
            tsmiUserListCall.Text = "유저 리스트 갱신 요청";
            tsmiUserListCall.Click += tsmiUserListCall_Click;
            // 
            // ClientForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(530, 508);
            Controls.Add(btnSend);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(txtPort);
            Controls.Add(txtAutoMsg);
            Controls.Add(txtMsg);
            Controls.Add(txtIp);
            Controls.Add(labID);
            Controls.Add(label1);
            Controls.Add(listUser);
            Controls.Add(listLog);
            Controls.Add(listMsg);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "ClientForm";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ListBox listMsg;
        private Label label1;
        private TextBox txtIp;
        private Label label2;
        private TextBox txtPort;
        private Button button1;
        private ListBox listUser;
        private TextBox txtAutoMsg;
        private Label labID;
        private Button btnSend;
        private TextBox txtMsg;
        private ListBox listLog;
        private ToolStripMenuItem tsmiConnect;
        private ToolStripMenuItem tsmiConnectReady;
        private ToolStripMenuItem tsmiSendMessage;
        private ToolStripMenuItem tsmiUserListCall;
    }
}
