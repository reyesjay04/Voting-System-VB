Public Class ADS
    Private Sub ADS_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            Home.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ADS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try

            Home.Show()
            Me.TopMost = True

        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub
End Class