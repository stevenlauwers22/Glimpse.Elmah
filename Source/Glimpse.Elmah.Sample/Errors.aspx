<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Errors.aspx.cs" Inherits="Glimpse.Elmah.Sample.Errors" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
    
    <head>
        
        <title>Glimpse.Elmah.Sample</title>

    </head>

    <body>
        
        <form id="form" runat="server">
            <fieldset>
                <p>
                    <asp:DropDownList ID="ExceptionTypeToThrowDropDownList" runat="server" />
                </p>
            </fieldset>

            <br />

            <asp:Button ID="SubmitButton" runat="server" Text="Throw" OnClick="SubmitButtonClick" />
        </form>

    </body>

</html>