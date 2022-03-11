Module BtnHandler

    Public VoteCountAC As Integer = 1
    Public VoteCountEC As Integer = 1

    Public ListOfBod As List(Of String) = New List(Of String)
    Public ListOfAC As List(Of String) = New List(Of String)
    Public ListOfEC As List(Of String) = New List(Of String)

    Public Sub Vote_BOD(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim CheckBoxBOD As CheckBox = DirectCast(sender, CheckBox)
            Dim CheckBoxBODName = CheckBoxBOD.Name
            Dim ChecBoxBODText As String = CheckBoxBOD.Text
            If CheckBoxBOD.Checked Then
                BODVOTECOUNT += 1
                ListOfBod.Add(CheckBoxBODName)
            Else
                BODVOTECOUNT -= 1
                For i = ListOfBod.Count - 1 To 0 Step -1
                    If ListOfBod(i) = CheckBoxBODName Then
                        ListOfBod.RemoveAt(i)
                    End If
                Next
            End If
            Dim result As List(Of String) = ListOfBod.Distinct().ToList
            For Each element As String In result
                Console.WriteLine("BOD : " & element)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
    Public Sub Vote_AC(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim CheckBoxAC As CheckBox = DirectCast(sender, CheckBox)
            Dim CheckBoxACName = CheckBoxAC.Name
            Dim ChecBoxACText As String = CheckBoxAC.Text
            If CheckBoxAC.Checked Then
                If VoteCountAC <= 3 Then
                    VoteCountAC += 1
                    ACVOTECOUNT += 1
                    ListOfAC.Add(CheckBoxACName)
                Else
                    CheckBoxAC.Checked = False
                    MessageBox.Show("Three candidate per position only", "NOTICE", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                VoteCountAC -= 1
                ACVOTECOUNT -= 1
                For i = ListOfAC.Count - 1 To 0 Step -1
                    If ListOfAC(i) = CheckBoxACName Then
                        ListOfAC.RemoveAt(i)
                    End If
                Next
            End If
            Dim result As List(Of String) = ListOfAC.Distinct().ToList
            For Each element As String In result
                Console.WriteLine("AC : " & element)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
    Public Sub Vote_EC(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim CheckBoxEC As CheckBox = DirectCast(sender, CheckBox)
            Dim CheckBoxECName = CheckBoxEC.Name
            Dim ChecBoxECText As String = CheckBoxEC.Text
            If CheckBoxEC.Checked Then
                If VoteCountEC <= 3 Then
                    VoteCountEC += 1
                    ECVOTECOUNT += 1
                    ListOfEC.Add(CheckBoxECName)
                Else
                    CheckBoxEC.Checked = False
                    MessageBox.Show("Three candidate per position only", "NOTICE", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                VoteCountEC -= 1
                ECVOTECOUNT -= 1
                For i = ListOfEC.Count - 1 To 0 Step -1
                    If ListOfEC(i) = CheckBoxECName Then
                        ListOfEC.RemoveAt(i)
                    End If
                Next
            End If
            Dim result As List(Of String) = ListOfEC.Distinct().ToList
            For Each element As String In result
                Console.WriteLine("EC : " & element)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AuditTrail(ex.ToString)
        End Try
    End Sub
End Module
