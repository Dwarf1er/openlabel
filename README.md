<div align="center">

# OpenLabel
##### ZPL Labeling with Open-Source Flexibility

<img alt="OpenLabel logo" height="280" src="/assets/openlabel.png" />

![Build Status](https://img.shields.io/github/actions/workflow/status/Dwarf1er/openlabel/publish.yml?branch=master&style=for-the-badge)
![License](https://img.shields.io/github/license/Dwarf1er/openlabel?style=for-the-badge)
![Version](https://img.shields.io/github/v/release/Dwarf1er/openlabel?style=for-the-badge)
![Issues](https://img.shields.io/github/issues/Dwarf1er/openlabel?style=for-the-badge)
![PRs](https://img.shields.io/github/issues-pr/Dwarf1er/openlabel?style=for-the-badge)
![Contributors](https://img.shields.io/github/contributors/Dwarf1er/openlabel?style=for-the-badge)
![Stars](https://img.shields.io/github/stars/Dwarf1er/openlabel?style=for-the-badge)

</div>

# Project Description

OpenLabel is a C# library designed to simplify working with **Zebra Programming Language (ZPL)** for label printing. It enables developers to:

- **Print labels** to Zebra printers over the network.
- **Scale ZPL code** to fit different printer resolutions (DPI).
- **Use a powerful templating system** to replace placeholders and handle conditional statements.

This library is ideal for scenarios where ZPL labels are generated dynamically or created using external tools like [Labelary](https://labelary.com). Whether you need to automate label printing, generate custom ZPL templates, or adjust labels for different printers, OpenLabel makes it easy and efficient. It is the foundation for any application to print labels to your label printers!

If you find OpenLabel useful, consider starring the repo to show your support! 🌟

# Why OpenLabel?

**No more struggling with proprietary software to print your ZPL labels over the network!** OpenLabel provides a developer-friendly way to manage label scaling, templating, and printing, making it perfect for:

- ✅ E-commerce & Warehousing – Automate label printing for shipping.
- ✅ Manufacturing – Print product labels dynamically.
- ✅ Retail – Generate barcode labels for inventory.

# Table of Contents

- [OpenLabel](#openlabel)
        - [ZPL Labeling with Open-Source Flexibility](#zpl-labeling-with-open-source-flexibility)
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
- 🖨️ **Print ZPL Labels** – Send ZPL commands to a Zebra printer over the network.
- 📏 **Scale Labels** – Adjust ZPL code to fit different printer resolutions (DPI).
- 📝 **Template System** – Replace placeholders with actual values and handle conditional statements inside ZPL templates.

## Installation

```bash
dotnet add package OpenLabel
```

## Usage

### 1. Print a Label
To print a label, you can send a ZPL string to a Zebra printer over the network:
```csharp
NetworkPrinter printer = new NetworkPrinter();
await printer.PrintLabelAsync(@"\\server\printer", 5, "^XA^FO50,50^A0N,50,50^FDHello, World!^FS^XZ");
```

### 2. Scale a Label
You can scale a ZPL label to fit a specific printer resolution (DPI):
```csharp
string scaledZPL = LabelScaler.ScaleZPL("^XA^FO50,50^FS^XZ", 203, 300);
```

### 3. Use Templates
If you want to dynamically generate labels with placeholders, you can use the template system:
```csharp
TemplateHandler templateHandler = new TemplateHandler();
string template = "{{IF CONDITION}}^FO50,50^FDText^FS{{ENDIF}}";
Dictionary<string, string> placeholders = new Dictionary<string, string> { { "CONDITION", "1" } };
string renderedLabel = templateHandler.RenderTemplate(template, placeholders);
```

# Get Involved

- 💡 Have ideas or feature requests? Open an [issue](https://github.com/Dwarf1er/openlabel/issues) or start a discussion!
- 🔧 Want to contribute? Fork the repo, submit PRs, and help improve OpenLabel!
- 📢 Using OpenLabel? Let us know! Share your use case, report issues, or give feedback.
- 🚀 Spread the word! ⭐ Star the repo, share it on social media, and help grow the community!

Join us in making OpenLabel the best open-source ZPL tool! 🎉

# License

This software is licensed under the [MIT license](LICENSE)

# Acknowledgements

- The code for the ZPL scaling was in large part possible with the work of:
  -  [isatufan](https://gist.github.com/isatufan/e22dc07ac7968fcb8e9a6046fa15f57a)
  -  [deexno](https://github.com/deexno/Zebra-ZPL-rescaler)