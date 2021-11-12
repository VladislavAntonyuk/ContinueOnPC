# Continue On PC

## Setup Firebase account
1. Create new project on https://console.firebase.google.com/
1. Enable authentication by Email/Password on https://console.firebase.google.com/u/0/project/YOUR_PROJECT_ID/authentication/providers
1. Create new user on https://console.firebase.google.com/u/0/project/YOUR_PROJECT_ID/authentication/users
1. Copy User UID.
1. Create realtime database on https://console.firebase.google.com/u/0/project/YOUR_PROJECT_ID/database
1. Choose start in LockedMode
1. Edit rules:
```json
{
  "rules": {
    ".read": "auth.uid === 'USER UID from step 4'",
    ".write": "auth.uid === 'USER UID from step 4'"
  }
}
```

## Setup application
1. Start iOS App.
1. Fill all fields.
1. Start desktop app.
1. Enter the same data as on step 2.
1. Click connect.
1. Switch back to the iOS app. Click Test button. If everything is correct, the link should appear in Firebase Readtime database.
1. Switch back to the desktop. The browser should open the new tab with the link from the phone.