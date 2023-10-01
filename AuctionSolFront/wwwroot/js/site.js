$(document).ready(function () {
    //-------------------------------------- SignalR Section -------------------------------------------
    SignalRConnection();

    async function SignalRConnection() {
        try {
            // Initialize the SignalR client
            var connection = new signalR.HubConnectionBuilder()
                .withUrl("/chatHub?userName=" + encodeURIComponent("MyUser")) // Update the URL to match your hub route
                .configureLogging(signalR.LogLevel.Information)
                .build();

            await connection.start();

            // Handle the click event of the Send button
            $("button.btn-primary").click(function () {
                // Get the user ID from the displayed value
                const userId = $("#userId").val();


                // Get the message from the input field
                const message = $("#message").val();

                // Append the message to the "adminMessage" div
                const adminMessageDiv = $("#adminMessage");
                const messageElement = document.createElement("p");
                messageElement.textContent = message;
                adminMessageDiv.append(messageElement);

                // Clear the text box
                $("#message").val("");

                // Send the message to the server
                connection.invoke("SendMessage", userId, message)
                    .catch(err => console.error(err));
            });


            connection.on("ReceiveMessage", RenderMessage);

            connection.on("UserStatusChanged", UpdateUserStatus);

            console.log("SignalR connection started.");
        } catch (error) {
            console.error("Error starting SignalR connection:", error);
        }
    }

    function RenderMessage(message) {
        // Create a new message element (e.g., a <p> tag)
        const messageElement = document.createElement("p");
        messageElement.textContent = message; // Set the message text

        // Get the div where you want to append the message
        const bidMessageDiv = document.getElementById("bidMessage");

        // Append the message element to the div
        bidMessageDiv.appendChild(messageElement);

        // Scroll to the bottom of the div to show the latest message
        bidMessageDiv.scrollTop = bidMessageDiv.scrollHeight;

        console.log("Received message:", message);
    }

    function UpdateUserStatus(userId, status) {
        // Get the div element by its id
        const statusLiveDiv = document.getElementById("StatusLive");

        // Check if the div element exists
        if (statusLiveDiv) {
            // Update the content of the div with the new status
            statusLiveDiv.textContent = userId + " is " + status;
        } else {
            console.error("Element with id 'StatusLive' not found.");
        }

        console.log("User status changed:", status);
    }

});
