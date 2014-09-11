
$(function () {
    console.log("init");
    // Reference the auto-generated proxy for the hub.
    var mainHub = $.connection.mainHub;

    // Create a function that the hub can call back to display messages.
    mainHub.client.notifyCreateTableResult = function (result) {
        // Add the message to the page.
        var notificationText;
        if (result.ErrorMessage != null) {
            notificationText = "<li><a href='#'><i class='fa fa-times danger'></i>" + result.ErrorMessage + "</a></li>";
        }
        else {
            notificationText = "<li><a href='#'><i class='fa fa-check-square-o success'></i> Action OK </a></li>";
        }
        $('#notificationsList').append($(notificationText).hide().fadeIn(2000));
        updateNotificationsCount();
    };

    // Display a Table on the Dashboard
    function displaySelectedTable(tableName) {
        $.get('/SignalRSQLAdmin/Main/DisplaySelectedTable/'+tableName,  function(result){
            $('#bodyResult').html(result);
        });
        console.log(result);
        console.log("plop");
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

// Update number of notifications in menu bar.
function updateNotificationsCount() {
    var numberOfNotifs = $("#notificationsList li").length;
    $("#numberOfNotifications").text(numberOfNotifs);
}