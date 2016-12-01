This is a Unity template that only requires the editor to play.

# Instillation
1. [install Unity]( <https://store.unity.com/download?ref=personal>)
2. [Create an Unity ID]( <https://id.unity.com/en/conversations/a5947373-c6dd-4d10-807e-7eabee1af0db007f?view=register>)
(Note that there is an accessibility captcha, just arrow down till you hear the get accessible option).
3. Download this project
4. Change the lines in `ScreenreaderAccessibleUnityTemplate\Assets\libraries\debugger.cs` to point to where you wish your logs to be saved
5. Run MainScene.unity and get some sighted help to log into your Unity account and open the scene for the first time. (Note that it may be possible to do this with a screen reader, it is just very difficult and only takes 5 seconds for a sighted person to do once)
6. Once the main scene has imported all the assets, there should be a screen titled: `Unity Personal (64bit) - MainScene.unity - ScreenreaderAccessibleUnityTemplate - PC, Mac & Linux Standalone <DX11>`
7. Press ctrl+p, wait about 4 seconds and press space. A sound should play and you are done!
8. Press alt+space and exit

# Running Unity
1. Open ` ScreenreaderAccessibleUnityTemplate\Assets\ MainScene.unity`
2. Press alt and if “system” comes up, press escape and enter a couple times. Eventually a screen will show up that says `Unity Personal (64bit) - MainScene.unity - ScreenreaderAccessibleUnityTemplate - PC, Mac & Linux Standalone <DX11>`
3. Press ctrl+o and navigate to the mainScene.unity file and hit enter. If a dialogue pops up saying that the scene you have chosen is not in the current project, open the scene anyways.
4. Press ctrl+p, wait 4 seconds and the game has started. If you are running the default game, press space and a sound should play.
5. To stop playing press ctrl+p again.

# Unity Workflow
The unity editor compiles all the scripts every time ctrl+p is pressed and the game runs. This means that one can have their text editor or visual studio up and when they save a change, they alt+tab to the Unity window and press ctrl+p and the new script compiles and runs. Once they test and wish to continue development, they press ctrl+p and the game view closes. Next time they press ctrl+p, the scripts recompile with new changes and so on.
Anything in the assets folder is compiled, so there can be any structure to the assets folder one wishes. The only thing that needs to be there is the Resources folder for sounds and graphics.

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

Contributing
Please create an issue or a pull request if you would like to add or change anything.

