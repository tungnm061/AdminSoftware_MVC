function ChangePassword() {
    InitWindowModal('/Base/ChangePassword', false, 550, 220, "Đổi mật khẩu", false);
}
function ChangeInfomation() {
    InitWindowModal('/Base/ChangeInfomation', false, 500, 250, "Cập nhật thông tin", false);
}
/*Kendo function*/
function ClearSessionEventClose() {
    var url = document.location.pathname.replace('#', '');
    if (url.split('/').length > 3) {
        url = url.split('/')[0] + "/" + url.split('/')[1] + "/" + url.split('/')[2];
    }
    $.post(url + "/ClearSession");
}
function GridCallback(element)
{
    $(element).data('kendoGrid').dataSource.read();
}
function TreeListCallback(element) {
    $(element).data('kendoTreeList').dataSource.read();
}
function GetGridRowSelectedKeyValue(element) {
    var grid = $(element).data("kendoGrid");
    if (grid != typeof undefined && grid != null) {
        var selectedItem = grid.dataItem(grid.select());
        if (selectedItem != typeof undefined && selectedItem != null) {
            return selectedItem.id;
        }
        return null;
    }
    return null;
}
function GetGridRowSelected(element) {
    var grid = $(element).data("kendoGrid");
    if (grid != typeof undefined && grid != null) {
        var selectedItem = grid.dataItem(grid.select());
        return selectedItem;
    }
    return null;
}
function GetTreeListSelectedKeyValue(element) {
    var treeList = $(element).data("kendoTreeList");
    if (treeList != typeof undefined && treeList != null) {
        var selectedItem = treeList.dataItem(treeList.select());
        if (selectedItem != typeof undefined && selectedItem != null) {
            return selectedItem.id;
        }
        return null;
    }
    return null;
}
function GetTreeListRowSelected(element) {
    var treeList = $(element).data("kendoTreeList");
    if (treeList != typeof undefined && treeList != null) {
        var selectedItem = treeList.dataItem(treeList.select());
        if (selectedItem != typeof undefined && selectedItem != null) {
            return selectedItem;
        }
        return null;
    }
    return null;
}
function GetGridMultiRowSelectedKeyValue(element) {
    return $(element).data("kendoGrid").selectedKeyNames();
}
function SetGridRowSelected(array,element) {
    var items = $(element).data("kendoGrid").dataSource.data();
    if (array.length > 0) {
        for (var i = 0; i < array.length; i++) {
            var item = items.find(x=>x.id == array[i]);
            if (item != null && item != typeof undefined) {
                var row = $(element).data("kendoGrid").table.find("[data-uid=" + item.uid + "]");
                $(element).data("kendoGrid").select(row);
            }
        }
    }
}
function InitWindowModal(content, isMaximize, width, height, title, iframe) {
    $('#processing').show();
    var element = $('#windowBase');
    element.kendoWindow({
        width: width,
        height: height,
        title: title,
        content: content,
        modal: true,
        actions: ["Close"],
        resizable: false,
        iframe:iframe,
        scrollable: true,
        close: function (e) {
            CloseWindowModal();
        },
        refresh: function () {
            $('#processing').hide();
        }
    });
    if (isMaximize) {
        element.data("kendoWindow").open().maximize();
    } else {
        element.data("kendoWindow").center().open();
    }
    element.unbind('dblclick');
    element.data("kendoWindow").wrapper.find(".k-i-close").click(function (e) {
        CloseWindowModal();
    });
}
function InitChildWindowModal(content, width, height, title) {
    $('#processing').show();
    var element = $('#childWindoModal');
    element.kendoWindow({
        width: width,
        height: height,
        title: title,
        content: content,
        modal: true,
        actions: ["Close"],
        resizable: false,
        scrollable: true,
        close: function (e) {
            CloseChildWindowModal();
        },
        refresh: function () {
            $('#processing').hide();
        }
    });
    element.data("kendoWindow").center().open();
    element.unbind('dblclick');
    element.unbind('dblclick');
    element.data("kendoWindow").wrapper.find(".k-i-close").click(function (e) {
        CloseChildWindowModal();
    });
}
function InitChildOfChildWindowModal(content, width, height, title) {
    $('#processing').show();
    var element = $('#childOfChildWindoModal');
    element.kendoWindow({
        width: width,
        height: height,
        title: title,
        content: content,
        modal: true,
        actions: ["Close"],
        resizable: false,
        scrollable: true,
        close: function (e) {
            CloseChildOfChildWindowModal();
        },
        refresh: function () {
            $('#processing').hide();
        }
    });
    element.data("kendoWindow").center().open();
    element.unbind('dblclick');
    element.unbind('dblclick');
    element.data("kendoWindow").wrapper.find(".k-i-close").click(function (e) {
        CloseChildOfChildWindowModal();
    });
}
function CloseWindowModal() {
    var element = $('#windowBase');
    element.data("kendoWindow").destroy();
    ClearSessionEventClose();
    $('body').append('<div id="windowBase"></div>');
}
function CloseChildWindowModal() {
    var element = $('#childWindoModal');
    element.data("kendoWindow").destroy();
    $('body').append('<div id="childWindoModal"></div>');
}
function CloseChildOfChildWindowModal() {
    var element = $('#childOfChildWindoModal');
    element.data("kendoWindow").destroy();
    $('body').append('<div id="childOfChildWindoModal"></div>');
}
$(function () {
    $("#grdMain[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        $('#btnEdit').click();
    });
    //LoadNotifications();
});
/*signalr hub connection*/
//$(function () {
//    var notification = $.connection.notificationHub;
//    notification.client.broadcastMessage = function (message) {
//        LoadNotifications();
//        $.msgBox({
//            title: "Thông báo",
//            type: "info",
//            content: response.Message,
//            buttons: [{ value: "Đồng ý" }],
//            success: function () {
//            }
//        });
//    }
//    $.connection.hub.start().done(function () {
//        notification.server.welcome().done(function (message) {
//        });
//    });
//    var chatHub = $.connection.chatHub;
//    chatHub.client.broadCastMessages = function (chats) {
//        var html = '';
//        if (chats.length > 0) {
//            for (var i = 0; i < chats.length; i++) {
//                html += '<div class="message"><div class="message-header"><span><strong>' + chats[i].UserName + '</strong></span><span>on ' + chats[i].TimeDisplay + '</span></div><div class="message-text">' + chats[i].Message + '</div></div>';
//            }
//        }
//        $('.chat-content[data-display="messages"]').html(html);
//        $('.chat-content[data-display="messages"]').scrollTop($('.chat-content[data-display="messages"]')[0].scrollHeight);
//    }
//    $.connection.hub.start().done(function () {
//        chatHub.server.broadCastMessages().done(function (chats) {
//            var html = '';
//            if (chats.length > 0) {
//                for (var i = 0; i < chats.length; i++) {
//                    html += '<div class="message"><div class="message-header"><span><strong>' + chats[i].UserName + '</strong></span><span>on ' + chats[i].TimeDisplay + '</span></div><div class="message-text">' + chats[i].Message + '</div></div>';
//                }
//            }
//            $('.chat-content[data-display="messages"]').html(html);
//            $('.chat-content[data-display="messages"]').scrollTop($('.chat-content[data-display="messages"]')[0].scrollHeight);
//        });
//    });
//    $("#chat-message").keypress(function (e) {
//        if (e.which === 13) {
//            var msg = $("#chat-message").val();
//            if (msg.length > 0) {
//                chatHub.server.sendMessageToAll(msg);
//                $("#chat-message").val('');
//            }
//        }
//    });
//    $('#chat-attach-image').change(function () {
//        var files = $("#chat-attach-image").get(0).files;
//        if (files.length > 0) {
//            var reader = new FileReader();
//            reader.onload = function () {
//                var base64Image = reader.result;
//                var message = '<img src="' + base64Image + '" />';
//                chatHub.server.sendMessageToAll(message);
//            };
//            reader.readAsDataURL(files[0]);
//        }
//    });
//});
//function LoadNotifications() {
//    $.ajax({
//        type: 'POST',
//        url: '/Dashboard/Notifications',
//        contentType: 'application/json;charset=utf-8',
//        success:function(response) {
//            if (response.length > 0) {
//                var html = '';
//                for (var i = 0; i < response.length; i++) {
//                    html += '<div class="row"><div class="col-md-12"><p><a href="javascript:void(0)" title="Tin nhắn được gửi vào lúc ' + response[i].DisplayDate + '"><strong><i class="fa fa-comment-o"></i> ' + response[i].FromUser + ': </strong></a>' + response[i].Message + '</p></div></div>';
//                }
//                $('#notification-message').html(html);
//            }
//        }
//    });
//}
//function resizeChatBox() {
//    var actionHeight = $('.chat-box .chat-action').height();
//    var sidebarHeight = $('.control-sidebar-tabs').height();
//    $('.chat-box .chat-content').attr('style', 'height:' + (window.innerHeight - actionHeight - sidebarHeight - 20) + 'px');
//}
//$(function() {
//    resizeChatBox();
//});
//window.onresize = function(event) {
//    resizeChatBox();
//};