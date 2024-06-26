﻿Imports System.IO
Imports System.Web.Services
Imports System.Xml
Imports System.Net
Imports System.Net.Mail

Public Class OldGenerateLog
    Inherits System.Web.UI.Page

    Dim oWS As ServiceSupport.ServiceSupportSoapClient
    Dim dt As DataTable
    Dim ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("Username").ToString = String.Empty And
               Session("Customer").ToString = String.Empty And
               Session("Category").ToString = String.Empty And
               Session("CustGroup").ToString = String.Empty Then
                Response.Redirect("Signin.aspx")
                Response.End()
            Else
                DisabledEnabledDropdown()
                DisabledDropdown()
                GetStatusSupport()
                GetCustomer(Session("CustGroup").ToString)
                GetModuleSupport()
                GetRequestor(String.Empty)
                GetDatabaseSite(String.Empty)
                GetProgramGroup(String.Empty)
                GetSeverity()
                CalculateProjectedDate()
                GetInformProblem()
                GetResponse()
                GetRequestAndResponseTime()
                If ddlCustomer.SelectedItem.Value <> String.Empty Then
                    SetCustomerEmailSupport(ddlCustomer.SelectedItem.Value)
                End If
                'txtRequestDate.Text = Date.Now.ToString("dd/MM/yyyy")
                'txtResponseDate.Text = Date.Now.ToString("dd/MM/yyyy")
            End If
        End If
        ddlStat.SelectedIndex = ddlStat.Items.IndexOf(ddlStat.Items.FindByValue("O"))
    End Sub

    Protected Sub DisabledDropdown()
        If Session("Category") = "Admin" Then 'Session("Customer") = "PPCC" Then
            ddlStat.Attributes.Remove("disabled")
            ddlSeverity.Attributes.Remove("disabled")
            'ddlCustomer.Attributes.Remove("disabled")
        Else
            ddlStat.Attributes.Add("disabled", "disabled")
            ddlSeverity.Attributes.Add("disabled", "disabled")
            'ddlCustomer.Attributes.Add("disabled", "disabled")
        End If
    End Sub

    Protected Sub GetRequestor(ByVal sSession As String)
        'Dim Customer As String
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        ds = New DataSet
        ddlRequestor.Items.Clear()
        If Session("Category").ToString <> "Admin" Then 'sSession <> "PPCC" Then
            dt = oWS.GetRequestorSupport(ddlCustomer.SelectedItem.Value)
            ds.Tables.Add(dt)
            For Each dRow As Data.DataRow In ds.Tables(0).Rows
                ddlRequestor.Items.Add(New ListItem(dRow("description"), dRow("req")))
            Next
        Else
            If ddlCustomer.SelectedItem.Value <> String.Empty Then
                dt = oWS.GetRequestorSupport(ddlCustomer.SelectedItem.Value)
                ds.Tables.Add(dt)
                For Each dRow As Data.DataRow In ds.Tables(0).Rows
                    ddlRequestor.Items.Add(New ListItem(dRow("description"), dRow("req")))
                Next
            End If
        End If
        ddlRequestor.Items.Insert(0, New ListItem("", ""))
    End Sub

    Protected Sub GetStatusSupport()
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        ds = New DataSet

        ddlStat.Items.Clear()
        dt = oWS.GetStatusSupport()
        ds.Tables.Add(dt)
        For Each dRow As Data.DataRow In ds.Tables(0).Rows
            ddlStat.Items.Add(New ListItem(dRow("description"), dRow("stat")))
        Next
        ddlStat.SelectedIndex = ddlStat.Items.IndexOf(ddlStat.Items.FindByValue("O"))
    End Sub

    Protected Sub GetModuleSupport()
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        ds = New DataSet

        ddlModule.Items.Clear()
        dt = oWS.GetSupportModule()
        ds.Tables.Add(dt)
        For Each dRow As Data.DataRow In ds.Tables(0).Rows
            ddlModule.Items.Add(New ListItem(dRow("desctiption"), dRow("module")))
        Next
        ddlModule.Items.Insert(0, New ListItem("", ""))
    End Sub

    Protected Sub GetSeverity()
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        ds = New DataSet

        ddlSeverity.Items.Clear()
        dt = oWS.GetSeverity()
        ds.Tables.Add(dt)
        For Each dRow As Data.DataRow In ds.Tables(0).Rows
            ddlSeverity.Items.Add(New ListItem(dRow("Value") & " " & dRow("Description"), dRow("Value")))
        Next
        ddlSeverity.SelectedIndex = ddlSeverity.Items.IndexOf(ddlSeverity.Items.FindByValue("Medium"))
        'ddlSeverity.Items.Insert(0, New ListItem("", ""))
    End Sub

    Protected Sub GetProgramGroup(ByVal sModule As String)
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        ds = New DataSet

        ddlProgramGroup.Items.Clear()
        dt = oWS.GetProgramGroup(sModule)
        ds.Tables.Add(dt)
        For Each dRow As Data.DataRow In ds.Tables(0).Rows
            ddlProgramGroup.Items.Add(New ListItem(dRow("programe_group"), dRow("programe_group")))
        Next
        ddlProgramGroup.Items.Insert(0, New ListItem("", ""))
    End Sub

    Protected Sub GetCustomer(ByVal sCustGroup As String)
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        ds = New DataSet

        ddlCustomer.Items.Clear()
        dt = oWS.GetCustomer(Session("CustGroup").ToString)
        ds.Tables.Add(dt)
        For Each dRow As Data.DataRow In ds.Tables(0).Rows
            ddlCustomer.Items.Add(New ListItem(dRow("name"), dRow("cust_num")))
        Next

        If Session("CustGroup").ToString <> String.Empty Then
            If Session("Category") = "Admin" Then 'Session("Customer") = "PPCC" Then
                ddlCustomer.Items.Insert(0, New ListItem("", ""))
            Else
                If ds.Tables(0).Rows.Count = 1 Then
                    ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(Session("Customer").ToString))
                Else
                    ddlCustomer.Items.Insert(0, New ListItem("", ""))
                End If
            End If
        Else
            'condition when session customer is null
        End If
    End Sub

    'Protected Sub GetSupportType()
    '    oWS = New ServiceSupport.ServiceSupportSoapClient
    '    dt = New DataTable
    '    ds = New DataSet
    '    ddlType.Items.Clear()
    '    dt = oWS.GetTypeSupport()
    '    ds.Tables.Add(dt)
    '    For Each dRow As Data.DataRow In ds.Tables(0).Rows
    '        ddlType.Items.Add(New ListItem(dRow("description"), dRow("type")))
    '    Next
    '    ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue("L"))
    'End Sub

    Protected Sub GetDatabaseSite(ByVal sSession As String)
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        ds = New DataSet

        ddlDbSite.Items.Clear()

        If Session("Category") <> "Admin" Then 'sSession <> "PPCC" Then
            dt = oWS.GetDatabaseSite(ddlCustomer.SelectedItem.Value)
            ds.Tables.Add(dt)
            For Each dRow As Data.DataRow In ds.Tables(0).Rows
                ddlDbSite.Items.Add(New ListItem(dRow("description"), dRow("db_site")))
            Next
        Else
            If ddlCustomer.SelectedItem.Value <> String.Empty Then
                dt = oWS.GetDatabaseSite(ddlCustomer.SelectedItem.Value)
                ds.Tables.Add(dt)
                For Each dRow As Data.DataRow In ds.Tables(0).Rows
                    ddlDbSite.Items.Add(New ListItem(dRow("description"), dRow("db_site")))
                Next
            End If
        End If
        ddlDbSite.Items.Insert(0, New ListItem("", ""))
    End Sub

    Protected Sub SetCustomerEmailSupport(ByVal sCustNum As String)
        Dim sEmailSupport As String
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        dt = oWS.GetCustomerEmailSupport(sCustNum)

        If dt.Rows.Count > 0 Then
            sEmailSupport = IIf(IsDBNull(dt.Rows(0).Item("email_support")), String.Empty, dt.Rows(0).Item("email_support"))
        Else
            sEmailSupport = String.Empty
        End If
        txtEmail.Text = sEmailSupport
    End Sub

    Protected Function GetCustomerEmailSupport(ByVal sCustNum As String) As String
        Dim sEmailSupport As String
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        dt = oWS.GetCustomerEmailSupport(sCustNum)

        If dt.Rows.Count > 0 Then
            sEmailSupport = IIf(IsDBNull(dt.Rows(0).Item("email_support")), String.Empty, dt.Rows(0).Item("email_support"))
        Else
            sEmailSupport = String.Empty
        End If

        Return sEmailSupport
    End Function

    Protected Function GetSupportMailByCustomer(CustNum As String) As String
        Dim mail As String
        Dim objMail As GetMailSupport = New GetMailSupport()
        mail = objMail.GetMailSupport(CustNum)
        Return mail
    End Function

    Protected Sub UploadFile()
        Dim sFileName As String
        Dim sFilePathName As String
        Dim sBasePath As String

        sFilePathName = String.Empty
        If FileUpload.HasFiles Then
            sFileName = Path.GetFileName(FileUpload.FileName)
            sBasePath = AppDomain.CurrentDomain.BaseDirectory + "Files"
            If Not Directory.Exists(sBasePath) Then
                Directory.CreateDirectory(sBasePath)
            End If
            sFilePathName = sBasePath & "\" & sFileName
            FileUpload.SaveAs(sFilePathName)
        End If
    End Sub

    Protected Sub ClearText()
        GetStatusSupport()
        GetCustomer(Session("CustGroup").ToString())
        GetRequestor(ddlCustomer.SelectedItem.Value)
        GetDatabaseSite(ddlCustomer.SelectedItem.Value)
        GetModuleSupport()
        GetSeverity()
        GetProgramGroup(String.Empty)
        txtEmail.Text = String.Empty
        txtProgramName.Text = String.Empty
        txtSubject.Text = String.Empty
        txtDesc.Text = String.Empty
        FileUpload.Attributes.Clear()
        CalculateProjectedDate()
        GetInformProblem()
        GetResponse()
        ddlRequestTime.Items.Clear()
        ddlResponseTime.Items.Clear()
        'txtResponseDate.Text = String.Empty
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim sLog As String = String.Empty
        Dim sEmail As String = String.Empty
        Dim sEmailSupport As String = String.Empty
        Dim objSendMail As SendMail = New SendMail()
        Dim arrEmail As String()
        Dim iAfterWork As Integer
        Dim iHoliday As Integer
        iAfterWork = 0
        iHoliday = 0
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        ds = New DataSet
        Try
            If CheckBoxAfterWork.Checked = True Then
                iAfterWork = 1
            Else
                iAfterWork = 0
            End If

            If CheckBoxHoliday.Checked = True Then
                iHoliday = 1
            Else
                iHoliday = 0
            End If

            UploadFile()
            CalculateProjectedDate()
            dt = oWS.InsertSupportLog(txtLogID.Text,
                                      ddlStat.SelectedItem.Value,
                                      ddlCustomer.SelectedItem.Value,
                                      ddlDbSite.SelectedItem.Value,
                                      txtProgramName.Text,
                                      txtSubject.Text,
                                      RTrim(LTrim(txtDesc.Text.Replace("<div>", "").Replace("</div>", ""))),
                                      IIf(FileUpload.HasFiles, "Files\" & Path.GetFileName(FileUpload.FileName), String.Empty),
                                      txtEmail.Text,
                                      ddlRequestor.SelectedItem.Value,
                                      ddlModule.SelectedItem.Value,
                                      ddlSeverity.SelectedItem.Value,
                                      ddlProgramGroup.SelectedItem.Value,
                                      txtProjectedDate.Text,
                                      txtRequestDate.Text,
                                      iAfterWork,
                                      iHoliday,
                                      ddlInfProblem.SelectedItem.Value,
                                      RTrim(LTrim(txtCause.Text.Replace("<div>", "").Replace("</div>", ""))),
                                      txtHowtoCheck.Text,
                                      txtCurrentSol.Text,
                                      txtLongTermSol.Text,
                                      ddlRequestTime.SelectedItem.Text,
                                      ddlAcknowledge.SelectedItem.Value,
                                      txtResponseDate.Text,
                                      ddlResponseTime.SelectedItem.Text,
                                      Session("Username").ToString()
)

            If dt.Rows.Count > 0 Then
                sEmailSupport = GetSupportMailByCustomer(ddlCustomer.SelectedItem.Value)
                sLog = dt.Rows(0).Item("log_id")
                sEmail = dt.Rows(0).Item("email")
                arrEmail = dt.Rows(0).Item("email").ToString().Split(",")
                objSendMail.SendMailSupport(arrEmail, sEmailSupport, dt)
                PassNotifyPanel.Visible = True
                PassText.Text = "Log ID. " & sLog & " saved successfully. Information will be sent to " & dt.Rows(0).Item("email").ToString.Replace(",", " ")
            Else
                NotPassNotifyPanel.Visible = True
                NotPassText.Text = "Saved failed. Please try agian."
            End If
            ClearText()
        Catch ex As Exception
            NotPassNotifyPanel.Visible = True
            NotPassText.Text = ex.Message
        End Try
    End Sub

    Protected Sub ddlCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCustomer.SelectedIndexChanged
        GetDatabaseSite(ddlCustomer.SelectedItem.Value)
        GetRequestor(ddlCustomer.SelectedItem.Value)
        SetCustomerEmailSupport(ddlCustomer.SelectedItem.Value)
        PassNotifyPanel.Visible = False
        NotPassNotifyPanel.Visible = False
    End Sub

    Protected Sub ddlRequestor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRequestor.SelectedIndexChanged
        Dim sEmail As String = String.Empty
        Dim sSupportEmail As String = String.Empty
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        sSupportEmail = GetCustomerEmailSupport(ddlCustomer.SelectedItem.Value)
        If ddlRequestor.SelectedItem.Value <> String.Empty Then
            dt = oWS.GetEmailRequestor(ddlCustomer.SelectedItem.Value,
                                       ddlRequestor.SelectedItem.Value)
            If dt.Rows.Count > 0 Then
                sEmail = IIf(IsDBNull(dt.Rows(0).Item("email")), String.Empty, dt.Rows(0).Item("email"))
            Else
                sEmail = String.Empty
            End If

            If sSupportEmail <> String.Empty Then
                sEmail = sSupportEmail + IIf(sEmail = String.Empty, String.Empty, "," + sEmail)
            Else
                sEmail = sEmail
            End If

            txtEmail.Text = sEmail
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ClearText()
    End Sub

    Protected Sub ddlModule_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlModule.SelectedIndexChanged
        If ddlModule.SelectedItem.Value <> String.Empty Then
            GetProgramGroup(ddlModule.SelectedItem.Value)
        End If
    End Sub

    Protected Sub DisabledEnabledDropdown()
        'ddlProgramGroup.Attributes.Add("disabled", "disabled")
        txtProjectedDate.Attributes.Add("disabled", "disabled")
    End Sub

    Protected Sub ddlSeverity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSeverity.SelectedIndexChanged
        Dim sReqDate As Date
        Dim sDateNow As Date
        sReqDate = CDate(txtRequestDate.Text)
        sDateNow = Date.Now.ToString("dd/MM/yyyy")
        If sReqDate = sDateNow Then
            CalculateProjectedDate()
        Else
            CalculateProjectedDateWithReqDate(sReqDate)
        End If
    End Sub

    Protected Sub CalculateProjectedDate()
        Dim sSeverity As String
        Dim iSeverity As Integer
        Dim dProjectDate As Date
        Dim arrSeverity() As Char
        Dim sDay As String
        iSeverity = 0
        sDay = String.Empty
        If ddlSeverity.SelectedItem.Value <> String.Empty Then
            sSeverity = ddlSeverity.SelectedItem.Text
            arrSeverity = sSeverity.ToCharArray()

            For Each CharSeverity As Char In arrSeverity
                If Char.IsDigit(CharSeverity) Then
                    sDay = sDay & CharSeverity
                End If
            Next

            If Len(sDay) > 1 Then
                iSeverity = CInt(Left(sDay, 1))
            Else
                iSeverity = CInt(sDay)
            End If

            'Select Case sSeverity
            '    Case "High"
            '        iSeverity = 1
            '    Case "Medium"
            '        iSeverity = 3
            '    Case "Low"
            '        iSeverity = 5
            '    Case Else
            '        iSeverity = 3
            'End Select

            oWS = New ServiceSupport.ServiceSupportSoapClient
            dt = New DataTable
            dt = oWS.GetProjectedDate(Date.Now(), iSeverity, "WD")
            If dt.Rows.Count > 0 Then
                dProjectDate = IIf(IsDBNull(dt.Rows(0).Item("WorkingDate")), "", dt.Rows(0).Item("WorkingDate"))
            Else
                dProjectDate = Date.Now()
            End If
            txtProjectedDate.Text = dProjectDate
        End If
    End Sub

    Protected Sub CalculateProjectedDateWithReqDate(ByVal ReqDate As Date)
        Dim sSeverity As String
        Dim iSeverity As Integer
        Dim dProjectDate As Date
        Dim arrSeverity() As Char
        Dim sDay As String
        iSeverity = 0
        sDay = String.Empty
        If ddlSeverity.SelectedItem.Value <> String.Empty Then
            sSeverity = ddlSeverity.SelectedItem.Text
            arrSeverity = sSeverity.ToCharArray()

            For Each CharSeverity As Char In arrSeverity
                If Char.IsDigit(CharSeverity) Then
                    sDay = sDay & CharSeverity
                End If
            Next

            If Len(sDay) > 1 Then
                iSeverity = CInt(Left(sDay, 1))
            Else
                iSeverity = CInt(sDay)
            End If

            'Select Case sSeverity
            '    Case "High"
            '        iSeverity = 1
            '    Case "Medium"
            '        iSeverity = 3
            '    Case "Low"
            '        iSeverity = 5
            '    Case Else
            '        iSeverity = 3
            'End Select

            oWS = New ServiceSupport.ServiceSupportSoapClient
            dt = New DataTable
            dt = oWS.GetProjectedDate(ReqDate, iSeverity, "WD")
            If dt.Rows.Count > 0 Then
                dProjectDate = IIf(IsDBNull(dt.Rows(0).Item("WorkingDate")), "", dt.Rows(0).Item("WorkingDate"))
            Else
                dProjectDate = Date.Now()
            End If
            txtProjectedDate.Text = dProjectDate
        End If
    End Sub

    Protected Sub txtRequestDate_TextChanged(sender As Object, e As EventArgs) Handles txtRequestDate.TextChanged
        Dim sReqDate As Date
        If txtRequestDate.Text <> String.Empty Then
            sReqDate = CDate(txtRequestDate.Text)
            CalculateProjectedDateWithReqDate(sReqDate)
        End If
    End Sub

    Protected Sub GetInformProblem()
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        ds = New DataSet

        ddlInfProblem.Items.Clear()
        dt = oWS.GetInformProblem()
        ds.Tables.Add(dt)
        For Each dRow As Data.DataRow In ds.Tables(0).Rows
            ddlInfProblem.Items.Add(New ListItem(dRow("description"), dRow("Value")))
        Next
        ddlInfProblem.SelectedIndex = ddlInfProblem.Items.IndexOf(ddlInfProblem.Items.FindByValue("O"))
    End Sub

    Protected Sub GetResponse()
        oWS = New ServiceSupport.ServiceSupportSoapClient
        dt = New DataTable
        ds = New DataSet

        ddlAcknowledge.Items.Clear()
        dt = oWS.GetResource()
        ds.Tables.Add(dt)
        For Each dRow As Data.DataRow In ds.Tables(0).Rows
            ddlAcknowledge.Items.Add(New ListItem(dRow("res_id") & " - " & dRow("name"), dRow("res_id")))
        Next
        ddlAcknowledge.Items.Insert(0, New ListItem("", ""))
    End Sub

    Protected Sub GetRequestAndResponseTime()
        'Set start time (00:00 means 12:00 AM)
        Dim StartTime As DateTime = DateTime.ParseExact("00:00", "HH:mm", Nothing)
        'Set end time (23:55 means 11:55 PM)
        Dim EndTime As DateTime = DateTime.ParseExact("23:55", "HH:mm", Nothing)
        'Set 5 minutes interval
        Dim Interval As New TimeSpan(0, 5, 0)
        'To set 1 hour interval
        'Dim Interval As New TimeSpan(1, 0, 0)
        'ddlRequestTime.Items.Clear()
        'ddlResponseTime.Items.Clear()
        Dim RequestTime As String = txtRequestDate.Text
        Dim ResponseTime As String = txtResponseDate.Text
        While StartTime <= EndTime
            ddlRequestTime.Items.Add(StartTime.ToShortTimeString())
            ddlResponseTime.Items.Add(StartTime.ToShortTimeString())
            StartTime = StartTime.Add(Interval)
        End While
        ddlRequestTime.Items.Insert(0, New ListItem(RequestTime, "0"))
        ddlResponseTime.Items.Insert(0, New ListItem(ResponseTime, "0"))
    End Sub

End Class