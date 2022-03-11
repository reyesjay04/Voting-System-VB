
Module PublicFunctions
    Dim DateNow
    Public Function FullDate24HR()
        Try
            DateNow = Format(Now(), "yyyy-MM-dd HH:mm:ss")
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        Return DateNow
    End Function
    Public Function ConvertToBase64(str As String)
        Dim byt As Byte() = System.Text.Encoding.UTF8.GetBytes(str)
        Dim byt2 = Convert.ToBase64String(byt)
        Return byt2
    End Function
    Public Function ConvertB64ToString(str As String)
        Dim b As Byte() = Convert.FromBase64String(str)
        Dim byt2 = System.Text.Encoding.UTF8.GetString(b)
        Return byt2
    End Function
    Public Function RemoveCharacter(ByVal stringToCleanUp, ByVal characterToRemove)
        ' replace the target with nothing
        ' Replace() returns a new String and does not modify the current one
        Return stringToCleanUp.Replace(characterToRemove, "")
    End Function
    Public Sub AcceptNumbersOnly(ByVal sender As Object, ByVal e As KeyPressEventArgs)


        If e.KeyChar <> ControlChars.Back Then
            e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or e.KeyChar = "v" Or e.KeyChar = ControlChars.Cr)
        Else
            'If e.KeyChar = ControlChars.Cr AndAlso e.KeyChar = "v" Then
            '    MsgBox("asd")
            'End If

        End If
    End Sub
    Public Sub AcceptNumeric(sender As Object, e As KeyEventArgs, textbox As TextBox)
        If e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.V Then
            textbox.Text = Clipboard.GetText()
        End If
    End Sub

    Public Function GetIPAddress() As String
        Dim strHostName As String
        Dim strIPAddress As String
        Try
            strHostName = Net.Dns.GetHostName()
            strIPAddress = Net.Dns.GetHostByName(strHostName).AddressList(0).ToString()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
        Return "Host: " & strHostName & "; IP: " & strIPAddress & " "
    End Function
End Module
