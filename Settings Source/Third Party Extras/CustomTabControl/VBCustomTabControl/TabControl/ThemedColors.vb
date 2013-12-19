'
' * This code is provided under the Code Project Open Licence (CPOL)
' * See http://www.codeproject.com/info/cpol10.aspx for details
'


Imports System.Drawing
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles

Namespace System.Drawing

	Friend NotInheritable Class ThemedColors

		#Region "    Variables and Constants "

		Private Const NormalColor As String = "NormalColor"
		Private Const HomeStead As String = "HomeStead"
		Private Const Metallic As String = "Metallic"
		Private Const NoTheme As String = "NoTheme"

		Private Shared _toolBorder As Color()
		#End Region

		#Region "    Properties "

		<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")> _
		Public Shared ReadOnly Property CurrentThemeIndex() As ColorScheme
			Get
				Return ThemedColors.GetCurrentThemeIndex()
			End Get
		End Property

		<System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")> _
		Public Shared ReadOnly Property ToolBorder() As Color
			Get
				Return ThemedColors._toolBorder(CInt(ThemedColors.CurrentThemeIndex))
			End Get
		End Property

		#End Region

		#Region "    Constructors "

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")> _
		Shared Sub New()
			ThemedColors._toolBorder = New Color() {Color.FromArgb(127, 157, 185), Color.FromArgb(164, 185, 127), Color.FromArgb(165, 172, 178), Color.FromArgb(132, 130, 132)}
		End Sub

		Private Sub New()
		End Sub

		#End Region

		Private Shared Function GetCurrentThemeIndex() As ColorScheme
			Dim theme As ColorScheme = ColorScheme.NoTheme

			If VisualStyleInformation.IsSupportedByOS AndAlso VisualStyleInformation.IsEnabledByUser AndAlso Application.RenderWithVisualStyles Then


				Select Case VisualStyleInformation.ColorScheme
					Case NormalColor
						theme = ColorScheme.NormalColor
						Exit Select
					Case HomeStead
						theme = ColorScheme.HomeStead
						Exit Select
					Case Metallic
						theme = ColorScheme.Metallic
						Exit Select
					Case Else
						theme = ColorScheme.NoTheme
						Exit Select
				End Select
			End If

			Return theme
		End Function

		Public Enum ColorScheme
			NormalColor = 0
			HomeStead = 1
			Metallic = 2
			NoTheme = 3
		End Enum

	End Class

End Namespace
