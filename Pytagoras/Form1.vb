Public Class Form1
    Property _X As Integer
    Property _Y As Integer
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListView1.Items.Clear()
        Dim t As New Threading.Thread(Sub()
                                          For x As Integer = 1 To 100
                                              For y As Integer = 1 To 100
                                                  Dim a As New ListViewItem
                                                  a.Text = x.ToString()
                                                  a.SubItems.Add(y.ToString())
                                                  a.SubItems.Add(Math.Pow(Math.Pow(x, 2) + Math.Pow(y, 2), 0.5))
                                                  Dim count = ListView1.Items.Count
                                                  Dim abc = False
                                                  _Y = y
                                                  _X = x
                                                  Dim items = ListView1.Items.Cast(Of ListViewItem).Where(Function(xyz)
                                                                                                              If xyz.Text = _Y.ToString Then
                                                                                                                  Return True
                                                                                                              End If
                                                                                                              Return False
                                                                                                          End Function)
                                                  If Me.Invoke(Function() items.Count) > 0 Then
                                                      'Debug.Print(items(0).Text + "|" + items(0).SubItems(1).Text)
                                                  Else
                                                      Me.Invoke(Sub()
                                                                    ListView1.Items.Add(a)
                                                                    'Application.DoEvents()
                                                                    If Int(a.SubItems(2).Text) = Double.Parse(a.SubItems(2).Text) Then
                                                                        RichTextBox1.Text += x.ToString + "|" + a.SubItems(1).Text + "|" + a.SubItems(2).Text + vbNewLine
                                                                    End If
                                                                End Sub)
                                                  End If
                                                  Try
                                                  Catch : End Try
                                              Next
                                          Next
                                      End Sub)
        t.IsBackground = True
        t.Start()
        Try
        Catch : End Try
    End Sub
End Class
