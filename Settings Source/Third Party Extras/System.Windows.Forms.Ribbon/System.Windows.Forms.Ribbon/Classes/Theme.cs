using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class Theme
    {
        public static bool blnRenderOnly = false;
        public static bool blnSetOnly = false;

        //public static Ribbon MainRibbon;
        //public static ProToolstripRenderer MainToolstripRenderer;

        private static RibbonProfesionalRendererColorTable _colorTable = new RibbonProfesionalRendererColorTable();
        public static RibbonProfesionalRendererColorTable ColorTable
        {
            get { return _colorTable; }
            set { _colorTable = value; }
        }

        private static RibbonOrbStyle _ThemeStyle;
        public static RibbonOrbStyle ThemeStyle
        {
            get { return _ThemeStyle; }
            set { _ThemeStyle = value; }
        }


        //public static RibbonOrbStyle ThemeStyle
        //{
        //    get { return _ThemeStyle; }
        //    set
        //    {
        //        _ThemeStyle = value;
        //        //if (blnSetOnly == false) {MainRibbon.OrbStyle = _ThemeStyle;}//08/01/2013 Michael Spradlin - Changed to Code Below because if MainRibbon was set to null it was blowing up and you should still be able to set the theme even if you are not using the ribbon control.
        //        if (blnSetOnly == false && MainRibbon != null)
        //        {
        //            MainRibbon.OrbStyle = _ThemeStyle;
        //        }
        //    }
        //}

        private static RibbonTheme _Theme = RibbonTheme.Blue;
        public static RibbonTheme ThemeColor
        {
            get { return _Theme; }
            set
            {
                _Theme = value;
                //if (blnSetOnly == false) {MainRibbon.ThemeColor = _Theme;} //08/01/2013 Michael Spradlin - Changed to Code Below because if MainRibbon was set to null it was blowing up and you should still be able to set the theme even if you are not using the ribbon control.
                //if (blnSetOnly == false)// && MainRibbon != null)                               
                 
                    //MainRibbon.ThemeColor = _Theme;
                if (ThemeColor == RibbonTheme.Normal)
                    ColorTable = new RibbonProfesionalRendererColorTableNormal();
                else if (ThemeColor == RibbonTheme.Blue)
                    ColorTable = new RibbonProfesionalRendererColorTable();
                else if (ThemeColor == RibbonTheme.Black)
                    ColorTable = new RibbonProfesionalRendererColorTableBlack();
                else if (ThemeColor == RibbonTheme.Green)
                    ColorTable = new RibbonProfesionalRendererColorTableGreen();
                else if (ThemeColor == RibbonTheme.Purple)
                    ColorTable = new RibbonProfesionalRendererColorTablePurple();
                else if (ThemeColor == RibbonTheme.JellyBelly)
                    ColorTable = new RibbonProfesionalRendererColorTableJellyBelly();
                else if (ThemeColor == RibbonTheme.Halloween)
                    ColorTable = new RibbonProfesionalRendererColorTableHalloween();
                

                //System.Windows.Forms.ToolStripColors.SetUpThemeColors(blnRenderOnly);
            }
        }
    }
}