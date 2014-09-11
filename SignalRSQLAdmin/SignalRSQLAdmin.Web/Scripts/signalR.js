
$(function () {
    console.log("init");
    // Reference the auto-generated proxy for the hub.
    var mainHub = $.connection.mainHub;

    // display notification on table creation
    mainHub.client.notifyCreateTableResult = function (result) {
        var notificationText;
        if (result.ErrorMessage != null) {
            notificationText = generateNotificationText("error", result.ErrorMessage);
        }
        else {
            notificationText = generateNotificationText("success", "La table " + result.TableModel.Name + " a bien été créée.");
        }
        
        addNotification(notificationText);
    };

    // display notification on table deletion
    mainHub.client.notifyDeleteTableResult = function (result) {
        // Add the message to the page.
        var notificationText;
        if (result.ErrorMessage != null) {
            notificationText = generateNotificationText("error", result.ErrorMessage);
        }
        else {
            notificationText = generateNotificationText("success", "La table " + result.Name + " a bien été supprimée.");
        }
        addNotification(notificationText);
    };

    // Display a Table on the Dashboard
    function displaySelectedTable(tableName) {
        $.get('/SignalRSQLAdmin/Main/DisplaySelectedTable/'+tableName,  function(result){
            $('#bodyResult').html(result);
        });
    }

    // Start the connection.
    $.connection.hub.start().done(function () {
        $("#buttonSubmitForm").click(function () {
            // Verifs a faire!
            var tableName = $('#formTableName').val();
            var valuesForm = $('.valueForm');
            var jsonTable = {
                Name: tableName,
                Fields: []
            };
            var i = 0;
            valuesForm.each(function () {
                var vName = $(this).find('#formValueName').val();
                var vIsNull = $(this).find('#formIsNull').val();
                var vIsPrimary = $(this).find('#formIsPrimary').val();
                var vType = $('#formValueType option:selected').text()

                jsonTable['Fields'][i] = { Name: vName, Type: vType, MaxLength: 55 };
                i++;
            });
            var p = mainHub.server.createTable(jsonTable);
            console.log('jsuisla');
            p.done(function (result) {
                if (result.ErrorMessage) {
                    console.log(result.ErrorMessage);
                }
                else {
                    $('#formTable').modal('toggle');
                    var newTableName = result.TableModel.Name;
                    var newLi = '<li class="table-selector"><a href="#" style="margin-left: 10px;"><i class="fa fa-angle-double-right"></i> <span class="table-name" data-name="' + newTableName + '">' + newTableName + '</span></a></li>';
                    $('#menuTableList').append(newLi);
                    displaySelectedTable(tableName);
                }
            });
        });

        //delete a table
        $('#deleteTableTest').click(function () {
            // Call the Send method on the hub.
            var fakejson = {
                Database: "TestSignalR",
                Name: "lol"
            };
            mainHub.server.deleteTable(fakejson);
        });

        // Load a Table
        $('.table-selector').click(function () {
           var tableName = $(this).find('.table-name').attr("data-name");
           displaySelectedTable(tableName);
        });
    });
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

// adds a notification to notification list
function addNotification(text) {
    $('#notificationsList').append($(text).hide().fadeIn(2000));
    updateNotificationsCount();
}

// Update number of notifications in menu bar.
function updateNotificationsCount() {
    var numberOfNotifs = $("#notificationsList li").length;
    $("#numberOfNotifications").text(numberOfNotifs);
}

// generate a notification's text of a certain type
// type can be success or error, nothing else !
function generateNotificationText(type, message) {
    if (type == "success") {
        return "<li><a href='#'><i class='fa fa-check-square-o success'></i>"
                + message
                + "</a></li>";
    }
    else if (type == "error") {
        return "<li><a href='#'><i class='fa fa-times danger'></i>"  + message + "</a></li>";
    }
    throw "Invalid message type";
}