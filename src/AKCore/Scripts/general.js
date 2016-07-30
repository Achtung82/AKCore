﻿$(".account")
    .on("click",
        ".login",
        function(e) {
            e.preventDefault();
            $("#loginModal").modal("show");
        });

$("#loginForm")
    .on("submit",
        function(e) {
            e.preventDefault();
            var form = $(this);
            var success = form.find(".alert-success");
            var error = form.find(".alert-danger");
            $.ajax({
                url: form.attr("action"),
                type: "POST",
                data: form.serialize(),
                success: function(res) {
                    if (res.success) {
                        window.location.reload();
                    } else {
                        error.text(res.message);
                        error.slideDown().delay(4000).slideUp();
                    }
                },
                error: function(err) {
                    error.text("Misslyckades att logga in");
                    error.slideDown().delay(4000).slideUp();
                }
            });
        });

$("#fb-login")
    .on("click",
        function(e) {
            e.preventDefault();
            fbGetLoginId(function(fbId) {
                $.ajax({
                    url: "/Account/FbLogin?fbId=" + fbId,
                    type: "POST",
                    success: function(res) {
                        if (res.success) {
                            window.location.reload();
                        } else {
                            var error = $("#loginForm").find(".alert-danger");
                            error.text("Kontot är ej kopplat med facebook");
                            error.slideDown().delay(4000).slideUp();
                        }
                    },
                    error: function(err) {
                        console.log(err);
                    }
                });
            });

        });


$(document)
    .ready(function() {
        $.ajaxSetup({ cache: true });
        $.getScript("//connect.facebook.net/en_US/sdk.js",
            function() {
                FB.init({
                    appId: "283356482023145",
                    version: "v2.7"
                });
            });
    });


function fbGetLoginId(callback) {
    FB.getLoginStatus(function(response) {
        if (response.status === "connected") {
            callback(response.authResponse.userID);
        } else if (response.status === "not_authorized") {
            FB.login(function(lresp) {
                callback(lresp.authResponse.userID);
            });
        } else {
            FB.login(function(lresp) {
                callback(lresp.authResponse.userID);
            });
        }
    });
}