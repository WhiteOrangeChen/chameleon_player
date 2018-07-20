### Overview
**Chameleon Player** Music player that supports **mp3, m4a, ogg** format. Player gives you ability to change **ID3 tags** of file such is **artwork, title, artist, album, genre, year**.

### Example
[Download Chameleon Player](https://vnat.co/files/Chameleon_Player.zip)

### Requirements
* Visual Studio Version 14.0
* .NET Framework 4.0

### Folder Structure
* **Player/Documents/** - Resources used in project, like *DLLs* and *Images*
* **Player/Files/** - Content of this folder **will be copied in *root* directory of *application***. Contains 1 directory:
  * **core** - Html, CSS, JavaScript files

### Notes
* Pre-build event command line from **Properties** -> **Build Events** will create **core/** folder in application root directory and copy content from **Player/Files/core/** to **core/** folder.

### External resources used
* **jQuery v1.8.2** - a cross-platform JavaScript library designed to simplify the client-side scripting of HTML. [Website](https://jquery.com/) | [License](https://jquery.org/license/)
* **Disable Text Select Plugin v1.1** - Used to stop users from selecting text. [Website](http://www.jdempster.com/category/jquery/disabletextselect/) | [License](https://opensource.org/licenses/MIT)
* **Color Animation** - This plugin is based on Color Animations by John Resig. It fixes a major bug and also adds support for the borderColor-property. [Website](http://www.bitstorm.org/jquery/color-animation/) | [License](https://opensource.org/licenses/MIT)
* **jPlayer Plugin for jQuery** - Developed by Happyworm, jPlayer is Free, Open Source and licensed under the MIT license. [Website](http://jplayer.org/) | [License](https://opensource.org/licenses/MIT)

### Screenshots
![1](Player/Documents/Screenshots/1.jpg?raw=true)  
![2](Player/Documents/Screenshots/2.jpg?raw=true)  
![3](Player/Documents/Screenshots/3.jpg?raw=true)  
![4](Player/Documents/Screenshots/4.jpg?raw=true)  