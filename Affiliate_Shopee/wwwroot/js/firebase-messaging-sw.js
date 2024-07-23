// Initialize Firebase
const firebaseConfig = {
    apiKey: "YOUR_API_KEY",
    authDomain: "YOUR_PROJECT_ID.firebaseapp.com",
    projectId: "YOUR_PROJECT_ID",
    storageBucket: "YOUR_PROJECT_ID.appspot.com",
    messagingSenderId: "YOUR_MESSAGING_SENDER_ID",
    appId: "YOUR_APP_ID",
    measurementId: "YOUR_MEASUREMENT_ID"
};

firebase.initializeApp(firebaseConfig);

const messaging = firebase.messaging();

// Request permission to receive notifications
messaging.requestPermission()
    .then(() => {
        console.log('Notification permission granted.');
        return messaging.getToken();
    })
    .then((token) => {
        console.log('FCM Registration Token:', token);
        // Send token to server or save it as needed
        sendTokenToServer(token);
    })
    .catch((err) => {
        console.log('Unable to get permission to notify.', err);
    });

function sendTokenToServer(token) {
    // Implement this function to send the token to your server
    fetch('/Account/SaveToken', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': '@Html.AntiForgeryToken()'
        },
        body: JSON.stringify({ token: token })
    });
}
