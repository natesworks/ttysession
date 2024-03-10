# TtySessionManager

A session manager made in .NET 8

(Normally you would run this in tty this is just an example)
![screenshot](https://i.ibb.co/Twj2MRd/image.png)

## Installation

Dotnet runtime not required

```bash
sudo wget -O /usr/bin/ttysession https://github.com/natesworks/ttysession/releases/download/0.1-beta/ttysession
```

You can add this to your ~/.profile or to ~/.zlogin if your using zsh.

```bash
#!/bin/bash

if [ "$(tty)" = "/dev/tty1" ];then
  exec ttysession
fi
```

## Configuration

To configure you can run ttysession config where you can set the session name and the executable to run.