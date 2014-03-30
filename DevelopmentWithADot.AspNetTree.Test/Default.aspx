<%@ Page Language="C#" CodeBehind="Default.aspx.cs" Inherits="DevelopmentWithADot.AspNetTree.Test.Default" %>
<%@ Register Assembly="DevelopmentWithADot.AspNetTree" Namespace="DevelopmentWithADot.AspNetTree" TagPrefix="web" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title></title>
	<style type="text/css">

		.tree
		{
			background-color: cyan;
			border: solid 1px;
			overflow: auto;
		}

	</style>
</head>
<body>
	<form runat="server">
	<div>
		<asp:ScriptManager runat="server" />
		<web:Tree runat="server" ID="tree" ShowCheckBoxes="Leaf" SelectedNodeStyle-BackColor="Yellow" CssClass="tree" RootText="World" RootImageUrl="~/0.png" OnPopulateTreeNode="OnPopulateNode" BorderStyle="Solid" BorderColor="Blue" BorderWidth="1px" Width="200px" Height="300px" />
		<asp:Button runat="server" Text="Select" OnClick="OnClick" />
	</div>
	</form>
</body>
</html>
