<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TestControls.aspx.cs" Inherits="Mics_TestControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <fieldset>
        <sars:DropDownField 
            runat="server" 
            ID="ddlTest" 
            AutoBind="True" 
            DataValueField="CountryCode" 
            Width="350px" 
            DataTextField="Country" 
            DatasetName="dsCountries" />
    </fieldset>
    <fieldset>
        <sars:RadioButtonListField 
            RepeatColumns="3" 
            runat="server" 
            ID="DropDownField1" 
            AutoBind="True" 
            DataValueField="CountryCode" 
            DataTextField="Country" 
            DatasetName="dsCountries" />
    </fieldset>

    <fieldset>
        <sars:CheckBoxListField 
            RepeatColumns="3" 
            runat="server" 
            ID="RadioButtonListField1" 
            AutoBind="True" 
            DataValueField="CountryCode" 
            DataTextField="Country" 
            DatasetName="dsCountries" />

    </fieldset>

</asp:Content>


