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

        string highlight = "#335463";
        string m_backgroundColor = "#000000";
        string lightGray = "#D0D0D0";
        string medGray = "#757575";
        string textColor = "#F8F8F2";

        public RibbonProfesionalRendererColorTableNormal()
        {
            ThemeName = "Normal";
            ThemeAuthor = "";
            ThemeAuthorWebsite = "";
            ThemeAuthorEmail = "";
            ThemeDateCreated = "";

            OrbDropDownDarkBorder = FromHex(m_backgroundColor);
            OrbDropDownLightBorder = FromHex(textColor);
            OrbDropDownBack = FromHex(m_backgroundColor);
            OrbDropDownNorthA = FromHex(m_backgroundColor);
            OrbDropDownNorthB = FromHex(m_backgroundColor);
            OrbDropDownNorthC = FromHex(m_backgroundColor);
            OrbDropDownNorthD = FromHex(m_backgroundColor);
            OrbDropDownSouthC = FromHex(m_backgroundColor);
            OrbDropDownSouthD = FromHex(m_backgroundColor);
            OrbDropDownContentbg = FromHex(m_backgroundColor);
            OrbDropDownContentbglight = FromHex(m_backgroundColor);
            OrbDropDownSeparatorlight = FromHex(textColor);
            OrbDropDownSeparatordark = FromHex(m_backgroundColor);
            Caption1 = FromHex(textColor);
            Caption2 = FromHex(textColor);
            Caption3 = FromHex(textColor);
            Caption4 = FromHex(textColor);
            Caption5 = FromHex(textColor);
            Caption6 = FromHex(textColor);
            Caption7 = FromHex(textColor);
            QuickAccessBorderDark = FromHex(m_backgroundColor);
            QuickAccessBorderLight = FromHex(lightGray);
            QuickAccessUpper = FromHex(medGray);
            QuickAccessLower = FromHex(medGray);
            OrbOptionBorder = FromHex(lightGray);
            OrbOptionBackground = FromHex(medGray);
            OrbOptionShine = FromHex(medGray);
            Arrow = FromHex(medGray);
            ArrowLight = FromHex(lightGray);
            ArrowDisabled = FromHex("#B7B7B7");
            Text = FromHex(textColor);
            RibbonBackground = FromHex(m_backgroundColor);
            TabBorder = FromHex(textColor);

            TabNorth = FromHex(highlight);
            TabSouth = FromHex(highlight);
            TabGlow = FromHex(highlight);
            TabText = FromHex(textColor);

            TabActiveText = FromHex(m_backgroundColor);
            TabContentNorth = FromHex(medGray);
            TabContentSouth = FromHex("#B7B7B7");
            TabSelectedGlow = FromHex(highlight);
            PanelDarkBorder = FromHex(medGray);
            PanelLightBorder = FromHex(lightGray);
            PanelTextBackground = FromHex(m_backgroundColor);
            PanelTextBackgroundSelected = FromHex(medGray);
            PanelText = FromHex(textColor);
            PanelBackgroundSelected = FromHex(medGray);
            PanelOverflowBackground = FromHex(highlight);
            PanelOverflowBackgroundPressed = FromHex(highlight);
            PanelOverflowBackgroundSelectedNorth = FromHex(medGray);
            PanelOverflowBackgroundSelectedSouth = FromHex(medGray);

            ButtonBgOut = FromHex(medGray);
            ButtonBgCenter = FromHex(m_backgroundColor);
            ButtonBorderOut = FromHex(lightGray);
            ButtonBorderIn = FromHex(medGray);
            ButtonGlossyNorth = FromHex(lightGray);
            ButtonGlossySouth = FromHex(medGray);

            ButtonDisabledBgOut = FromHex(medGray);
            ButtonDisabledBgCenter = FromHex(medGray);
            ButtonDisabledBorderOut = FromHex(lightGray);
            ButtonDisabledBorderIn = FromHex(medGray);
            ButtonDisabledGlossyNorth = FromHex(lightGray);
            ButtonDisabledGlossySouth = FromHex(lightGray);

            ButtonSelectedBgOut = FromHex(medGray);
            ButtonSelectedBgCenter = FromHex(m_backgroundColor);
            ButtonSelectedBorderOut = FromHex(medGray);
            ButtonSelectedBorderIn = FromHex(medGray);
            ButtonSelectedGlossyNorth = FromHex(lightGray);
            ButtonSelectedGlossySouth = FromHex(medGray);

            ButtonPressedBgOut = FromHex(medGray);
            ButtonPressedBgCenter = FromHex(m_backgroundColor);
            ButtonPressedBorderOut = FromHex(lightGray);
            ButtonPressedBorderIn = FromHex(medGray);
            ButtonPressedGlossyNorth = FromHex(lightGray);
            ButtonPressedGlossySouth = FromHex(medGray);

            ButtonCheckedBgOut = FromHex(medGray);
            ButtonCheckedBgCenter = FromHex(m_backgroundColor);
            ButtonCheckedBorderOut = FromHex(lightGray);
            ButtonCheckedBorderIn = FromHex(medGray);
            ButtonCheckedGlossyNorth = FromHex(lightGray);
            ButtonCheckedGlossySouth = FromHex(medGray);

            ItemGroupOuterBorder = FromHex(lightGray);
            ItemGroupInnerBorder = FromHex(medGray);
            ItemGroupSeparatorLight = FromHex(lightGray);
            ItemGroupSeparatorDark = FromHex(medGray);
            ItemGroupBgNorth = FromHex(m_backgroundColor);
            ItemGroupBgSouth = FromHex(m_backgroundColor);
            ItemGroupBgGlossy = FromHex(medGray);
            ButtonListBorder = FromHex(lightGray);
            ButtonListBg = FromHex(m_backgroundColor);
            ButtonListBgSelected = FromHex(lightGray);
            DropDownBg = FromHex(m_backgroundColor);
            DropDownImageBg = FromHex(m_backgroundColor);
            DropDownImageSeparator = FromHex(lightGray);
            DropDownBorder = FromHex(lightGray);
            DropDownGripNorth = FromHex(lightGray);
            DropDownGripSouth = FromHex(lightGray);
            DropDownGripBorder = FromHex(medGray);
            DropDownGripDark = FromHex(m_backgroundColor);
            DropDownGripLight = FromHex(textColor);
            SeparatorLight = FromHex(lightGray);
            SeparatorDark = FromHex(medGray);
            SeparatorBg = FromHex(m_backgroundColor);
            SeparatorLine = FromHex(medGray);
            TextBoxUnselectedBg = FromHex(medGray);
            TextBoxBorder = FromHex(lightGray);
            ToolTipContentNorth = FromHex(medGray);
            ToolTipContentSouth = FromHex(m_backgroundColor);
            ToolTipDarkBorder = FromHex(medGray);
            ToolTipLightBorder = FromHex(lightGray);
            ToolStripItemTextPressed = FromHex(m_backgroundColor);
            ToolStripItemTextSelected = FromHex(m_backgroundColor);
            ToolStripItemText = FromHex(m_backgroundColor);
            clrVerBG_Shadow = FromHex("#FFFFFF");

            ButtonPressed_2013 = FromHex(highlight);
            ButtonSelected_2013 = FromHex(highlight);

            OrbButton_2013 = FromHex(highlight);
            OrbButtonSelected_2013 = FromHex(highlight);
            OrbButtonPressed_2013 = FromHex(highlight);

            TabText_2013 = FromHex(textColor);
            TabTextSelected_2013 = FromHex(textColor);

            PanelBorder_2013 = FromHex(medGray);
            RibbonBackground_2013 = FromHex(m_backgroundColor);
            TabCompleteBackground_2013 = FromHex(m_backgroundColor);
            TabNormalBackground_2013 = FromHex(m_backgroundColor);
            TabActiveBackground_2013 = FromHex(highlight);
            TabBorder_2013 = FromHex(medGray);
            TabCompleteBorder_2013 = FromHex(medGray);
            TabActiveBorder_2013 = FromHex(highlight);
            OrbButtonText_2013 = FromHex(m_backgroundColor);
            PanelText_2013 = FromHex(textColor);
            RibbonItemText_2013 = FromHex(textColor);
            ToolTipText_2013 = FromHex(textColor);
            ToolStripItemTextPressed_2013 = FromHex(medGray);
            ToolStripItemTextSelected_2013 = FromHex(medGray);
            ToolStripItemText_2013 = FromHex(textColor);
        }

    }
}
