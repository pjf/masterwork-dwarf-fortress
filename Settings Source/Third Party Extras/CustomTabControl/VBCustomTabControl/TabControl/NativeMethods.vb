'
' * This code is provided under the Code Project Open Licence (CPOL)
' * See http://www.codeproject.com/info/cpol10.aspx for details
'


Imports System.Drawing
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports System.Windows.Forms

Namespace System.Windows.Forms
	''' <summary>
	''' Description of NativeMethods.
	''' </summary>
	'[SecurityPermission(SecurityAction.Assert, Flags=SecurityPermissionFlag.UnmanagedCode)]
	Friend NotInheritable Class NativeMethods
		Private Sub New()
		End Sub

		#Region "Windows Constants"

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Const WM_GETTABRECT As Integer = &H130a
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Const WS_EX_TRANSPARENT As Integer = &H20
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Const WM_SETFONT As Integer = &H30
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Const WM_FONTCHANGE As Integer = &H1d
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Const WM_HSCROLL As Integer = &H114
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Const TCM_HITTEST As Integer = &H130d
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Const WM_PAINT As Integer = &Hf
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Const WS_EX_LAYOUTRTL As Integer = &H400000
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Const WS_EX_NOINHERITLAYOUT As Integer = &H100000

		#End Region

		#Region "Content Alignment"

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Shared ReadOnly AnyRightAlign As ContentAlignment = ContentAlignment.BottomRight Or ContentAlignment.MiddleRight Or ContentAlignment.TopRight
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Shared ReadOnly AnyLeftAlign As ContentAlignment = ContentAlignment.BottomLeft Or ContentAlignment.MiddleLeft Or ContentAlignment.TopLeft
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Shared ReadOnly AnyTopAlign As ContentAlignment = ContentAlignment.TopRight Or ContentAlignment.TopCenter Or ContentAlignment.TopLeft
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Shared ReadOnly AnyBottomAlign As ContentAlignment = ContentAlignment.BottomRight Or ContentAlignment.BottomCenter Or ContentAlignment.BottomLeft
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Shared ReadOnly AnyMiddleAlign As ContentAlignment = ContentAlignment.MiddleRight Or ContentAlignment.MiddleCenter Or ContentAlignment.MiddleLeft
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
		Public Shared ReadOnly AnyCenterAlign As ContentAlignment = ContentAlignment.BottomCenter Or ContentAlignment.MiddleCenter Or ContentAlignment.TopCenter

		#End Region

		#Region "User32.dll"

		'        [DllImport("user32.dll"), SecurityPermission(SecurityAction.Demand)]
		'		public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

		Public Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
			'	This Method replaces the User32 method SendMessage, but will only work for sending
			'	messages to Managed controls.
			Dim control__1 As Control = Control.FromHandle(hWnd)
			If control__1 Is Nothing Then
				Return IntPtr.Zero
			End If

			Dim message As New Message()
			message.HWnd = hWnd
			message.LParam = lParam
			message.WParam = wParam
			message.Msg = msg

			Dim wproc As MethodInfo = control__1.[GetType]().GetMethod("WndProc", BindingFlags.NonPublic Or BindingFlags.InvokeMethod Or BindingFlags.FlattenHierarchy Or BindingFlags.IgnoreCase Or BindingFlags.Instance)

			Dim args As Object() = New Object() {message}
			wproc.Invoke(control__1, args)

			Return CType(args(0), Message).Result
		End Function


		'		[DllImport("user32.dll")]
		'		public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT paintStruct);
		'		
		'		[DllImport("user32.dll")]
		'		[return: MarshalAs(UnmanagedType.Bool)]
		'		public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT paintStruct);
		'
		#End Region

		#Region "Misc Functions"

		Public Shared Function LoWord(dWord As IntPtr) As Integer
			Return dWord.ToInt32() And &Hffff
		End Function

		Public Shared Function HiWord(dWord As IntPtr) As Integer
			If (dWord.ToInt32() And &H80000000UI) = &H80000000UI Then
				Return (dWord.ToInt32() >> 16)
			Else
				Return (dWord.ToInt32() >> 16) And &Hffff
			End If
		End Function

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2106:SecureAsserts")> _
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")> _
		Public Shared Function ToIntPtr([structure] As Object) As IntPtr
			Dim lparam As IntPtr = IntPtr.Zero
			lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf([structure]))
			Marshal.StructureToPtr([structure], lparam, False)
			Return lparam
		End Function


		#End Region

		#Region "Windows Structures and Enums"

		<Flags> _
		Public Enum TCHITTESTFLAGS
			TCHT_NOWHERE = 1
			TCHT_ONITEMICON = 2
			TCHT_ONITEMLABEL = 4
			TCHT_ONITEM = TCHT_ONITEMICON Or TCHT_ONITEMLABEL
		End Enum



		<StructLayout(LayoutKind.Sequential)> _
		Public Structure TCHITTESTINFO

			Public Sub New(location As Point)
				pt = location
				flags = TCHITTESTFLAGS.TCHT_ONITEM
			End Sub

			Public pt As Point
			Public flags As TCHITTESTFLAGS
		End Structure

		<StructLayout(LayoutKind.Sequential, Pack := 4)> _
		Public Structure PAINTSTRUCT
			Public hdc As IntPtr
			Public fErase As Integer
			Public rcPaint As RECT
			Public fRestore As Integer
			Public fIncUpdate As Integer
			<MarshalAs(UnmanagedType.ByValArray, SizeConst := 32)> _
			Public rgbReserved As Byte()
		End Structure

		<StructLayout(LayoutKind.Sequential)> _
		Public Structure RECT
			Public left As Integer
			Public top As Integer
			Public right As Integer
			Public bottom As Integer

			Public Sub New(left As Integer, top As Integer, right As Integer, bottom As Integer)
				Me.left = left
				Me.top = top
				Me.right = right
				Me.bottom = bottom
			End Sub

			Public Sub New(r As Rectangle)
				Me.left = r.Left
				Me.top = r.Top
				Me.right = r.Right
				Me.bottom = r.Bottom
			End Sub

			Public Shared Function FromXYWH(x As Integer, y As Integer, width As Integer, height As Integer) As RECT
				Return New RECT(x, y, x + width, y + height)
			End Function

			Public Shared Function FromIntPtr(ptr As IntPtr) As RECT
				Dim rect As RECT = CType(Marshal.PtrToStructure(ptr, GetType(RECT)), RECT)
				Return rect
			End Function

			Public ReadOnly Property Size() As Size
				Get
					Return New Size(Me.right - Me.left, Me.bottom - Me.top)
				End Get
			End Property
		End Structure


		#End Region

	End Class

End Namespace
