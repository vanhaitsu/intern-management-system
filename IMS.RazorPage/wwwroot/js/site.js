"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalRServer")
    .build();

connection.on("LoadAccount", function () {
    if (window.location.pathname === '/Admin/Account') {
        setTimeout(function () {
            location.reload();
        }, 2000); 
    }
});

connection.on("LoadIntern", function () {
    if (window.location.pathname === '/Admin/Intern') {
        setTimeout(function () {
            location.reload();
        }, 2000);
    }
});

connection.on("LoadCampaign", function () {
    if (window.location.pathname === '/Admin/Intern') {
        setTimeout(function () {
            location.reload();
        }, 2000);
    }
});
connection.on("LoadApplication", function () {
    var currentPath = window.location.pathname;
    //console.log("Current path:", currentPath);

    if (currentPath.startsWith('/HR/Application/')) {
       // console.log("Reloading page in 2 seconds...");
        setTimeout(function () {
            location.reload();
        }, 2000);
    }
});


connection.start().catch(function (err) {
    return console.error(err.toString());
});
