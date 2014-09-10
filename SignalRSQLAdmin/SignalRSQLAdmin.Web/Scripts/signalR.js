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
    mainHub.client.displaySelectedTable = function (result) {
        console.log(result);
        console.log("plop");
    }

    // Start the connection.
    $.connection.hub.start().done(function () {
        $('#discussion').click(function () {
            // Call the Send method on the hub.
            var fakejson = {
                Name: "lol",
                Fields: [
                    { Name: "test", IsPrimaryKey: true, IsNullable: false, Type: "int", MaxLength: 200 }
                ]
            };
            mainHub.server.createTable(fakejson);
        });

        // Load a Table
        $('.table-selector').click(function () {
           var tableName = $(this).find('.table-name').attr("data-name");
           mainHub.server.displayTableResult(tableName);
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