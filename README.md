# Description
- trying to create a bezier curve that dosent need a globally defined upvector
- The bigger part of the repo is to implement second order dynamics to move objects on a procedual way
- Custom property Drawer to help display the behaviour
- Graph drawer class (ImporvedEditorGraph.cs)

# SecondOrderDynamics and GraphDrawer example
![image](https://user-images.githubusercontent.com/62551684/192576411-0105cc69-1d34-4906-affb-ab048898f40e.png)
![Demo|5](https://user-images.githubusercontent.com/62551684/192579508-d636df7f-e56f-44d2-8019-9d93e0fd4b7a.gif)

 # How to install / See example scene
 - download the ziped code and open it as a Unity project. Open scene Assets/Scenes/SampleScene.unity to see examples
 
# To use SecondOrderDynamics in other projects
- Package the Assets/SecondOrderDynamics folder and import it to another project to use :)
![image](https://user-images.githubusercontent.com/62551684/192576803-7f779328-fc1a-4f14-a019-00c1fc924420.png)

# References / Soruces
- maths of secondOrderDynamics is heavaly based on the wonderful procedual animations youtube video by T3ssel8r https://www.youtube.com/watch?v=KPoeNZZ6H4s
- SpecialAbility related script examples are taken from https://nosuchstudio.medium.com/learn-unity-editor-scripting-property-drawers-part-2-6fe6097f1586
- ImprovedEditorGraph is based on https://gist.github.com/thelastpointer/c52c4b1f147dc47961798e39e3a7ea10#comments

# Future improvements
- Make a templated version if possible
- Make verion that supports Quaternions
- Give ImprovedEditorGraph a better name and support mulitple graphs in same view
