# Main Scene Auto Loading
Main, initial, loader, boot, bootstrap - you name it.

Whenever you enter playmode this tool firstly loads the main scene, and only after that loads desired scene(s). 

For more details why you need this and how to implement it: https://forum.unity.com/threads/executing-first-scene-in-build-settings-when-pressing-play-button-in-editor.157502/

Known issues:
 + Hierarchy state and selections are lost on domain reload (when exiting playmode after script compilation)

## Installation
Install via git url by adding this entry in your **manifest.json**

`"com.sag.main-scene-auto-loading": "https://github.com/STARasGAMES/unity-main-scene-auto-loading.git#upm"`
