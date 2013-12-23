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
                If conOpt IsNot Nothing Then
                    If c.Enabled Then conOpt.loadOption() 'only load options of enabled controls
                End If
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
                    'btn.BackgroundImage = Nothing
                    'btn.BackgroundImageLayout = ImageLayout.None
                    btn.BackColor = Theme.ColorTable.RibbonBackground_2013
                    btn.FlatAppearance.CheckedBackColor = Theme.ColorTable.RibbonBackground_2013
                Else
                    'btn.BackgroundImage = My.Resources.transp_1
                    'btn.BackgroundImageLayout = ImageLayout.Tile
                    btn.BackColor = Drawing.Color.FromArgb(130, 0, 0, 0) 'Color.Transparent
                    btn.FlatAppearance.CheckedBackColor = Drawing.Color.FromArgb(130, 0, 0, 0) 'Color.Transparent
                End If

            Case GetType(Label)
                If Theme.ThemeColor = RibbonTheme.Normal Then
                    c.BackColor = Drawing.Color.FromArgb(0, 0, 0, 0) 'Color.Transparent
                Else
                    If TypeOf c.Parent Is TabPage Then
                        c.BackColor = Drawing.Color.FromArgb(0, 0, 0, 0)
                    Else
                        c.BackColor = c.Parent.BackColor
                    End If
                End If
                c.ForeColor = Theme.ColorTable.Caption1

            Case GetType(GroupBox), GetType(Panel), GetType(mwGroupBox)
                c.ForeColor = Theme.ColorTable.Caption1
                If Theme.ThemeColor <> RibbonTheme.Normal Then
                    'c.BackgroundImage = Nothing
                    'c.BackgroundImageLayout = ImageLayout.None
                    c.BackColor = Theme.ColorTable.PanelDarkBorder
                Else
                    'c.BackgroundImage = My.Resources.transp_1
                    c.BackColor = Drawing.Color.FromArgb(130, 0, 0, 0) 'Color.Transparent
                    'c.BackgroundImageLayout = ImageLayout.Tile
                End If

            Case GetType(ComboBox)
                    Dim cb As ComboBox = DirectCast(c, ComboBox)
                    cb.ForeColor = Theme.ColorTable.Text
                    cb.BackColor = Theme.ColorTable.DropDownBg
                    cb.FlatStyle = FlatStyle.Flat

            Case GetType(KRBTabControl.KRBTabControl)
                    Dim tabMain As KRBTabControl.KRBTabControl = CType(c, KRBTabControl.KRBTabControl)

                    If Theme.ThemeColor = RibbonTheme.Normal Then
                        tabMain.BorderColor = Drawing.Color.FromArgb(0, 0, 0, 0) 'Color.Transparent
                        tabMain.BackgroundColor = Drawing.Color.FromArgb(0, 0, 0, 0) 'Color.Transparent
                        tabMain.TabBorderColor = Drawing.Color.FromArgb(0, 0, 0, 0) 'Color.Transparent
                    Else
                        tabMain.BorderColor = Theme.ColorTable.PanelBorder_2013
                        tabMain.BackgroundColor = Theme.ColorTable.RibbonBackground_2013
                        tabMain.TabBorderColor = Theme.ColorTable.TabText_2013
                    End If



                    tabMain.TabGradient.ColorStart = Theme.ColorTable.TabActiveBackground_2013
                    tabMain.TabGradient.ColorEnd = Theme.ColorTable.TabActiveBackground_2013

                    tabMain.TabGradient.TabPageSelectedTextColor = Theme.ColorTable.Caption1
                    tabMain.TabGradient.TabPageTextColor = Theme.ColorTable.TabText_2013
        End Select
    End Sub

End Class
