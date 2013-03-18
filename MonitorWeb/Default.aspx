<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MonitorWeb._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            setInterval('getDate()', 1000);
            setInterval('getStatus()', 5000);
            getDraw();
            $("#tutorial").mousemove(function(e) {
            $('#lbltxt').html((e.pageX -8) + ', ' + (e.pageY-115));
            }); 
        });
        function getDate() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Default.aspx/DisplayData",
                data: "{}",
                dataType: "json",
                success: function(data) {
                    $("#lbltxt").text(data.d);
                },
                error: function(result) {
                    alert("Error");
                }
            });
        }
        function getStatus() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Default.aspx/GetStatus",
                data: "{}",
                dataType: "json",
                success: function(data) {
                    var testL = data.d;
                    if ($(testL).length > 0) {
                        var testList = testL.sort(function(a, b) { return a.Order > b.Order; });
                        var row = "<tr>"
                        $.each(testList, function(i, item) {
                        row = row + "<td title='" + item.AvgTime + "'" + "style='background-color: " + getColor(item.PageTime, item.AvgTime) + ";' >" + item.LasTime + " (" + item.PageTime + ")" + "</td>";
                        });
                        row = row + "</tr>";
                        if ($("#Dashboard")[0].rows.length > 1)
                            $("#Dashboard")[0].deleteRow(1);
                        $("#Dashboard").append(row);
                    }
                },
                error: function(result) {
                    alert("Error");
                }
            });
        }
        function GetTestFlow(rows) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Default.aspx/GetTestFlow",
                data: "{numberOfRows: " + rows + "}",
                dataType: "json",
                success: function(data) {
                    var testL = data.d;
                    if ($(testL).length > 0) {
                        var testList = testL.sort(function(a, b) { return a.TestNumber < b.TestNumber; });
                        while ($("#History")[0].rows.length > 1) {
                            $("#History")[0].deleteRow(1);
                        }
                        var row = "<tr>"
                        $.each(testList, function(i, item) {
                            row = "<tr>"
                            row = row + "<td>" + item.TestNumber + "</td>";
                            row = row + "<td>" + item.LoanNumber + "</td>";
                            row = row + "<td>" + item.Start + "</td>";
                            row = row + "<td>" + item.Finish + "</td>";
                            var val;
                            (item.ApplicationForm == -1) ? val = '' : val = item.ApplicationForm;
                            row = row + "<td>" + val + "</td>";
                            (item.PrivacyPolicy == -1) ? val = '' : val = item.PrivacyPolicy;
                            row = row + "<td>" + val + "</td>";
                            (item.UpSell == -1) ? val = '' : val = item.UpSell;
                            row = row + "<td>" + val + "</td>";
                            (item.ESign == -1) ? val = '' : val = item.ESign;
                            row = row + "<td>" + val + "</td>";
                            (item.ThankYou == -1) ? val = '' : val = item.ThankYou;
                            row = row + "<td>" + val + "</td>";
                            row = row + "</tr>";
                            $("#History").append(row);
                        });
                    }
                },
                error: function(result) {
                    alert("Error");
                }
            });
        }
        function getColor(pageTime, avgTime) {
            var dif = avgTime - pageTime;
            var bufferGreen=2,bufferYellow=5;
            if (avgTime - (pageTime - bufferGreen) >= 0)
                return "lightgreen";
            else if (avgTime - (pageTime - bufferYellow) >= 0)
                return "yellow";
            else
                return "red";
        }
        function getDraw() {
            var canvas = document.getElementById('tutorial');
            if (canvas.getContext) {
                var ctx = canvas.getContext("2d");

                roundedRect(ctx, 12, 12, 150, 150, 15);
                roundedRect(ctx, 19, 19, 150, 150, 9);
                roundedRect(ctx, 53, 53, 49, 33, 10);
                roundedRect(ctx, 53, 119, 49, 16, 6);
                roundedRect(ctx, 135, 53, 49, 33, 10);
                roundedRect(ctx, 135, 119, 25, 49, 10);
                //Pacman
                ctx.beginPath();
                ctx.arc(37, 37, 13, Math.PI / 7, -Math.PI / 7, false);
                ctx.lineTo(31, 37);
                ctx.fillStyle = "Yellow";
                ctx.fill();
                ctx.stroke();
                ctx.beginPath();
                ctx.arc(37, 30,2, 0, Math.PI * 2, true);
                ctx.fillStyle = "Black";
                ctx.fill();
                ctx.stroke();
                
                
                ctx.beginPath();
                for (var i = 0; i < 8; i++) {
                    ctx.fillStyle = "Black";
                    ctx.fill();
                    ctx.fillRect(51 + i * 16, 35, 4, 4);
                }
                for (i = 0; i < 6; i++) {
                    ctx.fillRect(115, 51 + i * 16, 4, 4);
                }
                for (i = 0; i < 8; i++) {
                    ctx.fillRect(51 + i * 16, 99, 4, 4);
                }
                //Pink Ghost
                ctx.beginPath();
                ctx.strokeStyle = 'Black';
                ctx.moveTo(24, 140);
                ctx.lineTo(24, 126);
                ctx.moveTo(24, 126);
                ctx.bezierCurveTo(24, 115, 48, 115, 48, 126);                
                ctx.moveTo(48, 126);
                ctx.lineTo(48, 140);
                ctx.lineTo(44, 135);
                ctx.lineTo(40, 140);
                ctx.lineTo(36, 135);
                ctx.lineTo(32, 140);
                ctx.lineTo(28, 135);
                ctx.lineTo(24, 140);
                ctx.lineTo(24, 126);
                ctx.fillStyle = "Pink";
                ctx.stroke();
                ctx.fill();
                ctx.beginPath();
                ctx.moveTo(28, 131);
                ctx.bezierCurveTo(28, 131, 27.5, 130, 28, 126);
                ctx.bezierCurveTo(28, 126, 31, 122, 34, 126);
                ctx.bezierCurveTo(34, 126, 35.5, 131, 34, 131);
                ctx.bezierCurveTo(34, 131, 31, 135, 28, 131);
                ctx.fillStyle = "White";

                ctx.moveTo(40, 131);
                ctx.bezierCurveTo(40, 131, 39.5, 130, 40, 126);
                ctx.bezierCurveTo(40, 126, 43, 122, 46, 126);
                ctx.bezierCurveTo(46, 126, 47.5, 131, 46, 131);
                ctx.bezierCurveTo(46, 131, 43, 135, 40, 131);
                ctx.fillStyle = "White";
                //ctx.stroke();
                ctx.fill();
                ctx.beginPath();
                ctx.arc(31, 126, 2, 0, Math.PI * 2, true);
                ctx.fillStyle = "Blue";
                ctx.fill();
                ctx.beginPath();
                ctx.arc(43, 126, 2, 0, Math.PI * 2, true);
                ctx.fillStyle = "Blue";
                ctx.fill();
                
                ctx.beginPath();
                ctx.strokeStyle = 'Black';
                ctx.moveTo(83, 116);
                ctx.lineTo(83, 102);
                ctx.bezierCurveTo(83, 94, 89, 88, 97, 88);
                ctx.bezierCurveTo(105, 88, 111, 94, 111, 102);
                ctx.stroke();
                ctx.lineTo(111, 116);
                ctx.lineTo(106.333, 111.333);
                ctx.lineTo(101.666, 116);
                ctx.lineTo(97, 111.333);
                ctx.lineTo(92.333, 116);
                ctx.lineTo(87.666, 111.333);
                ctx.lineTo(83, 116);
                ctx.fillStyle = "Blue";
                ctx.fill();                
                ctx.beginPath();
                ctx.moveTo(91, 96);
                ctx.bezierCurveTo(88, 96, 87, 99, 87, 101);
                ctx.bezierCurveTo(87, 103, 88, 106, 91, 106);
                ctx.bezierCurveTo(94, 106, 95, 103, 95, 101);
                ctx.bezierCurveTo(95, 99, 94, 96, 91, 96);
                ctx.moveTo(103, 96);
                ctx.bezierCurveTo(100, 96, 99, 99, 99, 101);
                ctx.bezierCurveTo(99, 103, 100, 106, 103, 106);
                ctx.bezierCurveTo(106, 106, 107, 103, 107, 101);
                ctx.bezierCurveTo(107, 99, 106, 96, 103, 96);
                ctx.fillStyle = "white";
                ctx.fill();
                ctx.fillStyle = "black";
                ctx.beginPath();
                ctx.arc(101, 102, 2, 0, Math.PI * 2, true);
                ctx.fill();
                ctx.beginPath();
                ctx.arc(89, 102, 2, 0, Math.PI * 2, true);
                ctx.fill();
            }
        }
        function roundedRect(ctx, x, y, width, height, radius) {
            ctx.beginPath();
            ctx.moveTo(x, y + radius);
            ctx.lineTo(x, y + height - radius);
            ctx.quadraticCurveTo(x, y + height, x + radius, y + height);
            ctx.lineTo(x + width - radius, y + height);
            ctx.quadraticCurveTo(x + width, y + height, x + width, y + height - radius);
            ctx.lineTo(x + width, y + radius);
            ctx.quadraticCurveTo(x + width, y, x + width - radius, y);
            ctx.lineTo(x + radius, y);
            ctx.quadraticCurveTo(x, y, x, y + radius);
            ctx.stroke();
        }
        function getXYPosition(e) {
            var x = e.pageX - this.offsetLeft;
            var y = e.pageY - this.offsetTop;

            $('#lbltxt').html(x + ', ' + y);
        }
    </script >        
    <style type="text/css">
        canvas {border:1px solid black;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    <input type="button" id="Test" value="Test" onclick="GetTestFlow(10);"/>
    <input type="button" id="Button1" value="Draw" onclick="getDraw();"/>
    <table id="Dashboard" border="1" cellspacing="0" style="width:50%; text-align:center;">
    <tr>
        <td>Application Form</td>
        <td>Privacy Policy</td>
        <td>Up Sell</td>
        <td>eSign</td>
        <td>Thank you</td>
    </tr>
    </table>
    <table id="History" border="1" cellspacing="0" style="width:50%; text-align:center;">
    <tr>
        <td>Test Number</td>
        <td>Loan Number</td>
        <td>Start</td>
        <td>Finish</td>
        <td>Application Form</td>
        <td>Privacy Policy</td>
        <td>Up Sell</td>
        <td>eSign</td>
        <td>Thank you</td>
    </tr>
    </table>
    <canvas id="tutorial" width="150" height="150">    
    </canvas>    <label id="lbltxt" runat="server" ></label>
    </form>
</body>
</html>
