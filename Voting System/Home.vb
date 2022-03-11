Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Threading

Public Class Home
    Public CloseHomePage As Boolean = False
    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ToolStripLabel1.Text = TimeOfDay
            Dim myCulture As Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
            Dim dayOfWeek As DayOfWeek = myCulture.Calendar.GetDayOfWeek(Date.Today)
            Dim dayName As String = myCulture.DateTimeFormat.GetDayName(dayOfWeek)
            ToolStripLabel2.Text = dayName & ", " & DateTime.Today.ToString("MMMM") & Format(Date.Now, " dd, yyyy")

            Label3.Text = VotersFullName
            CheckForIllegalCrossThreadCalls = False
            BackgroundWorkerBOD.WorkerReportsProgress = True
            BackgroundWorkerBOD.WorkerSupportsCancellation = True
            BackgroundWorkerBOD.RunWorkerAsync()
            Enabled = False
            Timer1.Start()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
    Dim ThreadListBOD As List(Of Thread) = New List(Of Thread)
    Dim ThreadBOD As Thread
    Private Sub BackgroundWorkerBOD_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerBOD.DoWork
        Try
            For i = 0 To 100
                BackgroundWorkerBOD.ReportProgress(i)
                'Thread.Sleep(10)
                If i = 0 Then
                    ThreadBOD = New Thread(AddressOf LocalhostConn)
                    ThreadBOD.Start()
                    ThreadListBOD.Add(ThreadBOD)
                End If
            Next
            For Each t In ThreadListBOD
                t.Join()
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorkerBOD_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorkerBOD.ProgressChanged
        Try

        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorkerBOD_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorkerBOD.RunWorkerCompleted
        Try
            If LocalConnectionIsOnOrValid Then
                GetBODList()
                GetACList()
                GetECList()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
    Private Sub LoadDefault()
        UpdateVotersLogged(VotersCode, 0)

        VotersUserID = ""
        VotersCode = ""
        VotersFullName = ""

        VoteCountAC = 1
        VoteCountEC = 1

        ACVOTECOUNT = 0
        ECVOTECOUNT = 0

        ListOfAC.Clear()
        ListOfBod.Clear()
        ListOfEC.Clear()
        ADS.Close()

        CloseHomePage = True
        VotersID.Show()
        Close()
    End Sub
    Private Sub GetBODList()
        Try
            Dim cmd As MySqlCommand
            Dim da As MySqlDataAdapter
            Dim dt As DataTable
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            cmd = New MySqlCommand("SELECT id, fullname, position, profile FROM candidates WHERE position = 'Board Of Directors' AND status = 'Active' ", ConnectionLocal)

            FlowLayoutPanelBOD.Controls.Clear()
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable()
            da.Fill(dt)

            Dim folder As String = My.Settings.ProfileImagePath & "\"

            Dim Fullname As String = ""
            Dim Position As String = ""
            Dim Profile As String = ""
            Dim CandidateID As String = ""
            For Each row As DataRow In dt.Rows

                Fullname = row("fullname")
                Position = row("position")
                Profile = row("profile")
                CandidateID = row("id")

                Dim GroupBoxCandidate As New GroupBox
                Dim CandidatesProfile As New PictureBox
                Dim CheckBoxVote As New CheckBox

                With GroupBoxCandidate
                    .Height = 150
                    .Width = 175
                    .Text = Fullname.ToUpper
                    .Font = New Font("Segoe UI Semi Bold", 9)
                End With

                With CandidatesProfile
                    .Location = New Point(5, 20)
                    .Parent = GroupBoxCandidate
                    .Height = 105
                    .Width = 150
                    .BackgroundImageLayout = ImageLayout.Stretch
                    .BorderStyle = BorderStyle.None
                End With

                If File.Exists(folder & Profile) Then
                    Dim filename As String = Path.Combine(folder, Profile)
                    Using fs As New FileStream(filename, FileMode.Open)
                        CandidatesProfile.BackgroundImage = New Bitmap(Image.FromStream(fs))
                    End Using
                Else

                    Dim filename As String = Path.Combine(folder, "Unknown.jpg")
                    Using fs As New FileStream(filename, FileMode.Open)
                        CandidatesProfile.BackgroundImage = New Bitmap(Image.FromStream(fs))
                    End Using
                End If

                With CheckBoxVote
                    .Location = New Point(5, 123)
                    .Parent = GroupBoxCandidate
                    .Name = CandidateID
                    .Text = "VOTE "
                End With

                FlowLayoutPanelBOD.Controls.Add(GroupBoxCandidate)
                AddHandler CheckBoxVote.Click, AddressOf Vote_BOD
            Next
            ConnectionLocal.Close()
            da.Dispose()
            cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
            LoadDefault()
        End Try
    End Sub


    Private Sub GetACList()
        Try
            Dim cmd As MySqlCommand
            Dim da As MySqlDataAdapter
            Dim dt As DataTable
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            cmd = New MySqlCommand("SELECT id, fullname, position, profile FROM candidates WHERE position = 'Audit Committee' AND status = 'Active' ", ConnectionLocal)

            FlowLayoutPanelAC.Controls.Clear()
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable()
            da.Fill(dt)

            Dim folder As String = My.Settings.ProfileImagePath & "\"

            Dim Fullname As String = ""
            Dim Position As String = ""
            Dim Profile As String = ""
            Dim CandidateID As String = ""
            For Each row As DataRow In dt.Rows

                Fullname = row("fullname")
                Position = row("position")
                Profile = row("profile")
                CandidateID = row("id")

                Dim GroupBoxCandidate As New GroupBox
                Dim CandidatesProfile As New PictureBox
                Dim CheckBoxVote As New CheckBox

                With GroupBoxCandidate
                    .Height = 150
                    .Width = 175
                    .Text = Fullname.ToUpper
                    .Font = New Font("Segoe UI Semi Bold", 9)
                End With

                With CandidatesProfile
                    .Location = New Point(5, 20)
                    .Parent = GroupBoxCandidate
                    .Height = 105
                    .Width = 150
                    .BackgroundImageLayout = ImageLayout.Stretch
                    .BorderStyle = BorderStyle.None
                End With

                If File.Exists(folder & Profile) Then
                    Dim filename As String = Path.Combine(folder, Profile)
                    Using fs As New FileStream(filename, FileMode.Open)
                        CandidatesProfile.BackgroundImage = New Bitmap(Image.FromStream(fs))
                    End Using
                Else

                    Dim filename As String = Path.Combine(folder, "Unknown.jpg")
                    Using fs As New FileStream(filename, FileMode.Open)
                        CandidatesProfile.BackgroundImage = New Bitmap(Image.FromStream(fs))
                    End Using
                End If

                With CheckBoxVote
                    .Location = New Point(5, 123)
                    .Parent = GroupBoxCandidate
                    .Name = CandidateID
                    .Text = "VOTE "
                End With

                FlowLayoutPanelAC.Controls.Add(GroupBoxCandidate)
                AddHandler CheckBoxVote.Click, AddressOf Vote_AC
            Next
            ConnectionLocal.Close()
            da.Dispose()
            cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
            LoadDefault()
        End Try
    End Sub
    Private Sub GetECList()
        Try
            Dim cmd As MySqlCommand
            Dim da As MySqlDataAdapter
            Dim dt As DataTable
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            cmd = New MySqlCommand("SELECT id, fullname, position, profile FROM candidates WHERE position = 'Election Committee' AND status = 'Active' ", ConnectionLocal)

            FlowLayoutPanelEC.Controls.Clear()
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable()
            da.Fill(dt)

            Dim folder As String = My.Settings.ProfileImagePath & "\"

            Dim Fullname As String = ""
            Dim Position As String = ""
            Dim Profile As String = ""
            Dim CandidateID As String = ""
            For Each row As DataRow In dt.Rows

                Fullname = row("fullname")
                Position = row("position")
                Profile = row("profile")
                CandidateID = row("id")

                Dim GroupBoxCandidate As New GroupBox
                Dim CandidatesProfile As New PictureBox
                Dim CheckBoxVote As New CheckBox

                With GroupBoxCandidate
                    .Height = 150
                    .Width = 175
                    .Text = Fullname.ToUpper
                    .Font = New Font("Segoe UI Semi Bold", 9)
                End With

                With CandidatesProfile
                    .Location = New Point(5, 20)
                    .Parent = GroupBoxCandidate
                    .Height = 105
                    .Width = 150
                    .BackgroundImageLayout = ImageLayout.Stretch
                    .BorderStyle = BorderStyle.None
                End With

                If File.Exists(folder & Profile) Then
                    Dim filename As String = Path.Combine(folder, Profile)
                    Using fs As New FileStream(filename, FileMode.Open)
                        CandidatesProfile.BackgroundImage = New Bitmap(Image.FromStream(fs))
                    End Using
                Else

                    Dim filename As String = Path.Combine(folder, "Unknown.jpg")
                    Using fs As New FileStream(filename, FileMode.Open)
                        CandidatesProfile.BackgroundImage = New Bitmap(Image.FromStream(fs))
                    End Using
                End If

                With CheckBoxVote
                    .Location = New Point(5, 123)
                    .Parent = GroupBoxCandidate
                    .Name = CandidateID
                    .Text = "VOTE "
                End With

                FlowLayoutPanelEC.Controls.Add(GroupBoxCandidate)
                AddHandler CheckBoxVote.Click, AddressOf Vote_EC
            Next
            ConnectionLocal.Close()
            da.Dispose()
            cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
            LoadDefault()
        End Try
    End Sub

    Private Sub ButtonSubmitVote_Click(sender As Object, e As EventArgs) Handles ButtonSubmitVote.Click
        Try
            If BODVOTECOUNT > 0 And ACVOTECOUNT = 3 And ECVOTECOUNT = 3 Then
                Enabled = False
                VoteListConfirm.Show()
            Else
                MessageBox.Show("Three votes are required for Audit Committee/Election Committee.", "NOTICE", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonLogOut.Click
        Try
            Dim msg = MessageBox.Show("Are you sure you want to logout?", "NOTICE", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If msg = DialogResult.Yes Then
                UpdateVotersLogged(VotersCode, 0)

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

                CloseHomePage = True
                VotersID.Show()
                Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            ToolStripLabel1.Text = TimeOfDay
            Dim myCulture As System.Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
            Dim dayOfWeek As DayOfWeek = myCulture.Calendar.GetDayOfWeek(Date.Today)
            Dim dayName As String = myCulture.DateTimeFormat.GetDayName(dayOfWeek)
            ToolStripLabel2.Text = dayName & ", " & DateTime.Today.ToString("MMMM") & Format(Date.Now, " dd, yyyy")
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub Home_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If CloseHomePage = False Then
            e.Cancel = True
        End If
    End Sub
End Class