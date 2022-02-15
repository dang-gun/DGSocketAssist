namespace SocketServerTest
{
	partial class ServerForm
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
			this.txtSendMsg = new System.Windows.Forms.TextBox();
			this.btnSend = new System.Windows.Forms.Button();
			this.listUser = new System.Windows.Forms.ListBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.btnStart = new System.Windows.Forms.Button();
			this.listLog = new System.Windows.Forms.ListBox();
			this.btnDir = new System.Windows.Forms.Button();
			this.txtDir = new System.Windows.Forms.TextBox();
			this.btnImageSend = new System.Windows.Forms.Button();
			this.pbDownImage = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pbDownImage)).BeginInit();
			this.SuspendLayout();
			// 
			// txtSendMsg
			// 
			this.txtSendMsg.Location = new System.Drawing.Point(3, 366);
			this.txtSendMsg.Name = "txtSendMsg";
			this.txtSendMsg.Size = new System.Drawing.Size(302, 21);
			this.txtSendMsg.TabIndex = 17;
			// 
			// btnSend
			// 
			this.btnSend.Location = new System.Drawing.Point(311, 366);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(162, 23);
			this.btnSend.TabIndex = 16;
			this.btnSend.Text = "메시지 전송";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// listUser
			// 
			this.listUser.FormattingEnabled = true;
			this.listUser.ItemHeight = 12;
			this.listUser.Location = new System.Drawing.Point(311, 32);
			this.listUser.Name = "listUser";
			this.listUser.Size = new System.Drawing.Size(162, 184);
			this.listUser.TabIndex = 15;
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(191, 3);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 14;
			this.btnStop.Text = "서버 중지";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(3, 5);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(100, 21);
			this.txtPort.TabIndex = 13;
			this.txtPort.Text = "7000";
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(109, 3);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 12;
			this.btnStart.Text = "서버 시작";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// listLog
			// 
			this.listLog.FormattingEnabled = true;
			this.listLog.ItemHeight = 12;
			this.listLog.Location = new System.Drawing.Point(3, 32);
			this.listLog.Name = "listLog";
			this.listLog.Size = new System.Drawing.Size(302, 328);
			this.listLog.TabIndex = 11;
			// 
			// btnDir
			// 
			this.btnDir.Location = new System.Drawing.Point(359, 395);
			this.btnDir.Name = "btnDir";
			this.btnDir.Size = new System.Drawing.Size(33, 23);
			this.btnDir.TabIndex = 32;
			this.btnDir.Text = "...";
			this.btnDir.UseVisualStyleBackColor = true;
			this.btnDir.Click += new System.EventHandler(this.btnDir_Click);
			// 
			// txtDir
			// 
			this.txtDir.Location = new System.Drawing.Point(3, 397);
			this.txtDir.Name = "txtDir";
			this.txtDir.Size = new System.Drawing.Size(350, 21);
			this.txtDir.TabIndex = 31;
			this.txtDir.Text = "C:\\Users\\Kim\\Pictures\\favicon.ico";
			// 
			// btnImageSend
			// 
			this.btnImageSend.Location = new System.Drawing.Point(398, 395);
			this.btnImageSend.Name = "btnImageSend";
			this.btnImageSend.Size = new System.Drawing.Size(75, 23);
			this.btnImageSend.TabIndex = 30;
			this.btnImageSend.Text = "파일 전송";
			this.btnImageSend.UseVisualStyleBackColor = true;
			this.btnImageSend.Click += new System.EventHandler(this.btnImageSend_Click);
			// 
			// pbDownImage
			// 
			this.pbDownImage.Location = new System.Drawing.Point(311, 222);
			this.pbDownImage.Name = "pbDownImage";
			this.pbDownImage.Size = new System.Drawing.Size(162, 138);
			this.pbDownImage.TabIndex = 33;
			this.pbDownImage.TabStop = false;
			// 
			// ServerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(478, 428);
			this.Controls.Add(this.pbDownImage);
			this.Controls.Add(this.btnDir);
			this.Controls.Add(this.txtDir);
			this.Controls.Add(this.btnImageSend);
			this.Controls.Add(this.txtSendMsg);
			this.Controls.Add(this.btnSend);
			this.Controls.Add(this.listUser);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.listLog);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "ServerForm";
			this.Text = "Socket Server Test";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.pbDownImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtSendMsg;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.ListBox listUser;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.ListBox listLog;
		private System.Windows.Forms.Button btnDir;
		private System.Windows.Forms.TextBox txtDir;
		private System.Windows.Forms.Button btnImageSend;
		private System.Windows.Forms.PictureBox pbDownImage;
	}
}

