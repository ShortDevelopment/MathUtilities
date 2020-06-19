Imports System.Windows.Forms
Public Class Dialog2
    Property UseFunction As Boolean = False
    Property [Function] As String
    Property Min As Integer
    Property Max As Integer
    Property [Step] As Decimal
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If RadioButton1.Checked Then
            UseFunction = True
            [Function] = TextBox1.Text
            Min = NumericUpDown1.Value
            Max = NumericUpDown2.Value
            [Step] = NumericUpDown3.Value
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If sender.Checked Then
            GroupBox1.Enabled = True
        Else
            GroupBox1.Enabled = False
        End If
    End Sub
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://docs.microsoft.com/de-de/dotnet/api/system.math?view=netframework-4.7.2#methoden")
    End Sub
End Class
