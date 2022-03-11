Imports MySql.Data.MySqlClient

Module Retrieve
    Public Function CheckUserCode(Usercode) As Boolean
        Dim ReturnUsername As Boolean = False
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim sql = "SELECT id, usercode, fullname FROM voters WHERE usercode = '" & Usercode & "' AND status = 'Active'"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, ConnectionLocal)
            Using reader As MySqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    While reader.Read
                        VotersUserID = reader("id").ToString
                        VotersCode = reader("usercode")
                        VotersFullName = reader("fullname")
                    End While
                    ReturnUsername = True
                Else
                    ReturnUsername = False
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
        Console.Write(ReturnUsername)
        Return ReturnUsername
    End Function

    Public Function CheckAlreadyVoted(VotersID) As Boolean
        Dim Voted As Boolean = False
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim sql = "SELECT usercode FROM voted WHERE usercode = '" & VotersID & "' AND status = 'Active'"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, ConnectionLocal)
            Using reader As MySqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    Voted = True
                Else
                    Voted = False
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
        Console.Write(Voted)
        Return Voted
    End Function

    Public Function IsVoterLoggedIn(VotersID) As Boolean
        Dim Logged As Boolean = False
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim sql = "SELECT logged FROM voters WHERE usercode = '" & VotersID & "' AND status = 'Active'"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, ConnectionLocal)
            Using reader As MySqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    While reader.Read
                        If reader("logged") = 1 Then
                            Logged = True
                        Else
                            Logged = False
                        End If
                    End While
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
        Console.Write(Logged)
        Return Logged
    End Function
End Module
