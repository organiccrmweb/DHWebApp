<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="WebApplication3.Contact" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
</head>
<body>
    <form id="form1" runat="server">

         <strong>Please Enter a Email address</strong><asp:TextBox ID="TextBox1" runat="server" Height="22px" Width="182px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Height="31px" OnClick="Button1_Click" style="font-weight: 700" Text="Search" Width="130px" />

                    <asp:Label ID="lbl_active" runat="server"></asp:Label>
                    <asp:Label ID="lbl_fading" runat="server"></asp:Label>
                    <asp:Label ID="lbl_cold" runat="server"></asp:Label>
                    <asp:Label ID="lbl_inactive" runat="server"></asp:Label>
                    <br />
                    <span class="newStyle3"><strong>Contact Summary</strong></span><br />
                    <asp:Label ID="lbl_fromustotal" runat="server"></asp:Label>
                    <span class="newStyle2">
                    <br />
                    </span>
                    <asp:Label ID="lbl_fromthemtotal" runat="server"></asp:Label>
                    <span class="newStyle2">
                    <br />
                    </span>
                    <asp:Label ID="lbl_totalemailcount" runat="server"></asp:Label>
                    <span class="newStyle2">
                    <br />
                    </span>
                    <asp:Label ID="lbl_totalinteractiontime" runat="server"></asp:Label>
                    <span class="newStyle2">
                    <br />
                    </span>
                    <asp:Label ID="lbl_interactionration" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lbl_totalmeeting" runat="server" CssClass="newStyle1"></asp:Label>
                    <br />
                    <br />
                    <span class="newStyle4"><strong>Relationship Health Status</strong></span></div>
                      
            

         
            <asp:Chart ID="Chart1" runat="server" Height="128px" Width="229px">
                <Series>
                    <asp:Series ChartType="Pie" Name="Series1">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
        </asp:Chart>
         <br />
         <br />
         Colleagues<br />
         <asp:GridView ID="GridView3" runat="server">
         </asp:GridView>
         <br />
         Contacts<br />
                      <asp:GridView ID="GridView2" runat="server">
                 </asp:GridView>

            

         
            <br /></form>
</body>
</html>
