'Author: Nehal Patel (http://www.itspatel.com/)
'Last Updated: 3/22/2013

Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Public Class XboxLeaders

#Region "Properties"
    Dim _Endpoint As String = "https://www.xboxleaders.com/api/"
    ''' <summary>
    ''' Gets or sets the location of the API endpoint.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Endpoint As String
        Get
            Return _Endpoint
        End Get
        Set(ByVal value As String)
            _Endpoint = value
        End Set
    End Property

    Dim _Timeout As Integer = 8000
    ''' <summary>
    ''' Gets or sets the time-out value in milliseconds.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Timeout As Integer
        Get
            Return _Timeout
        End Get
        Set(ByVal value As Integer)
            _Timeout = value
        End Set
    End Property

    Dim _Format As String = "xml"
    ''' <summary>
    ''' Gets or sets the format of the API response. (Possible values: json, php, xml)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Format As String
        Get
            Return _Format
        End Get
        Set(ByVal value As String)
            _Format = value
        End Set
    End Property
#End Region

#Region "Async Web Request Prerequisites"
    Dim ResponseAvailable As Boolean = False
    Dim ResponseValue As HttpWebResponse

    Private Sub loadResponseValue(ByVal result As IAsyncResult)
        ResponseValue = TryCast(TryCast(result.AsyncState, HttpWebRequest).EndGetResponse(result), HttpWebResponse)
        ResponseAvailable = True
    End Sub
#End Region

#Region "Encode GET Variables"
    Private Function Encode(ByVal str As String) As String
        Dim charClass = [String].Format("0-9a-zA-Z{0}", Regex.Escape("-_.!~*'()"))
        Return Regex.Replace(str, [String].Format("[^{0}]", charClass), New MatchEvaluator(AddressOf EncodeEvaluator))
    End Function

    Private Function EncodeEvaluator(ByVal match As Match) As String
        Return If((match.Value = " "), "+", [String].Format("%{0:X2}", Convert.ToInt32(match.Value(0))))
    End Function
#End Region

    Private Function http(ByVal Request As String, ByVal Parameters As String)
        Dim URL As String = Endpoint & Request & "." & Format & "?" & Parameters
        Parameters = Encode(Parameters)

        Dim HTTPRequest As HttpWebRequest
        HTTPRequest = CType(WebRequest.Create(URL), HttpWebRequest)

        HTTPRequest.AllowAutoRedirect = True
        HTTPRequest.Timeout = Timeout
        HTTPRequest.UserAgent = "VB.NET XboxLeaders API Wrapper"

        HTTPRequest.BeginGetResponse(New AsyncCallback(AddressOf loadResponseValue), HTTPRequest)

        Do Until ResponseAvailable = True
            Application.DoEvents()
        Loop
        ResponseAvailable = False

        Dim Result As String = New StreamReader(ResponseValue.GetResponseStream()).ReadToEnd()

        Return Result
    End Function

    ''' <summary>
    ''' Gets the validity of a gamertag.
    ''' </summary>
    ''' <param name="Gamertag">This is the gamertag that you want to validate.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsGamertagValid(ByVal Gamertag As String)
        If Not Regex.IsMatch(Gamertag, "~^(?=.{1,15}$)[a-zA-Z][a-zA-Z[0-9]]*(?: [a-zA-Z[0-9]]+)*$~") Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Gets the profile data for a gamertag.
    ''' </summary>
    ''' <param name="Gamertag">This is the gamertag that you want to fetch the profile of.</param>
    ''' <returns>Profile data for a gamertag in the specified format</returns>
    ''' <remarks></remarks>
    Public Function FetchProfile(ByVal Gamertag As String)
        If isGamertagValid(Gamertag) Then
            Return http("profile", "gamertag=" & Gamertag)
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Gets a list of games played by a gamertag.
    ''' </summary>
    ''' <param name="Gamertag">This is the gamertag that you want to fetch the played games of.</param>
    ''' <returns>Games played by a gamertag in the specified format</returns>
    ''' <remarks></remarks>
    Public Function FetchGames(ByVal Gamertag As String)
        If isGamertagValid(Gamertag) Then
            Return http("games", "gamertag=" & Gamertag)
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Gets achievement data for a gamertag and game id.
    ''' </summary>
    ''' <param name="Gamertag">This is the gamertag that you want to fetch the achievements of.</param>
    ''' <param name="GameID">This is the ID of the game that you want to fetch the achivements of.</param>
    ''' <returns>Achievements earned by a gamertag in the specfied format</returns>
    ''' <remarks></remarks>
    Public Function FetchAchievements(ByVal Gamertag As String, ByVal GameID As String)
        If isGamertagValid(Gamertag) Then
            Return http("achievements", "gamertag=" & Gamertag & "&titleid=" & GameID)
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Gets the friend list of a gamertag.
    ''' </summary>
    ''' <param name="Gamertag">This is the gamertag that you want to fetch the friends of.</param>
    ''' <returns>Friends of a gamertag in the specified format</returns>
    ''' <remarks></remarks>
    Public Function FetchFriends(ByVal Gamertag As String)
        If isGamertagValid(Gamertag) Then
            Return http("friends", "gamertag=" & Gamertag)
        Else
            Return False
        End If
    End Function
End Class