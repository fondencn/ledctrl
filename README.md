# ledctrl - A tool for controlling SEED Respeaker HAT LED Ring

A simple command line tool for controlling the LED ring on the Seeed ReSpeaker 4-Mic Raspberry Pi Hat.

## Features

- Control the LED ring on the Seeed ReSpeaker 4-Mic Raspberry Pi Hat
- Command-line interface for easy scripting and automation
- Written in C#

## Requirements

- Raspberry Pi with Seeed ReSpeaker 4-Mic Hat
- .NET SDK (version compatible with your codebase)
- [Optional] Any dependencies specified in your project file

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/fondencn/ledctrl.git
   cd ledctrl
   ```
2. Build the project:
   ```bash
   dotnet build
   dotnet publish -c Release -r linux-arm64 --self-contained true /p:PublishTrimmed=true
   ```

## Usage

Run the tool from the command line. Example:
```bash
./ledctrol
```

The console application ledctrl accepts the following commands (case-insensitive, German keywords for colors):
First select `hat` if using the respeaker raspberry pi hat or `usb` if you are using the respeaker usb device. 

Then enter valid commands like
- rot
- blau
- grün
- wait
- spin
- trace (for DOA-visualization)


## Needed permisisons
Please note that correct permissions are needed to run this application, so either run this tool as `sudo` or adjust your `udev` rules.

### UDEV settings (ubuntu)
To allow your user to access the ReSpeaker USB device (Vendor ID 2886, Product ID 0018) without root, create a udev rule:

Create the rule file:
``` 
sudo nano /etc/udev/rules.d/99-respeaker.rules
```
Paste this line:
```
SUBSYSTEM=="usb", ATTRS{idVendor}=="2886", ATTRS{idProduct}=="0018", MODE="0666"
```
Save and exit (Ctrl+O, Enter, Ctrl+X).

Reload udev rules and replug the device:
```
sudo udevadm control --reload-rules
sudo udevadm trigger
```

Now you can run your app as a normal user.


## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License.
