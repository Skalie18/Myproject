<%@ Page Language="C#" AutoEventWireup="true" CodeFile="updateadditionalinfo.aspx.cs" Inherits="queueMonitors_MANUAL_updateadditionalinfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="UPDATE ADDITIONAL INFO" />
        <br />
        <br />
        <asp:Button ID="btnUpdateReportingPeriod" runat="server" OnClick="btnUpdateReportingPeriod_Click" Text="UPDATE REPORTING PERIOD" />
        <br />
        <br />
        <asp:Button ID="btnUpdateCorrections" runat="server" OnClick="btnUpdateCorrections_Click" Text="UPDATE MESSAGE SPEC CORRECTION FIELD" />
    </form>
</body>
</html>
