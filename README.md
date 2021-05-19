# Main Scene Auto Loading
Main, initial, loader, boot, bootstrap - you name it.

Whenever you enter playmode this tool firstly loads the main scene, and only after that loads desired scene(s). 

![image](https://user-images.githubusercontent.com/23558898/118852487-81cd5c00-b8db-11eb-8c40-de2e1ae2a458.png)


https://user-images.githubusercontent.com/23558898/118852692-ab868300-b8db-11eb-9f79-9c9e76c96f76.mp4


https://user-images.githubusercontent.com/23558898/118852714-ae817380-b8db-11eb-8217-cbb1f810175e.mp4



For more details why you need this and how to implement it: https://forum.unity.com/threads/executing-first-scene-in-build-settings-when-pressing-play-button-in-editor.157502/

Features:
 + Persist hierarchy state(selected and expanded objects) on entering/exiting playmode.
 + Convinient settings UI located in ProjectSettings window.
 + UPM support, simple install.
 + Highly extendable for any project.

Known issues:
 + Hierarchy state and selections are lost on domain reload (when exiting playmode after script compilation)
 + Prefab selection missed after scene load

## Installation
Install via git url by adding this entry in your **manifest.json**

`"com.sag.main-scene-auto-loading": "https://github.com/STARasGAMES/unity-main-scene-auto-loading.git#upm"`

Or using PackageManager window:
1. copy this link `https://github.com/STARasGAMES/unity-main-scene-auto-loading.git#upm`,
2. open PackageManager window,
3. click `+` button in top-left corner,
4. select `Add package from git URL`,
5. in appeared field paste the link you copied before (`https://github.com/STARasGAMES/unity-main-scene-auto-loading.git#upm`).
