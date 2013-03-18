<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="MonitorWeb._Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/raphael/2.1.0/raphael-min.js"></script>
    <script type="text/javascript" src="http://cdn.oesmith.co.uk/morris-0.4.1.min.js"></script>
    <script src="Scripts/popbox.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            setInterval('getDate()', 1000);
            setInterval('getStatus()', 5000);
            setInterval('drawChart()', 5000);
            $('.popbox').popbox();            
    });
    function drawChart() {
        $('#chart').html('');
        var avg,last;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "Dashboard.aspx/GetStatus",
            data: "{}",
            dataType: "json",
            success: function(data) {

                if (data.d != null) {
                    avg = data.d.sort(function(a, b) { return a.Order > b.Order; });

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Default.aspx/GetTestFlow",
                        data: "{numberOfRows: 1}",
                        dataType: "json",
                        success: function(data) {
                        if ($(data.d).length > 0) {
                            var last = data.d[0];
                                new Morris.Line({
                                    element: 'chart',
                                    data: [
                                    { phase: 'Application Form', avg: avg[0].AvgTime, last: (last.ApplicationForm == -1) ? 0 : last.ApplicationForm },
                                    { phase: 'Privacy Policy', avg: avg[1].AvgTime, last: (last.PrivacyPolicy == -1) ? 0 : last.PrivacyPolicy },
                                    { phase: 'UpSell', avg: avg[2].AvgTime, last: (last.UpSell == -1) ? 0 : last.UpSell },
                                    { phase: 'eSign', avg: avg[3].AvgTime, last: (last.ESign == -1) ? 0 : last.ESign },
                                    { phase: 'ThankYou', avg: avg[4].AvgTime, last: (last.ThankYou == -1) ? 0 : last.ThankYou }
                                  ],
                                    xkey: 'phase',
                                    ykeys: ['avg', 'last'],
                                    labels: ['Average', 'Last'],
                                    parseTime: false
                                });

                            }
                        },
                        error: function(result) {
                            alert("Error");
                        }
                    });
                }
            },
            error: function(result) {
                alert("Error");
            }
        });
    }
        function getDate() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Dashboard.aspx/DisplayData",
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
                url: "Dashboard.aspx/GetStatus",
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
        function getTestFlow(rows) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Dashboard.aspx/GetTestFlow",
                data: "{numberOfRows: " + rows + "}",
                dataType: "json",
                success: function(data) {
                    var testL = data.d;
                    if ($(testL).length > 0) {
                        var testList = testL.sort(function(a, b) { return a.TestNumber < b.TestNumber; });
                        while ($("#history")[0].rows.length > 1) {
                            $("#history")[0].deleteRow(1);
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
                            $("#history").append(row);
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
            var bufferGreen = 2, bufferYellow = 5;
            if (avgTime - (pageTime - bufferGreen) >= 0)
                return "lightgreen";
            else if (avgTime - (pageTime - bufferYellow) >= 0)
                return "yellow";
            else
                return "red";
        }
    </script>
    <link rel="stylesheet" href="http://cdn.oesmith.co.uk/morris-0.4.1.min.css" />
    <link href="Css/popbox.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        table.grid
        {
            font-family: verdana,arial,sans-serif;
            font-size: 11px;
            color: #333333;
            border-width: 1px;
            border-color: #666666;
            border-collapse: collapse;
        }
        table.grid th
        {
            border-width: 1px;
            padding: 8px;
            border-style: solid;
            border-color: #666666;
            background-color: #dedede;
        }
        table.grid td
        {
            border-width: 1px;
            padding: 8px;
            border-style: solid;
            border-color: #666666;
            background-color: #ffffff;
        }
        #content
        {
            margin-left: -125px;
            position: fixed;
            left: 45%;
            width: 250px;
            height: 70px;
            visibility: visible;
            z-index:1;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <h1>
            Checkn'Go - Dashboard</h1>
        <h2>ILP - WI</h2>
        <div><label id="lbltxt" runat="server"></label></div>        
    </div>        
    <div class="popbox">
        <a class='open' href='#' onclick="getTestFlow(10);">Show History</a>
        <div class="collapse">
            <div class="box">
                <table id="history" class="grid" border="1" cellspacing="0" style="width: 50%; text-align: center;">
                    <tr>
                        <th>
                            Test Number
                        </th>
                        <th>
                            Loan Number
                        </th>
                        <th>
                            Start
                        </th>
                        <th>
                            Finish
                        </th>
                        <th>
                            Application Form
                        </th>
                        <th>
                            Privacy Policy
                        </th>
                        <th>
                            Up Sell
                        </th>
                        <th>
                            eSign
                        </th>
                        <th>
                            Thank you
                        </th>
                    </tr>
                </table>
                <a href="#" class="close">close</a>
            </div>
        </div>
    </div>
    <div id="content">
        <table id="Dashboard" class="grid" border="1" cellspacing="0" style="width: 50%;
            text-align: center;">
            <tr>
                <th>
                    Application Form
                </th>
                <th>
                    Privacy Policy
                </th>
                <th>
                    Up Sell
                </th>
                <th>
                    eSign
                </th>
                <th>
                    Thank you
                </th>
            </tr>
        </table>
        <div id="chart" style="height:250px;width:400px;position:relative;"/>    
    </div>
    </form>
</body>
</html>
