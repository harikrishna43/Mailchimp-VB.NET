Imports System
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Http.Json
Imports Newtonsoft.Json
Imports Mailcimp.Example.Mailchimp.Example.Models
Imports Mailcimp.Example.Mailchimp.Example.Member
Imports Mailcimp.Example.Mailchimp.Example.Journey

Module Program
    'LIST ID
    Dim list_id = "{LIST_ID}"
    'BASE URL
    Sub Main(args As String())
        Console.WriteLine("Add Member in Audiance")

        Dim member As New Member()

        Dim tagList() As String = {"NGS Free Trial"}
        Dim obj As New Subscribe
        obj.email_address = "johnsmith3@gmail.com"
        obj.status = "subscribed"
        obj.status_if_new = "subscribed"
        obj.language = "en"
        obj.tags = tagList
        Dim dictionary As New Dictionary(Of String, String)
        dictionary.Add("FNAME", "Harikrishna")
        dictionary.Add("LNAME", "Parmar")
        obj.merge_fields = dictionary


        '----------------------------------------------------------------------------------------------------
        '---- ADD/UPDATE MEMBER
        '----------------------------------------------------------------------------------------------------
        'Update Subscriber if already exists
        Dim response = member.AddOrUpdateMember(list_id, obj)
        If response.StatusCode = 200 Then
            Console.WriteLine("Member added/updated successfully.")
        Else
            Using content As HttpContent = response.Content
                Dim result As String = content.ReadAsStringAsync().Result
                Dim errorResponse = JsonConvert.DeserializeObject(Of ErrorResponse)(result)
                Console.WriteLine($"Status: '{errorResponse.status}'")
                Console.WriteLine($"Title: '{errorResponse.title}'")
                Console.WriteLine($"Message: '{errorResponse.detail}'")
            End Using
        End If


        '----------------------------------------------------------------------------------------------------
        '---- UPDATE STEPS FOR JOURNEY
        '----------------------------------------------------------------------------------------------------
        'Customer Journey
        Dim journey_id = "1949"
        Dim step_id = "12429"
        Dim journey As New Journey
        journey.UpdateJourney(obj.email_address, journey_id, step_id)


        '----------------------------------------------------------------------------------------------------
        '---- ADD /REMOVE TAGS
        '----------------------------------------------------------------------------------------------------
        'Data for the Tag update
        Dim tag1 As New Dictionary(Of String, String)
        tag1.Add("name", "NGS Customer")
        tag1.Add("status", "inactive")

        Dim tag2 As New Dictionary(Of String, String)
        tag2.Add("name", "NGS")
        tag2.Add("status", "active")
        Dim tag3 As New Dictionary(Of String, String)
        tag3.Add("name", "TEST")
        tag3.Add("status", "inactive")

        Dim objTags As New Tag

        Dim tags = New List(Of Dictionary(Of String, String))()
        tags.Add(tag1)
        tags.Add(tag2)
        tags.Add(tag3)
        objTags.tags = tags

        'Add/Remove Tags
        member.AddRemoveTag(list_id, obj.email_address, objTags)

        '----------------------------------------------------------------------------------------------------
        '---- UNSUBSCRIBE MEMBER FROM AUDIANCE CONTACT LIST
        '----------------------------------------------------------------------------------------------------
        member.Unsubscribe(list_id, obj.email_address)
    End Sub
End Module