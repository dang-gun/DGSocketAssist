﻿namespace SocketClientNet5Test
{
	/// <summary>
	/// 유저 UI
	/// </summary>
	partial class ClientForm
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.button1 = new System.Windows.Forms.Button();
			this.btnSend = new System.Windows.Forms.Button();
			this.txtMsg = new System.Windows.Forms.TextBox();
			this.listUser = new System.Windows.Forms.ListBox();
			this.listMsg = new System.Windows.Forms.ListBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.txtAutoMsg = new System.Windows.Forms.TextBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiConnectReady = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiConnect = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSendTest = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSendMessage = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSendFile = new System.Windows.Forms.ToolStripMenuItem();
			this.btnLogin = new System.Windows.Forms.Button();
			this.btnImageSend = new System.Windows.Forms.Button();
			this.btnDir = new System.Windows.Forms.Button();
			this.txtDir = new System.Windows.Forms.TextBox();
			this.txtIP = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtID = new System.Windows.Forms.TextBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.pbDownImage = new System.Windows.Forms.PictureBox();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbDownImage)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(357, 342);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(81, 23);
			this.button1.TabIndex = 23;
			this.button1.Text = "자동 메시지";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// btnSend
			// 
			this.btnSend.Location = new System.Drawing.Point(446, 369);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(75, 23);
			this.btnSend.TabIndex = 20;
			this.btnSend.Text = "보내기";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// txtMsg
			// 
			this.txtMsg.Location = new System.Drawing.Point(15, 371);
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.Size = new System.Drawing.Size(423, 21);
			this.txtMsg.TabIndex = 17;
			// 
			// listUser
			// 
			this.listUser.FormattingEnabled = true;
			this.listUser.ItemHeight = 12;
			this.listUser.Location = new System.Drawing.Point(356, 27);
			this.listUser.Name = "listUser";
			this.listUser.Size = new System.Drawing.Size(165, 160);
			this.listUser.TabIndex = 15;
			// 
			// listMsg
			// 
			this.listMsg.FormattingEnabled = true;
			this.listMsg.ItemHeight = 12;
			this.listMsg.Location = new System.Drawing.Point(0, 27);
			this.listMsg.Name = "listMsg";
			this.listMsg.Size = new System.Drawing.Size(350, 340);
			this.listMsg.TabIndex = 16;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			// 
			// txtAutoMsg
			// 
			this.txtAutoMsg.Location = new System.Drawing.Point(446, 342);
			this.txtAutoMsg.Name = "txtAutoMsg";
			this.txtAutoMsg.Size = new System.Drawing.Size(75, 21);
			this.txtAutoMsg.TabIndex = 18;
			this.txtAutoMsg.Text = "자동 메시지";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.serverToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(526, 24);
			this.menuStrip1.TabIndex = 24;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// serverToolStripMenuItem
			// 
			this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.tsmiConnectReady,
			this.tsmiConnect,
			this.tsmiSendTest,
			this.tsmiSendMessage,
			this.tsmiSendFile});
			this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
			this.serverToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
			this.serverToolStripMenuItem.Text = "Server Test";
			// 
			// tsmiConnectReady
			// 
			this.tsmiConnectReady.Name = "tsmiConnectReady";
			this.tsmiConnectReady.Size = new System.Drawing.Size(154, 22);
			this.tsmiConnectReady.Text = "접속 준비";
			this.tsmiConnectReady.Click += new System.EventHandler(this.tsmiConnectReady_Click);
			// 
			// tsmiConnect
			// 
			this.tsmiConnect.Name = "tsmiConnect";
			this.tsmiConnect.Size = new System.Drawing.Size(154, 22);
			this.tsmiConnect.Text = "접속";
			this.tsmiConnect.Click += new System.EventHandler(this.tsmiConnect_Click);
			// 
			// tsmiSendTest
			// 
			this.tsmiSendTest.Name = "tsmiSendTest";
			this.tsmiSendTest.Size = new System.Drawing.Size(154, 22);
			this.tsmiSendTest.Text = "보내기(테스트)";
			this.tsmiSendTest.Click += new System.EventHandler(this.tsmiSendTest_Click);
			// 
			// tsmiSendMessage
			// 
			this.tsmiSendMessage.Name = "tsmiSendMessage";
			this.tsmiSendMessage.Size = new System.Drawing.Size(154, 22);
			this.tsmiSendMessage.Text = "보내기(체팅)";
			this.tsmiSendMessage.Click += new System.EventHandler(this.tsmiSendMessage_Click);
			// 
			// tsmiSendFile
			// 
			this.tsmiSendFile.Name = "tsmiSendFile";
			this.tsmiSendFile.Size = new System.Drawing.Size(154, 22);
			this.tsmiSendFile.Text = "보내기(파일)";
			// 
			// btnLogin
			// 
			this.btnLogin.Location = new System.Drawing.Point(446, 427);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(75, 23);
			this.btnLogin.TabIndex = 25;
			this.btnLogin.Text = "Login";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// btnImageSend
			// 
			this.btnImageSend.Location = new System.Drawing.Point(446, 398);
			this.btnImageSend.Name = "btnImageSend";
			this.btnImageSend.Size = new System.Drawing.Size(75, 23);
			this.btnImageSend.TabIndex = 27;
			this.btnImageSend.Text = "파일 전송";
			this.btnImageSend.UseVisualStyleBackColor = true;
			this.btnImageSend.Click += new System.EventHandler(this.btnImageSend_Click);
			// 
			// btnDir
			// 
			this.btnDir.Location = new System.Drawing.Point(407, 398);
			this.btnDir.Name = "btnDir";
			this.btnDir.Size = new System.Drawing.Size(33, 23);
			this.btnDir.TabIndex = 29;
			this.btnDir.Text = "...";
			this.btnDir.UseVisualStyleBackColor = true;
			this.btnDir.Click += new System.EventHandler(this.btnDir_Click);
			// 
			// txtDir
			// 
			this.txtDir.Location = new System.Drawing.Point(15, 400);
			this.txtDir.Name = "txtDir";
			this.txtDir.Size = new System.Drawing.Size(386, 21);
			this.txtDir.TabIndex = 28;
			this.txtDir.Text = "C:\\Users\\Kim\\Pictures\\favicon.jpg";
			// 
			// txtIP
			// 
			this.txtIP.Location = new System.Drawing.Point(52, 429);
			this.txtIP.Name = "txtIP";
			this.txtIP.Size = new System.Drawing.Size(133, 21);
			this.txtIP.TabIndex = 35;
			this.txtIP.Text = "127.0.0.1";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 429);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(30, 23);
			this.label2.TabIndex = 32;
			this.label2.Text = "IP";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(308, 427);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(30, 23);
			this.label3.TabIndex = 33;
			this.label3.Text = "ID";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(191, 429);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(30, 23);
			this.label4.TabIndex = 34;
			this.label4.Text = "Port";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtID
			// 
			this.txtID.Location = new System.Drawing.Point(344, 429);
			this.txtID.Name = "txtID";
			this.txtID.Size = new System.Drawing.Size(96, 21);
			this.txtID.TabIndex = 30;
			this.txtID.Text = "test";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(227, 429);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(75, 21);
			this.txtPort.TabIndex = 31;
			this.txtPort.Text = "7000";
			// 
			// pbDownImage
			// 
			this.pbDownImage.Location = new System.Drawing.Point(357, 198);
			this.pbDownImage.Name = "pbDownImage";
			this.pbDownImage.Size = new System.Drawing.Size(164, 138);
			this.pbDownImage.TabIndex = 26;
			this.pbDownImage.TabStop = false;
			// 
			// ClientForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(526, 455);
			this.Controls.Add(this.txtIP);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtID);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.btnDir);
			this.Controls.Add(this.txtDir);
			this.Controls.Add(this.btnImageSend);
			this.Controls.Add(this.pbDownImage);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnSend);
			this.Controls.Add(this.txtMsg);
			this.Controls.Add(this.listUser);
			this.Controls.Add(this.listMsg);
			this.Controls.Add(this.txtAutoMsg);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "ClientForm";
			this.Text = "Socket Client Test";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbDownImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.TextBox txtMsg;
		private System.Windows.Forms.ListBox listUser;
		private System.Windows.Forms.ListBox listMsg;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.TextBox txtAutoMsg;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tsmiConnectReady;
		private System.Windows.Forms.ToolStripMenuItem tsmiConnect;
		private System.Windows.Forms.ToolStripMenuItem tsmiSendMessage;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.Button btnImageSend;
		private System.Windows.Forms.Button btnDir;
		private System.Windows.Forms.TextBox txtDir;
		private System.Windows.Forms.TextBox txtIP;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtID;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.ToolStripMenuItem tsmiSendTest;
		private System.Windows.Forms.ToolStripMenuItem tsmiSendFile;
		private System.Windows.Forms.PictureBox pbDownImage;
	}
}
