namespace SocketClientTest
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
			this.labID = new System.Windows.Forms.Label();
			this.btnSend = new System.Windows.Forms.Button();
			this.txtMsg = new System.Windows.Forms.TextBox();
			this.listUser = new System.Windows.Forms.ListBox();
			this.listMsg = new System.Windows.Forms.ListBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.txtAutoMsg = new System.Windows.Forms.TextBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiConnectReady = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiConnect = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSendMessage = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(356, 292);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(81, 23);
			this.button1.TabIndex = 23;
			this.button1.Text = "자동 메시지";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// labID
			// 
			this.labID.Location = new System.Drawing.Point(0, 350);
			this.labID.Name = "labID";
			this.labID.Size = new System.Drawing.Size(100, 23);
			this.labID.TabIndex = 21;
			this.labID.Text = "ID 출력";
			this.labID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnSend
			// 
			this.btnSend.Location = new System.Drawing.Point(443, 350);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(75, 23);
			this.btnSend.TabIndex = 20;
			this.btnSend.Text = "로그인";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// txtMsg
			// 
			this.txtMsg.Location = new System.Drawing.Point(106, 350);
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.Size = new System.Drawing.Size(331, 21);
			this.txtMsg.TabIndex = 17;
			// 
			// listUser
			// 
			this.listUser.FormattingEnabled = true;
			this.listUser.ItemHeight = 12;
			this.listUser.Location = new System.Drawing.Point(356, 27);
			this.listUser.Name = "listUser";
			this.listUser.Size = new System.Drawing.Size(162, 256);
			this.listUser.TabIndex = 15;
			// 
			// listMsg
			// 
			this.listMsg.FormattingEnabled = true;
			this.listMsg.ItemHeight = 12;
			this.listMsg.Location = new System.Drawing.Point(0, 27);
			this.listMsg.Name = "listMsg";
			this.listMsg.Size = new System.Drawing.Size(350, 316);
			this.listMsg.TabIndex = 16;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(354, 318);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 23);
			this.label1.TabIndex = 22;
			this.label1.Text = "Port";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtAutoMsg
			// 
			this.txtAutoMsg.Location = new System.Drawing.Point(443, 292);
			this.txtAutoMsg.Name = "txtAutoMsg";
			this.txtAutoMsg.Size = new System.Drawing.Size(75, 21);
			this.txtAutoMsg.TabIndex = 18;
			this.txtAutoMsg.Text = "자동 메시지";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(443, 320);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(75, 21);
			this.txtPort.TabIndex = 19;
			this.txtPort.Text = "7000";
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
			this.tsmiSendMessage});
			this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
			this.serverToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
			this.serverToolStripMenuItem.Text = "Server Test";
			// 
			// tsmiConnectReady
			// 
			this.tsmiConnectReady.Name = "tsmiConnectReady";
			this.tsmiConnectReady.Size = new System.Drawing.Size(180, 22);
			this.tsmiConnectReady.Text = "접속 준비";
			this.tsmiConnectReady.Click += new System.EventHandler(this.tsmiConnectReady_Click);
			// 
			// tsmiConnect
			// 
			this.tsmiConnect.Name = "tsmiConnect";
			this.tsmiConnect.Size = new System.Drawing.Size(180, 22);
			this.tsmiConnect.Text = "접속";
			this.tsmiConnect.Click += new System.EventHandler(this.tsmiConnect_Click);
			// 
			// tsmiSendMessage
			// 
			this.tsmiSendMessage.Name = "tsmiSendMessage";
			this.tsmiSendMessage.Size = new System.Drawing.Size(180, 22);
			this.tsmiSendMessage.Text = "메시지 보내기";
			this.tsmiSendMessage.Click += new System.EventHandler(this.tsmiSendMessage_Click);
			// 
			// ClientForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(526, 378);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.labID);
			this.Controls.Add(this.btnSend);
			this.Controls.Add(this.txtMsg);
			this.Controls.Add(this.listUser);
			this.Controls.Add(this.listMsg);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtAutoMsg);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "ClientForm";
			this.Text = "Socket Client Test";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label labID;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.TextBox txtMsg;
		private System.Windows.Forms.ListBox listUser;
		private System.Windows.Forms.ListBox listMsg;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtAutoMsg;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tsmiConnectReady;
		private System.Windows.Forms.ToolStripMenuItem tsmiConnect;
		private System.Windows.Forms.ToolStripMenuItem tsmiSendMessage;
	}
}

