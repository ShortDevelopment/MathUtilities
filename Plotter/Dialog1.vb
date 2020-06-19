Imports System.Windows.Forms
Public Class Dialog1
    Public Points As New List(Of Plotter.Point)
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Points = New List(Of Plotter.Point)
        For Each item As ListViewItem In ListView1.Items
            Try
                Points.Add(New Point(Double.Parse(item.Text), Double.Parse(item.SubItems(1).Text)))
            Catch : End Try
        Next
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Shadows Function ShowDialog(_Points As List(Of Plotter.Point)) As DialogResult
        For Each point As Plotter.Point In _Points
            Dim a As New ListViewItem
            a.Text = point.X.ToString()
            a.SubItems.Add(point.Y.ToString)
            ListView1.Items.Add(a)
        Next
        Return MyBase.ShowDialog()
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim x = Double.Parse(TextBox1.Text)
            Dim y = Double.Parse(TextBox2.Text)
            Dim a As New ListViewItem
            a.Text = x.ToString()
            a.SubItems.Add(y.ToString)
            ListView1.Items.Add(a)
        Catch ex As Exception
            MsgBox("Nur Zahlen!", MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListView1.Items.Clear()
    End Sub
End Class