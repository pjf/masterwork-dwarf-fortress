'
' * Created by SharpDevelop.
' * User: mjackson
' * Date: 28/06/2010
' * Time: 13:38
' * 
' * To change this template use Tools | Options | Coding | Edit Standard Headers.
' 

Imports System.Windows.Forms

''' <summary>
''' Class with program entry point.
''' </summary>
Friend NotInheritable Class Program
	''' <summary>
	''' Program entry point.
	''' </summary>
	<STAThread> _
	Friend Shared Sub Main()
		Application.EnableVisualStyles()
		Application.SetCompatibleTextRenderingDefault(False)
		Application.Run(New MainForm())
	End Sub



End Class
