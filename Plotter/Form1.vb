Imports System.CodeDom.Compiler
Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.IO
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Text
Public Class Form1
    Dim Graphs As New List(Of List(Of Point))
    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Dim points As New List(Of Point)
        Try
            For x As Integer = -50 To 50
                points.Add(New Point(x, x ^ 2))
                Dim a As New ListViewItem
                a.Text = x.ToString()
                a.SubItems.Add(((x ^ 2).ToString()))
                ListView1.Items.Add(a)
            Next
        Catch ex As Exception
            MsgBox("Fehler beim Berechnen der Punkte!")
        End Try
        Dim abc As New ListViewItem
        abc.Checked = True
        abc.Text = "x^2"
        ListView2.Items.Add(abc)
        GraphEngine1.AddGraph(points)
        PropertyGrid1.SelectedObject = GraphEngine1
        PropertyGrid1.BrowsableAttributes = New AttributeCollection(New CategoryAttribute("Graph"))
    End Sub
    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        Dim a As New Form1
        a.Show()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Dim a As New OpenFileDialog
        a.Title = "Plot laden"
        a.Filter = "CSV-Dateien|*.csv"
        If a.ShowDialog = DialogResult.OK Then
            GraphEngine1.ImportCsvFile(a.FileName)
        End If
    End Sub
    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Dim a As New SaveFileDialog
        a.Title = "Plot speichern"
        a.Filter = "CSV-Dateien|*.csv|Bild-Datei|*.jpg"
        If a.ShowDialog = DialogResult.OK Then
            If IO.Path.GetExtension(a.FileName) = ".jpg" Then
                Try
                    CType(GraphEngine1.DrawToBitmap(), Image).Save(a.FileName)
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            Else
                Try
                    MsgBox("Geht leider noch nicht!", MsgBoxStyle.Critical)
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End If
        End If
    End Sub
    Private Sub ZuBildKonvertierenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZuBildKonvertierenToolStripMenuItem.Click
        Dim a As New SaveFileDialog
        a.Title = "Plot konvertieren"
        a.Filter = "Bild-Datei|*.jpg"
        If a.ShowDialog = DialogResult.OK Then
            Try
                CType(GraphEngine1.DrawToBitmap(), Image).Save(a.FileName)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If MsgBox("Wollen Sie das Programm wirklich verlassen?!", vbYesNo) = vbNo Then
            e.Cancel = True
        End If
    End Sub
    Private Sub PunkteEditierenToolStripMenuItem_Click() Handles PunkteEditierenToolStripMenuItem.Click
        If ListView2.SelectedItems.Count = 0 Then
            MsgBox("Bitte erst einen Graphen wählen!", MsgBoxStyle.Critical)
            Exit Sub
        End If
        Dim a As New Dialog1
        Try
            If a.ShowDialog(GraphEngine1.Graphs.Item(ListView2.SelectedItems(0).Index)) = DialogResult.OK Then
                GraphEngine1.ChangeGraph(ListView2.SelectedItems(0).Index, a.Points)
                Graphs.Add(a.Points)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub ListView2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView2.SelectedIndexChanged
        If ListView2.SelectedItems.Count = 0 Then Exit Sub
        ListView1.Items.Clear()
        For Each points As Point In GraphEngine1.Graphs.Item(ListView2.SelectedItems(0).Index)
            Dim a As New ListViewItem
            a.Text = points.X.ToString()
            a.SubItems.Add(points.Y.ToString())
            ListView1.Items.Add(a)
        Next
    End Sub
    Private Sub HinzufügenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HinzufügenToolStripMenuItem.Click
        Dim a As New Dialog2
        Dim errors As String = ""
        Try
            If a.ShowDialog = DialogResult.OK Then
                If a.UseFunction Then
                    Dim thread As New Threading.Thread(Sub()
                                                           Try
                                                               Dim points As New List(Of Point)
                                                               points = String2Value.EvaluateFunction(a.Function, a.Min, a.Max, a.Step) 'New DataTable().Compute(...)
                                                               Me.Invoke(Sub()
                                                                             Dim abc As New ListViewItem
                                                                             abc.Checked = True
                                                                             abc.Text = a.Function
                                                                             ListView2.Items.Add(abc)
                                                                             GraphEngine1.AddGraph(points)
                                                                             If Not errors = "" Then MsgBox("Es sind Fehler aufgetreten!" + vbNewLine + errors, MsgBoxStyle.Critical)
                                                                         End Sub)
                                                           Catch ex As Exception
                                                               Me.Invoke(Sub() MsgBox(ex.Message, MsgBoxStyle.Critical))
                                                           End Try
                                                       End Sub)
                    thread.IsBackground = True
                    thread.Start()
                Else
                    Dim abc As New ListViewItem
                    abc.Checked = True
                    abc.Text = "Custom" + GraphEngine1.Graphs.Count.ToString
                    ListView2.Items.Add(abc)
                    GraphEngine1.AddGraph(New List(Of Point))
                    Dim b As New Dialog1
                    Try
                        If b.ShowDialog(New List(Of Point)) = DialogResult.OK Then
                            GraphEngine1.ChangeGraph(GraphEngine1.Graphs.Count - 1, b.Points)
                            Graphs.Add(b.Points)
                        End If
                    Catch ex As Exception
                        MsgBox(ex.Message, MsgBoxStyle.Critical)
                    End Try
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub LöschenToolStripMenuItem_Click() Handles LöschenToolStripMenuItem.Click
        If ListView2.SelectedItems.Count = 0 Then
            MsgBox("Bitte erst einen Graphen wählen!", MsgBoxStyle.Critical)
            Exit Sub
        End If
        If MsgBox("Wollen Sie diesen Graphen wirklich löschen?!", vbYesNo) = vbYes Then
            GraphEngine1.RemoveGraph(ListView2.SelectedItems(0).Index)
            ListView2.Items.RemoveAt(ListView2.SelectedItems(0).Index)
        End If
    End Sub
    Private Sub ListView2_KeyDown(sender As Object, e As KeyEventArgs) Handles ListView2.KeyDown
        If e.KeyData = Keys.Delete Then
            LöschenToolStripMenuItem_Click()
        End If
    End Sub

    Private Sub CalculatorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CalculatorToolStripMenuItem.Click
        Calculator.Show()
        Calculator.WindowState = FormWindowState.Normal
        Calculator.BringToFront()
    End Sub
    Private Sub HilfeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HilfeToolStripMenuItem.Click
        Dialog3.ShowDialog()
    End Sub
End Class
Public Class GraphEngine
    Inherits Control
    Dim crossList As New List(Of Rectangle)
#Region "Propertys"
#Region "Main"
    'Dim _points As New List(Of Point)
    '<Description("Legt fest, welche Punkte gezeichnet werden sollen")>
    '<Category("Graph")>
    'Property Points As List(Of Point)
    '    Get
    '        Return _points
    '    End Get
    '    Set(value As List(Of Point))
    '        _points = value
    '        Me.Refresh()
    '    End Set
    'End Property
    <Description("Legt fest, welche Hintergrundfarbe benutztwerden soll")>
    <Category("Graph")>
    Overrides Property BackColor As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(value As Color)
            MyBase.BackColor = value
            Me.Refresh()
        End Set
    End Property

    Dim _OffsetX As Integer = 500
    <Description("Legt fest, wie wie weit der Graph in X Richtung verschoben ist")>
    <Category("Graph")>
    Property OffsetX As Integer
        Get
            Return _OffsetX
        End Get
        Set(value As Integer)
            _OffsetX = value
            Me.Refresh()
        End Set
    End Property
    Dim _OffsetY As Integer = 100
    <Description("Legt fest, wie wie weit der Graph in Y Richtung verschoben ist")>
    <Category("Graph")>
    Property OffsetY As Integer
        Get
            Return _OffsetY
        End Get
        Set(value As Integer)
            _OffsetY = value
            Me.Refresh()
        End Set
    End Property
    Dim _StepY As Integer = 20
    <Description("Legt fest, wie groß die Abstände in Y Richtung sind")>
    <Category("Graph")>
    Property StepY As Integer
        Get
            Return _StepY
        End Get
        Set(value As Integer)
            _StepY = value
            Me.Refresh()
        End Set
    End Property
    Dim _StepX As Integer = 20
    <Description("Legt fest, wie groß die Abstände in X Richtung sind")>
    <Category("Graph")>
    Property StepX As Integer
        Get
            Return _StepX
        End Get
        Set(value As Integer)
            _StepX = value
            Me.Refresh()
        End Set
    End Property
    Dim _zoom As Integer = 1
    <Description("Legt fest, wie detalliert der Graph dargestellt werden soll")>
    <Category("Graph")>
    Property Zoom As Integer
        Get
            Return _zoom
        End Get
        Set(value As Integer)
            If value <= 0 Then
                Throw New Exception("Value not allowed" + vbNewLine + "Must be bigger than 0")
            Else
                _zoom = value
            End If
        End Set
    End Property
    Dim _GraphThickness = 1
    <Description("Legt fest, wie dick die Line des Graphen sein sollen")>
    <Category("Graph")>
    Property GraphThickness As Integer
        Get
            Return _GraphThickness
        End Get
        Set(value As Integer)
            _GraphThickness = value
            Me.Refresh()
        End Set
    End Property
    'Dim _GraphColor As Color = Color.Black
    <Description("Legt fest, welche Farbe der Graphen haben soll")>
    <Category("Graph")>
    Property GraphColor As Color
        Get
            Return MyBase.ForeColor
        End Get
        Set(value As Color)
            MyBase.ForeColor = value
            Me.Refresh()
        End Set
    End Property
    Dim _TextColor As Color = Color.Green
    <Description("Legt fest, welche Farbe die Beschriftung haben")>
    <Category("Graph")>
    Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(value As Color)
            _TextColor = value
            Me.Refresh()
        End Set
    End Property
    Dim _graphs As New List(Of List(Of Point))
    ReadOnly Property Graphs As List(Of List(Of Point))
        Get
            Return _graphs
        End Get
    End Property
#End Region
#Region "Marker"
    Dim _MarkerThickness = 2
    <Description("Legt fest, wie dick die MarkierungsLinen sein sollen")>
    <Category("Graph")>
    Property MarkerThickness As Integer
        Get
            Return _MarkerThickness
        End Get
        Set(value As Integer)
            _MarkerThickness = value
            Me.Refresh()
        End Set
    End Property
    Dim _MarkerColor As Color = Color.Green
    <Description("Legt fest, welche Farbe die MarkierungsLinien haben sollen")>
    <Category("Graph")>
    Property MarkerColor As Color
        Get
            Return _MarkerColor
        End Get
        Set(value As Color)
            _MarkerColor = value
            Me.Refresh()
        End Set
    End Property
#End Region
#Region "Crosses"
    Dim _ShowCrosses = True
    <Description("Legt fest, ob Kreuze für die einzelnen Punkte gezeichnet werden sollen")>
    <Category("Graph")>
    Property CrossesVisible As Boolean
        Get
            Return _ShowCrosses
        End Get
        Set(value As Boolean)
            _ShowCrosses = value
            Me.Refresh()
        End Set
    End Property
    Dim _CrossSize = New Size(10, 10)
    <Description("Legt fest, wie groß die Kreuze sein sollen")>
    <Category("Graph")>
    Property CrossSize As Size
        Get
            Return _CrossSize
        End Get
        Set(value As Size)
            _CrossSize = value
            Me.Refresh()
        End Set
    End Property
    Dim _CrossThickness As Integer = 2
    <Description("Legt fest, wie dick die Kreuze sein sollen")>
    <Category("Graph")>
    Property CrossThickness As Integer
        Get
            Return _CrossThickness
        End Get
        Set(value As Integer)
            _CrossThickness = value
            Me.Refresh()
        End Set
    End Property
    Dim _CrossColor As Color = Color.Black
    <Description("Legt fest, welche Farbe die Achsen haben sollen")>
    <Category("Graph")>
    Property CrossColor As Color
        Get
            Return _CrossColor
        End Get
        Set(value As Color)
            _CrossColor = value
            Me.Refresh()
        End Set
    End Property
#End Region
#Region "Achsen"
    Dim _AchseThickness = 1
    <Description("Legt fest, wie dick die Achsen sein sollen")>
    <Category("Graph")>
    Property AchseThickness As Integer
        Get
            Return _AchseThickness
        End Get
        Set(value As Integer)
            _AchseThickness = value
            Me.Refresh()
        End Set
    End Property
    Dim _AchseColor As Color = Color.Green
    <Description("Legt fest, welche Farbe die Achsen haben sollen")>
    <Category("Graph")>
    Property AchseColor As Color
        Get
            Return _AchseColor
        End Get
        Set(value As Color)
            _AchseColor = value
            Me.Refresh()
        End Set
    End Property
#End Region
#End Region
    Sub New()
        MyBase.BackColor = Color.White
        MyBase.Dock = DockStyle.Fill
        MyBase.Cursor = Cursors.Cross
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        DrawOnGraphics(e.Graphics)
    End Sub
    Private Sub DrawOnGraphics(ByRef Graphics As Graphics)
        If Not Graphs Is Nothing Then
            For Each Points As List(Of Point) In Graphs
                For i As Integer = 0 To Points.Count - 1
                    Try
                        Dim p1 = New System.Drawing.Point(Points(i).X * StepX + OffsetX, Me.Height - (Points(i).Y * StepY + OffsetY))
                        If CrossesVisible Then
                            Graphics.DrawLine(New Pen(CrossColor, CrossThickness), New System.Drawing.Point(p1.X - (CrossSize.Width / 2), p1.Y - (CrossSize.Height / 2)), New System.Drawing.Point(p1.X + (CrossSize.Width / 2), p1.Y + (CrossSize.Height / 2)))
                            Graphics.DrawLine(New Pen(CrossColor, CrossThickness), New System.Drawing.Point(p1.X + (CrossSize.Width / 2), p1.Y - (CrossSize.Height / 2)), New System.Drawing.Point(p1.X - (CrossSize.Width / 2), p1.Y + (CrossSize.Height / 2)))
                        End If
                        Dim p2 = New System.Drawing.Point(Points(i + 1).X * StepX + OffsetX, Me.Height - (Points(i + 1).Y * StepY + OffsetY))
                        Graphics.DrawLine(New Pen(GraphColor, GraphThickness), p1, p2)
                    Catch : End Try
                Next
            Next
        End If
        Graphics.DrawLine(New Pen(AchseColor, AchseThickness), New System.Drawing.Point(OffsetX, 0), New System.Drawing.Point(OffsetX, Me.Height))
        Graphics.DrawLine(New Pen(AchseColor, AchseThickness), New System.Drawing.Point(0, Me.Height - OffsetY), New System.Drawing.Point(Me.Width, Me.Height - OffsetY))
        For xi As Integer = -50 To 50
            Dim x = xi * StepX + OffsetX
            Dim textx = xi.ToString
            If textx = "0" Then textx = ""
            Graphics.DrawString(textx, Me.Font, New SolidBrush(TextColor), New System.Drawing.Point(x - 7, Height - OffsetY + 5))
            Graphics.DrawLine(New Pen(MarkerColor, MarkerThickness), New System.Drawing.Point(x, Me.Height - OffsetY + 5), New System.Drawing.Point(x, Me.Height - OffsetY - 5))
        Next
        For yi As Integer = -50 To 50
            Dim y = yi * StepY
            Dim texty = yi.ToString
            If texty = "0" Then texty = ""
            Graphics.DrawString(texty, Me.Font, New SolidBrush(TextColor), New System.Drawing.Point(OffsetX + 8, Me.Height - y - OffsetY - Font.SizeInPoints / 2))
            Graphics.DrawLine(New Pen(MarkerColor, MarkerThickness), New System.Drawing.Point(OffsetX - 5, Me.Height - y - OffsetY), New System.Drawing.Point(OffsetX + 5, Me.Height - y - OffsetY))
        Next
        'For y As Integer = -1000 To 1000 Step StepY
        '    Dim texty = (y / StepY).ToString
        '    If texty = "0" Then texty = ""
        '    Graphics.DrawString(texty, Me.Font, Brushes.Green, New Point(OffsetX + 8, Me.Height - y - OffsetY - Font.SizeInPoints / 2))
        '    Graphics.DrawLine(New Pen(Color.Green, 2), New Point(OffsetX - 5, Me.Height - y - OffsetY), New Point(OffsetX + 5, Me.Height - y - OffsetY))
        'Next
        'For x As Integer = OffsetX - 1000 To OffsetX + 1000 Step StepX
        '    Dim textx = ((x / StepX) - (OffsetX / StepX)).ToString
        '    If textx = "0" Then textx = ""
        '    Graphics.DrawString(textx, Me.Font, Brushes.Green, New Point(x - 5, Height - OffsetY + 5))
        '    Graphics.DrawLine(New Pen(Color.Green, 2), New Point(x, Me.Height - OffsetY + 5), New Point(x, Me.Height - OffsetY - 5))
        'Next
    End Sub
    Public Shadows Function DrawToBitmap() As Bitmap
        Dim bmp = New Bitmap(Me.Width, Me.Height)
        Dim _graphics = Graphics.FromImage(bmp)
        _graphics.Clear(Me.BackColor)
        DrawOnGraphics(_graphics)
        Return bmp
    End Function
    Public Sub ImportCsvFile(ByVal filename As String)
        Dim file As FileInfo = New FileInfo(filename)
        Dim con As OleDbConnection = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=""" + file.DirectoryName + """; Extended Properties ='text;HDR=Yes;FMT=Delimited';")
        Dim cmd As OleDbCommand = New OleDbCommand(String.Format("SELECT * FROM [{0}]", file.Name), con)
        con.Open()
        'Dim reader As OleDbDataReader = cmd.ExecuteReader
        'While reader.Read : End While
        Dim adp As OleDbDataAdapter = New OleDbDataAdapter(cmd)
        Dim tbl As DataTable = New DataTable("MyTable")
        adp.Fill(tbl)
        Debug.Print("-----------------")
        Debug.Print("tbl.Rows.Count:" + tbl.Rows.Count.ToString())
        For Each row As DataRow In tbl.Rows
            For Each item In row.ItemArray()
                Debug.Print(item.ToString())
            Next
        Next
        Debug.Print("-------------")
        If tbl.Rows.Count <= 0 Then
            Dim result = My.Computer.FileSystem.ReadAllBytes(filename)
            Dim sKey = "12345678"
            Dim table = (Encoding.Default.GetString(result, 0, result.Length - 1)).Split(New String() {"\r\n", "\r", "\n"}, StringSplitOptions.None)
            Dim encryptedStream = New MemoryStream()
            encryptedStream.Write(result, 0, result.Length)

            Dim DES = New DESCryptoServiceProvider()
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey)
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey)
            Dim desdecrypt = DES.CreateDecryptor()
            Dim decryptedStream = New MemoryStream()
            encryptedStream.Position = 0
            Dim decryptStream = New CryptoStream(encryptedStream, desdecrypt, CryptoStreamMode.Read)
            decryptStream.CopyTo(decryptedStream)
            decryptedStream.Position = 0
            Dim filest = New FileStream("d:\\file.txt", FileMode.Create, FileAccess.Write)
            decryptedStream.WriteTo(filest)
            filest.Close()
        End If
        Debug.Print("-----------------")
    End Sub
    Public Sub AddGraph(points As List(Of Point))
        _graphs.Add(points)
        Me.Refresh()
    End Sub
    Public Sub ChangeGraph(index As Integer, points As List(Of Point))
        _graphs(index) = points
        Me.Refresh()
    End Sub
    Public Sub RemoveGraph(index As Integer)
        _graphs.RemoveAt(index)
        Me.Refresh()
    End Sub
#Region "MouseEvents"
    Dim drag As Boolean = False
    Dim x, y As Integer
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = MouseButtons.Middle Then
            MyBase.Cursor = Cursors.SizeAll
            x = Cursor.Position.X
            y = Cursor.Position.Y
            drag = True
        End If
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        MyBase.Cursor = Cursors.Cross
        drag = False
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If drag Then
            OffsetX += Cursor.Position.X - x
            OffsetY -= Cursor.Position.Y - y
        End If
    End Sub
    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        MyBase.OnMouseWheel(e)
        StepX += e.Delta / (120 * 2)
        StepY += e.Delta / (120 * 2)
    End Sub
#End Region
#Region "Redraw needed"
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        Me.Refresh()
    End Sub
#End Region
End Class
<Serializable>
Public Structure Point
    Sub New(_x As Double, _y As Double)
        X = _x
        Y = _y
    End Sub
    Public Property X As Double
    Public Property Y As Double
End Structure
Class String2Value
    Public Shared Function GetValue(ByVal expression As String) As Double
        Return EvaluateExpression(expression)
    End Function
    Protected Shared Function EvaluateExpression(ByVal expression As String) As Double
        Dim code As String = "
Imports System
Public Class Func
    Public Shared Function func() As Double
        return " + expression + "
    End Function
End Class"
        Dim compilerResults As CompilerResults = CompileScript(code)
        If compilerResults.Errors.HasErrors Then
            Throw New InvalidOperationException("Expression has a syntax error.")
        End If

        Dim assembly As Assembly = compilerResults.CompiledAssembly
        Dim method As MethodInfo = assembly.GetType("Func").GetMethod("func")
        Return CType(method.Invoke(Nothing, Nothing), Double)
    End Function
    Public Shared Function EvaluateFunction(ByVal funktion As String, min As Integer, max As Integer, [step] As Integer) As List(Of Plotter.Point)
        Dim code As String = "
Imports System
Imports System.Collections.Generic
Imports System.Math
Public Class Func
    Public Shared Function func() As System.Collections.Generic.List(Of List(Of Double))
        Dim points As New System.Collections.Generic.List(Of List(Of Double))        
        For x As Integer = " + min.ToString + " To " + max.ToString + " Step " + [step].ToString + "
            Try
                Dim List = New List(Of Double)
                List.Add(x)
                List.Add(" + funktion + ")
                points.Add(List)
            Catch : End Try
        Next
        Return points
    End Function
    Public Structure Point
        Sub New(_x As Double, _y As Double)
            X = _x
            Y = _y
        End Sub
        Public Property X As Double
        Public Property Y As Double
    End Structure
End Class"
        Dim compilerResults As CompilerResults = CompileScript(code)
        If compilerResults.Errors.HasErrors Then
            Debug.Print("-----------------------")
            For Each a As CompilerError In compilerResults.Errors
                Debug.Print(a.Column.ToString + ": " + a.ErrorText)
            Next
            Debug.Print("-----------------------")
            Throw New InvalidOperationException("Expression has a syntax error.")
        End If

        Dim assembly As Assembly = compilerResults.CompiledAssembly
        Dim method As MethodInfo = assembly.GetType("Func").GetMethod("func")
        Dim points As New List(Of Point)
        Dim list = CType(method.Invoke(Nothing, Nothing), List(Of List(Of Double)))
        Debug.Print("-----------------------" + list.Count.ToString)
        For Each item As List(Of Double) In list
            points.Add(New Plotter.Point(item(0), item(1)))
        Next
        Return points
    End Function
    Protected Shared Function CompileScript(ByVal source As String) As CompilerResults
        Dim parms As CompilerParameters = New CompilerParameters()
        parms.GenerateExecutable = False
        parms.GenerateInMemory = True
        parms.IncludeDebugInformation = False
        Dim compiler As CodeDomProvider = New VBCodeProvider()
        Return compiler.CompileAssemblyFromSource(parms, source)
    End Function
End Class