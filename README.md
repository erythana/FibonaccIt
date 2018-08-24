# FibonaccIt


FibonaccIt is an Visual Studio Extension to apply an Fibonacci-Styled formatting to your code.
  - Applies Code to active Window
  - Reformats to (Visual Studio) Default-Settings, based on your language
  
# Changed Settings
| Setting | Value |
| ------ | ------ |
|  Options-->Text-Editor-->Basic-->Advanced-->"Pretty listing (reformatting) of code" | "Unchecked" |
|  Options-->Text-Edtior-->All Langauges-->Tabstops--> | "Insert Spaces"  |
  
When reformatting, these settings are set back to their default values (Pretty listing enabled, Keep Tabstops)

| Setting | Value |
| ------ | ------ |
|  Options-->Text-Editor-->Basic-->Advanced-->"Pretty listing (reformatting) of code" | "Checked" |
|  Options-->Text-Edtior-->All Langauges-->Tabstops--> | "Keep Tabstops"  |


# ToDo
Evaluate wheter it would be usefull (/stylish!) to apply the formatter to the whole project
Implement handling of languages which require Indention, like Python

# Bugs/Features
  - If you use this tool on code which requires Indention, like Python, the code will get unusable (but pretty looking, though) - looking into handling this sort of things  
    The Reformatter won't work here! But you can revert (Ctrl+Z) these changes...
