Public Class Calculator
    Private Sub NumberButttonCLicked(sender As Control, e As EventArgs) Handles Button9.Click, Button7.Click, Button6.Click, Button2.Click, Button15.Click, Button14.Click, Button13.Click, Button12.Click, Button11.Click, Button10.Click
        Edit(sender.Text)
    End Sub
    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        Edit(sender.Text)
    End Sub
    Private Sub Backspace() Handles Button8.Click
        If start Then
            Label1.Text = Nothing
        Else
            Label1.Text = Strings.Left(Label1.Text, Label1.Text.Length - 1)
        End If
    End Sub
    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        Edit("-")
    End Sub
    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Edit("+")
    End Sub
    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Edit("-")
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Edit("*")
    End Sub
    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        Edit("/")
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Edit("^")
    End Sub
    Dim start = True, warten = False
    Sub Edit(value As String)
        If warten Then Exit Sub
        If start Then Label1.Text = Nothing
        Label1.Text += value
        start = False
    End Sub
    Private Sub Calculator_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyData = Keys.Back Then
            Backspace()
        ElseIf e.KeyData = Keys.Enter Then
            Calculate()
            MsgBox("")
        End If
    End Sub
    Private Sub Calculate() Handles Button19.Click
        Try
            warten = True
            start = True
            Dim value = Label1.Text
            Label1.Text += vbNewLine + "="
            Label1.Text += String2Value.GetValue(value).ToString()
            warten = False
        Catch ex As Exception
            Label1.Text += "[Error]"
            MsgBox(ex.Message)
        End Try
    End Sub
End Class