// no_verification.js
document.addEventListener("DOMContentLoaded", function () {
    const messaging = firebase.messaging();

    messaging.requestPermission().then(() => {
        console.log('Notification permission granted.');

        return messaging.getToken();
    }).then((token) => {
        if (token) {
            console.log('FCM Token:', token);
            document.getElementById('fcmToken').value = token;
        } else {
            console.log('No registration token available. Request permission to generate one.');
        }
    }).catch((err) => {
        console.log('An error occurred while retrieving token. ', err);
    });

    messaging.onMessage((payload) => {
        console.log('Message received. ', payload);
        // Customize notification here
    });
});
