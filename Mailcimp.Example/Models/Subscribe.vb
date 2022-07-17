Public Class Subscribe
    Public Property email_address As String
    Public Property status As String
    Public Property language As String
    Public Property status_if_new As String
    Public Property merge_fields As Dictionary(Of String, String)
    Public Property tags As String()
End Class
Public Class Unsubscribe
    Public Property email_address As String
    Public Property status As String
End Class