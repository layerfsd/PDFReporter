AxiomCoders.PDFTemplateEditor Release Notes

Version 2.1 Final Version
Date: 06/17/2010

- Measure unit can be changed in options window for all items, selecting measure unit for each item is disabled
- Changed property Image Type for DynamicImage, now it has only be File or Data Source
- Added Project Properties dialog so you can edit project info like: Subject, Title, Author
- Improved generating algorithm for bottom docked balloons
- Implemented validation for detecting dynamic items without Data Streams set
- Implemented background image support on report page
- Implemented FillColor on Balloons and report page
- Implemented embedding of images to template files
- Implemented embedding of true type fonts to template files
- Fixed visibility issue of command items used for sizing and moving
- Fixed issue where default for AlwaysShowOnPage was false
- Fixed issue with defaults on FitToContent, CanGrow
- Fixed bug with “Can Grow” property of the dynamic balloon causing overlapping with static balloons
- Fixed various bugs in color choose dialog
- Fixed bug where auto save is saving something even without opened project
- Fixed bug where borders are not drawing correctly if rotation is used
- Fixed bug with placing balloons when page is zoomed
- Fixed multiple selections moving 
- Fixed working with sizing markers when page is zoomed
- Fixed issues with docking (when items are removed and undo/redo is used)
- Fixed issue with generator not being able to show correct preview in some cases
- Fixed undo/redo issue when load project is used
- Fixed undo/redo issue and saving project when /2 /3 /4 options are used

Version 2.0 (Beta)
Date: 10/03/2010

- Implemented some shortcuts for cut, copy, paste and close project for easy access
- Implemented themes
- Implemented custom balloon borders
- Implemented new and faster generating algorithm that gives you more freedom
- Changed docking code to be more accurate with complicated reports
- Fixed some rotation issues when you rotate object and then move it
- Fixed 1/2w, 1/4w, 1/3w, and 1/2h, 1/3h, 1/4h options
- Fixed report page object showing in objects hierarchy
- Fixed some preview mode issues

Version 1.9 
Date: 12/05/2009

- Implemented font metrics saving, for usage of different fonts
- Implemented new icons for toolbars
- Implemented object hierarchy bellow property grid
- Implemented Cancel and Help button on some dialogs
- Implemented some missing pictures and icons
- Changed About dialog to have more info
- Changed exception reporter to use silent reporting
- Changed some icons to be in same style
- Changed some number only properties to accept only numbers
- Fixed toolbars to be movable
- Fixed issue about gradient not drawing correctly
- Fixed deleting items from object hierarchy
- Fixed some issues in New Project dialog
- Fixed exit shortcut
- Fixed dialog boxes so you can’t maximize them
- Fixed "Show Grid" and "Show Balloon Borders" checked/unchecked states

Version 1.8 
Date: 10/14/2009

- Implemented standard toolbar (New, Open, Save, Cut, Copy, Paste)
- Implemented bring to front and send to back commands
- Implemented ExceptionReporter for central issue reporting
- Changed name of options file to Options.cfg instead of Options.xml
- Fixed issue with zoomed grid and selections
- Fixed issues with options not being loaded correctly in all cases
- Fixed some issues with generator (Not aligning objects properly)

Version 1.7 
Date: 08/10/2009

- Implemented button for preview mode of the project
- Implemented undo/redo for properties
- Implemented moving of objects with keyboard
- Implemented new project dialog
- Changed look of balloons
- Changed base location of text object, it is now bottom line instead of top line
- Fixed some issue with saving rectangle shape and gradient information
- Fixed loading of rectangle shape
- Fixed some minor issue with docking in parents
- Fixed some issues with text drawing

Version 1.6 
Date: 04/25/2009

- Implemented combo box that contains objects
- Implemented copy, cut, paste
- Implemented Precalculated item and Dynamic image item
- Fixed load/save methods
- Fixed resizing controls
- Fixed page scrolling
- Fixed docking issues

Version Pre - 1.6 (Custom development project)
Date: N/A
