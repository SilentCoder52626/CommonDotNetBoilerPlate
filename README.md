##Common Boiler Plate

## Email Setup

Currently, Setup as per 'Ethernal Mail server' is done.
User Name : lera.waters@ethereal.email
password : 4xG54TAu4d4J6vUzzS

For using Google Account as a sender.

• Navigate to our Google Account - the account you will send the emails from (https://myaccount.google.com/)
• In the menu on the left, we should select Security
• Then under the "Signing in to Google" section, we can see that 2-Step Verification is off - so we have to click on it
• Click Get Started, provide your password, and confirm the code by providing a mobile number
• If everything goes well, you should see the Turn On option, so just click on it

At this point, we have enabled our 2-Step verification and we can return to the Security page. There, under the same "Signing in to Google" section, we can find the App passwords option set to None.
So, we have to:
• Click on it
• Provide a password
• Click the Select app menu and choose the Other (Custom Name) option
• Now, all we have to do is to provide any name we like for our app and click the Generate button
This will generate a new password for us, which we should use in our appsettings.json file instead of our personal password.