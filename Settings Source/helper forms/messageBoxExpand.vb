Imports System.Windows.Forms

Public Class messageBoxExpand

    Private m_expanded As Boolean

    Private m_okResult As DialogResult
    Private m_cancelResult As DialogResult
    Private m_ignoreResult As DialogResult

    Private m_tt As New ToolTip

    Public Sub New(ByVal formTitle As String, ByVal messageTitle As String, ByVal iconType As MessageBoxIcon, ByVal message As String, ByVal buttons As MessageBoxButtons, Optional ByVal details As String = "")

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = formTitle
        lblTitle.Text = messageTitle : m_tt.SetToolTip(lblTitle, messageTitle)
        lblMessage.Text = message : m_tt.SetToolTip(lblMessage, message)
        txtDetails.Text = details
        If details = "" Then lblExpand.Visible = False
        setIcon(iconType)
        setButtons(buttons)
        txtDetails.ReadOnly = True
    End Sub

    Private Sub setIcon(ByVal iconType As MessageBoxIcon)
        Select Case iconType
            Case MessageBoxIcon.Error, MessageBoxIcon.Stop
                messageIcon.Image = My.Resources.cross_circle
            Case MessageBoxIcon.Exclamation, MessageBoxIcon.Warning
                messageIcon.Image = My.Resources.exclamation
            Case MessageBoxIcon.Question
                messageIcon.Image = My.Resources.question
            Case MessageBoxIcon.Information
                messageIcon.Image = My.Resources.information_large
            Case MessageBoxIcon.Asterisk
                messageIcon.Image = My.Resources.tick            
        End Select
    End Sub

    Private Sub setButtons(ByVal buttons As MessageBoxButtons)
        Select Case buttons
            Case MessageBoxButtons.OK
                Cancel_Button.Visible = False : Ignore_Button.Visible = False
            Case MessageBoxButtons.RetryCancel
                OK_Button.Text = "Retry" : m_okResult = Windows.Forms.DialogResult.Retry
                Ignore_Button.Visible = False
            Case MessageBoxButtons.YesNo
                OK_Button.Text = "Yes" : Cancel_Button.Text = "No" : m_okResult = Windows.Forms.DialogResult.Yes : m_cancelResult = Windows.Forms.DialogResult.No
                Ignore_Button.Visible = False
            Case MessageBoxButtons.YesNoCancel
                OK_Button.Text = "Yes" : Cancel_Button.Text = "No" : Ignore_Button.Text = "Cancel"
                m_okResult = Windows.Forms.DialogResult.Yes : m_cancelResult = Windows.Forms.DialogResult.No : m_ignoreResult = Windows.Forms.DialogResult.Cancel                
            Case MessageBoxButtons.AbortRetryIgnore
                OK_Button.Text = "Retry" : Cancel_Button.Text = "Abort"
                m_okResult = Windows.Forms.DialogResult.Retry : m_cancelResult = Windows.Forms.DialogResult.Abort : m_ignoreResult = Windows.Forms.DialogResult.Ignore
        End Select
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = m_okResult
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = m_cancelResult
        Me.Close()
    End Sub

    Private Sub Ignore_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ignore_Button.Click
        Me.DialogResult = m_ignoreResult
        Me.Close()
    End Sub

    Private Sub lblExpand_Click(sender As Object, e As EventArgs) Handles lblExpand.Click
        If Not m_expanded Then
            Me.Height += 200
            lblExpand.Text = "Hide Details"
            m_expanded = True
        Else            
            Me.Height -= 200
            lblExpand.Text = "Show Details"
            m_expanded = False
        End If
    End Sub
End Class
