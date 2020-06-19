Imports System.Reflection
Imports System.Windows.Forms
Public Class Dialog3
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub Dialog3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each method As MethodInfo In GetType(System.Math).Assembly.GetType("System.Math").GetMethods
            Dim paramsstr As String
            For Each param As ParameterInfo In method.GetParameters
                If paramsstr = Nothing Then
                    paramsstr = param.Name + " As " + param.ParameterType.Name
                Else
                    paramsstr += ", " + param.Name + " As " + param.ParameterType.Name
                End If
            Next
            Dim a As New ListViewItem
            a.Text = method.Name
            a.SubItems.Add(paramsstr)
            a.SubItems.Add(method.Name + "(" + paramsstr + ")")
            ListView1.Items.Add(a)
        Next
    End Sub
    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            TextBox1.Text = ListView1.SelectedItems(0).SubItems(2).Text
        Catch : End Try
    End Sub
End Class
