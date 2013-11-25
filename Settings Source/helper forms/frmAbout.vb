Public Class frmAbout

    Private Sub frmAbout_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblVersion.Text = "Version: " & My.Application.Info.Version.Major.ToString + "." + My.Application.Info.Version.Minor.ToString

        Me.BackColor = Theme.ColorTable.TabSelectedGlow
        Me.ForeColor = Theme.ColorTable.Text

        lblBackground.ActiveLinkColor = Theme.ColorTable.PanelDarkBorder
        lblIcons.ActiveLinkColor = Theme.ColorTable.PanelDarkBorder
        lblRibbon.ActiveLinkColor = Theme.ColorTable.PanelDarkBorder
        lblTabs.ActiveLinkColor = Theme.ColorTable.PanelDarkBorder

        lblBackground.LinkColor = Theme.ColorTable.Text
        lblIcons.LinkColor = Theme.ColorTable.Text
        lblRibbon.LinkColor = Theme.ColorTable.Text
        lblTabs.LinkColor = Theme.ColorTable.Text

        lblBackground.VisitedLinkColor = Theme.ColorTable.Text
        lblIcons.VisitedLinkColor = Theme.ColorTable.Text
        lblRibbon.VisitedLinkColor = Theme.ColorTable.Text
        lblTabs.VisitedLinkColor = Theme.ColorTable.Text

    End Sub

    Private Sub lblLink_Click(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblBackground.LinkClicked, lblIcons.LinkClicked, lblRibbon.LinkClicked, lblTabs.LinkClicked
        Process.Start(CType(sender, LinkLabel).Tag.ToString)
    End Sub
End Class