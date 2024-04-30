using System.Text;

using ChatGlobal;
using SocketClient6Test.Global;


namespace SocketClient6Test;

public partial class ClientForm : Form
{
    /// <summary>
    /// ���� ����
    /// </summary>
    public enum UiStateType
    {
        /// <summary>
        /// ����
        /// </summary>
        None = 0,
        /// <summary>
        /// ������
        /// </summary>
        Connecting,
        /// <summary>
        /// ���� �Ϸ�
        /// </summary>
        Connect,

        /// <summary>
        /// ���� ����
        /// </summary>
        Disconnect,
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private UiStateType m_typeState = UiStateType.None;

    public ClientForm()
    {
        InitializeComponent();

        GlobalStatic.MainForm = this;
        this.Text = string.Format("Socket Client Test({0})"
                                    , ChatSetting.SiteTitle);
    }

    private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (null != GlobalStatic.MainClient)
        {
            GlobalStatic.MainClient.Disconnect();
        }
    }

    /// <summary>
    /// �α���/�޽��� ������ ��ư Ŭ��
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSend_Click(object sender, EventArgs e)
    {
        switch (m_typeState)
        {
            case UiStateType.None://�⺻
                if ("" == txtMsg.Text)
                {
                    //�Է°��� ������ ����
                    MessageBox.Show("���̵� �ְ� �õ��� �ּ���");
                    return;
                }
                else
                {
                    //���̵� ������ ������ ����

                    //�����̸� �����ϰ�
                    this.UI_Setting(UiStateType.Connecting);

                    int nPort = Convert.ToInt32(txtPort.Text);

                    //�������� ó�� ��ü 
                    //�������� ����
                    GlobalStatic.MainClient
                        .ConnectStart(
                            this.txtIp.Text
                            , nPort
                            , this.txtMsg.Text);
                }
                break;

            case UiStateType.Connect://���� ����
                                   //�̻��¿����� �޽����� ������.
                GlobalStatic.MainClient.SendMsg(ChatCommandType.Msg, txtMsg.Text);
                this.txtMsg.Text = "";
                break;
        }
    }



    /// <summary>
    /// UI ����
    /// </summary>
    /// <param name="typeSet"></param>
    public void UI_Setting(UiStateType typeSet)
    {
        //���� ���� �����ϰ�
        m_typeState = typeSet;

        switch (typeSet)
        {
            case UiStateType.None://�⺻
            case UiStateType.Disconnect: //����
                GlobalStatic.CrossThread_Winfom(this
                    , new Action(delegate ()
                    {
                        this.labID.Text = "ID �Է�";

                        this.txtMsg.Enabled = true;
                        this.btnSend.Text = "�α���";
                        this.btnSend.Enabled = true;

                        //���� ����Ʈ ����
                        this.UserList_Clear();
                    }));

                //ó������ ���������� typeState.None�� �ʱ�ȭ �Ѵ�.
                m_typeState = UiStateType.None;
                break;

            case UiStateType.Connecting:  //������
                txtMsg.Enabled = false;
                btnSend.Text = "������";
                btnSend.Enabled = false;
                break;
            case UiStateType.Connect: //����Ϸ�
                GlobalStatic.CrossThread_Winfom(this
                    , new Action(delegate ()
                    {
                        this.labID.Text = "�޽���";

                        this.txtMsg.Enabled = true;
                        this.txtMsg.Text = "";

                        this.btnSend.Text = "������";
                        this.btnSend.Enabled = true;
                    }));
                break;
        }
    }


    /// <summary>
    /// �޾ƿ� �޽����� ��� �Ѵ�.
    /// </summary>
    /// <param name="nMessage"></param>
    public void DisplayMsg(string nMessage)
    {
        StringBuilder sb = new StringBuilder();

        //�ð� �߰�
        sb.Append(
            String.Format("[{0}] "
                , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

        //����� �޽��� �ϼ�
        sb.Append(nMessage);

        GlobalStatic.CrossThread_Winfom(this
            , new Action(
                delegate ()
                {
                    this.listMsg.Items.Add(sb.ToString());
                }));
    }

    #region ���� ����Ʈ ����
    /// <summary>
    /// ���� ����Ʈ�� ID �߰�
    /// </summary>
    /// <param name="sId"></param>
    public void UserList_Add(string sId)
    {
        if (string.Empty != sId)
        {
            GlobalStatic.CrossThread_Winfom(this
                , new Action(
                    delegate ()
                    {
                        this.listUser.Items.Add(sId);
                    }));
        }

    }

    /// <summary>
    /// ���� ����Ʈ�� �ٽ� �ִ´�.
    /// </summary>
    /// <remarks>���� ����Ʈ�� ���� ���޹��� ����Ʈ�� �ٽ� �߰��Ѵ�.</remarks>
    /// <param name="sUserList"></param>
    public void UserList_Add_List(string sUserList)
    {
        GlobalStatic.CrossThread_Winfom(this
            , new Action(
                delegate ()
                {
                    //����Ʈ�� ����
                    listUser.Items.Clear();

                    //����Ʈ�� �ٽ� ä���ش�.
                    string[] sList = sUserList.Split(',');
                    for (int i = 0; i < sList.Length; ++i)
                    {
                        if (string.Empty != sList[i])
                        {
                            listUser.Items.Add(sList[i]);
                        }
                    }
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

    public void DisplayLog(string nMessage)
    {
        StringBuilder sb = new StringBuilder();

        //�ð� �߰�
        sb.Append(
            String.Format("[{0}] "
                , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

        //����� �޽��� �ϼ�
        sb.Append(nMessage);

        GlobalStatic.CrossThread_Winfom(this
            , new Action(
                delegate ()
                {
                    this.listLog.Items.Add(sb.ToString());

                    this.listLog.SelectedIndex = listLog.Items.Count - 1;
                    this.listLog.SelectedIndex = -1;
                }));
    }

    private void tsmiUserListCall_Click(object sender, EventArgs e)
    {
        //���� ����Ʈ ���� ��û
        GlobalStatic.MainClient
            .SendMsg(ChatCommandType.User_List_Get, "");
    }

    private void tsmiSendMessage_Click(object sender, EventArgs e)
    {

    }
}