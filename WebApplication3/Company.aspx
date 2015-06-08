<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Company.aspx.cs" Inherits="WebApplication3.WebForm1" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>


<head runat="server">
    <title></title>
<script src="scripts/jquery-1.7.1.min.js"></script>
    <script>
        jQuery(document).ready(function () {
            jQuery('.tabs .tab-links a').on('click', function (e) {
                var currentAttrValue = jQuery(this).attr('href');

                // Show/Hide Tabs
                jQuery('.tabs ' + currentAttrValue).show().siblings().hide();

                // Change/remove current tab to active
                jQuery(this).parent('li').addClass('active').siblings().removeClass('active');

                e.preventDefault();
            });
        });
    </script>

    <style>


    .tabs {
    width:100%;
    display:inline-block;
    }

    /*----- Tab Links -----*/
    /* Clearfix */
    .tab-links:after {
    display:block;
    clear:both;
    content:'';
    }

    .tab-links li {
    margin:0px 1px;
    float:left;
    list-style:none;
    }

    .tab-links a {
    padding:9px 15px;
    display:inline-block;
    border-radius:3px 3px 0px 0px;
    background:#f27922;
    font-family:Arial;
    font-size:12px;
    font-weight:300;
    color:#fff;
    transition:all linear 0.15s;
    }

    .tab-links a:hover {
    background:#c9c9c9;
    text-decoration:none;
    }

    li.active a, li.active a:hover {
    background:#c9c9c9;
    color:#fff;
    }

    /*----- Content of Tabs -----*/
    .tab-content {
    padding:15px;
    border-radius:3px;
    box-shadow:-1px 1px 1px rgba(0,0,0,0.15);
    background:#fff;
    }

    .tab {
    display:none;
    }

    .tab.active {
    display:block;
    }
    </style>
</head>

 

<body>
    
    <form id="form1" runat="server">
     <div>
    
        <strong>Please Enter a Domain</strong><asp:TextBox ID="TextBox1" runat="server" Height="22px" Width="182px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Height="31px" OnClick="Button1_Click" style="font-weight: 700" Text="Search" Width="130px" />
        <br />
    
    </div>
    <div class="tabs">
       

        <div class="tab-content">


                     <asp:Label ID="Label1" runat="server"></asp:Label>
                        
                        <asp:GridView ID="GridView1" runat="server" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
        </asp:GridView>
                        
        

              <asp:Label ID="lbl_FromUsTotal" runat="server" Font-Bold="True" Font-Size="Large" Text="Label"></asp:Label>
                        <br />
                        <asp:Label ID="lbl_FromThemTotal" runat="server" Font-Bold="True" Font-Size="Large" Text="Label"></asp:Label>
                        <br />
                        <asp:Label ID="lbl_TotalEmailCount" runat="server" Font-Bold="True" Font-Size="Large" Text="Label"></asp:Label>
                        <br />
                        <asp:Label ID="lbl_TotalInteractionTime" runat="server" Font-Bold="True" Font-Size="Large" Text="Label"></asp:Label>
                        <br />
                     <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                        <br />
                     <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                        <br />
                        <asp:Label ID="lbl_interactionratio" runat="server" Font-Bold="True" Font-Size="Large" Text="Label"></asp:Label>
          <asp:Chart ID="Chart1" runat="server" Height="298px" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" Width="963px">
            <series>
                <asp:Series ChartType="Line" Name="Series1">
                </asp:Series>
            </series>
            <chartareas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </chartareas>
        </asp:Chart>

        <br />
        <span class="auto-style1"><span class="auto-style2">Blue Line</span> - Outgoing Email</span><br class="auto-style1" />
        <span class="auto-style1"><span class="auto-style3">Orange Line</span> - Incoming Email </span>
                    </div>

        
            <asp:GridView ID="GridView2" runat="server">
                 </asp:GridView>

        

            <div id="tab5" class="tab">
                <p>Tab #4 content goes here!</p>
                <p>Donec pulvinar neque sed semper lacinia. Curabitur lacinia ullamcorper nibh; quis imperdiet velit eleifend ac. Donec blandit mauris eget aliquet lacinia! Donec pulvinar massa interdum risus ornare mollis. In hac habitasse platea dictumst. Ut euismod tempus hendrerit. Morbi ut adipiscing nisi. Etiam rutrum sodales gravida! Aliquam tellus orci, iaculis vel.</p>
            </div>



                 
                 <asp:TextBox ID="TextBox2" runat="server" Height="133px" Width="823px"></asp:TextBox>



    </form>
</body>

