﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/OldSupport.Master" CodeBehind="OldIssueSummary.aspx.vb" Inherits="Support.OldIssueSummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker.css" rel="stylesheet" />
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="http://jqueryjs.googlecode.com/files/jquery-1.3.1.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var Start = $('#<%=txtStartReqDate.ClientID%>');
            var End = $('#<%=txtEndReqDate.ClientID%>');
            Start.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });

            End.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        });
    </script>

    <script type='text/javascript'>
        function openModal() {
            $('[id*=myModal]').modal('show');
        } 
    </script>
    
    <style type="text/css">
       .hide { display: none; }

       .lebel-text {
           font-family: 'Cloud-Light';
           color: #000000;
           font-weight: normal;
           font-size:medium;
       }

    </style> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
    <div class="row">
        <div class="col-xs-12">
            <div class="input-group">
                <br />
            </div>
        </div>
        <div class="col-xs-12">
            <div class="input-group">
                <div class="col-xs-2 text-right">
                    <asp:Label ID="lblCustomer" runat="server" class="form-control control-label hor-form" BorderWidth="0px" Text="Customer"></asp:Label>
                </div>
                <div class="col-xs-3">
                    <asp:DropDownList ID="ddlStartCust" AutoPostBack="true" runat="server" class="form-control"></asp:DropDownList>
                </div>
                <div class="col-xs-3">
                    <asp:DropDownList ID="ddlEndCust" AutoPostBack="true" runat="server" class="form-control"></asp:DropDownList>
                </div>
                <div class="col-xs-4">
                </div>
            </div>
        </div>
        <div class="col-xs-12">
            <div class="input-group">
                <div class="col-xs-2 text-right">
                    <asp:Label ID="lblLogDate" runat="server" class="form-control control-label hor-form" BorderWidth="0px" Text="Request Date"></asp:Label>
                </div>
                <div class="col-xs-3">
                    <asp:TextBox runat="server" ID="txtStartReqDate" AutoPostBack="true" AutoComplete="off" ToolTip="Filter by Request Date" placeholder="Start Request Date" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-xs-3">
                    <asp:TextBox runat="server" ID="txtEndReqDate" AutoPostBack="true" AutoComplete="off" ToolTip="Filter by Request Date" placeholder="End Request Date" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-xs-4">
                </div>
                
            </div>
        </div>

        <div class="col-xs-12">
            <div class="input-group">
                <div class="col-xs-2 text-right">
                    
                </div>
                <div class="col-xs-3">
                    <asp:CheckBox ID="CheckBox1" CssClass="form-check-input" runat="server" Text=" Case ที่ยังไม่ทราบสาเหตุ" />
                </div>
                <div class="col-xs-3">
                    
                </div>
                <div class="col-xs-4">
                </div>
                
            </div>
        </div>

        <div class="col-xs-12">
            <div class="input-group">
                <div class="col-xs-3">
                </div>
                <div class="col-xs-4">
                    <br />
                    <label class="login-do hvr-shutter-in-horizontal login-sub">
                        <asp:Button ID="bntLoad" runat="server" Text="Download Excel" />
                    </label>
                </div>
                <div class="col-xs-5">
                </div>
            </div>
        </div>
        <div class="col-xs-12">
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
        </div>
    </div>
    </div>

</asp:Content>

