$(function () {
    // Reference the auto-generated proxy for the hub.
    var mainHub = $.connection.mainHub;

    // Create a function that the hub can call back to display messages.
    mainHub.client.notifyCreateTableResult = function (result) {
        // Add the message to the page.
        if (result.ErrorMessage != null) {
            $('#discussion').append("<div>" + result.ErrorMessage + "</div>");
        }
        else {
            $('#discussion').append("<div> Action OK </div>");
        }

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