$(function () {
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

    // Start the connection.
    $.connection.hub.start().done(function () {
        $('#discussion').click(function () {
            // Call the Send method on the hub.
            var fakejson = {
                Name: "lol",
                Fields: [
                    { Name: "Test", IsPrimaryKey: true, IsNullable: false, Type: "int", MaxLength: 2000 }
                ]
            };
            mainHub.server.createTable(fakejson);
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