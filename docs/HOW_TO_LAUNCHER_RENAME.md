# How to rename the launcher completely
______
## Renaming Nighthold Launcher project

1. From the Solution Explorer on the right side right click on "Nighthold Launcher" -> Rename.

2. Right click on the solution -> Clean Solution then -> Build solution.

3. Right click on the renamed **Project -> Properties -> Application** and change:
   - In the **Assembly name** field change to ``Server Name Launcher``
   - In the **Default namespace** field change to ``Server_Name_Launcher``
   - Click on the **Assembly Information..** button and update to your preferences
   - Hit ``CTRL+S`` to save.

4. In the project explorer find and open file **NightholdLauncher.xaml.cs**
   - Rename the namespace ``Nighthold_Launcher`` to ``Server_Name_Launcher`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**
   - Rename the partial class ``NightholdLauncher`` to ``ServerNameLauncher`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**

5. Right click on the solution -> Clean Solution then -> Build solution.

6. Right click on **NightholdLauncher.xaml** -> Rename to **ServerNameLauncher.xaml**

7. Right click on the solution -> Clean Solution then -> Build solution.

8. In the Project Explorer find and open file **App.xaml**
   - Rename ``StartupUri="NightholdLauncher.xaml"`` to ``"StartUpUri="ServerNameLauncher.xaml"``

9. Hit ``CTRL+SHIFT+F`` -> **Replace in files** Tab:
   - In the **Find** text box write: ``nightholdLauncher``
   - In the **Replace** text box write ``serverNameLauncher`
   - Then click **Replace All** button
   - After replace is successfull press ``CTRL+SHIFT+S`` to save all

10. Hit ``CTRL+SHIFT+F`` -> **Replace in files** Tab:
   - In the **Find** text box write: ``/Nighthold Launcher;component``
   - In the **Replace** text box write ``/Server Name Launcher;component`
   - Then click **Replace All** button
   - After replace is successfull press ``CTRL+SHIFT+S`` to save all

11. Right click on the solution -> Clean Solution then -> Build solution.

______
## Renaming Nighthold Login project

1. From the Solution Explorer on the right side right click on **Nighthold Login** -> Rename.

2. Right click on the solution -> Clean Solution then -> Build solution.

3. Right click on the renamed **Project -> Properties -> Application** and change:
   - In the **Assembly name** field change to ``Server Name Login``
   - In the **Default namespace** field change to ``Server_Name_Login``
   - Click on the **Assembly Information..** button and update to your preferences
   - Hit ``CTRL+S`` to save.

4. In the project explorer find and open file **NightholdLogin.xaml.cs**
   - Rename the namespace ``Nighthold_Login`` to ``Server_Name_Login`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**
   - Rename the partial class ``NightholdLogin`` to ``ServerNameLogin`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**

5. Right click on the solution -> Clean Solution then -> Build solution.

6. Right click on **NightholdLogin.xaml** -> rename to **ServerNameLogin.xaml**

7. Right click on the solution -> Clean Solution then -> Build solution.

8. In the Project Explorer find and open file **App.xaml**
   - Rename ``StartupUri="NightholdLogin.xaml"`` to ``"StartUpUri="ServerNameLogin.xaml"``

9. Hit ``CTRL+SHIFT+F`` -> **Replace in files** Tab:
   - In the **Find** text box write: ``NightholdLogin``
   - In the **Replace** text box write ``ServerNameLogin``
   - Then click **Replace All** button
   - After replace is successfull press ``CTRL+SHIFT+S`` to save all

10. Right click on the solution -> Clean Solution then -> Build solution.

11. Hit ``CTRL+SHIFT+F`` -> **Replace in files** Tab:
   - In the **Find** text box write: ``/Nighthold Login;component``
   - In the **Replace** text box write ``/Server Name Login;component``
   - Then click **Replace All** button
   - After replace is successfull press ``CTRL+SHIFT+S`` to save all

12. Right click on the solution -> Clean Solution then -> Build solution.

13. In the Project Explorer find and open file **ServerNameLogin.xaml.cs**
    - Replace line:
       - Process.Start($"Nighthold Launcher.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");
    - To:
       - Process.Start($"Server Name Launcher.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");

14. Right click on the solution -> Clean Solution then -> Build solution.

______
## Renaming the Nighthold Updater project

1. From the Solution Explorer on the right side right click on **Nighthold Updater** -> Rename.

2. Right click on the solution -> Clean Solution then -> Build solution.

3. Right click on the renamed **Project -> Properties -> Application** and change:
   - In the **Assembly name** field change to ``Server Name Updater``
   - In the **Default namespace** field change to ``Server_Name_Updater``
   - Click on the **Assembly Information..** button and update to your preferences
   - Hit ``CTRL+S`` to save.

4. In the project explorer find and open file **MainWindow.xaml.cs**
   - Rename the namespace ``Nighthold_Updater`` to ``Server_Name_Updater`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**
   - Rename the partial class ``MainWindow`` to ``ServerNameUpdater`` then **Right Click** on it -> **Quick Actions and Refactoring** -> **Rename all...**

5. Right click on the solution -> Clean Solution then -> Build solution.

6. Right click on **NightholdLogin.xaml** -> rename to **ServerNameUpdater.xaml**

7. Right click on the solution -> Clean Solution then -> Build solution.

8. In the Project Explorer find and open file **App.xaml**
   - Rename ``StartupUri="MainWindow.xaml"`` to ``"StartUpUri="ServerNameUpdater.xaml"``

9. Hit ``CTRL+SHIFT+F`` -> **Replace in files** Tab:
   - In the **Find** text box write: ``/Nighthold Updater;component``
   - In the **Replace** text box write ``/Server Name Updater;componen``
   - Then click **Replace All** button
   - After replace is successfull press ``CTRL+SHIFT+S`` to save all

10. Right click on the solution -> Clean Solution then -> Build solution.

11. In the Project Explorer find and open file **ServerNameLogin.xaml.cs**
    - Replace line:
       - Process.Start($"Nighthold Login.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");
    - To:
       - Process.Start($"Server Name Login.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");


12. Right click on the solution -> Clean Solution then -> Build solution.

______
## Fixing the API avatars not updating after renaming the launcher

1. Go to **YourWebsite/application/modules/avatars.php** open and edit:
   - Replace line:
      - ``if(substr($db_avatar_url, 0, 29) === "/Nighthold Launcher;component/")``
   - To:
      - ``if(substr($db_avatar_url, 0, 32) === "/Server Name Launcher;component/")``
      - Don't forget to count all characters between the quotes including the space(s) to replace 29 with the correct number of characters.

2. Save, done.
