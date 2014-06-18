Namespace My
    Partial Friend Class MyApplication
        Private Sub AppStart(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf ResolveAssemblies
        End Sub

        Private Function ResolveAssemblies(sender As Object, e As System.ResolveEventArgs) As Reflection.Assembly
            Try
                Dim desiredAssembly = New Reflection.AssemblyName(e.Name)

                Select Case desiredAssembly.Name
                    Case Is = "CheckBoxComboBox"
                        Return Reflection.Assembly.Load(My.Resources.CheckBoxComboBox)
                    Case Is = "System.Windows.Forms.Ribbon35"
                        Return Reflection.Assembly.Load(My.Resources.System_Windows_Forms_Ribbon35)
                    Case Is = "Newtonsoft.Json"
                        Return Reflection.Assembly.Load(My.Resources.Newtonsoft_Json)
                    Case Is = "KRBTabControl"
                        Return Reflection.Assembly.Load(My.Resources.KRBTabControl)
                    Case Else
                        Return Nothing
                End Select
            Catch ex As Exception
                Return Nothing
            End Try
        End Function
    End Class
End Namespace