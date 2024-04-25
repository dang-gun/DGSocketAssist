using System.Text;

using SocketServer6Test.Global;


namespace SocketServer6Test;

public partial class ServerForm : Form
{

    public ServerForm()
    {
        InitializeComponent();

        GlobalStatic.MainForm = this;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnStart_Click(object sender, EventArgs e)
    {
        //��ư ǥ��
        BtnDisplay(false);

        int nPort = Convert.ToInt32(txtPort.Text);
        GlobalStatic.MainServer.Start(nPort);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnStop_Click(object sender, EventArgs e)
    {
        //���� ����
        GlobalStatic.MainServer.Stop();
        this.BtnDisplay(true);
    }

    /// <summary>
    /// ��ư�� ȭ�鿡 ǥ���ϰų� ������.
    /// </summary>
    /// <param name="bView"></param>
    private void BtnDisplay(bool bView)
    {
        if (true == bView)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }
        else
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

    }

    #region ���� ����Ʈ ����
    /// <summary>
    /// ���� ����Ʈ�� ID �߰�
    /// </summary>
    /// <param name="sId"></param>
    public void UserList_Add(string sId)
    {
        GlobalStatic.CrossThread_Winfom(this
            , new Action(
                delegate ()
                {
                    this.listUser.Items.Add(sId);
                }));
    }

    /// <summary>
    /// ���� ����Ʈ�� ID ����
    /// </summary>
    /// <param name="sId"></param>
    public void UserList_Remove(string sId)
    {
        GlobalStatic.CrossThread_Winfom(this
            , new Action(
                delegate ()
                {
                    this.listUser
                        .Items
                        .RemoveAt(this.listUser.FindString(sId));
                }));
    }

    /// <summary>
    /// ���� ����Ʈ ����
    /// </summary>
    public void UserList_Clear()
    {
        GlobalStatic.CrossThread_Winfom(this
            , new Action(
                delegate ()
                {
                    this.listUser.Items.Clear();
                }));
    }
    #endregion

    /// <summary>
    /// �޾ƿ� �޽����� ��� �Ѵ�.
    /// </summary>
    /// <param name="nMessage"></param>
    public void DisplayMsg(string nMessage)
    {
        StringBuilder buffer = new StringBuilder();

        //�ð� �߰�
        buffer.Append(
            String.Format("[{0}] "
                , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

        //����� �޽��� �ϼ�
        buffer.Append(nMessage);

        GlobalStatic.CrossThread_Winfom(this
            , new Action(
                delegate ()
                {
                    this.listMsg.Items.Add(buffer.ToString());

                    this.listMsg.SelectedIndex = listMsg.Items.Count - 1;
                    this.listMsg.SelectedIndex = -1;
                }));
    }

    /// <summary>
    /// �޾ƿ� �޽����� ��� �Ѵ�.
    /// </summary>
    /// <param name="nMessage"></param>
    public void DisplayLog(string nMessage)
    {
        StringBuilder buffer = new StringBuilder();

        //�ð� �߰�
        buffer.Append(
            String.Format("[{0}] "
                , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

        //����� �޽��� �ϼ�
        buffer.Append(nMessage);

        GlobalStatic.CrossThread_Winfom(this
            , new Action(
                    delegate ()
                    {
                        this.listLog.Items.Add(buffer.ToString());

                        this.listLog.SelectedIndex = listLog.Items.Count - 1;
                        this.listLog.SelectedIndex = -1;
                    }));
    }

    /// <summary>
    /// ��� �����ڿ��� �޽����� ������.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSend_Click(object sender, EventArgs e)
    {
        string sTossMsg = string.Format("server : {0}", this.txtSendMsg.Text);
        GlobalStatic.MainServer.SendMsg_All(sTossMsg);
        this.DisplayMsg(sTossMsg);
    }
}
