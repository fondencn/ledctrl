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

    on — Turns the LED ring on (white).
    off — Turns the LED ring off.
    rot — Sets the LED ring to red.
    grün — Sets the LED ring to green.
    blau — Sets the LED ring to blue.
    weiß — Sets the LED ring to white.
    Zufall — Activates a random LED pattern.
    RedAlert — Activates a red spinning alert pattern.
    exit — Exits the application.


## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License.

---

Would you like to customize any section (such as usage examples or dependencies)? If not, I can create and apply this content to your README.md file. Please confirm or provide extra details if needed.
