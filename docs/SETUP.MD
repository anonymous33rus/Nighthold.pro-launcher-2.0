## Table of Contents

- [Requirements](#Requirements) 
- [Setup steps before compiling the launcher](#Setup-steps-before-compiling-the-launcher) 
- [Compiling the launcher](#Compiling-the-launcher) 
- [Distributing the launcher](#Distributing-the-launcher) 
- [Uploading full client, minimal client or only custom patches](#uploading-full-client-minimal-client-or-only-custom-patches) 
- [Pushing a new client update](#Pushing-a-new-client-update)  
- [Giving access to admin panel](#Giving-access-to-admin-panel)  

## Make sure that you meet the following requirements
- VPS or Dedicated host
- PHP Webserver >= 7.x
- PHP Extensions (mysqli, Soap, cUrl, JSON, gmp)
- Soap Opened Outgoing Ports
- API and Client Files must be stored on the Webserver

## Pulling the source
Use the following git command to pull the source:
```git
git clone --recurse-submodules https://github.com/Nighthold-Launcher/Nighthold-Launcher.git Nighthold-Launcher
```

Use the following git command to update the submodule:
```git
git submodule update --progress --init --recursive --force --remote -- "Nighthold-API"
```

## Setup steps before compiling the launcher
 1. Create a folder for your launcher on your webserver named **"launcher"** or anything you prefer.
 2. Upload the files from **Nighthold-API** folder regarding your Emulator and CMS to your webserver in the launcher folder.
 3. Go to your webserver **launcher** folder and edit **Config.xml** to your preferences.
 4. Create a database named **launcher** or anything you prefer.
 5. Run the db installer from **launcher/application/sql/Launcher_db.sql** to your launcher database.
 6. Run **launcher/application/sql/Auth.account.sql** to your auth database.
 7. Go to your webserver and edit **launcher/application/config.php** accordingly.

## Compiling the launcher
 1. Run **Nighthold-Launcher/Nighthold/Nighthold.sln** with visual studio 2019.

 3. Edit **Nighthold Launcher->Properties->Settings**
    - Change **XMLDocumentUrl** Value to **"http://yourdomain.com/launcher/Config.xml"**.
    - Change **BuyDPUrl** Value to the purchase donation points page url.
    - Change **ServerName** Value to your server name.

 4. Edit **Nighthold Login->Properties->Settings**
    - Change **ServerName** Value to your server name.
    - Change **RegisterAccountUrl** Value to the registration page url.
    - Change **ResetPasswordUrl** Value to the password reset page url.

 5. Edit **Nighthold Updater->Config.cs**
    - Change **LauncherAPIUrl** with the example "http://yourdomain.com/launcher"

 6. Edit **WebHandler->Config.cs**
    - Change **LauncherAPIUrl** with the example "http://yourdomain.com/launcher"

 7. You can now compile the launcher by pressing **CTRL+SHIFT+B** or right-click on **solution->Build Solution**

## Distributing the launcher
 - The compiled launcher files can be found in source/**Nighthold/bin** folder
 - To start the launcher open **Nighthold Login.exe** so this is your main executable.

**You can Distribute the launcher as:**
   - ZIP archive
   - SFX Archive with desktop shortcut [video tutorial here](https://www.youtube.com/watch?v=koFeRlOuZgw)
   - Or 3rd party installer, for example [Advanced Installer](https://www.advancedinstaller.com/download.html) or [Actual Installer](https://www.actualinstaller.com/)

## Uploading full client, minimal client or only custom patches
 1. First of all, get used to the following expansion ids:
    - 1 = Classic
    - 2 = Burning Crusade
    - 3 = Wrath of the Lich King
    - 4 = Cataclysm
    - 5 = Mists of Pandaria
    - 6 = Warlords of Draenor
    - 7 = Legion
    - 8 = Battle for Azeroth
    - 9 = Shadowlands
 
 2. Upload your custom patches or full client to your **webserver/launcher/game/X** where **"X"** is your expansion ID. 
 You will notice there are a few dummy files in there. You can remove and replace them safely with yours.

## Pushing a new client update
 1. Upload or replace your client files on your **webserver/launcher/game/X** where **X** is the expansion id.
 2. Open and edit your **webserver/launcher/Config.xml** for example
    - Find node **"Expansion id="5" update_version="1""** and increase **update_version** to any number.
    - Done, now when your players will start the launcher will be forced to update their client.

## Giving access to admin panel
We are using a MD5 password in database for security reasons.
 1. Go to your **auth** database -> **account** table and search for your account.
 2. Generate a **MD5 Hash password**, for example on this website [MD5 Hash Generator](https://www.md5hashgenerator.com/)
 3. Store your **non** MD5 password somewhere safe.
 4. After you found your admin account go to **sec_pa** column and insert the **MD5 Hash password**.
 5. The required password to login the admin panel is the **non** MD5 password.
