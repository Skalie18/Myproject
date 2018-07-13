<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Mics_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <fieldset>
        <sars:DropDownField 
            runat="server" 
            ID="ddlTest" 
            AutoBind="True" 
            DataValueField="ID" 
            Width="350px" 
            DataTextField="Description" 
            DatasetName="dsFileCategories" />
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

