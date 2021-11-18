# GTA Mission Downloader

<p align="center">
  <b>Dark:</b>
</p>
<p align="center">
  <img src="https://user-images.githubusercontent.com/44268275/142441115-e07bddd8-14e8-47c1-ac6f-fd51e29abbd7.png">
</p>

<p align="center">
  <b>Light:</b>
</p>
<p align="center">
  <img src="https://user-images.githubusercontent.com/44268275/142441435-8f5852c3-0367-4d56-a816-ed63af765a2c.png">
</p>

## Main features:

* Two different options to download a mission file:
  * Automatically
  * Manually
* The program pulls the information about the GTA servers and displays it
* Option to join to the specific server:
  * Server 1
  * Server 2
  * Event server
  * TS server -> Ability to join to the specific channel
* Different configuration options for a user
* Everything saves in the one folder.

.Exe file: https://drive.google.com/drive/u/2/folders/1i8rxUqM7NRaO8hnexDDrQm5zYlWffbXy <br/>
Changelog: https://docs.google.com/document/d/1HzbVqK26YLsJtSBC2XJ7s_VcQ9IWH9ZWy3LEGEDwrJk/edit

----------------------------------------
#### Instaling Guide (If you just want a working .exe file, go here: https://drive.google.com/drive/u/2/folders/1i8rxUqM7NRaO8hnexDDrQm5zYlWffbXy):

0. Make sure that you have installed https://visualstudio.microsoft.com/vs/.
1. On main page of this repository click "Clone or download" -> Download ZIP -> Place the ZIP file on the desktop -> Place the folder on the desktop again.
2. Double click the .sln file.
3. On top of the Visual Studio, choose Debug or Release and then on the right click "Start". I would recommend to choose Release, if you want to place .exe file on your desktop.
4. The location of the .exe file is in \GTA-Downloader-master\GTADownloader\bin\Release.

----------------------------------------
#### YOUR OWN KEY:

Steps to get API key:
1. General info: https://www.daimto.com/google-drive-authentication-c/ -> the part that interests you is: https://gyazo.com/722acbd29ff1adcc6f9956f666486d9d
2. visit  https://console.developers.google.com
3. Create new project, name it w/e you want, go to "Credentials" -> "Create Credentials" -> choose API key -> you will get a key, copy and paste it into DataHelper -> service in "ApiKey".
4. Start debugging, everything should be working.
