This is a Unity template that only requires the editor to play.

# Installation
1. [Install Unity]( <https://store.unity.com/download?ref=personal>)
2. [Create an Unity ID]( <https://id.unity.com/en/conversations/a5947373-c6dd-4d10-807e-7eabee1af0db007f?view=register>)
(Note that there is an accessibility captcha, just arrow down till you hear the get accessible option).
3. [Download this project](https://github.com/frastlin/ScreenreaderAccessibleUnityTemplate/archive/master.zip)
4. Run MainScene.unity and get some sighted help to log into your Unity account and open the scene for the first time. (Note that it may be possible to do this with a screen reader, it is just very difficult and only takes 5 seconds for a sighted person to do once)
5. Once the main scene has imported all the assets, there should be a screen titled: `Unity Personal (64bit) - MainScene.unity - ScreenreaderAccessibleUnityTemplate - PC, Mac & Linux Standalone <DX11>`
6. Press ctrl+p, wait about 4 seconds and press space. A sound should play and you are done!
7. Press alt+space and exit
8. (Optional Step) Change the lines in `ScreenreaderAccessibleUnityTemplate\Assets\libraries\debugger.cs` to point to where you wish your logs to be saved

# Running Unity
You can start Unity and access this project one of two ways:
You either directly open the main scene of the project. This will also start Unity. The main scene is located under `ScreenreaderAccessibleUnityTemplate\Assets\MainScene.unity`
If you are starting Unity for the first time, this is the preferred method.
Alternatively, you can open Unity and then use the file menu to open the scene. 
Here are the steps:
1. Start Unity. This will open a project selection dialog window that is fairly inaccessible. If you already opened Unity with any project before, you can hit Ctrl+Enter to load the project last opened.
2. If not, read the title of the window with insert+t and if it says "unity" then your version of unity, press enter a couple times. Eventually a screen will show up that says `Unity Personal (64bit) - MainScene.unity - ScreenreaderAccessibleUnityTemplate - PC, Mac & Linux Standalone <DX11>`
3. Press ctrl+o and navigate to the mainScene.unity file and hit enter. If a dialogue pops up saying that the scene you have chosen is not in the current project, open the scene anyways.
4. Press ctrl+p, wait 4 seconds and the game has started. If you are running the default game, press space and a sound should play.
5. To stop playing press ctrl+p again.

# Unity Workflow
The unity editor compiles all the scripts every time a script is changed and the editor receives back focus. Press ctrl+p to jump into game mode run the game. The Editor will always finish compiling first before entering game mode. This means that one can have their text editor or visual studio up and after saving, alt+tab to the Unity Editor window and press ctrl+p. The script changes will compile and then the game runs. Once they test and wish to continue development, they press ctrl+p and the game view closes. Next time they press ctrl+p, the scripts recompile with new changes and so on.
Anything in the assets folder is compiled, so there can be any structure to the assets folder one wishes. The only thing that needs to be there is the Resources folder for sounds and graphics.
All the .meta files are there for version control reasons and unity creates them automatically. You can disable this, but it is not advised, as this can break your project if you are using a version control system such as Git, SVN or Perforce.

# Advocating for Greater Editor Accessibility
There is a
[Unity forum post]( <https://feedback.unity3d.com/suggestions/screen-reader-accessibility>)
That has a list of features that should be made accessible. But it needs more votes. You get 10 votes a month, so please put all 10 of your votes on this issue each month until it is the highest voted post on the forums.

# Stuff that still needs to be done
* Create a screen reader library that will work under every platform. It will probably use something like [Tolk]( <https://github.com/dkager/tolk>) for greatest native windows accessibility.
* Link to a nice clean documentation for developing completely in scripts
* Have both an action script and C# template
* Document the build process for deploying finished games
* Have a list of completed games in Unity by blind developers

# Contributing
Please create an issue or a pull request if you would like to add or change anything.

