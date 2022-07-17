Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Json
Imports Mailcimp.Example.Mailchimp.Example.Models
Imports Newtonsoft.Json

Namespace Mailchimp.Example.Journey
    Public Class Journey

        ''' <summary>
        ''' Update journey for the Member
        ''' </summary>
        ''' <param name="payload">Email address</param>
        ''' <param name="journey_id">Journey Id</param>
        ''' <param name="step_id">Journey Step Id</param>
        ''' <returns></returns>
        Function UpdateJourney(email As String, journey_id As String, step_id As String) As HttpResponseMessage
            Try
                Dim payload As New JourneyModel()
                payload.email_address = email
                Using client As HttpClient = New HttpClient()
                    client.DefaultRequestHeaders.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "{APIKEY}")
                    client.DefaultRequestHeaders.Accept.Add(New System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))
                    Using response As HttpResponseMessage = client.PostAsJsonAsync($"https://{SERVER}.api.mailchimp.com/3.0/customer-journeys/journeys/{journey_id}/steps/{step_id}/actions/trigger", payload).Result
                        If response.StatusCode = HttpStatusCode.NoContent Then
                            Console.WriteLine($"Journey updated for the member {payload.email_address }")
                        Else
                            Using content As HttpContent = response.Content
                                Dim result As String = content.ReadAsStringAsync().Result
                                Console.WriteLine($"Status: '{result}'")
                                Dim errorResponse = JsonConvert.DeserializeObject(Of ErrorResponse)(result)
                                Console.WriteLine($"Status: '{errorResponse.status}'")
                                Console.WriteLine($"Title: '{errorResponse.title}'")
                                Console.WriteLine($"Message: '{errorResponse.detail}'")
                            End Using
                        End If

                        Return response
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                Throw ex
            End Try
        End Function
    End Class
End Namespace
