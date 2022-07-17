Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Json
Imports Mailcimp.Example.Mailchimp.Example.Models
Imports Newtonsoft.Json

Namespace Mailchimp.Example.Member
    Public Class Member

        ''' <summary>
        ''' Add Member
        ''' </summary>
        ''' <param name="obj">Member data</param>
        ''' <returns></returns>
        Function PostData(list_id As String, obj As Subscribe) As HttpResponseMessage
            Using client As HttpClient = New HttpClient()
                client.DefaultRequestHeaders.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "{APIKEY}")
                client.DefaultRequestHeaders.Accept.Add(New System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))

                Using response As HttpResponseMessage = client.PostAsJsonAsync($"https://{SERVER}.api.mailchimp.com/3.0/lists/{list_id}/members", obj).Result

                    Return response
                End Using
            End Using
        End Function


        ''' <summary>
        ''' Update Member
        ''' </summary>
        ''' <param name="obj">MemberData</param>
        ''' <returns></returns>
        Function AddOrUpdateMember(list_id As String, obj As Subscribe) As HttpResponseMessage
            Using client As HttpClient = New HttpClient()
                client.DefaultRequestHeaders.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "{APIKEY}")
                client.DefaultRequestHeaders.Accept.Add(New System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))
                Using response As HttpResponseMessage = client.PutAsJsonAsync($"https://{SERVER}.api.mailchimp.com/3.0/lists/{list_id}/members/{obj.email_address}", obj).Result
                    Return response
                End Using
            End Using
        End Function

        ''' <summary>
        ''' Add or Update Tags
        ''' </summary>
        ''' <param name="email">Memner Email</param>
        ''' <param name="tags">Tag list with status. Status: active: Add tag, inactive:remove tag (status is case-sensetive.)</param>
        ''' <returns></returns>
        Function AddRemoveTag(list_id As String, email As String, tags As Tag) As HttpResponseMessage
            Dim json = JsonConvert.SerializeObject(tags)
            Using client As HttpClient = New HttpClient()
                client.DefaultRequestHeaders.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "{APIKEY}")
                client.DefaultRequestHeaders.Accept.Add(New System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))
                Using response As HttpResponseMessage = client.PostAsJsonAsync($"https://{SERVER}.api.mailchimp.com/3.0/lists/{list_id}/members/{email}/tags", tags).Result
                    Using content As HttpContent = response.Content
                        Dim result As String = content.ReadAsStringAsync().Result
                        If response.StatusCode = 204 Then
                            Console.WriteLine("Tags updated for the member")
                        End If
                    End Using
                    Return response
                End Using
            End Using
        End Function


        ''' <summary>
        ''' Add or Update Tags
        ''' </summary>
        ''' <param name="email">Memner Email</param>
        ''' <param name="tags">Tag list with status. Status: active: Add tag, inactive:remove tag (status is case-sensetive.)</param>
        ''' <returns></returns>
        Function Unsubscribe(list_id As String, email As String) As HttpResponseMessage
            Dim payload As New Dictionary(Of String, String)
            Dim obj As New Unsubscribe()
            obj.email_address = email
            obj.status = "unsubscribed"

            Using client As HttpClient = New HttpClient()
                client.DefaultRequestHeaders.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "{APIKEY}")
                client.DefaultRequestHeaders.Accept.Add(New System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))
                Using response As HttpResponseMessage = client.PutAsJsonAsync($"https://{SERVER}.api.mailchimp.com/3.0/lists/{list_id}/members/{email}", obj).Result
                    Using content As HttpContent = response.Content
                        Dim result As String = content.ReadAsStringAsync().Result
                        If response.StatusCode = 200 Then
                            Console.WriteLine("Member unsubscribed successfully.")
                        Else
                            Dim errorResponse = JsonConvert.DeserializeObject(Of ErrorResponse)(result)
                            Console.WriteLine($"Status: '{errorResponse.status}'")
                            Console.WriteLine($"Title: '{errorResponse.title}'")
                            Console.WriteLine($"Message: '{errorResponse.detail}'")
                        End If
                    End Using
                    Return response
                End Using
            End Using
        End Function
    End Class
End Namespace

