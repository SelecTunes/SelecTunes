#pragma checksum "/Users/njtucker/Dev/Classes/JR_2/Experiments/Nate/websocketsinitial/websocketsinitial/Views/Home/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "13c66619df4d95983b03a6dca2166112a33b61cd"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/njtucker/Dev/Classes/JR_2/Experiments/Nate/websocketsinitial/websocketsinitial/Views/_ViewImports.cshtml"
using websocketsinitial;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/njtucker/Dev/Classes/JR_2/Experiments/Nate/websocketsinitial/websocketsinitial/Views/_ViewImports.cshtml"
using websocketsinitial.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"13c66619df4d95983b03a6dca2166112a33b61cd", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c8932687ba1b8f929ce018cf5fa2ebd77d1bcb25", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "13c66619df4d95983b03a6dca2166112a33b61cd3286", async() => {
                WriteLiteral(@"
    <meta charset=""utf-8"" />
    <title></title>
    <style>
        table {
            border: 0
        }

        .commslog-data {
            font-family: Consolas, Courier New, Courier, monospace;
        }

        .commslog-server {
            background-color: red;
            color: white
        }

        .commslog-client {
            background-color: green;
            color: white
        }
    </style>
");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "13c66619df4d95983b03a6dca2166112a33b61cd4684", async() => {
                WriteLiteral(@"
    <h1>WebSocket Sample Application</h1>
    <p id=""stateLabel"">Ready to connect...</p>
    <div>
        <label for=""connectionUrl"">WebSocket Server URL:</label>
        <input id=""connectionUrl"" />
        <button id=""connectButton"" type=""submit"">Connect</button>
    </div>
    <p></p>
    <div>
        <label for=""sendMessage"">Message to send:</label>
        <input id=""sendMessage"" disabled />
        <button id=""sendButton"" type=""submit"" disabled>Send</button>
        <button id=""closeButton"" disabled>Close Socket</button>
    </div>

    <h2>Communication Log</h2>
    <table style=""width: 800px"">
        <thead>
            <tr>
                <td style=""width: 100px"">From</td>
                <td style=""width: 100px"">To</td>
                <td>Data</td>
            </tr>
        </thead>
        <tbody id=""commsLog"">
        </tbody>
    </table>

    <script>var connectionUrl = document.getElementById(""connectionUrl"");
        var connectButton = document.getElementBy");
                WriteLiteral(@"Id(""connectButton"");
        var stateLabel = document.getElementById(""stateLabel"");
        var sendMessage = document.getElementById(""sendMessage"");
        var sendButton = document.getElementById(""sendButton"");
        var commsLog = document.getElementById(""commsLog"");
        var closeButton = document.getElementById(""closeButton"");
        var socket;

        var scheme = document.location.protocol === ""https:"" ? ""wss"" : ""ws"";
        var port = document.location.port ? ("":"" + document.location.port) : """";

        connectionUrl.value = scheme + ""://"" + document.location.hostname + port + ""/ws"" ;

        function updateState() {
            function disable() {
                sendMessage.disabled = true;
                sendButton.disabled = true;
                closeButton.disabled = true;
            }
            function enable() {
                sendMessage.disabled = false;
                sendButton.disabled = false;
                closeButton.disabled = false;
     ");
                WriteLiteral(@"       }

            connectionUrl.disabled = true;
            connectButton.disabled = true;

            if (!socket) {
                disable();
            } else {
                switch (socket.readyState) {
                    case WebSocket.CLOSED:
                        stateLabel.innerHTML = ""Closed"";
                        disable();
                        connectionUrl.disabled = false;
                        connectButton.disabled = false;
                        break;
                    case WebSocket.CLOSING:
                        stateLabel.innerHTML = ""Closing..."";
                        disable();
                        break;
                    case WebSocket.CONNECTING:
                        stateLabel.innerHTML = ""Connecting..."";
                        disable();
                        break;
                    case WebSocket.OPEN:
                        stateLabel.innerHTML = ""Open"";
                        enable();
                        b");
                WriteLiteral(@"reak;
                    default:
                        stateLabel.innerHTML = ""Unknown WebSocket State: "" + htmlEscape(socket.readyState);
                        disable();
                        break;
                }
            }
        }

        closeButton.onclick = function () {
            if (!socket || socket.readyState !== WebSocket.OPEN) {
                alert(""socket not connected"");
            }
            socket.close(1000, ""Closing from client"");
        };

        sendButton.onclick = function () {
            if (!socket || socket.readyState !== WebSocket.OPEN) {
                alert(""socket not connected"");
            }
            var data = sendMessage.value;
            socket.send(data);
            commsLog.innerHTML += '<tr>' +
                '<td class=""commslog-client"">Client</td>' +
                '<td class=""commslog-server"">Server</td>' +
                '<td class=""commslog-data"">' + htmlEscape(data) + '</td></tr>';
        };

      ");
                WriteLiteral(@"  connectButton.onclick = function() {
            stateLabel.innerHTML = ""Connecting"";
            socket = new WebSocket(connectionUrl.value);
            socket.onopen = function (event) {
                updateState();
                commsLog.innerHTML += '<tr>' +
                    '<td colspan=""3"" class=""commslog-data"">Connection opened</td>' +
                '</tr>';
            };
            socket.onclose = function (event) {
                updateState();
                commsLog.innerHTML += '<tr>' +
                    '<td colspan=""3"" class=""commslog-data"">Connection closed. Code: ' + htmlEscape(event.code) + '. Reason: ' + htmlEscape(event.reason) + '</td>' +
                '</tr>';
            };
            socket.onerror = updateState;
            socket.onmessage = function (event) {
                commsLog.innerHTML += '<tr>' +
                    '<td class=""commslog-server"">Server</td>' +
                    '<td class=""commslog-client"">Client</td>' +
           ");
                WriteLiteral(@"         '<td class=""commslog-data"">' + htmlEscape(event.data) + '</td></tr>';
            };
        };

        function htmlEscape(str) {
            return str.toString()
                .replace(/&/g, '&amp;')
                .replace(/""/g, '&quot;')
                .replace(/'/g, '&#39;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;');
        }</script>
");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</html>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
