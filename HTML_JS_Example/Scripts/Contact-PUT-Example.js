var apiUrl = "https://test-api.i-360.com/1.1/Contacts";
var userId = '00000000-0000-0000-0000-000000000000'; // don't forget to replace with valid user id
var signalrUrl = 'https://test-api.i-360.com/1.1/signalr';
var MyBase64EncodedAuthString = btoa(userId + ':');

$(function () {
    $("#Import").click(function () {
        var state = $("#stateInput").val();
        var firstname = $("#fristNameInput").val();
        var lastname = $("#lastNameInput").val();
        var gender = $("#genderInput").val();
        var phone = $("#phoneInput").val();
        var email = $("#emailInput").val();
        var address1 = $("#address1Input").val();
        var address2 = $("#address2Input").val();
        var city = $("#cityInput").val();
        var zip = $("#zipInput").val();

        if (state == null || state == "") {
            alert("State must be filled out");
            return false;
        }

        if (firstname == null || firstname == "") {
            alert("First name must be filled out");
            return false;
        }

        if (lastname == null || lastname == "") {
            alert("Last name must be filled out");
            return false;
        }

        if (email == null || email == "") {
            alert("Email must be filled out");
            return false;
        }

        var contact = new Object();
        contact.RegisteredState = state;
        contact.firstname = firstname;
        contact.lastname = lastname;
        contact.gender = gender;
        contact.Phone = phone;
        contact.Email = email;
        contact.Address1 = address1;
        contact.Address2 = address2;
        contact.City = city;
        contact.State = state;
        contact.Zip = zip;

        var contactArray = new Array();
        contactArray.push(contact);
        var importJob = new Object();
        importJob.Contacts = contactArray;
        importJob.ApiNotification = true;
        var json = JSON.stringify(importJob);

        $.ajax({
            url: apiUrl,
            type: 'PUT',
            contentType: "application/json",
            dataType: 'json',
            data: json,
            crossDomain: false,
            success: function (data) {
                $("#responseText").html("Contact Import Id: " + data.Id + " Status: " + data.Status);
            },
            error: function (xhr) {
                $("#responseText").html("Error in Operation: " + xhr.status);
            }
        });
    });

    $.ajaxSetup({
        beforeSend: function (jqXhr, settings) {
            // SignalR handles authorization through its own pipeline
            if (settings.url.indexOf("/signalr") == -1)
                jqXhr.setRequestHeader('Authorization', 'Basic ' + MyBase64EncodedAuthString);
            return true;
        }
    });

    var connection = $.hubConnection();
    connection.logging = true;
    connection.url = signalrUrl;
    // SignalR authorization with the i360 Web Api is handled through the qs property
    connection.qs = "auth=" + MyBase64EncodedAuthString;
    var contactsHubProxy = connection.createHubProxy('contacts');
    contactsHubProxy.on('NotifyImportStatusChanged', function (importJob) {
        alert("Import " + importJob.Id + " status changed: " + importJob.Status);
    });
    connection.start()
        .done(function () { console.log('SignalR connection started'); })
        .fail(function(err) {
            // Is your client connected to this site with SSL?
            console.log('SignalR connection failed - ' + err);
    });

    // Display errors to console
    connection.error(function (err) {
        console.log('SignalR error - ' + err);
    });
});