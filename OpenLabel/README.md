# OpenLabel
##### ZPL Labeling with Open-Source Flexibility

# Project Description

OpenLabel is a C# library designed to streamline working with Zebra Programming Language (ZPL) for label printing. It enables developers to send ZPL commands to Zebra label printers, scale labels for different printer resolutions, and customize labels using a flexible templating system.
This library is particularly useful for scenarios where ZPL labels are generated using external tools like [Labelary](https://labelary.com) or other ZPL design software. With OpenLabel, developers can dynamically modify these labels by replacing placeholders, handling conditional statements, and ensuring compatibility across different printer resolutions.
Whether you need to automate label printing, generate dynamic ZPL templates, or adjust labels for varying printer resolutions, OpenLabel provides the flexibility and simplicity required for efficient label management.

# Table of Contents

- [OpenLabel](#openlabel)
- [Project Description](#project-description)
- [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Installation](#installation)
  - [Usage](#usage)
    - [1. Print a Label](#1-print-a-label)
    - [2. Scale a Label](#2-scale-a-label)
    - [3. Use Templates](#3-use-templates)
- [License](#license)
- [Acknowledgements](#acknowledgements)

## Features
- **Print ZPL Labels**: Easily send ZPL commands to a Zebra printer over the network.
- **Scale Labels**: Adjust ZPL code to fit different printer resolutions (DPI).
- **Template System**: Replace placeholders with actual values and handle conditional statements inside ZPL templates.

## Installation

Add this library to your project via [NuGet](https://www.nuget.org/packages/OpenLabel/) or by including the source code.

## Usage

### 1. Print a Label
```csharp
NetworkPrinter printer = new NetworkPrinter();
await printer.PrintLabelAsync(@"\\server\printer", 5, "^XA^FO50,50^A0N,50,50^FDHello, World!^FS^XZ");
```

### 2. Scale a Label
```csharp
string scaledZPL = LabelScaler.ScaleZPL("^XA^FO50,50^FS^XZ", 203, 300);
```

### 3. Use Templates
```csharp
TemplateHandler templateHandler = new TemplateHandler();
string template = "{{IF CONDITION}}^FO50,50^FDText^FS{{ENDIF}}";
Dictionary<string, string> placeholders = new Dictionary<string, string> { { "CONDITION", "1" } };
string renderedLabel = templateHandler.RenderTemplate(template, placeholders);
```

# License

This software is licensed under the [GPL-3.0 license](LICENSE)

# Acknowledgements

- The code for the ZPL scaling was in large part possible with the work of:
  -  [isatufan](https://gist.github.com/isatufan/e22dc07ac7968fcb8e9a6046fa15f57a)
  -  [deexno](https://github.com/deexno/Zebra-ZPL-rescaler)