This is a minimal Android app (Kotlin) that sends broadcast intents to the MAUI app's CarTapReceiver.

Purpose
- Install this APK on your phone (separately from the MAUI app).
- Launch the app (or use the DHU) and tap a button to send a broadcast with action com.companyname.taptrack.CAR_TAP_ACTION.
- The MAUI app's CarTapReceiver (in Platforms/Android) will receive the broadcast and forward it to your server.

Build & install
- Open this folder in Android Studio and build the app, or use the Gradle wrapper:
  ./gradlew :app:installDebug

Notes
- This is a test harness, not a real Android for Cars app. It intentionally avoids the Car App Library to stay minimal and easy to run.
- The broadcast includes extras:
  - "label": the button label (string)
  - "timestamp": ISO-8601 string of the timestamp

Replace the package name or action if you changed the receiver configuration in the MAUI app.
