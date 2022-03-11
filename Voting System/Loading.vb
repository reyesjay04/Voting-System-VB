Imports System.Threading

Public Class Loading
    Private Sub Loading_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            CheckForIllegalCrossThreadCalls = False
            BackgroundWorker1.WorkerSupportsCancellation = True
            BackgroundWorker1.WorkerReportsProgress = True
            BackgroundWorker1.RunWorkerAsync()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Dim threadList As List(Of Thread) = New List(Of Thread)
    Dim thread As Thread
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            For i = 0 To 100
                BackgroundWorker1.ReportProgress(i)
                Thread.Sleep(50)
                If i = 0 Then
                    Label1.Text = "Checking local connection..."
                    thread = New Thread(AddressOf LoadLocalConnection)
                    thread.Start()
                    threadList.Add(thread)
                End If
                For Each t In threadList
                    t.Join()
                    If BackgroundWorker1.CancellationPending = True Then
                        e.Cancel = True
                    End If
                Next
                If i = 10 Then
                    If ValidLocalConnection Then
                        If IO.Directory.Exists(My.Settings.ProfileImagePath) Then
                            Label1.Text = "Connected successfully..."
                            ValidProfileImagePath = True
                        Else
                            Label1.Text = "Invalid profile image path"
                            ValidProfileImagePath = False
                        End If
                    Else
                        Label1.Text = "Cannot connect to server..."
                    End If
                End If
                For Each t In threadList
                    t.Join()
                    If BackgroundWorker1.CancellationPending = True Then
                        e.Cancel = True
                    End If
                Next
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        Try
            ProgressBar1.Value = e.ProgressPercentage
            Label2.Text = e.ProgressPercentage
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            If ValidLocalConnection = False Then
                If ValidProfileImagePath = False Then
                    Settings.Show()
                    Close()
                End If
            Else
                If ValidProfileImagePath Then
                    VotersID.Show()
                    Close()
                Else
                    Settings.Show()
                    Close()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class