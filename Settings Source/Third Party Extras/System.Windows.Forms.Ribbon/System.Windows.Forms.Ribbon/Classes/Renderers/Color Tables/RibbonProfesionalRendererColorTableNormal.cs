// *********************************
// Message from Original Author:
//
// 2008 Jose Menendez Poo
// Please give me credit if you use this code. It's all I ask.
// Contact me for more info: menendezpoo@gmail.com
// *********************************
//
// Original project from http://ribbon.codeplex.com/
// Continue to support and maintain by http://officeribbon.codeplex.com/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms
{

    public class RibbonProfesionalRendererColorTableNormal
        : RibbonProfesionalRendererColorTable
    {

        string m_highlight = "#335463";
        string m_backgroundColor = "#000000";
        string m_lightGray = "#D0D0D0";
        string m_medGray = "#757575";
        string m_textColor = "#F8F8F2";
        string m_transBack = "#82000000";

        public RibbonProfesionalRendererColorTableNormal()
        {
            ThemeName = "Normal";
            ThemeAuthor = "";
            ThemeAuthorWebsite = "";
            ThemeAuthorEmail = "";
            ThemeDateCreated = "";

            OrbDropDownDarkBorder = FromHex(m_backgroundColor);
            OrbDropDownLightBorder = FromHex(m_textColor);
            OrbDropDownBack = FromHex(m_backgroundColor);
            OrbDropDownNorthA = FromHex(m_backgroundColor);
            OrbDropDownNorthB = FromHex(m_backgroundColor);
            OrbDropDownNorthC = FromHex(m_backgroundColor);
            OrbDropDownNorthD = FromHex(m_backgroundColor);
            OrbDropDownSouthC = FromHex(m_backgroundColor);
            OrbDropDownSouthD = FromHex(m_backgroundColor);
            OrbDropDownContentbg = FromHex(m_backgroundColor);
            OrbDropDownContentbglight = FromHex(m_backgroundColor);
            OrbDropDownSeparatorlight = FromHex(m_textColor);
            OrbDropDownSeparatordark = FromHex(m_backgroundColor);
            Caption1 = FromHex(m_textColor);
            Caption2 = FromHex(m_textColor);
            Caption3 = FromHex(m_textColor);
            Caption4 = FromHex(m_textColor);
            Caption5 = FromHex(m_textColor);
            Caption6 = FromHex(m_textColor);
            Caption7 = FromHex(m_textColor);
            QuickAccessBorderDark = FromHex(m_backgroundColor);
            QuickAccessBorderLight = FromHex(m_lightGray);
            QuickAccessUpper = FromHex(m_medGray);
            QuickAccessLower = FromHex(m_medGray);
            OrbOptionBorder = FromHex(m_lightGray);
            OrbOptionBackground = FromHex(m_medGray);
            OrbOptionShine = FromHex(m_medGray);
            Arrow = FromHex(m_medGray);
            ArrowLight = FromHex(m_lightGray);
            ArrowDisabled = FromHex("#B7B7B7");
            Text = FromHex(m_textColor);
            RibbonBackground = FromHex(m_backgroundColor);
            TabBorder = FromHex(m_textColor);

            TabNorth = FromHex(m_highlight);
            TabSouth = FromHex(m_highlight);
            TabGlow = FromHex(m_highlight);
            TabText = FromHex(m_textColor);

            TabActiveText = FromHex(m_backgroundColor);
            TabContentNorth = FromHex(m_medGray);
            TabContentSouth = FromHex("#B7B7B7");
            TabSelectedGlow = FromHex(m_highlight);

            PanelDarkBorder = FromHex(m_transBack);
            PanelLightBorder = FromHex(m_lightGray);
            PanelTextBackground = FromHex(m_backgroundColor);
            PanelTextBackgroundSelected = FromHex(m_medGray);
            PanelText = FromHex(m_textColor);
            PanelBackgroundSelected = FromHex(m_medGray);
            PanelOverflowBackground = FromHex(m_highlight);
            PanelOverflowBackgroundPressed = FromHex(m_highlight);
            PanelOverflowBackgroundSelectedNorth = FromHex(m_medGray);
            PanelOverflowBackgroundSelectedSouth = FromHex(m_medGray);

            ButtonBgOut = FromHex(m_medGray);
            ButtonBgCenter = FromHex(m_transBack);
            ButtonBorderOut = FromHex(m_lightGray);
            ButtonBorderIn = FromHex(m_medGray);
            ButtonGlossyNorth = FromHex(m_lightGray);
            ButtonGlossySouth = FromHex(m_medGray);

            ButtonDisabledBgOut = FromHex(m_medGray);
            ButtonDisabledBgCenter = FromHex(m_medGray);
            ButtonDisabledBorderOut = FromHex(m_lightGray);
            ButtonDisabledBorderIn = FromHex(m_medGray);
            ButtonDisabledGlossyNorth = FromHex(m_lightGray);
            ButtonDisabledGlossySouth = FromHex(m_lightGray);

            ButtonSelectedBgOut = FromHex(m_medGray);
            ButtonSelectedBgCenter = FromHex(m_backgroundColor);
            ButtonSelectedBorderOut = FromHex(m_medGray);
            ButtonSelectedBorderIn = FromHex(m_medGray);
            ButtonSelectedGlossyNorth = FromHex(m_lightGray);
            ButtonSelectedGlossySouth = FromHex(m_medGray);

            ButtonPressedBgOut = FromHex(m_medGray);
            ButtonPressedBgCenter = FromHex(m_backgroundColor);
            ButtonPressedBorderOut = FromHex(m_lightGray);
            ButtonPressedBorderIn = FromHex(m_medGray);
            ButtonPressedGlossyNorth = FromHex(m_lightGray);
            ButtonPressedGlossySouth = FromHex(m_medGray);

            ButtonCheckedBgOut = FromHex(m_medGray);
            ButtonCheckedBgCenter = FromHex(m_backgroundColor);
            ButtonCheckedBorderOut = FromHex(m_lightGray);
            ButtonCheckedBorderIn = FromHex(m_medGray);
            ButtonCheckedGlossyNorth = FromHex(m_lightGray);
            ButtonCheckedGlossySouth = FromHex(m_medGray);

            ItemGroupOuterBorder = FromHex(m_lightGray);
            ItemGroupInnerBorder = FromHex(m_medGray);
            ItemGroupSeparatorLight = FromHex(m_lightGray);
            ItemGroupSeparatorDark = FromHex(m_medGray);
            ItemGroupBgNorth = FromHex(m_backgroundColor);
            ItemGroupBgSouth = FromHex(m_backgroundColor);
            ItemGroupBgGlossy = FromHex(m_medGray);
            ButtonListBorder = FromHex(m_lightGray);
            ButtonListBg = FromHex(m_backgroundColor);
            ButtonListBgSelected = FromHex(m_lightGray);
            DropDownBg = FromHex(m_highlight);
            DropDownImageBg = FromHex(m_backgroundColor);
            DropDownImageSeparator = FromHex(m_lightGray);
            DropDownBorder = FromHex(m_lightGray);
            DropDownGripNorth = FromHex(m_lightGray);
            DropDownGripSouth = FromHex(m_lightGray);
            DropDownGripBorder = FromHex(m_medGray);
            DropDownGripDark = FromHex(m_backgroundColor);
            DropDownGripLight = FromHex(m_textColor);
            SeparatorLight = FromHex(m_lightGray);
            SeparatorDark = FromHex(m_medGray);
            SeparatorBg = FromHex(m_backgroundColor);
            SeparatorLine = FromHex(m_medGray);
            TextBoxUnselectedBg = FromHex(m_medGray);
            TextBoxBorder = FromHex(m_lightGray);
            ToolTipContentNorth = FromHex(m_medGray);
            ToolTipContentSouth = FromHex(m_backgroundColor);
            ToolTipDarkBorder = FromHex(m_medGray);
            ToolTipLightBorder = FromHex(m_lightGray);
            ToolStripItemTextPressed = FromHex(m_backgroundColor);
            ToolStripItemTextSelected = FromHex(m_backgroundColor);
            ToolStripItemText = FromHex(m_backgroundColor);
            clrVerBG_Shadow = FromHex("#FFFFFF");

            ButtonPressed_2013 = FromHex(m_highlight);
            ButtonSelected_2013 = FromHex(m_highlight);

            OrbButton_2013 = FromHex(m_highlight);
            OrbButtonSelected_2013 = FromHex(m_highlight);
            OrbButtonPressed_2013 = FromHex(m_highlight);

            TabText_2013 = FromHex(m_textColor);
            TabTextSelected_2013 = FromHex(m_textColor);

            PanelBorder_2013 = FromHex(m_medGray);
            RibbonBackground_2013 = FromHex(m_backgroundColor);
            TabCompleteBackground_2013 = FromHex(m_backgroundColor);
            TabNormalBackground_2013 = FromHex(m_backgroundColor);
            TabActiveBackground_2013 = FromHex(m_highlight);
            TabBorder_2013 = FromHex(m_medGray);
            TabCompleteBorder_2013 = FromHex(m_medGray);
            TabActiveBorder_2013 = FromHex(m_highlight);
            OrbButtonText_2013 = FromHex(m_backgroundColor);
            PanelText_2013 = FromHex(m_textColor);
            RibbonItemText_2013 = FromHex(m_textColor);
            ToolTipText_2013 = FromHex(m_textColor);
            ToolStripItemTextPressed_2013 = FromHex(m_medGray);
            ToolStripItemTextSelected_2013 = FromHex(m_medGray);
            ToolStripItemText_2013 = FromHex(m_textColor);
        }

    }
}
