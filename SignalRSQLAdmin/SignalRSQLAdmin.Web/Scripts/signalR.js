
$(function () {
    console.log("init");
    setListeners();
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
                    $('#errorModalMessage').html(result.ErrorMessage);
                }
                else {
                    $('#formTable').modal('toggle');
                    $('#errorModalMessage').html('');
                    var newTableName = result.TableModel.Name;
                    var newLi = '<li class="table-selector"><a href="#" style="margin-left: 10px;"><i class="fa fa-angle-double-right"></i> <span class="table-name" data-name="' + newTableName + '">' + newTableName + '</span></a></li>';
                    $('#menuTableList').append(newLi);
                    displaySelectedTable(tableName);
                }
            });
        });
        
    });



    function deleteTable(TableName) {

        var fakejson = {
            Database: "TestSignalR",
            Name: TableName
        };


        console.log(fakejson);
        var p = mainHub.server.deleteTable(fakejson);
        p.done(function () {
            displaySelectedTable(null);
            refreshLeftBar();
            setListeners();
        });

    };

    // This optional function html-encodes messages for display in the page.
    function htmlEncode(value) {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;
    }

    // adds a notification to notification list
    function addNotification(text) {
        $('#notificationsList').prepend($(text).hide().fadeIn(2000));
        updateNotificationsCount();
        setListeners();
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
            return "<li class='successMessage'><a href='#'><i class='fa fa-check-square-o success'></i>"
                    + message
                    + "</a></li>";
        }
        else if (type == "error") {
            return "<li><a href='#' class='errorMessage'><i class='fa fa-times danger'></i>" + message + "</a></li>";
        }
        throw "Invalid message type";
    }

    // Display a Table on the Dashboard
    function displaySelectedTable(tableName) {
        var queryUrl = "/SignalRSQLAdmin/Main/DisplaySelectedTable";
        if (tableName != null)
            queryUrl = '/SignalRSQLAdmin/Main/DisplaySelectedTable/' + tableName;
        $.get(queryUrl, function (result) {
            $('#bodyResult').html(result);
            setListeners();
        });
    }


    function refreshLeftBar() {
        $.get('/SignalRSQLAdmin/Main/DisplayLeftSideBar', function (result) {
            $('#left-side-bar').html(result);
            $('#menuTableList').fadeIn(500);
            setListeners();
        });
    }

    function setListeners() {
        

        $('#deleteTableTest').click(function () {
            console.log("plop");
            var TableName = $(this).attr("data-name");
            deleteTable(TableName);
        });

        $('#refresh-button').click(function () {
            refreshLeftBar();
        });

        $('.successMessage').click(function () {
            refreshLeftBar();
        });

        $('.table-selector').click(function () {
            var tableName = $(this).find('.table-name').attr("data-name");
            displaySelectedTable(tableName);
        });
    };
});




