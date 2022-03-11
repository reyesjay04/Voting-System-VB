Imports MySql.Data.MySqlClient
Module UpdateModule
    Public Sub UpdateVotersLogged(VotersID, Logged)
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim Sql = "UPDATE `voters` SET logged = " & Logged & " WHERE usercode = '" & VotersID & "'"
            Dim Cmd As MySqlCommand = New MySqlCommand(Sql, ConnectionLocal)
            Cmd.ExecuteNonQuery()
            ConnectionLocal.Close()
            Cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
End Module
