Imports System.Net

Namespace Mailchimp.Example.Models
    Public Class ErrorResponse
        Public Property title As String
        Public Property status As HttpStatusCode
        Public Property detail As String
    End Class
End Namespace
