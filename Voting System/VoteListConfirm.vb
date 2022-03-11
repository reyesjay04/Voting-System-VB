Imports System.Threading
Imports MySql.Data.MySqlClient

Public Class VoteListConfirm
    Private Sub VoteListConfirm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try

            CheckForIllegalCrossThreadCalls = False
            BackgroundWorkerLoadCandidates.WorkerReportsProgress = True
            BackgroundWorkerLoadCandidates.WorkerSupportsCancellation = True
            BackgroundWorkerLoadCandidates.RunWorkerAsync()
            Button1.Enabled = False
            Timer1.Start()
            'GLB_IP_ADD = GetIPAddress()

        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
    Dim ThreadListLoadCandidate As List(Of Thread) = New List(Of Thread)
    Dim ThreadLoadCandidate As Thread

    Dim DatatableBOD As DataTable
    Dim DatatableAC As DataTable
    Dim DatatableEC As DataTable

    Dim WorkerCancel As Boolean = False
    Private Sub BackgroundWorkerLoadCandidates_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerLoadCandidates.DoWork
        Try
            For i = 0 To 100
                ToolStripStatusLabel1.Text = "Loading..."
                BackgroundWorkerLoadCandidates.ReportProgress(i)
                Thread.Sleep(10)
                If WorkerCancel Then
                    Exit For
                End If
                If i = 0 Then
                    ThreadLoadCandidate = New Thread(AddressOf LocalhostConn)
                    ThreadLoadCandidate.Start()
                    ThreadListLoadCandidate.Add(ThreadLoadCandidate)
                End If
                For Each t In ThreadListLoadCandidate
                    t.Join()
                    If (BackgroundWorkerLoadCandidates.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        e.Cancel = True
                        Exit For
                    End If
                Next
                If i = 10 Then
                    If LocalConnectionIsOnOrValid Then
                        ThreadLoadCandidate = New Thread(AddressOf LoadBOD)
                        ThreadLoadCandidate.Start()
                        ThreadListLoadCandidate.Add(ThreadLoadCandidate)

                        ThreadLoadCandidate = New Thread(AddressOf LoadAC)
                        ThreadLoadCandidate.Start()
                        ThreadListLoadCandidate.Add(ThreadLoadCandidate)

                        ThreadLoadCandidate = New Thread(AddressOf LoadEC)
                        ThreadLoadCandidate.Start()
                        ThreadListLoadCandidate.Add(ThreadLoadCandidate)
                    End If
                End If
                For Each t In ThreadListLoadCandidate
                    t.Join()
                    If (BackgroundWorkerLoadCandidates.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        e.Cancel = True
                        Exit For
                    End If
                Next
            Next

        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorkerLoadCandidates_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorkerLoadCandidates.ProgressChanged
        Try
            If WorkerCancel = False Then
                ToolStripProgressBar1.Value = e.ProgressPercentage
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorkerLoadCandidates_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorkerLoadCandidates.RunWorkerCompleted
        Try

            ToolStripStatusLabel1.Text = "Complete"
            Button1.Enabled = True
            If WorkerCancel = False Then
                For i As Integer = 0 To DatatableBOD.Rows.Count - 1 Step +1
                    DataGridView1.Rows.Add(DatatableBOD(i)(0), DatatableBOD(i)(1))
                Next
                For i As Integer = 0 To DatatableAC.Rows.Count - 1 Step +1
                    DataGridView2.Rows.Add(DatatableAC(i)(0), DatatableAC(i)(1))
                Next
                For i As Integer = 0 To DatatableEC.Rows.Count - 1 Step +1
                    DataGridView3.Rows.Add(DatatableEC(i)(0), DatatableEC(i)(1))
                Next
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub LoadBOD()
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim ResultBOD As List(Of String) = ListOfBod.Distinct().ToList

            DatatableBOD = New DataTable
            DatatableBOD.Columns.Add("id")
            DatatableBOD.Columns.Add("fullname")

            Dim sql = ""
            Dim cmd As MySqlCommand
            Dim dt As DataTable
            Dim da As MySqlDataAdapter

            For Each element As String In ResultBOD
                sql = "SELECT id, fullname FROM candidates WHERE id = " & element
                cmd = New MySqlCommand(sql, ConnectionLocal)
                da = New MySqlDataAdapter(cmd)
                dt = New DataTable
                da.Fill(dt)
                For i As Integer = 0 To dt.Rows.Count - 1 Step +1
                    Dim R As DataRow = DatatableBOD.NewRow
                    R("id") = dt(i)(0)
                    R("fullname") = dt(i)(1)
                    DatatableBOD.Rows.Add(R)
                Next

            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
    Private Sub LoadAC()
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim ResultAC As List(Of String) = ListOfAC.Distinct().ToList

            DatatableAC = New DataTable
            DatatableAC.Columns.Add("id")
            DatatableAC.Columns.Add("fullname")

            Dim sql = ""
            Dim cmd As MySqlCommand
            Dim dt As DataTable
            Dim da As MySqlDataAdapter

            For Each element As String In ResultAC
                sql = "SELECT id, fullname FROM candidates WHERE id = " & element
                cmd = New MySqlCommand(sql, ConnectionLocal)
                da = New MySqlDataAdapter(cmd)
                dt = New DataTable
                da.Fill(dt)
                For i As Integer = 0 To dt.Rows.Count - 1 Step +1
                    Dim R As DataRow = DatatableAC.NewRow
                    R("id") = dt(i)(0)
                    R("fullname") = dt(i)(1)
                    DatatableAC.Rows.Add(R)
                Next

            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
    Private Sub LoadEC()
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim ResultEC As List(Of String) = ListOfEC.Distinct().ToList

            DatatableEC = New DataTable
            DatatableEC.Columns.Add("id")
            DatatableEC.Columns.Add("fullname")

            Dim sql = ""
            Dim cmd As MySqlCommand
            Dim dt As DataTable
            Dim da As MySqlDataAdapter

            For Each element As String In ResultEC
                sql = "SELECT id, fullname FROM candidates WHERE id = " & element
                cmd = New MySqlCommand(sql, ConnectionLocal)
                da = New MySqlDataAdapter(cmd)
                dt = New DataTable
                da.Fill(dt)
                For i As Integer = 0 To dt.Rows.Count - 1 Step +1
                    Dim R As DataRow = DatatableEC.NewRow
                    R("id") = dt(i)(0)
                    R("fullname") = dt(i)(1)
                    DatatableEC.Rows.Add(R)
                Next

            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub VoteListConfirm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            BackgroundWorkerLoadCandidates.CancelAsync()
            WorkerCancel = True
            Home.Enabled = True
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            If Label4.ForeColor = Color.Black Then
                Label4.ForeColor = Color.Red
            Else
                Label4.ForeColor = Color.Black
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If DataGridView1.Rows.Count = 0 And DataGridView2.Rows.Count = 0 And DataGridView3.Rows.Count = 0 Then
                MessageBox.Show("You did not vote yet", "NOTICE", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
            Else
                Dim msg = MessageBox.Show("Are you sure you want to submit your vote?", "NOTICE", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If msg = DialogResult.Yes Then
                    Enabled = False

                    BackgroundWorkerSubmitVotes.WorkerReportsProgress = True
                    BackgroundWorkerSubmitVotes.WorkerSupportsCancellation = True
                    BackgroundWorkerSubmitVotes.RunWorkerAsync()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
    Dim ThreadListSubmitVote As List(Of Thread) = New List(Of Thread)
    Dim ThreadSubmitVote As Thread

    Private Sub BackgroundWorkerSubmitVotes_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerSubmitVotes.DoWork
        Try
            For i = 0 To 100
                ToolStripStatusLabel1.Text = "Processing, Please wait..."
                BackgroundWorkerSubmitVotes.ReportProgress(i)
                Thread.Sleep(10)
                If WorkerCancel Then
                    Exit For
                End If
                If i = 0 Then
                    ThreadSubmitVote = New Thread(AddressOf LocalhostConn)
                    ThreadSubmitVote.Start()
                    ThreadListSubmitVote.Add(ThreadSubmitVote)
                End If
                For Each t In ThreadListSubmitVote
                    t.Join()
                    If (BackgroundWorkerSubmitVotes.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        e.Cancel = True
                        Exit For
                    End If
                Next
                If i = 10 Then
                    If LocalConnectionIsOnOrValid Then
                        ThreadSubmitVote = New Thread(AddressOf SubmitVote)
                        ThreadSubmitVote.Start()
                        ThreadListSubmitVote.Add(ThreadSubmitVote)
                    End If
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorkerSubmitVotes_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorkerSubmitVotes.ProgressChanged
        Try
            If WorkerCancel = False Then
                ToolStripProgressBar1.Value = e.ProgressPercentage
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorkerSubmitVotes_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorkerSubmitVotes.RunWorkerCompleted
        Try
            If WorkerCancel = False Then
                Enabled = True
                UpdateVotersLogged(VotersCode, 0)
                Home.CloseHomePage = True
                VotersID.Show()

                VotersUserID = ""
                VotersCode = ""
                VotersFullName = ""

                VoteCountAC = 1
                VoteCountEC = 1

                ACVOTECOUNT = 0
                ECVOTECOUNT = 0

                ListOfBod.Clear()
                ListOfAC.Clear()
                ListOfEC.Clear()

                Close()
                Home.Close()

            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub SubmitVote()
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            With DataGridView1
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim Sql As String = "INSERT INTO `votes` (`candidate_id`, `voters_id`, `position`, `created_by`, `created_at`, `status`, `ip_address`) VALUES (@1,@2,@3,@4,@5,@6,@7)"
                    Dim Cmd As MySqlCommand = New MySqlCommand(Sql, ConnectionLocal)
                    Cmd.Parameters.Add("@1", MySqlDbType.Text).Value = .Rows(i).Cells(0).Value
                    Cmd.Parameters.Add("@2", MySqlDbType.Text).Value = VotersUserID
                    Cmd.Parameters.Add("@3", MySqlDbType.Text).Value = "Board Of Directors"
                    Cmd.Parameters.Add("@4", MySqlDbType.Text).Value = VotersCode
                    Cmd.Parameters.Add("@5", MySqlDbType.Text).Value = FullDate24HR()
                    Cmd.Parameters.Add("@6", MySqlDbType.Text).Value = "Active"
                    Cmd.Parameters.Add("@7", MySqlDbType.Text).Value = GetIPAddress()
                    Cmd.ExecuteNonQuery()
                Next
            End With
            With DataGridView2
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim Sql As String = "INSERT INTO `votes` (`candidate_id`, `voters_id`, `position`, `created_by`, `created_at`, `status`, `ip_address`) VALUES (@1,@2,@3,@4,@5,@6,@7)"
                    Dim Cmd As MySqlCommand = New MySqlCommand(Sql, ConnectionLocal)
                    Cmd.Parameters.Add("@1", MySqlDbType.Text).Value = .Rows(i).Cells(0).Value
                    Cmd.Parameters.Add("@2", MySqlDbType.Text).Value = VotersUserID
                    Cmd.Parameters.Add("@3", MySqlDbType.Text).Value = "Audit Committee"
                    Cmd.Parameters.Add("@4", MySqlDbType.Text).Value = VotersCode
                    Cmd.Parameters.Add("@5", MySqlDbType.Text).Value = FullDate24HR()
                    Cmd.Parameters.Add("@6", MySqlDbType.Text).Value = "Active"
                    Cmd.Parameters.Add("@7", MySqlDbType.Text).Value = GetIPAddress()
                    Cmd.ExecuteNonQuery()
                Next
            End With
            With DataGridView3
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim Sql As String = "INSERT INTO `votes` (`candidate_id`, `voters_id`, `position`, `created_by`, `created_at`, `status`, `ip_address`) VALUES (@1,@2,@3,@4,@5,@6,@7)"
                    Dim Cmd As MySqlCommand = New MySqlCommand(Sql, ConnectionLocal)
                    Cmd.Parameters.Add("@1", MySqlDbType.Text).Value = .Rows(i).Cells(0).Value
                    Cmd.Parameters.Add("@2", MySqlDbType.Text).Value = VotersUserID
                    Cmd.Parameters.Add("@3", MySqlDbType.Text).Value = "Election Committee"
                    Cmd.Parameters.Add("@4", MySqlDbType.Text).Value = VotersCode
                    Cmd.Parameters.Add("@5", MySqlDbType.Text).Value = FullDate24HR()
                    Cmd.Parameters.Add("@6", MySqlDbType.Text).Value = "Active"
                    Cmd.Parameters.Add("@7", MySqlDbType.Text).Value = GetIPAddress()
                    Cmd.ExecuteNonQuery()
                Next
            End With
            Dim Query As String = "INSERT INTO `voted` (`usercode`, `created_at`, `status`) VALUES (@1,@2,@3)"
            Dim Command As MySqlCommand = New MySqlCommand(Query, ConnectionLocal)
            Command.Parameters.Add("@1", MySqlDbType.Text).Value = VotersCode
            Command.Parameters.Add("@2", MySqlDbType.Text).Value = FullDate24HR()
            Command.Parameters.Add("@3", MySqlDbType.Text).Value = "Active"
            Command.ExecuteNonQuery()
            VotersLogs("Voted Successfully : " & VotersUserID)
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
End Class