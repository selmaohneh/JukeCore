# JukeCore

Playing audio books triggred by a RFID card powered by .NET Core running on a Raspberry Pi! 

![](box3.jfif)
![](box1.jfif)

## Introduction

Inspired by the [tonies box](https://tonies.de/), [the phoniebox project](http://phoniebox.de/), and [this amazing blog article](https://splittscheid.de/selfmade-phoniebox/) I 
decided to build a self-made music box for my daughter, as well. Instead of using an existing software I implemented my own just to try out the cross-plattform functionality
of .NET Core.

## The hardware

* The software inside the box runs on a Raspberry Pi 3 B+. 
* Evertyhing is powered by an accu which gets loaded by an power adapter. 
* There is a button to switch the whole box on and off.
* A RFID-reader is connected to the Raspberry Pi via USB and simply forwards the read IDs as keyboard input.
* The media files are stored on a 64GB usb stick. I am planning on switching to a bigger external disk as soon as the stick gets too full.
* The audio output is realized via some cheap USB speakers.
* There are five arcade buttons to control the box (volume up/down, next/previous track, play/pause).

![](box2.jfif)

## The software

The concept of the software is actually straight-forward. It waits for the ID of a RFID card to be entered, then looks to which folder the given ID is mapped, and then plays all 
included files as playlist. All the media-handling is based on [libvlcsharp](https://github.com/videolan/libvlcsharp).

## Contributing

* Just pick [an existing issue or file your own](hhttps://github.com/selmaohneh/JukeCore/issues).
* [Kaffee? :-)](https://www.buymeacoffee.com/SaMAsU1N6)
