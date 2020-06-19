Imports System.Numerics
Imports BigNumbers

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim a As New ListViewItem
        a.Text = "6"
        a.SubItems.Add("1")
        a.SubItems.Add((6 / 1).ToString())
        a.SubItems.Add((6 / 2).ToString())
        ListView1.Items.Add(a)
        Dim thread As New Threading.Thread(Sub()
                                               Dim abc = False
                                               While abc = False
                                                   Try
                                                       Dim lastitem As ListViewItem
                                                       Me.Invoke(Sub() lastitem = ListView1.Items(ListView1.Items.Count - 1))
                                                       Dim b As New ListViewItem
                                                       Try
                                                           'If Long.IsNaN(Long.Parse(b.SubItems(2).Text)) Then abc = True
                                                       Catch : End Try
                                                       b.Text = Double.Parse(lastitem.Text) * 2
                                                       Dim abc2 = (Double.Parse(lastitem.SubItems(1).Text) / (Math.Sqrt((2 + Math.Sqrt((4 - Double.Parse(lastitem.SubItems(1).Text) ^ 2)))))).ToString()
                                                       If abc2 = 0 Then Exit While
                                                       b.SubItems.Add(abc2)
                                                       b.SubItems.Add((Double.Parse(b.SubItems(0).Text) * (b.SubItems(1)).Text).ToString())
                                                       b.SubItems.Add((Double.Parse(b.SubItems(2).Text) / 2).ToString())
                                                       Me.Invoke(Sub() ListView1.Items.Add(b))
                                                       Me.Invoke(Sub() lastitem.EnsureVisible())

                                                   Catch
                                                       Exit While
                                                   End Try
                                               End While
                                           End Sub)
        thread.IsBackground = True
        thread.Start()
    End Sub
    Function doabc(s As String)
        Dim a = New BigNumber(2 - New BigNumber(4 - BigNumber.Parse(s).Pow(2)).Sqrt()).Sqrt()
        Return a
    End Function
#Region "Old"
    'Dim a As New ListViewItem
    '    a.Text = "6"
    '    a.SubItems.Add("1")
    '    a.SubItems.Add((6 / 1).ToString())
    '    a.SubItems.Add((6 / 2).ToString())
    '    ListView1.Items.Add(a)
    '    Dim thread As New Threading.Thread(Sub()
    '                                           Dim abc = False
    '                                           While abc = False
    '                                               Try
    '                                                   Dim lastitem As ListViewItem
    '                                                   Me.Invoke(Sub() lastitem = ListView1.Items(ListView1.Items.Count - 1))
    '                                                   Dim b As New ListViewItem
    '                                                   Try
    '                                                       'If Long.IsNaN(Long.Parse(b.SubItems(2).Text)) Then abc = True
    '                                                   Catch : End Try
    '                                                   b.Text = Double.Parse(lastitem.Text) * 2
    '                                                   Dim abc2 = (Double.Parse(lastitem.SubItems(1).Text) / (Math.Sqrt((2 + Math.Sqrt((4 - Double.Parse(lastitem.SubItems(1).Text) ^ 2)))))).ToString()
    '                                                   If abc2 = 0 Then Exit While
    '                                                   b.SubItems.Add(abc2)
    '                                                   b.SubItems.Add((Double.Parse(b.SubItems(0).Text) * (b.SubItems(1)).Text).ToString())
    '                                                   b.SubItems.Add((Double.Parse(b.SubItems(2).Text) / 2).ToString())
    '                                                   Me.Invoke(Sub() ListView1.Items.Add(b))
    '                                                   Me.Invoke(Sub() lastitem.EnsureVisible())

    '                                               Catch
    '                                                   Exit While
    '                                               End Try
    '                                           End While
    '                                       End Sub)
    '    thread.IsBackground = True
    '    thread.Start()
#End Region
End Class
Class BigDouble
    Dim _Digits As Numerics.BigInteger
    ReadOnly Property Digits As Numerics.BigInteger
        Get
            Return _Digits
        End Get
    End Property
    Dim _KommaPos As Numerics.BigInteger
    ReadOnly Property KommaPos As Numerics.BigInteger
        Get
            Return _KommaPos
        End Get
    End Property
    Public Sub New(number As String)
        _Digits = BigInteger.Parse(number.Replace(",", ""))
        _KommaPos = number.IndexOf(",")
    End Sub
    Public Shared Operator -(ByVal a As Integer, ByVal b As BigDouble) As BigDouble
        Dim abc As String
        'MsgBox(b.Digits.ToString().Substring(0, b.KommaPos))
        abc = (a - BigInteger.Parse(b.Digits.ToString().Substring(0, b.KommaPos))).ToString + "," + b.Digits.ToString().Substring(b.KommaPos)
        Return New BigDouble(abc)
    End Operator
    Public Shared Operator *(ByVal a As BigDouble, ByVal b As Integer) As BigDouble
        Dim abc As String
        'MsgBox(b.Digits.ToString().Substring(0, b.KommaPos))
        Dim aa = a * BigInteger.Parse(a.Digits.ToString()).ToString

        Return New BigDouble(abc)
    End Operator
    Public Function Parse(number As String)
        Return New BigDouble(number)
    End Function
    Public Overrides Function ToString() As String
        Return _Digits.ToString().Insert(_KommaPos, ",")
    End Function
End Class