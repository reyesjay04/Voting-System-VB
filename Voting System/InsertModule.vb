Imports MySql.Data.MySqlClient
Module InsertModule
    Public Sub AuditTrail(Description As String)
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim Sql = "INSERT INTO errors (`description`, `created_by`, `created_at`, `status`) VALUES (@1,@2,@3,@4)"
            Dim Cmd As MySqlCommand = New MySqlCommand(Sql, ConnectionLocal)
            Cmd.Parameters.Add("@1", MySqlDbType.Text).Value = Description
            Cmd.Parameters.Add("@2", MySqlDbType.Text).Value = VotersUserID
            Cmd.Parameters.Add("@3", MySqlDbType.Text).Value = FullDate24HR()
            Cmd.Parameters.Add("@4", MySqlDbType.Text).Value = "Active"
            Cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Sub VotersLogs(Description As String)
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim Sql = "INSERT INTO voterslogs (`description`, `created_by`, `created_at`, `status`) VALUES (@1,@2,@3,@4)"
            Dim Cmd As MySqlCommand = New MySqlCommand(Sql, ConnectionLocal)
            Cmd.Parameters.Add("@1", MySqlDbType.Text).Value = Description
            Cmd.Parameters.Add("@2", MySqlDbType.Text).Value = VotersUserID
            Cmd.Parameters.Add("@3", MySqlDbType.Text).Value = FullDate24HR()
            Cmd.Parameters.Add("@4", MySqlDbType.Text).Value = "Active"
            Cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Module
