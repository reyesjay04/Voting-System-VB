Public Class VotersID
    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            LabelDate.Text = ""
            LabelDate.Text = Date.Now
            Timer1.Start()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            LabelDate.Text = Date.Now


        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        Try
            'AcceptNumbersOnly(sender, e)
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If CheckAlreadyVoted(TextBox1.Text) = False Then
                If CheckUserCode(TextBox1.Text) Then
                    If IsVoterLoggedIn(TextBox1.Text) = False Then
                        UpdateVotersLogged(TextBox1.Text, 1)
                        ADS.Show()
                        Close()
                    Else
                        MessageBox.Show("This member is currently logged in", "NOTICE", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    MessageBox.Show("Voters code does not exist", "NOTICE", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBox.Show("This member has already voted", "NOTICE", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        Try
            AcceptNumeric(sender, e, TextBox1)
            If e.KeyCode = Keys.Enter Then
                Button1.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
End Class
