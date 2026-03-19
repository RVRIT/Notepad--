# Notepad++

A Notepad++-inspired text editor built with **C#** and **WPF**.

---

## Features

### File Management
- Create, open, save, and save-as text files
- Multiple files open simultaneously as tabs
- Tabs display the filename and mark unsaved changes with `*`
- Close individual tabs or all at once, with unsaved change prompts
- Session restore — reopens the files you had open last time

### Folder Explorer
- Full disk tree view visible on startup, expandable per folder
- Hidden and system files are filtered out
- Double-click any file to open it in a new tab
- Right-click any folder for options:
  - **New File** — creates a file inside that folder and opens it
  - **Copy Path** — copies the folder path to clipboard
  - **Copy Folder** — copies the folder for pasting
  - **Paste Folder** — pastes the copied folder inside the selected one *(disabled until a folder is copied)*

### Search & Replace
- **Find** — search forward and backward through the current tab or all open tabs, with wrap-around
- **Replace** — find and replace one occurrence at a time
- **Replace All** — replaces every occurrence in the current tab or all tabs at once

### View
- Toggle the folder explorer panel on or off via the View menu
- Explorer and editor panel layout mirrors Notepad++

### Persistence
- Tree view visibility preference saved between sessions
- Open files and their unsaved content restored on next launch

---

## Tech Stack

- C#/.NET - Application logic
- WPF - UI Framework

---

## How to use

1. Clone the repository
```bash
   git clone https://github.com/RVRIT/notepad.git
```
2. Open `Notepad.sln` in Visual Studio
3. Build and run with `F5`

---
