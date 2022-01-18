// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function connectToSignalR() {
    console.log("Connecting to SignalR...");
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("DoSomethingReallyCool", displayNotification);
    conn.start().then(function () {
        console.log("SignalR started!");
    }).catch(function (err) {
        console.log(err);
    });
}

function displayNotification(user, json) {
    console.log('Displaying notification');
    let $div = $("#signalr-notifications");
    let data = JSON.parse(json);
    var $alert = $(`<div>New car alert!
        ${data.manufacturerName} ${data.modelName} (${data.color}, ${data.year}). 
        Price ${data.price} ${data.currencyCode}. 
        <a href="/vehicles/details/${data.registration}">Click here for more...</a></div>`);
    $div.prepend($alert);
    window.setTimeout(function () {
        $alert.fadeOut(2000,
            function () { $alert.remove(); })
    }, 5000);
}
$(document).ready(connectToSignalR);