Public Class utils


    Public Shared Sub initControls(ByVal parentControl As Control, ByRef toolTipMaker As ToolTip, ByVal loadSetting As Boolean, ByVal loadTooltip As Boolean, ByVal loadTheme As Boolean)
        For Each c As Control In parentControl.Controls

            If loadTheme Then
                Dim cTheme As iTheme = TryCast(c, iTheme)
                If cTheme IsNot Nothing Then
                    cTheme.applyTheme()
                Else
                    formatControl(c)
                End If

            End If

            If loadTooltip Then
                Dim cTool As iTooltip = TryCast(c, iTooltip)
                If cTool IsNot Nothing AndAlso toolTipMaker IsNot Nothing Then
                    Dim tt As String = toolTipMaker.GetToolTip(cTool)
                    Dim conTooltip As String = cTool.getToolTip
                    If tt.ToString = "" Then
                        tt = conTooltip
                    Else
                        tt = conTooltip & vbCrLf & vbCrLf & tt
                    End If
                    toolTipMaker.SetToolTip(cTool, tt)
                End If
            End If

            If loadSetting Then
                Dim conOpt As iToken = TryCast(c, iToken)
                If conOpt IsNot Nothing Then If c.Enabled Then conOpt.loadOption() 'only load options of enabled controls
            End If

            If c.HasChildren Then                
                initControls(c, toolTipMaker, loadSetting, loadTooltip, loadTheme)                
            End If
        Next
    End Sub

    Public Shared Sub formatControl(ByVal c As Control)
        'this formats any non-custom controls with the ribbon's theme colors
        'currently groupboxes and labels only have their forecolor changed,
        'since solid backgrounds hides our beautiful background image

        Select Case c.GetType
            Case GetType(Button)
                Dim btn As Button = DirectCast(c, Button)
                btn.ForeColor = Theme.ColorTable.Text

                btn.FlatAppearance.MouseOverBackColor = Theme.ColorTable.ButtonSelected_2013
                btn.FlatAppearance.MouseDownBackColor = Theme.ColorTable.ButtonSelected_2013
                btn.FlatAppearance.BorderSize = 0

                If Theme.ThemeColor <> RibbonTheme.Normal Then
                    btn.BackgroundImage = Nothing
                    btn.BackgroundImageLayout = ImageLayout.None
                    btn.BackColor = Theme.ColorTable.RibbonBackground_2013
                    btn.FlatAppearance.CheckedBackColor = Theme.ColorTable.RibbonBackground_2013
                Else
                    btn.BackgroundImage = My.Resources.transp_1
                    btn.BackgroundImageLayout = ImageLayout.Tile
                    btn.BackColor = Color.Transparent
                    btn.FlatAppearance.CheckedBackColor = Color.Transparent
                End If

            Case GetType(Label)
                c.ForeColor = Theme.ColorTable.Caption1
                c.BackColor = Color.Transparent

            Case GetType(GroupBox), GetType(Panel)
                c.ForeColor = Theme.ColorTable.Caption1
                If Theme.ThemeColor <> RibbonTheme.Normal Then
                    c.BackgroundImage = Nothing
                    c.BackgroundImageLayout = ImageLayout.None
                    c.BackColor = Theme.ColorTable.PanelDarkBorder
                Else
                    c.BackgroundImage = My.Resources.transp_1
                    c.BackColor = Color.Transparent
                    c.BackgroundImageLayout = ImageLayout.Tile
                End If

            Case GetType(ComboBox)
                Dim cb As ComboBox = DirectCast(c, ComboBox)
                cb.ForeColor = Theme.ColorTable.Text
                cb.BackColor = Theme.ColorTable.ButtonPressed_2013
                cb.FlatStyle = FlatStyle.Flat

            Case GetType(KRBTabControl.KRBTabControl)
                Dim tabMain As KRBTabControl.KRBTabControl = CType(c, KRBTabControl.KRBTabControl)

                If Theme.ThemeColor = RibbonTheme.Normal Then

                Else

                End If

                'If Theme.ThemeColor = RibbonTheme.Normal Then
                '    tabMain.DisplayStyle = TabStyle.VS2010
                '    tabMain.DisplayStyleProvider.BorderColor = Theme.ColorTable.RibbonBackground_2013
                '    tabMain.DisplayStyleProvider.Radius = 1
                'Else
                '    tabMain.DisplayStyle = TabStyle.Angled
                '    tabMain.DisplayStyleProvider.BorderColor = Theme.ColorTable.PanelDarkBorder
                'End If

                'tabMain.DisplayStyleProvider.SelectedTabColor = Theme.ColorTable.TabActiveBackground_2013
                'tabMain.DisplayStyleProvider.SelectedTabColorTop = Theme.ColorTable.TabActiveBackground_2013

                'tabMain.DisplayStyleProvider.TextColor = Theme.ColorTable.TabText_2013
                'tabMain.DisplayStyleProvider.TextColorDisabled = Theme.ColorTable.TabText_2013
                'tabMain.DisplayStyleProvider.TextColorSelected = Theme.ColorTable.TabText_2013

                'tabMain.DisplayStyleProvider.ShowTabCloser = False
                'tabMain.DisplayStyleProvider.HotTrack = False
                'tabMain.DisplayStyleProvider.FocusTrack = False
        End Select
    End Sub

End Class
