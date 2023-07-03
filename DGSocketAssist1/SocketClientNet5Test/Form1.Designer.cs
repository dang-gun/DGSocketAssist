namespace SocketClientNet5Test
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
            components = new System.ComponentModel.Container();
            txtIP = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            btnSend = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            txtPort = new System.Windows.Forms.TextBox();
            tsmiConnect = new System.Windows.Forms.ToolStripMenuItem();
            tsmiConnectReady = new System.Windows.Forms.ToolStripMenuItem();
            serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSendMessage = new System.Windows.Forms.ToolStripMenuItem();
            txtAutoMsg = new System.Windows.Forms.TextBox();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            timer1 = new System.Windows.Forms.Timer(components);
            txtMsg = new System.Windows.Forms.TextBox();
            listUser = new System.Windows.Forms.ListBox();
            listMsg = new System.Windows.Forms.ListBox();
            labID = new System.Windows.Forms.Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // txtIP
            // 
            txtIP.Location = new System.Drawing.Point(432, 244);
            txtIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtIP.Name = "txtIP";
            txtIP.Size = new System.Drawing.Size(93, 23);
            txtIP.TabIndex = 53;
            txtIP.Text = "127.0.0.1";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(363, 244);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(63, 29);
            label2.TabIndex = 50;
            label2.Text = "IP";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(363, 273);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(63, 29);
            label4.TabIndex = 52;
            label4.Text = "Port";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSend
            // 
            btnSend.Location = new System.Drawing.Point(450, 339);
            btnSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnSend.Name = "btnSend";
            btnSend.Size = new System.Drawing.Size(75, 23);
            btnSend.TabIndex = 43;
            btnSend.Text = "Login";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(363, 308);
            button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(81, 23);
            button1.TabIndex = 41;
            button1.Text = "자동 메시지";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // txtPort
            // 
            txtPort.Location = new System.Drawing.Point(432, 277);
            txtPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtPort.Name = "txtPort";
            txtPort.Size = new System.Drawing.Size(93, 23);
            txtPort.TabIndex = 49;
            txtPort.Text = "7000";
            // 
            // tsmiConnect
            // 
            tsmiConnect.Name = "tsmiConnect";
            tsmiConnect.Size = new System.Drawing.Size(150, 22);
            tsmiConnect.Text = "접속";
            tsmiConnect.Click += tsmiConnect_Click;
            // 
            // tsmiConnectReady
            // 
            tsmiConnectReady.Name = "tsmiConnectReady";
            tsmiConnectReady.Size = new System.Drawing.Size(150, 22);
            tsmiConnectReady.Text = "접속 준비";
            tsmiConnectReady.Click += tsmiConnectReady_Click;
            // 
            // serverToolStripMenuItem
            // 
            serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiConnectReady, tsmiConnect, tsmiSendMessage });
            serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            serverToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            serverToolStripMenuItem.Text = "Server Test";
            // 
            // tsmiSendMessage
            // 
            tsmiSendMessage.Name = "tsmiSendMessage";
            tsmiSendMessage.Size = new System.Drawing.Size(150, 22);
            tsmiSendMessage.Text = "메시지 보내기";
            tsmiSendMessage.Click += tsmiSendMessage_Click;
            // 
            // txtAutoMsg
            // 
            txtAutoMsg.Location = new System.Drawing.Point(450, 308);
            txtAutoMsg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtAutoMsg.Name = "txtAutoMsg";
            txtAutoMsg.Size = new System.Drawing.Size(75, 23);
            txtAutoMsg.TabIndex = 39;
            txtAutoMsg.Text = "자동 메시지";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { serverToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(534, 24);
            menuStrip1.TabIndex = 42;
            menuStrip1.Text = "menuStrip1";
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            // 
            // txtMsg
            // 
            txtMsg.Location = new System.Drawing.Point(124, 340);
            txtMsg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtMsg.Name = "txtMsg";
            txtMsg.Size = new System.Drawing.Size(320, 23);
            txtMsg.TabIndex = 38;
            // 
            // listUser
            // 
            listUser.FormattingEnabled = true;
            listUser.ItemHeight = 15;
            listUser.Location = new System.Drawing.Point(363, 26);
            listUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            listUser.Name = "listUser";
            listUser.Size = new System.Drawing.Size(165, 214);
            listUser.TabIndex = 36;
            // 
            // listMsg
            // 
            listMsg.FormattingEnabled = true;
            listMsg.ItemHeight = 15;
            listMsg.Location = new System.Drawing.Point(7, 26);
            listMsg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            listMsg.Name = "listMsg";
            listMsg.Size = new System.Drawing.Size(350, 304);
            listMsg.TabIndex = 37;
            // 
            // labID
            // 
            labID.Location = new System.Drawing.Point(7, 340);
            labID.Name = "labID";
            labID.Size = new System.Drawing.Size(100, 23);
            labID.TabIndex = 54;
            labID.Text = "ID 출력";
            labID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ClientForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(534, 372);
            Controls.Add(labID);
            Controls.Add(txtIP);
            Controls.Add(label2);
            Controls.Add(label4);
            Controls.Add(btnSend);
            Controls.Add(button1);
            Controls.Add(txtPort);
            Controls.Add(txtAutoMsg);
            Controls.Add(menuStrip1);
            Controls.Add(txtMsg);
            Controls.Add(listUser);
            Controls.Add(listMsg);
            Name = "ClientForm";
            Text = "Form1";
            FormClosing += ClientForm_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.ToolStripMenuItem tsmiConnect;
        private System.Windows.Forms.ToolStripMenuItem tsmiConnectReady;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiSendMessage;
        private System.Windows.Forms.TextBox txtAutoMsg;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.ListBox listUser;
        private System.Windows.Forms.ListBox listMsg;
        private System.Windows.Forms.Label labID;
    }
}
