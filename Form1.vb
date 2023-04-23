Imports System.Management
Imports System.Threading

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public th As Thread

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnStart.Click

        CheckForIllegalCrossThreadCalls = False


        If CheckBox1.Checked = True Then
            lblSecond.Text = txtInterval.Text * 60
        Else
            lblSecond.Text = txtInterval.Text * 1

        End If

        btnStart.Enabled = False
        btnStop.Enabled = True
        txtInterval.Enabled = False
        CheckBox1.Enabled = False

        th = New Thread(AddressOf TimerCount)
        th.Start()

    End Sub

    Sub RunPy()



        Dim pythonPath As String = txtPythonFile.Text ' مسار تثبيت Python على جهاز الكمبيوتر الخاص بك
        Dim scriptPath As String = txtPyFile.Text ' مسار ملف النص الخاص بـ Python

        Dim processX As New Process()
        processX.StartInfo.FileName = pythonPath
        processX.StartInfo.Arguments = scriptPath
        processX.Start()
        lblStatus.Text = "Process Runing"

    End Sub

    Private Sub KillProcessByPath(ByVal path As String)
        Try
            Dim searcher As New ManagementObjectSearcher("SELECT * FROM Win32_process WHERE ExecutablePath = '" _
                                                         + path.Replace("\", "\\") + "'")
            For Each result As ManagementObject In searcher.Get()
                Dim PID As Integer = CInt(result.Item("ProcessId"))
                Dim P As Process = Process.GetProcessById(PID)
                P.Kill()
            Next
        Catch ex As Exception
            ' handle exception
        End Try
    End Sub

    Sub TimerCount()
        Dim o As Integer = lblSecond.Text

        Dim Count As Integer = 0
        Dim x As Integer = lblSecond.Text

        While True
            x = x - 1
            lblSecond.Text = x
            lblStatus.Text = "Wait to Kill"
            If x = 0 Then
                x = o

                Count = Count + 1

                lblStatus.Text = "Process Killed wating 10 second to run Again"

                KillProcessByPath(txtFilePath.Text)


                Threading.Thread.Sleep(10000)


                RunPy()

                lblRunTime.Text = Count
            End If

            Threading.Thread.Sleep(1000)
        End While

    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        th.Abort()
        lblSecond.Text = "0"
        btnStop.Enabled = False
        btnStart.Enabled = True
        txtInterval.Enabled = True
        CheckBox1.Enabled = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.InitialDirectory = "C:\Windows"
        '  openFileDialog1.Filter = "PyEXE (*.exe)|*.exe|All Files (*.*)|*.*"
        openFileDialog1.Filter = "PyEXE (*.exe)|*.exe"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Dim selectedFilePath As String = openFileDialog1.FileName
            txtFilePath.Text = selectedFilePath
            ' Do something with the selected file path
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.InitialDirectory = "C:\"
        '  openFileDialog1.Filter = "PyEXE (*.exe)|*.exe|All Files (*.*)|*.*"
        openFileDialog1.Filter = "PyFile (*.py)|*.py"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Dim selectedFilePath As String = openFileDialog1.FileName
            txtPyFile.Text = selectedFilePath
            ' Do something with the selected file path
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.InitialDirectory = "C:\Program Files (x86)\"
        '  openFileDialog1.Filter = "PyEXE (*.exe)|*.exe|All Files (*.*)|*.*"
        openFileDialog1.Filter = "PythonEXE (*.exe)|*.exe"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Dim selectedFilePath As String = openFileDialog1.FileName
            txtPythonFile.Text = selectedFilePath
            ' Do something with the selected file path
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Label8.Text = "Minutes"
        Else
            Label8.Text = "Second"
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        KillProcessByPath(txtFilePath.Text)

    End Sub
End Class
